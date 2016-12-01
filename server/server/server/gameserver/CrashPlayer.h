#pragma once
class Session;
class CrashPlayer : public EventableObject
{
public:
	CrashPlayer(Session* session);
	CrashPlayer(Session* session, account_type acc);
	virtual ~CrashPlayer(void);
public:
	void SetInfo(message::CrashPlayerInfo info);	
	message::CrashPlayerInfo GetInfo();
	void SetSession(Session* session);
	Session* GetSession();
	void SaveCrashInfo();
	void StartSave();
	void StopSave();
	void StartDeleteClock();
	void StopDeleteClock();
	void SaveMap(message::MsgSaveMapReq* msg);
	void DelMap(message::MsgDelMapReq* msg);
	void sendPBMessage(google::protobuf::Message* p);
	void LoadConfig();
	account_type getAccount();
protected:
	bool havemapname(const char* mapname);
	void Destroy();

protected:
	Session* _session;
	message::CrashPlayerInfo _info;
};

