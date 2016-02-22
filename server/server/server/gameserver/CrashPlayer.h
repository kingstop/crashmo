#pragma once
class Session;
class CrashPlayer : public EventableObject
{
public:
	CrashPlayer(Session* session);
	virtual ~CrashPlayer(void);
public:
	void SetInfo(message::CrashPlayerInfo info);
	message::CrashPlayerInfo GetInfo();
	void SetSession(Session* session);
	Session* GetSession();
	void SaveCrashInfo();
	void StartSave();
	void StopSave();	
protected:
	Session* _session;
	message::CrashPlayerInfo _info;
};

