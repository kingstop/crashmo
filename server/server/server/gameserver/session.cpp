#include "stdafx.h"
#include "session.h"
#include "CrashPlayer.h"
//#include "NoneCharacter.h"
//#include "NoneCharacterManager.h"

/************************************************************************/

/*                          注册消息实例                                */  

/*  message::MsgDB2GSQueryCharResult 注册回调    prasePBTest             */

/************************************************************************/
typedef void(Session::*pn_msg_cb)(google::protobuf::Message*);
static std::map<std::string, pn_msg_cb > static_session_cb_funs;
static void registerCBFun(std::string str, pn_msg_cb fun)
{
    static_session_cb_funs.insert(std::make_pair(str, fun));
}

void Session::prasePBDefault(google::protobuf::Message* p)
{
     Mylog::log_server(LOG_INFO, "Parse message name [%s]", p->GetTypeName().c_str());
}


//这里负责注册消息
void Session::registerPBCall()
{
	registerCBFun(PROTOCO_NAME(message::MsgSaveMapReq), &Session::parseSaveMap);
}

void Session::parsePBMessage(google::protobuf::Message* p)
{
    std::map<std::string, pn_msg_cb >::iterator it = static_session_cb_funs.find(p->GetTypeName());
    if (it != static_session_cb_funs.end())
    {
        pn_msg_cb fun = boost::ref( it->second);
        if ( NULL != fun )
        {
            (this->*fun)(p);
            return ;
        }
    }
    prasePBDefault(p);
}

//////////////////////////////////////////////////////////////////////////

Session::Session(tran_id_type t, account_type a, u16 gate)
    :m_tranid(t), m_account(a), m_gate(gate),m_state(_session_online_)/*,_character(NULL)*/
{
	_player = gPlayerManager.GetPlayer(m_account);
	if (_player == NULL)
	{
		message::ApplyCharacterDataReq msg;
		msg.set_account(m_account);
		gGSDBClient.sendPBMessage(&msg, m_tranid);
	}
	else
	{
		_player->SetSession(this);

	}
}

Session::~Session()
{
	//if (_character!= NULL)
	//{
	//	_character->set_session(NULL);
	//}
	//gGSGirlsStorage.remove_online_girl(_girl);
  //  delete _girl;
}

void Session::queryDBPlayerInfo()
{

}

void Session::close()
{

}

void Session::setReconnet()
{
    m_state = _session_online_;
}

void Session::setWaitReconnet()
{
    m_state = _session_offline_;
}

void Session::sendPBMessage(google::protobuf::Message* p)
{
    if (m_state == _session_online_)
    {
	    gGSGateManager.sendMessage(p, m_tranid, m_gate);
    }
	else
    {

    }
}



void Session::parseSaveMap(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgSaveMapReq* msg = (message::MsgSaveMapReq*)p;
		if (msg->save_type() != message::OfficeMap)
		{

		}
	}
}

void Session::parseDelMap(google::protobuf::Message* p)
{
	if (_player != NULL)
	{
		message::MsgDelMapReq* msg = (message::MsgDelMapReq*)p;
		_player->DelMap(msg);
	}
}

void Session::createInfo()
{
	_player = gPlayerManager.CreatePlayer(m_account, this);
	message::CrashmoClientInit msg;
	message::CrashPlayerInfo* info = msg.mutable_info();
	info->CopyFrom(_player->GetInfo());
	sendPBMessage(&msg);
	_player->StartSave();
}

void Session::createInfo(message::CharacterDataACK* msg)
{
	_player = gPlayerManager.CreatePlayer(m_account, this, msg);
	message::CrashmoClientInit msg_to_client;
	message::CrashPlayerInfo* info = msg_to_client.mutable_info();
	info->CopyFrom(_player->GetInfo());
	sendPBMessage(&msg_to_client);
	_player->StartSave();

}


