#include "stdafx.h"
#include "OfficilMapManager.h"
#include "CrashPlayer.h"
#include "base64_encode.h"
#include "utilities.h"


#define _SAVE_OFFICIL_TIME_  (20 * _TIME_SECOND_MSEL_)
OfficilMapManager::OfficilMapManager()
{
	
}


OfficilMapManager::~OfficilMapManager()
{
}

u64 OfficilMapManager::generateMapIndex()
{
	return gGameConfig.GenerateMapIndex();
}

void OfficilMapManager::loadMap(DBQuery* p, const char* argu)
{
	std::string sql = "select *,UNIX_TIMESTAMP(`create_time`) from `player_map` where index_map in (";
	sql = sql + argu + ");";
	DBQuery& query = *p;
	query << sql;
	query.parse();
	SDBResult sResult = query.store();
	int count = sResult.size();
	std::list<u64> ids;
	for (int i = 0; i < count; i++)
	{
		message::CrashMapData map_data;
		DBRow row_map = sResult[i];
		bool is_complete = (bool)row_map["is_complete"];
		
		map_data.set_mapname(row_map["map_name"].c_str());
		map_data.set_chapter(0);
		map_data.set_section(0);
		map_data.set_creatername(row_map["creater_name"].c_str());
		map_data.set_gold(row_map["gold"]);
		map_data.set_create_time(row_map["UNIX_TIMESTAMP(`create_time`)"]);

		message::CrashmoMapBaseData * baseinfo = map_data.mutable_data();
		std::string data_temp = row_map["map_data"].c_str();
		baseinfo->ParseFromString(base64_decode(data_temp));
		baseinfo->set_map_index(row_map["index_map"]);
		gCrashMapManager.AddCrashMap(map_data);
	}
	query.reset();
	sResult.clear();
}

void OfficilMapManager::init(DBQuery* p)
{
	if (p)
	{
		DBQuery& query = *p;
		query << "select *,UNIX_TIMESTAMP(`create_time`) from `offical_map` ;";
		query.parse();
		SDBResult sResult = query.store();
		int count = sResult.size();
		std::list<u64> ids;
		for (int i = 0; i < count; i++)
		{
			DBRow row = sResult[i];
			//message::CrashMapData temp_map;
			int chapter_id = row["chapter"];
			int section_id = row["section"];
			u64 map_index = row["index_map"];
			ids.push_back(map_index);
			OFFICILMAPLIST::iterator it = _officilmap.find(map_index);
			if (it == _officilmap.end())
			{
				std::map<int, u64> map_temp;				
				_officilmap[chapter_id] = map_temp;
			}
			_officilmap[chapter_id][section_id] = map_index;
			u64 current_map_index = map_index;
			if (gGameConfig.GetMaxMapIndex() < current_map_index)
			{
				gGameConfig.SetMaxMapIndex(current_map_index);
			}

			
			//gCrashMapManager.AddCrashMap(temp_map);
		}
		char sz_temp[512];
		std::string sql_argu = "";
		std::list<u64>::iterator it = ids.begin();
		int load_count = 10;
		for (int i = 0; it != ids.end(); ++it, i ++)
		{
			if (sql_argu.empty() == false)
			{
				sql_argu += ",";
			}		
			u64 index_entry = (*it);
			sprintf(sz_temp, "%llu", index_entry);
			sql_argu += sz_temp;
			if (i >= load_count)
			{
				loadMap(p, sql_argu.c_str());
				sql_argu.clear();
				i = 0;
			}
		}

		if (sql_argu.empty() == false)
		{
			loadMap(p, sql_argu.c_str());
		}		
		query.reset();
		sResult.clear();
		query << "select * from `offical_section_names`";
		sResult = query.store();
		count = sResult.size();
		for (int i = 0; i < count; i ++)
		{
			DBRow row = sResult[i];
			_chapter_names[row["chapter_id"]] = row["chapter_name"].c_str();
		}
	}

	if (gEventMgr.hasEvent(this, EVENT_SAVE_OFFICIL_DATA_) == false)
	{
		gEventMgr.addEvent(this, &OfficilMapManager::saveOfficilMap, EVENT_SAVE_OFFICIL_DATA_, _SAVE_OFFICIL_TIME_, 99999999, 0);
	}
}

const OFFICILMAPLIST* OfficilMapManager::getOfficilMap()
{
	return &_officilmap;
}

void OfficilMapManager::getOfficilMap(CrashPlayer* p, int page)
{
	message::MsgOfficilMapACK msg;
	OFFICILMAPLIST::iterator it = _officilmap.find(page);
	if (it != _officilmap.end())
	{
		std::map<int, u64>::iterator it_temp = it->second.begin();
		for (; it_temp != it->second.end(); ++ it_temp)
		{
			u64 map_index = it_temp->second;
			const message::CrashMapData* map_entry = gCrashMapManager.GetCrashMap(map_index);
			message::CrashMapData* temp = msg.add_maps();
			temp->CopyFrom(*map_entry);
		}
	}
	p->sendPBMessage(&msg);
}


void OfficilMapManager::modifySectionName(int section, const char* name, CrashPlayer* player)
{
	message::MsgModifySectionNameACK msg;
	msg.set_section(section);
	msg.set_section_name(name);
	std::string name_temp;
	name_temp = name;
	_chapter_names[section] = name_temp;
	player->sendPBMessage(&msg);
}


void OfficilMapManager::saveOfficilMap()
{
	char sztemp[4096];
	std::string sql_head = "replace into `offical_map`(`account`,`creater_name`,`map_name`,`map_data`,`create_time`,`chapter`,`section`, `gold`, `index_map`) values";
	std::string sql_excute;
	int current_count = 0;
	int max_save_count = 5;
	//OFFICILMAPLIST::iterator it = _officilmap.begin();
	//for (; it != _officilmap.end(); ++ it)
	//{
	//	std::map<int, message::CrashMapData>::iterator it_temp = it->second.begin();
	//	for (; it_temp != it->second.end(); ++ it_temp)
	//	{
	//		if (current_count != 0)
	//		{
	//			sql_excute += ",";
	//		}
	//		else
	//		{
	//			sql_excute += sql_head;
	//		}
	//		current_count++;
	//		message::CrashMapData temp_map = it_temp->second;
	//		long acc = temp_map.chapter() * 100000 + temp_map.section();
	//		u64 map_index = temp_map.mutable_data()->map_index();
	//		std::string temp_data;
	//		std::string create_time = get_time(temp_map.create_time());
	//		temp_data = temp_map.data().SerializeAsString();
	//		temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());
	//		sprintf(sztemp, "(%lu,'%s','%s','%s','%s',%d, %d, %d, %llu)", acc, temp_map.creatername().c_str(), temp_map.mapname().c_str(),
	//			temp_data.c_str(), create_time.c_str(), temp_map.chapter(),temp_map.section(), temp_map.gold(), map_index);
	//		sql_excute += sztemp;			
	//		if (current_count > max_save_count)
	//		{
	//			message::ReqSaveOfficilMap msg;
	//			msg.set_sql(sql_excute.c_str());
	//			gGSDBClient.sendPBMessage(&msg,0);
	//			current_count = 0;
	//			sql_excute.clear();
	//		}

	//	}
	//}

	//if (sql_excute.empty() == false)
	//{
	//	message::ReqSaveOfficilMap msg;
	//	msg.set_sql(sql_excute.c_str());		
	//	gGSDBClient.sendPBMessage(&msg, 0);
	//	current_count = 0;
	//	sql_excute.clear();
	//}

	max_save_count = 10;
	sql_head = "replace into `offical_section_names`(`chapter_id`,`chapter_name`)values";
	CHAPTERSNAMES::iterator it_session = _chapter_names.begin();
	for (; it_session != _chapter_names.end(); ++ it_session)
	{
		if (current_count != 0)
		{
			sql_excute += ",";
		}
		else
		{
			sql_excute += sql_head;
		}
		current_count++;
		
		sprintf(sztemp, "(%d,'%s')", it_session->first, it_session->second.c_str());
		sql_excute += sztemp;
		if (current_count > max_save_count)
		{
			message::ReqSaveOfficilSectionNames msg;
			msg.set_sql(sql_excute.c_str());
			gGSDBClient.sendPBMessage(&msg, 0);
			current_count = 0;
			sql_excute.clear();

		}
	}

	if (sql_excute.empty() == false)
	{
		message::ReqSaveOfficilSectionNames msg;
		msg.set_sql(sql_excute.c_str());
		gGSDBClient.sendPBMessage(&msg, 0);
	}


}

void OfficilMapManager::saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p)
{
	OFFICILMAPLIST::iterator it = _officilmap.find(map_data->section());
	if (it != _officilmap.end())
	{
		std::map<int, u64> temp_map;
		_officilmap.insert(OFFICILMAPLIST::value_type(map_data->chapter(), temp_map));
	}

	std::map<int, u64>::iterator it_section = _officilmap[map_data->chapter()].find(map_data->section());
	if (it_section != _officilmap[map_data->chapter()].end())
	{
		gCrashMapManager.DelCrashMap(it_section->second);
	}
	const message::CrashMapData* entry = gCrashMapManager.CreateCrashMap(map_data, message::OfficeMap);
	_officilmap[map_data->chapter()][map_data->section()] = entry->data().map_index();

	message::MsgSaveMapACK msgACK;
	//msgACK.set_map_name(map_data->mapname().c_str());
	msgACK.set_save_type(message::OfficeMap);
	msgACK.set_error(message::ServerError_NO);
	message::CrashMapData* map_temp = msgACK.mutable_map();
	map_temp->CopyFrom(*entry);
	p->sendPBMessage(&msgACK);
}

const CHAPTERSNAMES& OfficilMapManager::getSectionNames()
{
	return _chapter_names;
}

const message::CrashMapData* OfficilMapManager::getOfficilMap(int chapter_id, int section_id)
{
	
	OFFICILMAPLIST::iterator it = _officilmap.find(chapter_id);
	if (it != _officilmap.end())
	{
		std::map<int, u64>::iterator it_section = it->second.find(section_id);
		if (it_section != it->second.end())
		{
			u64 map_index = it_section->second;
			return gCrashMapManager.GetCrashMap(map_index);
		}
	}
	return NULL;	
}