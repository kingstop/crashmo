#ifndef __player_session_h__
#define __player_session_h__
class CrashPlayer;
class Session :PUBLIC_BASE_OBJECT(Session)
{
	REGISTER_POOL_INFO(Session, 100, 0)
public:
	enum
	{
		_session_online_,
		_session_offline_,
	};
	void prasePBDefault(google::protobuf::Message* p);
	static void registerPBCall();
public:
	void queryDBPlayerInfo();
public:
	Session(tran_id_type t, account_type a, u16 gate);
	~Session();
	void close();
	void setReconnet();
	void setWaitReconnet();
	u16  getGateId() const {return m_gate ;}
	tran_id_type getTranId() const {return m_tranid ;}
	account_type getAccount() const {return m_account ;}
	void sendPBMessage(google::protobuf::Message* p);
	void parsePBMessage(google::protobuf::Message* p);
	void createInfo();
	void createInfo(message::CharacterDataACK* msg);
	void parseUpdateInstanceStatusReq(google::protobuf::Message* p);
	void parseAddToyReq(google::protobuf::Message* p);
	void parseInstancePassReq(google::protobuf::Message* p);
	//Player* getPlayer(){return m_pkOwer;}
protected:
	tran_id_type m_tranid;
	account_type m_account;
	u16 m_gate;
	u8  m_state;
	CrashPlayer * _player;
	//NoneCharacter* _character;
	//girl* _girl;
	// Player* m_pkOwer;
private:
};
#endif
