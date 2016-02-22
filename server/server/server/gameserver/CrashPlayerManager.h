#pragma once
class CrashPlayer;

typedef std::map<account_type, CrashPlayer*> CRASHPLAYERS;

class CrashPlayerManager
{
public:
	CrashPlayerManager(void);
	virtual ~CrashPlayerManager(void);
	CrashPlayer* CreatePlayer(account_type acc, Session* session);
	CrashPlayer* CreatePlayer(account_type acc, Session* session, message::CharacterDataACK* info);
	CrashPlayer* GetPlayer(account_type acc);
	u64 GenerateMapIndex();
protected:
	CRASHPLAYERS _players;
	u64 _map_index;
};

