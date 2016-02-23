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
	void SaveMap(message::MsgSaveMapReq* msg);
	void DelMap(message::MsgDelMapReq* msg);
	void sendPBMessage(google::protobuf::Message* p);
protected:
	bool havemapname(const char* mapname);

protected:
	Session* _session;
	message::CrashPlayerInfo _info;
};

