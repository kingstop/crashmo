#pragma once
/* 
#include "mole_farm_data.h"
#include "message/mole_common_server.pb.h"



class mole_farm_cache
{
public:
	mole_farm_cache(void);
	virtual ~mole_farm_cache(void);
	void removeOnline(tran_id_type t);
	void addOnline(tran_id_type t, const char* guid);
	std::string getGuid(tran_id_type id);

	mole_farm_data* addCacheData(const message::MoleFarmInfoFull* p, u16 gs);
	mole_farm_data* getCacheData(account_type a);
	mole_farm_data* getCacheDataByTrans(tran_id_type t);
	void updateFarm(const message::MoleFarmInfoFull* p, u16& gs);
	bool getTransAndgs(const char* guid, tran_id_type& t, u16& gs);

protected:
	obj_ptr_map<account_type, mole_farm_data> m_FarmCache;	// key account
	obj_ptr_map<std::string, mole_farm_data> m_GuidFarms;				// key guid 
	obj_map<tran_id_type, std::string> m_onlineGuid;				// 动态ID 和玩家GUID对应表
};
*/
