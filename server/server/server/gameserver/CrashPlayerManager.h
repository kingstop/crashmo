#pragma once
class CrashPlayer;

typedef std::map<account_type, CrashPlayer*> CRASHPLAYERS;

class CrashPlayerManager : public EventableObject
{
public:
	CrashPlayerManager(void);
	virtual ~CrashPlayerManager(void);
	void Init();
	CrashPlayer* CreatePlayer(account_type acc, Session* session);
	CrashPlayer* CreatePlayer(account_type acc, Session* session, message::CharacterDataACK* info);
	CrashPlayer* GetPlayer(account_type acc);
	void DestroyPlayer(CrashPlayer* p);
	u64 GenerateMapIndex();
	void DayUpdate();
	void EventPerMin();
protected:
	CRASHPLAYERS _players;
	u64 _map_index;
};

