#include "stdafx.h"
#include "db_quest.h"
#include "game_session.h"
#include "message/msg_game_db.pb.h"
#include <stdio.h>
#ifdef _WIN32_WINDOWS
#include <objbase.h>  
#else
#include <uuid/uuid.h>
#endif

enum
{
    _QUERY_SAVE_PLAYER_ = _NORMAL_THREAD + 1,
    _CREATE_PLAYER_,
	_RELATION_THREAD_,
	_MAIL_THREAD_,
	_UNLOCK_AREAS_THREAD_,
	_FARM_CONSTRUCSTIONS_THREAD_,
	_FARM_CREATE_THREAD_,
	_FARM_RESOURCE_THREAD_,
	_FARM_CONSTRUCTION_RESOURCE_THREAD_,
	_SAVE_FARM_THREAD_,
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

std::string get_time(time_t cur_time)
{
	time_t timep;
	if (cur_time == 0)
	{
		time(&timep); /*获取time_t类型的当前时间*/
	}
	else
	{
		timep = cur_time;
	}


	struct tm* cur = localtime(&timep);
	char sz_time[256];
	sprintf(sz_time, "%d-%d-%d %d:%d:%d",cur->tm_year + 1900, cur->tm_mon + 1, cur->tm_mday + 1, cur->tm_hour, cur->tm_min, cur->tm_sec);
	return std::string(sz_time);
}


void DBQuestManager::saveCharacterInfo(message::ReqSaveCharacterData* msg)
{
	message::CrashPlayerInfo* playerInfo = msg->mutable_data();
	account_type acc_temp = playerInfo->account();
	char sql[512];
	DBQParms parms;
	sprintf(sql, "replace into character(`account`, `pass_point`, `pass_section`, `name`) values (%u, %d, %d, '%s')",
		acc_temp, playerInfo->pass_point(), playerInfo->pass_section(), playerInfo->name());
	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, &parms, NULL, _QUERY_SAVE_PLAYER_);
	int temp_size = playerInfo->completemap_size();
	std::string temp_sql_replace = "replace into player_map(`map_index`, `account`, `creater_name`, `map_name`, `map_data`, `create_time`, `is_complete`) values";
	int count = 0;
	for (int i = 0; i < temp_size; i ++, count ++)
	{
		if (count != 0)
		{
			temp_sql_replace += ",";
		}
		const message::CrashMapData Data = playerInfo->completemap(i);
		std::string create_time = get_time(Data.create_time());
		sprintf(sql, "(%llu, %lu, '%s', '%s', '%s', '%s', %d )", Data.data().map_index(), acc_temp,Data.creatername().c_str(),
			Data.mapname().c_str(), Data.data().SerializeAsString().c_str(), create_time.c_str(), 1);
		temp_sql_replace += sql;
	}

	temp_size = playerInfo->incompletemap_size();
	for (int i = 0; i < temp_size; i ++, count ++)
	{
		if (count != 0)
		{
			temp_sql_replace += ",";
		}
		const message::CrashMapData Data = playerInfo->incompletemap(i);
		std::string create_time = get_time(Data.create_time());
		sprintf(sql, "(%llu, %lu, '%s', '%s', '%s', '%s', %d )", Data.data().map_index(), acc_temp,Data.creatername().c_str(),
			Data.mapname().c_str(), Data.data().SerializeAsString().c_str(), create_time.c_str(), 0);		
		temp_sql_replace += sql;
	}
	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, temp_sql_replace.c_str(), &parms, NULL, _QUERY_SAVE_PLAYER_);


}

//void DBQuestManager::saveCharacterInfo(message::NoneCharacterDataServer* msg)
//{
//	account_type account_temp = msg->account();
//	const message::NoneCharacterData Data = msg->data();
//	int size_temp = Data.pass_instances_size();
//	std::string instance_pass;
//	char sz_temp[512];
//	for (int i = 0 ;i < size_temp; i ++)
//	{
//		if (i != 0)
//		{
//			instance_pass += ",";
//		}
//		int instance_id = Data.pass_instances(i);
//		sprintf(sz_temp, "%d", instance_id);
//		instance_pass += sz_temp;
//	}
//	DBQParms parms;
//	char sql[512];
//	sprintf(sql, "replace into none_character(`account_id`, `character_guid`, `game_state`, `instance_state`) values (%u, '%s', '%s', '%s');", 
//		account_temp, Data.guid().c_str(), instance_pass.c_str(), Data.instances_status().c_str());
//	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, &parms, NULL, _QUERY_SAVE_PLAYER_);
//	
//	int toy_size = Data.toys_size();
//	if (toy_size != 0)
//	{
//		std::string sql_item = "replace into none_item(`toy_guid`, `character_guid`, `name`, `exp`, `level`) values ";
//		for (int i = 0; i < toy_size; i ++)
//		{
//			if (i != 0) 
//			{
//				sql_item += ",";
//			}
//			message::ToyStatData toy_data = Data.toys(i);
//			sprintf(sz_temp, "('%s', '%s', '%s', %d, %d)", toy_data.toy_guid().c_str(), Data.guid().c_str(), toy_data.name().c_str(), toy_data.exp(), toy_data.level());	
//			sql_item += sz_temp;
//		}
//		gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql_item.c_str(), &parms, NULL, _QUERY_SAVE_PLAYER_);
//	}
//
//	
//}

void DBQuestManager::dbDoQueryCharacter(DBQuery* p, const void* d)
{
	TSQueryInfo* pkParm = static_cast<TSQueryInfo*>(const_cast<void*>(d));
	DBQuery& query = *p;
	DBQParms parms;
	char sql[256];
	sprintf(sql, "select * from `character` where `account` = %u", pkParm->account_);
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
		info->set_pass_point(row["pass_point"]);
		info->set_pass_section(row["pass_section"]);
		int is_admin = row["isadmin"];
		if (is_admin == 0)
		{
			info->set_isadmin(false);
		}
		else
		{
			info->set_isadmin(true);
		}

		sprintf(sql, "select * from player_map where account = %u", pkParm->account_);
		query.clear();
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
				if (is_complete)
				{
					map_data = info->add_completemap();
				}
				else
				{
					map_data = info->add_incompletemap();
				}
				map_data->set_mapname(row_map["map_name"].c_str());
				map_data->set_number(0);
				map_data->set_section(0);
				message::CrashmoMapBaseData * baseinfo = map_data->mutable_data();
				std::string data_temp = row_map["map_data"].c_str();
				baseinfo->ParseFromString(data_temp);
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
