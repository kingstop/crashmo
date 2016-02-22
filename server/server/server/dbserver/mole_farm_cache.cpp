#include "stdafx.h"
/*
#include "mole_farm_cache.h"


mole_farm_cache::mole_farm_cache(void)
{
}


mole_farm_cache::~mole_farm_cache(void)
{
}



void mole_farm_cache::removeOnline(tran_id_type t)
{ 
	m_onlineGuid.eraseData(t);
}

void mole_farm_cache::addOnline(tran_id_type t, const char* guid)
{ 
	m_onlineGuid.addData(t, guid);
}

std::string mole_farm_cache::getGuid(tran_id_type t)
{
	std::string guid = INVALID_MOLE_FARM_ID; 
	m_onlineGuid.getData(t, guid); 
	return guid;
}


mole_farm_data* mole_farm_cache::addCacheData(const message::MoleFarmInfoFull* p, u16 gs)
{
	std::string guid = p->base_info().guid();
	u64 a = p->extra_info().account();
	mole_farm_data* pc = m_FarmCache.getData(a);
	if (pc)
	{
		// do nothing . cache is the new data ..
	}
	else
	{
		pc = new mole_farm_data(p, gs);
		//pc = new PlayerData(p, gs);
		m_FarmCache.addData(a, pc);
		m_GuidFarms.addData(guid, pc);
		
	}
	//m_NameCache.insert(p->sc_data().name());
	return pc;
}


mole_farm_data* mole_farm_cache::getCacheData(account_type a)
{
	return m_FarmCache.getData(a);
}

mole_farm_data* mole_farm_cache::getCacheDataByTrans(tran_id_type t)
{
	return m_GuidFarms.getData(getGuid(t));
}

void mole_farm_cache::updateFarm(const message::MoleFarmInfoFull* p, u16& gs)
{
	mole_farm_data* p_data = getCacheData(p->extra_info().account());
	if (p_data != NULL)
	{
		p_data->set(&p->base_info());
	}
	else
	{
		addCacheData(p, gs);
	}
	gDBQuestMgr.saveFarm(p->extra_info().account());
}

bool mole_farm_cache::getTransAndgs(const char* guid, tran_id_type& t, u16& gs)
{
	t = INVALID_TRANS_ID;
	if(m_onlineGuid.getKey(t, guid))
	{
		mole_farm_data* pkData = m_GuidFarms.getData(guid);
		if (pkData)
		{
			gs = pkData->gs_;
			return true;
		}
	}
	return false;
}
*/
