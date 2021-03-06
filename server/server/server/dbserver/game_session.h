#ifndef __game_session_h__
#define __game_session_h__

class GameSession : public tcp_session, public ProtocMsgBase<GameSession>
{
public:
	GameSession(void);
	~GameSession(void);
	static void initPBModule();
	void parseGameRegister(google::protobuf::Message* p, pb_flag_type flag);
	void parseGameMsg(google::protobuf::Message* p, pb_flag_type flag);
	void parseApplyCharacterDataReq(google::protobuf::Message* p, pb_flag_type flag);
	void parseReqSaveCharacterData(google::protobuf::Message* p, pb_flag_type flag);
	void saveMap(google::protobuf::Message* p, pb_flag_type flag);
	void parseDelOfficilMapReq(google::protobuf::Message* p, pb_flag_type flag);
	void parseSaveOfficilMapsReq(google::protobuf::Message* p, pb_flag_type flag);
	void parseSaveSectionNamesReq(google::protobuf::Message* p, pb_flag_type flag);
	void parseWorldDatabaseSql(google::protobuf::Message* p, pb_flag_type flag);
	void parseReqSaveCharacterDBSql(google::protobuf::Message* p, pb_flag_type flag);
	u16  getGameId() const{return m_game_id;}
public:
	virtual void proc_message( const message_t& msg );
	virtual void on_close( const boost::system::error_code& error );
private:
	u16 m_game_id;
};

#endif

