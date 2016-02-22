#include "stdafx.h"
#include "client.h"
#include "dlg_reservation.h"
#include "dlg_shop.h"
Client::Client(const net_info& info)
:tcp_client(*net_global::get_io_service()),m_info(info), m_client_state(_client_init_), m_transid(INVALID_TRANS_ID), m_MoveStateEnable(false)
{
    _proto_user_ptr = this;// connect(m_info._ip.c_str(), m_info._port);
	_first_login = true;
}
Client::~Client()
{
}
void Client::on_close( const boost::system::error_code& error )
{
	tcp_client::on_close(error);
	switch(m_client_state)
	{
	case _client_conn_login_:
		{
			m_client_state = _client_wait_gate_;
			connect(m_info._ip.c_str(), m_info._port);
		}break;   
	}
}

void Client::on_connect_failed( boost::system::error_code error )
{

}
void Client::update(u32 nDiff)
{
	run_no_wait();
}
bool Client::reConnect()
{
   connect(m_info._ip.c_str(), m_info._port);
   return true;
}
void Client::on_connect()
{
	tcp_client::on_connect();
	switch(m_client_state)
	{
	case _client_init_:
	case _client_wait_login_:
		{
			m_client_state = _client_conn_login_;
			message::LoginRequest msg;                      
			msg.set_name(_account.c_str());
			msg.set_pwd(_password.c_str());
			sendPBMessage(&msg);
		}
		break;
	case _client_wait_gate_:
		{
			m_client_state = _client_connet_gate_;
			message::LoginGame msg;
			msg.set_user_account(m_transid);
			sendPBMessage(&msg);
		}break;
	default:
		break;    
	}
}

void Client::set_account(const char* account)
{
	_account = account;
}
void Client::set_password(const char* password)
{
	_password = password;
}

void Client::modify_pass_word(const char* card, const char* pw)
{
	message::ModifyPasswordReq msg;
	msg.set_card(card);
	msg.set_password(pw);
	sendPBMessage(&msg);
}

void Client::modify_performacine(account_type account, int performance)
{
	message::ModifyPerformanceReq msg;
	msg.set_account(account);
	msg.set_modify_performance(performance);
	sendPBMessage(&msg);
}
void Client::initPBModule()
{
    ProtocMsgBase<Client>::registerSDFun(&Client::send_message, NULL);
    ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::LoginResponse), &Client::parseLoginResult);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::GirlInit), &Client::parseGirlInit);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::GirlListACK), &Client::parseGirlList);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::AdviceListACK), &Client::parseAdviceList);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::GirlsReservationInfoListACK), &Client::parseGirlsReservationsList);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::ModfiyPerformanceACK), &Client::parseModfiyPerformanceACK);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::CreateAccountACK), &Client::parseCreateAccount);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::notifyAddGirlInfo), &Client::parseNotifyAddGirl);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::ModifyPasswordACK), &Client::parseModifyPasswordACK);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::NotifyReservationStateModify),&Client::parseReceiveReservationACK);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::ModifyNewsACK), &Client::parseModifyNewsACK);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::AddGoodsACK), &Client::parseAddGoodsACK);
	ProtocMsgBase<Client>::registerCBFun(PROTOCO_NAME(message::GoodsCDKEYInfoListACK), &Client::parseGoodsCDKEYInfoListACK);
}
void Client::parseClientChar(google::protobuf::Message* p, pb_flag_type flag)
{

}

void Client::parseModifyNewsACK(google::protobuf::Message* p, pb_flag_type flag)
{
	g_dlg->set_title("发布新闻成功");
}

void Client::parseGoodsCDKEYInfoListACK(google::protobuf::Message* p, pb_flag_type flag)
{
	message::GoodsCDKEYInfoListACK* msg = (message::GoodsCDKEYInfoListACK*)(p);
	if (msg->end()&&_first_login )
	{
		g_girls_manager.add_goods_cdkey(msg);
		g_dlg->_dlg_login->ShowWindow(FALSE);
		g_dlg->update_tab_status();
		_first_login = false;
		
	}
}

void Client::parseAddGoodsACK(google::protobuf::Message* p, pb_flag_type flag)
{
	message::AddGoodsACK* msg = (message::AddGoodsACK*)(p);
	g_girls_manager.add_goods_cdkey(msg);
	g_dlg->_dlg_shop->enable(true);
}

void Client::parseReceiveReservationACK(google::protobuf::Message* p, pb_flag_type flag)
{
	g_dlg->set_title("接收订单成功");
	message::NotifyReservationStateModify* msg = (message::NotifyReservationStateModify*)p;
	message::GirlsReservationInfo* ret = g_girls_manager.get_reservation(msg->account(), msg->send_time());
	ret->mutable_info()->set_receive_time(msg->receive_time());
	g_dlg->_dlg_reservation->update_edits();
	g_dlg->_dlg_reservation->enable(true);
}

void Client::parseGirlInit(google::protobuf::Message* p, pb_flag_type flag)
{
	message::GirlInit* int_msg = (message::GirlInit*)p;
	if (int_msg->info().gm_flag() == false)
	{
		g_dlg->_dlg_login->set_notify("账号权限不足..");
		g_dlg->_dlg_login->enable(true);
	}
	else
	{
		message::GirlListReq msg;
		g_dlg->_dlg_login->set_notify("载入女孩信息中..");
		sendPBMessage(&msg);
	}

}

const char* Client::get_ip()
{
	return m_info._ip.c_str();
}
void Client::set_ip(const char* ip)
{
	m_info._ip = ip;
}

void Client::send_reservation_req()
{
	message::GirlsReservationInfoListReq msg_entry;
	sendPBMessage(&msg_entry);
}

void Client::parseAdviceList(google::protobuf::Message* p, pb_flag_type flag)
{
	message::AdviceListACK* msg = (message::AdviceListACK*)p;
	if (msg)
	{
		g_girls_manager.add_advice(msg);
	}
	g_dlg->_dlg_login->set_notify("载入预约信息中..");
	if (msg->end())
	{
		send_reservation_req();
	}
}

void Client::receive_reservation(account_type account, u64 send_time)
{
	message::ReservationReceivedReq msg;
	msg.set_account(account);
	msg.set_send_time(send_time);
	sendPBMessage(&msg);
}

void Client::send_news(const char* content, int repeated_count, int interval_time)
{
	message::ModifyNewsReq msg;
	msg.set_news_title(content);
	msg.set_repeated_count(repeated_count);
	msg.set_interval_time(interval_time);
	sendPBMessage(&msg);
}

void Client::add_goods_cdkey(int goods_id, int goods_price, int goods_count, const char* name, const char* descrip)
{
	message::AddGoodsReq msg;
	msg.set_good_count(goods_count);
	msg.mutable_info()->set_name(name);
	msg.mutable_info()->set_price(goods_price);
	msg.mutable_info()->set_description(descrip);
	msg.mutable_info()->set_good_id(goods_id);
	sendPBMessage(&msg);
}

void Client::parseModifyPasswordACK(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ModifyPasswordACK* msg = (message::ModifyPasswordACK*)p;
	if (msg->sucessful())
	{
		g_dlg->set_title("修改密码成功");
	}
	else
	{
		g_dlg->set_title("修改密码失败");
	}
	
}

void Client::parseModifyPerformanceACK(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ModfiyPerformanceACK* msg = (message::ModfiyPerformanceACK*)p;
	girl_info* entry_info = g_girls_manager.get_girl(msg->account());
	if (entry_info != NULL)
	{
		entry_info->performance_ = msg->current_performance();
		g_dlg->_dlg_girls->update_edit();
		g_dlg->_dlg_girls->enable(true);

	}
}

void Client::parseGirlsReservationsList(google::protobuf::Message* p, pb_flag_type flag)
{
	message::GirlsReservationInfoListACK* msg = (message::GirlsReservationInfoListACK*)p;
	if (msg)
	{
		g_girls_manager.add_girls_reservation_info_list_ACK(msg);
		if (_first_login&&msg->end())
		{
			message::GoodsCDKEYInfoListReq goods_cdkey_msg;
			sendPBMessage(&goods_cdkey_msg);
		}
		
	}

}

void Client::parseLoginResult(google::protobuf::Message* p, pb_flag_type flag)
{
    message::LoginResponse* msg = static_cast<message::LoginResponse*>(p);
    if (msg)
    {
		if (msg->result() == message::enumLoginResult_Success)
		{
			m_transid = msg->user_account();
			m_info._ip = msg->gate_ip();
			m_info._port = msg->gate_port();
			close();
		}
		else
		{
			g_dlg->_dlg_login->enable(true);
		}
    }
}
void Client::parseGirlList(google::protobuf::Message* p, pb_flag_type flag)
{
	message::GirlListACK* msg = static_cast<message::GirlListACK*>(p);
	if (msg)
	{
		g_girls_manager.load_girls(msg);
	}
	if (msg->end())
	{
		message::AdviceListReq msg_entry;
		g_dlg->_dlg_login->set_notify("载入建议信息中..");
		sendPBMessage(&msg_entry);
	}
	
}
void Client::parseModfiyPerformanceACK(google::protobuf::Message* p, pb_flag_type flag)
{
	message::ModfiyPerformanceACK* msg = static_cast<message::ModfiyPerformanceACK*>(p);
	if (msg)
	{
		girl_info* info = g_girls_manager.get_girl(msg->account());
		info->performance_ = msg->current_performance();
		g_dlg->_dlg_girls->update_edit();
		g_dlg->_dlg_girls->enable(true);
	}
}
void Client::parseCreateAccount(google::protobuf::Message* p, pb_flag_type flag)
{
	message::CreateAccountACK* msg = static_cast<message::CreateAccountACK*>(p);
	if (msg)
	{
		g_dlg->create_account_sucessful(msg->card().c_str(),msg->password().c_str());
	}
	g_dlg->_dlg_girls->enable(true);
}

void Client::parseNotifyAddGirl(google::protobuf::Message* p, pb_flag_type flag)
{
	message::notifyAddGirlInfo* msg = static_cast<message::notifyAddGirlInfo*>(p);
	if (msg)
	{
		g_girls_manager.add_girl_info(msg);
	}
}

void Client::parseClientInit(google::protobuf::Message* p, pb_flag_type flag)
{

}

void Client::moveSend()
{

}

void Client::moveRand()
{

}

void Client::proc_message( const message_t& msg )
{
    parsePBMessage(msg.data, msg.len);
}