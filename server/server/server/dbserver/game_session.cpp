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

void GameSession::initPBModule()
{
	ProtocMsgBase<GameSession>::registerSDFun(&GameSession::send_message, &GameSession::parseGameMsg);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::MsgServerRegister), &GameSession::parseGameRegister);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ApplyCharacterDataReq), &GameSession::parseApplyCharacterDataReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::gs2dbSaveOfficileMapReq), &GameSession::parseSaveOfficilMapReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::gs2dbDelOfficileReq), &GameSession::parseDelOfficilMapReq);
	ProtocMsgBase<GameSession>::registerCBFun(PROTOCO_NAME(message::ReqSaveCharacterData), &GameSession::parseReqSaveCharacterData);
}

void GameSession::proc_message( const message_t& msg )
{
	parsePBMessage(msg.data, msg.len);
}

void GameSession::on_close( const boost::system::error_code& error )
{
	gDBGameManager.removeGame(this);
	tcp_session::on_close(error);
	Mylog::log_server(LOG_INFO, "server game [%u] close", m_game_id);
}

