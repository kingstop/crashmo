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


void OfficilMapManager::init(DBQuery* p)
{
	if (p)
	{
		DBQuery& query = *p;
		query << "select *,UNIX_TIMESTAMP(`create_time`) from `offical_map`;";
		query.parse();
		SDBResult sResult = query.store();
		int count = sResult.size();
		for (int i = 0; i < count; i++)
		{
			message::CrashMapData temp_map;
			DBRow row = sResult[i];
			temp_map.set_mapname(row["map_name"].c_str());
			temp_map.set_creatername(row["creater_name"].c_str());
			message::CrashmoMapBaseData* data_base = temp_map.mutable_data();
			std::string str_data = row["map_data"].c_str();
			data_base->ParseFromString(base64_decode(str_data));			
			temp_map.set_create_time(row["UNIX_TIMESTAMP(`create_time`)"]);
			temp_map.set_number(row["section"]);
			temp_map.set_section(row["number"]);
			OFFICILMAPLIST::iterator it = _officilmap.find(temp_map.section());
			if (it == _officilmap.end())
			{
				std::map<int, message::CrashMapData> map_temp;				
				_officilmap[temp_map.section()] = map_temp;
			}
			_officilmap[temp_map.section()][temp_map.number()] = temp_map;		
		}

		query.reset();
		sResult.clear();
		query << "select * from `offical_section_names`";
		sResult = query.store();
		count = sResult.size();
		for (int i = 0; i < count; i ++)
		{
			DBRow row = sResult[i];
			_sections_names[row["section_id"]] = row["section_name"].c_str();
		}
	}

	if (gEventMgr.hasEvent(this, EVENT_SAVE_OFFICIL_DATA_) == false)
	{
		gEventMgr.addEvent(this, &OfficilMapManager::saveOfficilMap, EVENT_SAVE_OFFICIL_DATA_, _SAVE_OFFICIL_TIME_, 99999999, 0);
	}
}



void OfficilMapManager::getOfficilMap(CrashPlayer* p, int page)
{
	message::MsgOfficilMapACK msg;
	OFFICILMAPLIST::iterator it = _officilmap.find(page);
	if (it != _officilmap.end())
	{
		std::map<int, message::CrashMapData>::iterator it_temp = it->second.begin();
		for (; it_temp != it->second.end(); ++ it_temp)
		{
			message::CrashMapData* temp = msg.add_maps();
			temp->CopyFrom(it_temp->second);
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
	_sections_names[section] = name_temp;
	player->sendPBMessage(&msg);
}


void OfficilMapManager::saveOfficilMap()
{
	char sztemp[4096];
	std::string sql_head = "replace into `offical_map`(`account`,`creater_name`,`map_name`,`map_data`,`create_time`,`section`,`number`, `gold`) values";
	std::string sql_excute;
	int current_count = 0;
	int max_save_count = 5;
	OFFICILMAPLIST::iterator it = _officilmap.begin();
	for (; it != _officilmap.end(); ++ it)
	{
		std::map<int, message::CrashMapData>::iterator it_temp = it->second.begin();
		for (; it_temp != it->second.end(); ++ it_temp)
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
			message::CrashMapData temp_map = it_temp->second;
			long acc = temp_map.section() * 100000 + temp_map.number();
			
			std::string temp_data;
			std::string create_time = get_time(temp_map.create_time());
			temp_data = temp_map.data().SerializeAsString();
			temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());
			sprintf(sztemp, "(%lu,'%s','%s','%s','%s',%d, %d, %d)", acc, temp_map.creatername().c_str(), temp_map.mapname().c_str(),
				temp_data.c_str(), create_time.c_str(), temp_map.section(),temp_map.number(), temp_map.gold());
			sql_excute += sztemp;			
			if (current_count > max_save_count)
			{
				message::ReqSaveOfficilMap msg;
				msg.set_sql(sql_excute.c_str());
				gGSDBClient.sendPBMessage(&msg,0);
				current_count = 0;
				sql_excute.clear();
			}

		}
	}

	if (sql_excute.empty() == false)
	{
		message::ReqSaveOfficilMap msg;
		msg.set_sql(sql_excute.c_str());		
		gGSDBClient.sendPBMessage(&msg, 0);
		current_count = 0;
		sql_excute.clear();
	}

	max_save_count = 10;
	sql_head = "replace into `offical_section_names`(`section_id`,`section_name`)values";
	SECTIONSNAMES::iterator it_session = _sections_names.begin();
	for (; it_session != _sections_names.end(); ++ it_session)
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
		std::map<int, message::CrashMapData> temp_map;
		_officilmap.insert(OFFICILMAPLIST::value_type(map_data->section(), temp_map));
	}

	message::CrashMapData entry;
	entry.CopyFrom(*map_data);
	_officilmap[map_data->section()][map_data->number()] = entry;
	message::MsgSaveMapACK msgACK;
	msgACK.set_map_name(map_data->mapname().c_str());
	msgACK.set_save_type(message::OfficeMap);
	msgACK.set_error(message::ServerError_NO);
	message::CrashMapData* map_temp = msgACK.mutable_map();
	map_temp->CopyFrom(entry);
	p->sendPBMessage(&msgACK);
}

const SECTIONSNAMES& OfficilMapManager::getSectionNames()
{
	return _sections_names;
}