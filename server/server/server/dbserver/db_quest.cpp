#include "stdafx.h"
#include "db_quest.h"
#include "game_session.h"
#include "message/msg_game_db.pb.h"
#include <stdio.h>
#include "base64_encode.h"
/*
#ifdef _WIN32_WINDOWS
#include <objbase.h>  
#else
#include <uuid/uuid.h>
#endif
*/
enum
{
    _QUERY_SAVE_PLAYER_ = _NORMAL_THREAD + 1,
	_DELETE_OFFICIL_MAP_,
	_SAVE_OFFICIL_MAP_
};

struct TSQueryInfo
{
	TSQueryInfo(account_type acc, u16 gid, tran_id_type trid)
	{
		account_ = acc;
		gsid_ = gid;
		tranid_ = trid;
	}
	account_type account_;
	u16 gsid_;
	u8 result_;
	tran_id_type tranid_;
	message::CharacterDataACK Data;
};

void DBQuestManager::queryInfo(account_type a, tran_id_type t, u16 gs)
{
	gDBCharDatabase.addBatchTask(this, &DBQuestManager::dbDoQueryCharacter, &DBQuestManager::dbCallQueryCharacter, new TSQueryInfo(a, gs, t), "create char info"/*, _CREATE_PLAYER_*/);
}



//void DBQuestManager::saveOfficilMap(message::MsgSaveOfficilMapReq* msg, tran_id_type t)
//{
//
//}
//void DBQuestManager::officilMapReq(message::MsgOfficilMapReq* msg, tran_id_type t)
//{
//
//}

void DBQuestManager::CharacterDatabaseSql(const char* szsql)
{
	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, szsql, 0, NULL, _QUERY_SAVE_PLAYER_);
}
void DBQuestManager::worldDatabaseSql(const char* szsql)
{
	gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, szsql, 0, NULL, _SAVE_OFFICIL_MAP_);
}

void DBQuestManager::delOfficilMap(int chapter, int section)
{
	char sql[512];
	sprintf(sql, "delete from offical_map where chapter=%d and section=%d", chapter, section);
	gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, 0, NULL, _DELETE_OFFICIL_MAP_);
}

void DBQuestManager::saveMap(message::gs2dbSaveMapReq* msg)
{
	char sql[4096];
	message::CrashMapData* map_temp = msg->mutable_data();
	u64 map_index = map_temp->data().map_index();
	u64 account = msg->account();
	int temp_number = map_temp->section();
	if (msg->type() == message::OfficeMap)
	{

		std::string str_sql;
		sprintf(sql, "delete from offical_map where map_chapter=%d and map_section=%d;", map_temp->chapter(), map_temp->section());
		std::string temp_sql_replace;
		temp_sql_replace += sql;
		u64 map_index = map_temp->data().map_index();
		temp_sql_replace += "replace into offical_map(`account`, `chapter`, `section`, `index_map`) values";
		
		
		sprintf(sql, "(%d, %d, %d, %llu)", 0,
			map_temp->chapter(),
			temp_number, map_index);
		temp_sql_replace += sql;
		gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, temp_sql_replace.c_str(), 0, NULL, _SAVE_OFFICIL_MAP_);
		temp_sql_replace.clear();
	}
	std::string temp_data;
	temp_data = map_temp->data().SerializeAsString();
	temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());

	std::string create_time = get_time(map_temp->create_time());
	std::string str_sql_replace_map = "replace into `player_map`(`index_map`, `account`, `map_name`, `map_data`, `create_time`, `is_complete`, `gold`) values";
	sprintf(sql, "(%llu, %llu, '%s', '%s', '%s', %d, %d)", map_index, account, map_temp->mapname().c_str(), temp_data.c_str(), create_time.c_str(),
		map_temp->chapter(),
		temp_number);
	str_sql_replace_map += sql;
	CharacterDatabaseSql(str_sql_replace_map.c_str());	
	//gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, str_sql_replace_map.c_str(), 0, NULL, _SAVE_OFFICIL_MAP_);

}

void DBQuestManager::saveCharacterInfo(message::ReqSaveCharacterData* msg)
{
	/*
	message::CrashPlayerInfo* playerInfo = msg->mutable_data();
	account_type acc_temp = playerInfo->account();
	char sql[4096];
	DBQParms parms;
	google::protobuf::RepeatedPtrField< ::message::intPair >* resources = playerInfo->mutable_resources();
	google::protobuf::RepeatedPtrField< ::message::intPair >::const_iterator it = resources->begin();
	std::string resource_str;
	char sz_resource[256];
	for (; it != resources->end(); it ++)
	{
		if (resource_str.empty() == false)
		{
			resource_str += ";";
		}
		sprintf(sz_resource, "%d,%d", it->number_1(), it->number_2());
		resource_str += sz_resource;
	}
	sprintf(sql, "replace into `character`(`account`, `pass_chapter`, `pass_section`,\
		 `name`, `isadmin`, `map_width`,\
		 `map_height`, `map_count`, `group_count`, `gold`) values (%llu, %d, %d, '%s', %d, %d, %d, %d, '%s', %d)",
		acc_temp, 0, 0, playerInfo->name().c_str(), (int)playerInfo->isadmin(),
		playerInfo->map_width(), playerInfo->map_height(), playerInfo->map_count(), resource_str.c_str(), playerInfo->gold());
	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, &parms, NULL, _QUERY_SAVE_PLAYER_);
	int temp_size = playerInfo->completemap_size();
	

	bool need_save = false;
	if (temp_size > 0)
	{
		need_save = true;
	}

	std::string temp_sql_replace;
	sprintf(sql, "delete from player_map where `account`=%llu;", acc_temp);
	temp_sql_replace += sql;
	temp_sql_replace += "replace into `player_map`(`index_map`, `account`, `creater_name`, `map_name`, `map_data`, `create_time`, `is_complete`, `gold`) values";
	int count = 0;
	std::string temp_data;
	for (int i = 0; i < temp_size; i ++, count ++)
	{
		if (count != 0)
		{
			temp_sql_replace += ",";
		}

		u64 map_index = playerInfo->completemap(i);
		message::CrashMapData* DataMap = gCrashMapManager.GetCrashMap(map_index);
		//const message::CrashMapData Data = playerInfo->completemap(i);
		std::string create_time = get_time(DataMap->create_time());
		temp_data = DataMap->data().SerializeAsString();
		temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());

		sprintf(sql, "(%llu, %llu, '%s', '%s', '%s', '%s', %d, %d )", DataMap->data().map_index(), acc_temp, DataMap->creatername().c_str(),
			DataMap->mapname().c_str(), temp_data.c_str(),
			create_time.c_str(), 1, DataMap->gold());
		temp_sql_replace += sql;
	}

	temp_size = playerInfo->incompletemap_size();
	if (temp_size > 0)
	{
		need_save = true;
	}

	for (int i = 0; i < temp_size; i ++, count ++)
	{
		if (count != 0)
		{
			temp_sql_replace += ",";
		}
		u64 map_index = playerInfo->incompletemap(i);
		message::CrashMapData* DataMap = gCrashMapManager.GetCrashMap(map_index);
		//const message::CrashMapData Data = playerInfo->incompletemap(i);
		std::string create_time = get_time(DataMap->create_time());
		temp_data = DataMap->data().SerializeAsString();
		temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());
		sprintf(sql, "(%llu, %llu, '%s', '%s', '%s', '%s', %d, %d )", DataMap->data().map_index(), acc_temp, DataMap->creatername().c_str(),
			DataMap->mapname().c_str(), temp_data.c_str(), create_time.c_str(), 0, DataMap->gold());
		temp_sql_replace += sql;
	}
	if (need_save)
	{
		gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, temp_sql_replace.c_str(), &parms, NULL, _QUERY_SAVE_PLAYER_);
	}
	*/


}


void DBQuestManager::dbDoQueryCharacter(DBQuery* p, const void* d)
{
	TSQueryInfo* pkParm = static_cast<TSQueryInfo*>(const_cast<void*>(d));
	DBQuery& query = *p;
	DBQParms parms;
	char sql[256];
	sprintf(sql, "select * , UNIX_TIMESTAMP(`last_accept_task_time`),UNIX_TIMESTAMP(`last_publish_map_time`) from `character` where `account` = %llu", pkParm->account_);
	query << sql;
	query.parse();
	SDBResult sResult = query.store(parms);
	if (sResult.size() != 0)
	{
		DBRow row = sResult[0];
		message::CharacterDataACK msg;		
		message::CrashPlayerInfo* info = msg.mutable_data();
		info->set_account(row["account"]);
		info->set_name(row["name"].c_str());
		int is_admin = row["isadmin"];
		info->set_map_width(row["map_width"]);
		info->set_map_height(row["map_height"]);
		info->set_map_count(row["map_count"]);
		info->set_gold(row["gold"]);
		info->set_jewel(row["jewel"]);
		info->set_complete_task_count(row["complate_task_count"]);		
		info->set_last_accept_task_time(row["UNIX_TIMESTAMP(`last_accept_task_time`)"]);
		info->set_last_publish_map_time(row["UNIX_TIMESTAMP(`last_publish_map_time`)"]);
		//info->last_publish_map_time
		std::vector<std::string> vcStr1;
		std::vector<std::string> vcStr2;
		std::string task = row["task"].c_str();
		std::string officil_game_record = row["officil_game_record"].c_str();
		if (officil_game_record.empty() == false)
		{
			SplitStringA(officil_game_record, ";", vcStr1);
			int size_str_split_count = vcStr1.size();
			for (size_t i = 0; i < size_str_split_count; i++)
			{
				SplitStringA(vcStr1[i], ",", vcStr2);
				if (vcStr2.size() == 2)
				{
					message::intPair* pair_entry = info->add_passed_record();
					int number_1 = atoi(vcStr2[0].c_str());
					int number_2 = atoi(vcStr2[1].c_str());
					pair_entry->set_number_1(number_1);
					pair_entry->set_number_2(number_2);
				}
			}
		}

		if (task.empty() == false)
		{
			SplitStringA(task, ";", vcStr1);
			int size_str_split_count = vcStr1.size();
			for (size_t i = 0; i < size_str_split_count; i++)
			{
				SplitStringA(vcStr1[i], ",", vcStr2);
				if (vcStr2.size() == 4)
				{
					message::TaskInfo* entry = info->add_current_task();
					int task_id = atoi(vcStr2[0].c_str());
					int number_1 = atoi(vcStr2[1].c_str());
					int number_2 = atoi(vcStr2[2].c_str());
					int number_3 = atoi(vcStr2[3].c_str());
					entry->set_task_id(task_id);
					entry->set_argu_1(number_1);
					entry->set_argu_2(number_2);
					entry->set_argu_3(number_3);
				}
			}
		}
		std::string group_str =	row["group_count"].c_str();
		if (group_str.empty() == false)
		{
			SplitStringA(group_str, ";", vcStr1);
			int size_str_split_count = vcStr1.size();
			for (size_t i = 0; i < size_str_split_count; i++)
			{
				SplitStringA(vcStr1[i], ",", vcStr2);
				if (vcStr2.size() == 2)
				{
					message::intPair* pair_entry = info->add_resources();
					int number_1 = atoi(vcStr2[0].c_str());
					int number_2 = atoi(vcStr2[1].c_str());
					pair_entry->set_number_1(number_1);
					pair_entry->set_number_2(number_2);
				}

			}
		}
		if (is_admin == 0)
		{
			info->set_isadmin(false);
		}
		else
		{
			info->set_isadmin(true);
		}

		sprintf(sql, "select *,UNIX_TIMESTAMP(`create_time`) from `player_map` where account = %llu", pkParm->account_);
		query.reset();
		//query.clear();
		query << sql;
		sResult.clear();
		sResult = query.store(parms);
		if (sResult.size() != 0)
		{
			int count_size = sResult.size();
			message::CrashMapData* map_data = NULL;
			for (int i = 0; i < count_size; i++)
			{
				DBRow row_map = sResult[i];
				bool is_complete = (bool)row_map["is_complete"];
				message::CrashMapData* map_data = msg.add_maps();				
				map_data->set_mapname(row_map["map_name"].c_str());
				map_data->set_chapter(0);
				map_data->set_section(0);
				map_data->set_creatername(row_map["creater_name"].c_str());
				map_data->set_gold(row_map["gold"]);				
				map_data->set_create_time(row_map["UNIX_TIMESTAMP(`create_time`)"]);
				
				message::CrashmoMapBaseData * baseinfo = map_data->mutable_data();
				std::string data_temp = row_map["map_data"].c_str();
				baseinfo->ParseFromString(base64_decode(data_temp));
				baseinfo->set_map_index(row_map["index_map"]);
				if (is_complete)
				{
					info->add_completemap(baseinfo->map_index());
				}
				else
				{
					info->add_incompletemap(baseinfo->map_index());
				}
			}
		}
		gDBGameManager.sendMessage(&msg, pkParm->tranid_, pkParm->gsid_);
	}
	else
	{
		message::NeedCreateCharacter msg;
		msg.set_account(pkParm->account_);
		gDBGameManager.sendMessage(&msg, pkParm->tranid_, pkParm->gsid_);
	}


}

void DBQuestManager::dbCallQueryCharacter(const void* d, bool s)
{

}
