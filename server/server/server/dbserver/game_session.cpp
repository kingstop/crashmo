#include "stdafx.h"
#include "game_session.h"
#include "message/msg_game_db.pb.h"

GameSession::GameSession(): tcp_session( *net_global::get_io_service() )
{
	_proto_user_ptr = this;
    m_game_id = INVALID_GAME_ID;
}

GameSession::~GameSession()
{

}

void GameSession::parseGameMsg(google::protobuf::Message* p, pb_flag_type flag)
{

}

void GameSession::parseGameRegister(google::protobuf::Message* p, pb_flag_type flag)
{
	message::MsgServerRegister* msg = static_cast<message::MsgServerRegister*>(p);
	if (msg)
	{
        const ::message::ServerVersion& v = msg->version();
        m_game_id = msg->serverid();
        if (!gDBVersion.checkVersion(v.major(), v.minor(), v.build(), v.appsvn()))
        {
            Mylog::log_server(LOG_ERROR,"register Game [%u] error, version is too old! ",m_game_id);
            close();
        }
		gDBGameManager.addGame(m_game_id, this);
		Mylog::log_server(LOG_INFO,"register Game [%u] ", m_game_id);
	}
}

void GameSession::parseApplyCharacterDataReq(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ApplyCharacterDataReq* msg = (message::ApplyCharacterDataReq*) p;
	gDBQuestMgr.queryInfo(msg->account(), flag, m_game_id);
}


void GameSession::parseSaveOfficilMapReq(google::protobuf::Message* p, pb_flag_type flag)
{
	message::gs2dbSaveOfficileMapReq* msg = (message::gs2dbSaveOfficileMapReq*)p;
	gDBQuestMgr.saveOfficilMap(msg);
}

void GameSession::parseWorldDatabaseSql(google::protobuf::Message* p, pb_flag_type flag)
{
	message::gs2dbWorldDatabaseSql* msg = (message::gs2dbWorldDatabaseSql*)p;
	gDBQuestMgr.worldDatabaseSql(msg->sql().c_str());
}

void GameSession::parseReqSaveCharacterData(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ReqSaveCharacterData* msg  = (message::ReqSaveCharacterData*) p;
	gDBQuestMgr.saveCharacterInfo(msg);
}

void GameSession::parseDelOfficilMapReq(google::protobuf::Message* p, pb_flag_type flag)
{
	message::gs2dbDelOfficileReq* msg = (message::gs2dbDelOfficileReq*)p;
	gDBQuestMgr.delOfficilMap(msg->section(), msg->number());
}

void GameSession::parseReqSaveCharacterDBSql(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ReqSaveCharacterDBSql* msg = (message::ReqSaveCharacterDBSql*)p;
	gDBQuestMgr.CharacterDatabaseSql(msg->sql().c_str());
}

void GameSession::initPBModule()
{
	ProtocMsgBase<GameSession>::registerSDFun(&GameSession::send_message, &GameSession::parseGameMsg);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::MsgServerRegister), &GameSession::parseGameRegister);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ApplyCharacterDataReq), &GameSession::parseApplyCharacterDataReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::gs2dbSaveOfficileMapReq), &GameSession::parseSaveOfficilMapReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::gs2dbDelOfficileReq), &GameSession::parseDelOfficilMapReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ReqSaveCharacterData), &GameSession::parseReqSaveCharacterData);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ReqSaveOfficilMap), &GameSession::parseSaveOfficilMapsReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ReqSaveOfficilSectionNames), &GameSession::parseSaveSectionNamesReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ReqSaveCharacterDBSql), &GameSession::parseReqSaveCharacterDBSql);
}


void GameSession::parseSaveOfficilMapsReq(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ReqSaveOfficilMap* msg = (message::ReqSaveOfficilMap*) p;
	gDBQuestMgr.worldDatabaseSql(msg->sql().c_str());

}

void GameSession::parseSaveSectionNamesReq(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ReqSaveOfficilSectionNames* msg = (message::ReqSaveOfficilSectionNames*)p;
	gDBQuestMgr.worldDatabaseSql(msg->sql().c_str());

}

void GameSession::proc_message( const message_t& msg )
{
	parsePBMessage(msg.data, msg.len, msg.base64);
}

void GameSession::on_close( const boost::system::error_code& error )
{
	gDBGameManager.removeGame(this);
	tcp_session::on_close(error);
	Mylog::log_server(LOG_INFO, "server game [%u] close", m_game_id);
}

