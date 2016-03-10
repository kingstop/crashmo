#include "StdAfx.h"
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

CrashPlayer* CrashPlayerManager::CreatePlayer(account_type acc, Session* session)
{
	CrashPlayer* player = NULL;
	player = GetPlayer(acc);
	if (player == NULL)
	{
		message::CrashPlayerInfo info;
		info.set_name("name");
		info.set_pass_point(0);
		info.set_pass_section(0);
		info.set_account(acc);
		info.set_isadmin(true);
		player = new CrashPlayer(session);
		player->SetInfo(info);
	}
	return player;
}

