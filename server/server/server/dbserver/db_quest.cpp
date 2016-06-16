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

void DBQuestManager::saveOfficilObjs(const char* szsql)
{
	gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, szsql, 0, NULL, _SAVE_OFFICIL_MAP_);
}

void DBQuestManager::delOfficilMap(int section, int number)
{
	char sql[512];
	sprintf(sql, "delete from offical_map where map_section=%d and map_section=%d", section, number);
	gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, 0, NULL, _DELETE_OFFICIL_MAP_);

}

void DBQuestManager::saveOfficilMap(message::gs2dbSaveOfficileMapReq* msg)
{
	char sql[4096];
	message::CrashMapData* map_temp = msg->mutable_data();
	std::string str_sql;
	sprintf(sql, "delete from offical_map where map_section=%d and map_section=%d;", map_temp->section(), map_temp->number());	
	std::string temp_sql_replace;
	temp_sql_replace += sql;
	temp_sql_replace += "replace into offical_map(`account`, `creater_name`, `map_name`, `map_data`, `create_time`, `section`, `number`) values";
	std::string temp_data;
	temp_data = map_temp->data().SerializeAsString();
	int temp_number = map_temp->number();
	std::string create_time = get_time(map_temp->create_time());
	sprintf(sql, "(%lu, '%s', '%s', '%s', '%s', %d, %d)",0, map_temp->creatername().c_str(),
		map_temp->mapname().c_str(), 
		base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size()).c_str(), 
		create_time.c_str(), 
		map_temp->section(), 
		temp_number);
	temp_sql_replace += sql;
	gWorldDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, temp_sql_replace.c_str(), 0, NULL, _SAVE_OFFICIL_MAP_);

}

void DBQuestManager::saveCharacterInfo(message::ReqSaveCharacterData* msg)
{
	message::CrashPlayerInfo* playerInfo = msg->mutable_data();
	account_type acc_temp = playerInfo->account();
	char sql[4096];
	DBQParms parms;
	sprintf(sql, "replace into `character`(`account`, `pass_point`, `pass_section`, `name`, `isadmin`) values (%u,%d,%d,'%s',%d)",
		acc_temp, playerInfo->pass_point(), playerInfo->pass_section(), playerInfo->name().c_str(), (int)playerInfo->isadmin());
	gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, sql, &parms, NULL, _QUERY_SAVE_PLAYER_);
	int temp_size = playerInfo->completemap_size();
	
	bool need_save = false;
	if (temp_size > 0)
	{
		need_save = true;
	}

	std::string temp_sql_replace;
	sprintf(sql, "delete from player_map where `account`=%lu;", acc_temp);
	temp_sql_replace += sql;
	temp_sql_replace += "replace into `player_map`(`index_map`, `account`, `creater_name`, `map_name`, `map_data`, `create_time`, `is_complete`) values";
	int count = 0;
	std::string temp_data;
	for (int i = 0; i < temp_size; i ++, count ++)
	{
		if (count != 0)
		{
			temp_sql_replace += ",";
		}
		const message::CrashMapData Data = playerInfo->completemap(i);
		std::string create_time = get_time(Data.create_time());
		temp_data = Data.data().SerializeAsString();
		temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());

		sprintf(sql, "(%llu, %lu, '%s', '%s', '%s', '%s', %d )", Data.data().map_index(), acc_temp,Data.creatername().c_str(),
			Data.mapname().c_str(), temp_data.c_str(),
			create_time.c_str(), 1);
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
		const message::CrashMapData Data = playerInfo->incompletemap(i);
		std::string create_time = get_time(Data.create_time());
		temp_data = Data.data().SerializeAsString();
		temp_data = base64_encode((const unsigned char*)temp_data.c_str(), temp_data.size());
		sprintf(sql, "(%llu, %lu, '%s', '%s', '%s', '%s', %d )", Data.data().map_index(), acc_temp,Data.creatername().c_str(),
			Data.mapname().c_str(), temp_data.c_str(), create_time.c_str(), 0);
		temp_sql_replace += sql;
	}
	if (need_save)
	{
		gDBCharDatabase.addSQueryTask(this, &DBQuestManager::dbCallNothing, temp_sql_replace.c_str(), &parms, NULL, _QUERY_SAVE_PLAYER_);
	}
	


}


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

		sprintf(sql, "select *,UNIX_TIMESTAMP(`create_time`) from player_map where account = %u", pkParm->account_);
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
				map_data->set_creatername(row_map["creater_name"].c_str());
				
				map_data->set_create_time(row_map["UNIX_TIMESTAMP(`create_time`)"]);
				message::CrashmoMapBaseData * baseinfo = map_data->mutable_data();
				std::string data_temp = row_map["map_data"].c_str();
				baseinfo->ParseFromString(base64_decode(data_temp));
				baseinfo->set_map_index(row_map["index_map"]);
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
