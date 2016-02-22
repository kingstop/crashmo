#pragma once
#include <map>
#include <vector>
#include <string>
#include "message/girls.pb.h"
enum exchange_type
{
	exchange_type_no,
	exchange_type_exchanged

};

typedef std::map<std::string, exchange_type> CDKEYS;
typedef std::map<int, int> ACTIONSCORS;
struct goodsinfo
{
	message::goodinfo info_;
	CDKEYS cdkeys_;		
	void get(message::GoodsCDKEYInfo* info)
	{
		info->mutable_info()->CopyFrom(info_);
		info->clear_cdkeys();
		CDKEYS::iterator it = cdkeys_.begin();
		for (; it != cdkeys_.end(); ++ it)
		{
			message::string_bool_pair* info_pair = info->add_cdkeys();
			std::string key_temp = it->first;
			bool exchanged = (bool)it->second;
			info_pair->set_key(key_temp.c_str());
			info_pair->set_exchanged(exchanged);
		}
	}
};
typedef std::map<int,goodsinfo> GOODS;
class girl_shop
{
public:
	girl_shop(void);
	~girl_shop(void);

public:
	void Load(DBQuery* p);
	int get_action_score(int act);
	bool have_cdkey(const char* cd_key);
	void save_goods();
	void dbCallNothing(const SDBResult* , const void* d, bool s);
	void addgoods(Session* p, message::AddGoodsReq* msg);
	std::string get_new_cd_key();
	std::string create_cd_key();
	GOODS& get_goods();
public: 
	GOODS _goods;
	std::vector<std::string> _all_ckeys;
	ACTIONSCORS _action;
};

