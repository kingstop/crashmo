#include "pch.h"
#include "test_udp_client.h"
#include "login.pb.h"


test_udp_client::test_udp_client(): ProtocMsgBase<test_udp_client>(this)
{
	_send_count = 0;
	_send_max_count = 10 + rand() % 500;
}


test_udp_client::~test_udp_client()
{
}

void test_udp_client::on_close()
{

}

void test_udp_client::on_connect()
{
	udp_client::on_connect();
	set_reconnect(true);
	message::LoginRequest msg;
	msg.set_name("12345");
	msg.set_pwd("54321");
	sendPBMessage(&msg, 0);
}

void test_udp_client::_proc_message(const message_t& msg)
{
	proc_message(msg);
}

void test_udp_client::proc_message(const message_t& msg)
{
	parsePBMessage(msg.data, msg.len, msg.base64);
}

void test_udp_client::parseGameMsg(google::protobuf::Message* p, pb_flag_type flag)
{

}

void test_udp_client::parseLogin(google::protobuf::Message* p, pb_flag_type flag)
{
	message::LoginRequest* msg = dynamic_cast<message::LoginRequest*> (p);
	_send_count++;
	sendPBMessage(msg);
	if (_send_count >= _send_max_count)
	{
		_send_count = 0;

		close();
	}
}

void test_udp_client::initPBModule()
{
	ProtocMsgBase<test_udp_client>::registerSDFun(&test_udp_client::send_message, &test_udp_client::parseGameMsg);
	registerCBFun(PROTOCO_NAME(message::LoginRequest), &test_udp_client::parseLogin);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgServerRegister), &test_udp_client::parseGameRegister);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgGS2GTOnlines), &test_udp_client::parseGameOnlines);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgDB2GTChangeGS), &test_udp_client::parseChangeGS);

}