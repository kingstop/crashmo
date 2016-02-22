#include "stdafx.h"
/*  
#include "player_mole_farm_manager.h"
#include "Player_mole_farm.h"


player_mole_farm_manager::player_mole_farm_manager(void)
{
}


player_mole_farm_manager::~player_mole_farm_manager(void)
{
}


Player_mole_farm* player_mole_farm_manager::get_player_mole_farm(const char* guid)
{
	return _player_mole_farm_map.getData(guid);
}
Player_mole_farm* player_mole_farm_manager::get_player_mole_farm(account_type a)
{
	std::string guid = INVALID_MOLE_FARM_ID;
	if (_account_map.getData(a, guid))
	{
		return get_player_mole_farm(guid.c_str());
	}
	return NULL;
}

Player_mole_farm* player_mole_farm_manager::create_player_mole_farm(Session* s, const message::MoleFarmInfoFull * p)
{

	if (!s || !p )
	{   return NULL ;}

	std::string guid =  p->base_info().guid();
	Player_mole_farm* mole_farm = get_player_mole_farm(guid.c_str());
	if (NULL == mole_farm)
	{
		mole_farm = Memory::createObject<Player_mole_farm, Session*, const message::MoleFarmInfoFull*>(s, p);
		_player_mole_farm_map.addData(guid, mole_farm);
		_account_map.addData(p->extra_info().account(), guid);
	}else
	{
		// assert(false);
		mole_farm->setReconnet(s);
		Mylog::log_server(LOG_ERROR, "mole_farm [%s] memory error .......", guid.c_str());
	}
	if (mole_farm == NULL)
	{
		Mylog::log_server(LOG_ERROR, "mole_farm == NULL [%s]", guid.c_str());
	}
	return mole_farm;
}
*/
