#include "stdafx.h"
#include "CrashPlayerManager.h"
#include "CrashPlayer.h"


CrashPlayerManager::CrashPlayerManager(void)
{
}


CrashPlayerManager::~CrashPlayerManager(void)
{
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
	}
	return player;

}

void CrashPlayerManager::DestroyPlayer(CrashPlayer* p)
{
	CRASHPLAYERS::iterator it = _players.find(p->getAccount());
	if (it != _players.end())
	{
		_players.erase(it);
	}
	delete p;
}

CrashPlayer* CrashPlayerManager::CreatePlayer(account_type acc, Session* session)
{
	CrashPlayer* player = NULL;
	player = GetPlayer(acc);
	if (player == NULL)
	{

		player = new CrashPlayer(session);
		player->LoadConfig();
		
		//player->SetInfo(info);
	}
	return player;
}

