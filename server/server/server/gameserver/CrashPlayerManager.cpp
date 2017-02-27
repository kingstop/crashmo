#include "stdafx.h"
#include "CrashPlayerManager.h"
#include "CrashPlayer.h"


CrashPlayerManager::CrashPlayerManager(void)
{
}


CrashPlayerManager::~CrashPlayerManager(void)
{
}


void CrashPlayerManager::Init()
{
	time_t server_time = g_server_start_time;
	tm* p1 = localtime(&server_time);

	if (gEventMgr.hasEvent(this, EVENT_PER_MIN) == false)
	{
		int next_min = (60 - p1->tm_sec) * _TIME_SECOND_MSEL_;
		gEventMgr.addEvent(this, &CrashPlayerManager::EventPerMin, EVENT_PER_MIN, next_min, -1, 0);
	}

}


void CrashPlayerManager::EventPerMin()
{
	time_t server_time = g_server_time;
	tm* p1 = localtime(&server_time);
	if (p1->tm_min == 0)
	{
		if (p1->tm_hour == gGameConfig.getMapConfig()->day_refrash_time_)
		{
			DayUpdate();
		}

	}
	if (gEventMgr.hasEvent(this, EVENT_PER_MIN) == true)
	{
		gEventMgr.removeEvents(this, EVENT_PER_MIN);
	}
	int next_min = (60 - p1->tm_sec) * _TIME_SECOND_MSEL_;
	gEventMgr.addEvent(this, &CrashPlayerManager::EventPerMin, EVENT_PER_MIN, next_min, -1, 0);
}
u64 CrashPlayerManager::GenerateMapIndex()
{
	return _map_index;
}

CrashPlayer* CrashPlayerManager::GetPlayer(account_type acc)
{
	CrashPlayer* player = NULL;
	CRASHPLAYERS::iterator it = _players.find(acc);
	if (it != _players.end())
	{
		player = it->second;
	}
	return player;
}

CrashPlayer* CrashPlayerManager::CreatePlayer(account_type acc, Session* session, message::CharacterDataACK* info)
{
	CrashPlayer* player = NULL;
	player = GetPlayer(acc);
	if (player == NULL)
	{
		message::CrashPlayerInfo info_temp;
		info_temp.CopyFrom(info->data());
		player = new CrashPlayer(session);
		player->SetInfo(info_temp);
		_players.insert(CRASHPLAYERS::value_type(player->getAccount(), player));
	}
	return player;

}

void CrashPlayerManager::DestroyPlayer(CrashPlayer* p)
{
	int length = _players.size();
	CRASHPLAYERS::iterator it = _players.find(p->getAccount());
	if (it != _players.end())
	{
		_players.erase(it);
	}
	delete p;
}

void CrashPlayerManager::DayUpdate()
{
	CRASHPLAYERS::iterator it = _players.begin();
	for (; it != _players.end(); ++ it)
	{
		CrashPlayer* player = it->second;
		if (player != NULL)
		{
			player->DayUpdate();
		}
	}

}

CrashPlayer* CrashPlayerManager::CreatePlayer(account_type acc, Session* session)
{
	CrashPlayer* player = NULL;
	player = GetPlayer(acc);
	if (player == NULL)
	{

		player = new CrashPlayer(session);
		player->LoadConfig();
		_players.insert(CRASHPLAYERS::value_type(player->getAccount(), player));
		//player->SetInfo(info);
	}
	return player;
}

