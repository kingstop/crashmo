#include "stdafx.h"
#include "girl_shop.h"
#include "session.h"


girl_shop::girl_shop(void)
{
}


girl_shop::~girl_shop(void)
{

}

int girl_shop::get_action_score(int act)
{
	ACTIONSCORS::iterator it = _action.find(act);
	if (it != _action.end())
	{
		return it->second;
	}
	return 0;
}

void girl_shop::dbCallNothing(const SDBResult* , const void* d, bool s)
{


}

void girl_shop::addgoods(Session* p, message::AddGoodsReq* msg)
{
	int create_count = msg->good_count();
	if (create_count > 100)
	{
		return;
	}
	int good_id = msg->info().good_id();
	GOODS::iterator it = _goods.find(good_id);
	if (it != _goods.end())
	{
		
	}
	else
	{
		goodsinfo info_entry;
		info_entry.info_.CopyFrom(msg->info());
		info_entry.info_.set_good_id(good_id);
		for (int i = 0; i < create_count; i ++)
		{
			std::string key_temp = get_new_cd_key();
			if (key_temp.empty() == false)
			{
				info_entry.cdkeys_[key_temp] = exchange_type_no;
			}
		}
		_goods[good_id] = info_entry;
		message::AddGoodsACK msg_send;
		info_entry.get(msg_send.mutable_info());
		p->sendPBMessage(&msg_send);		
	}
}

std::string girl_shop::create_cd_key()
{
	std::string cur_key;
	char sz[] = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};
	for (int i = 0; i < 8; i ++)
	{
		int key_index  = rand()%26;
		cur_key += sz[key_index];
	}
	return cur_key;
}

GOODS& girl_shop::get_goods()
{
	return _goods;
}

std::string girl_shop::get_new_cd_key()
{
	std::string temp_key;
	for (int i = 0; i < 30; i ++)
	{
		std::string new_key = create_cd_key();
		if (have_cdkey(new_key.c_str()))
		{
			
		}
		else
		{
			temp_key = new_key;
			break;
		}
	}

	if (temp_key.empty() == false)
	{
		_all_ckeys.push_back(temp_key);
	}
	return temp_key;
	
}

bool girl_shop::have_cdkey(const char* cd_key)
{
	std::vector<std::string>::iterator it = _all_ckeys.begin();
	for (; it != _all_ckeys.end(); ++ it)
	{
		if (cd_key == (*it))
		{
			return true;
		}
	}
	return false;
}

void girl_shop::save_goods()
{
	std::string excute_sql = "delete from good_cdkey; delete from good_info;";
	if (_goods.size() != 0)
	{
		std::string sql_head = "insert into good_cdkey( `cdkey`, `good_id`, `exchanged`) values";
		int max_save_count = 30;
		int i = 0;
		char sz_temp[512];
		std::string cd_key;
		int change_type = 0;
		DBQParms parms;
		GOODS::iterator it = _goods.begin();
		for ( ;it != _goods.end(); ++ it)
		{
			goodsinfo entry_goods = it->second;
			int good_id_temp = entry_goods.info_.good_id();
			CDKEYS::iterator it_cd_key = entry_goods.cdkeys_.begin();
			for (; it_cd_key != entry_goods.cdkeys_.end(); ++ it_cd_key, i ++)
			{
				cd_key = it_cd_key->first;
				change_type = it_cd_key->second;
				if (i >= 30)
				{
					i = 0;
					gWorldDatabase.addSQueryTask(this, &girl_shop::dbCallNothing, excute_sql.c_str(), &parms, NULL, _SAVE_GOODS_);
					excute_sql.clear();

				}

				if (i == 0)
				{
					excute_sql += sql_head;
				}
				else
				{
					excute_sql += ",";
				}
				sprintf(sz_temp,"(\'%s\', %d, %d)", Utf8ToGBK(cd_key.c_str()).c_str(), good_id_temp, change_type);
				excute_sql += sz_temp;
			}
		}
		if (excute_sql.empty() == false)
		{
			gWorldDatabase.addSQueryTask(this, &girl_shop::dbCallNothing, excute_sql.c_str(), &parms, NULL, _SAVE_GOODS_);
		}

		sql_head = "insert into good_info(`name`, `description`, `good_id`, `price`) values";
		it = _goods.begin();
		for (int i = 0; it != _goods.end(); ++ it,  i++)
		{
			if (i > max_save_count)
			{
				gWorldDatabase.addSQueryTask(this, &girl_shop::dbCallNothing, excute_sql.c_str(), &parms, NULL, _SAVE_GOODS_);
				i = 0;
				excute_sql.clear();
			}
			if (i == 0)
			{
				excute_sql = sql_head;
			}
			else
			{
				excute_sql += ",";
			}
			goodsinfo entry_goods = it->second;
			sprintf(sz_temp, "(\'%s\', \'%s\', %d, %d)", 
				Utf8ToGBK(entry_goods.info_.name().c_str()).c_str(), Utf8ToGBK(entry_goods.info_.description().c_str()).c_str(), entry_goods.info_.good_id(), entry_goods.info_.price());
			excute_sql += sz_temp;
		}
		if (excute_sql.empty() == false)
		{
			gWorldDatabase.addSQueryTask(this, &girl_shop::dbCallNothing, excute_sql.c_str(), &parms, NULL, _SAVE_GOODS_);
		}

	}

}

void girl_shop::Load(DBQuery* p)
{
	DBQuery& query = *p;
	query.reset();
	query << "SELECT * FROM good_info;";
	query.parse();
	SDBResult result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		goodsinfo entry;
		entry.info_.set_good_id(row["good_id"]);
		entry.info_.set_price(row["price"]);
		entry.info_.set_name(GBKToUtf8(row["name"].c_str()));
		std::string des = GBKToUtf8(row["description"].c_str());
		entry.info_.set_description(des);
		_goods.insert(GOODS::value_type(entry.info_.good_id(), entry));
	}

	query.reset();
	result.clear();
	query << "SELECT * FROM good_cdkey;";
	query.parse();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		int good_id_temp = row["good_id"];
		std::string cdkey = GBKToUtf8(row["cdkey"].c_str());
		if (have_cdkey(cdkey.c_str()))
		{
			continue;
		}
		int type_exchange = row["exchanged"];
		
		GOODS::iterator it = _goods.find(good_id_temp);
		if (it != _goods.end())
		{
			if (type_exchange == 0)
			{
				it->second.cdkeys_[cdkey] = exchange_type_no;
			}
			else
			{
				it->second.cdkeys_[cdkey] = exchange_type_exchanged;

			}			
		}

	}


	query.reset();
	result.clear();
	query << "SELECT * FROM act_config;";
	query.parse();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		_action[row["action_type"]] = row["action_scores"];
	}

}