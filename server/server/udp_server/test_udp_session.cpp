#include "test_udp_session.h"
#include "protoc_common.h"
#include "login.pb.h"

test_udp_session::test_udp_session():ProtocMsgBase<test_udp_session>(this)
{

}

void test_udp_session::on_connect()
{
	udp_session::on_connect();
	message::LoginRequest msg;
	msg.set_name("54321");
	msg.set_pwd("12345");
	sendPBMessage(&msg);
}

void test_udp_session::_proc_message(const message_t& msg)
{
	proc_message(msg);
}
void test_udp_session::proc_message(const message_t& msg)
{
	parsePBMessage(msg.data, msg.len, msg.base64);
}

void test_udp_session::parseLogin(google::protobuf::Message* p, pb_flag_type flag)
{
	message::LoginRequest* msg = dynamic_cast<message::LoginRequest*> (p);	
	sendPBMessage(msg);
}
//这里负责注册消息
void test_udp_session::registerPBCall()
{
	registerSDFun(&test_udp_session::send_message, &test_udp_session::parseGameMsg);
	registerCBFun(PROTOCO_NAME(message::LoginRequest), &test_udp_session::parseLogin);	
}

void test_udp_session::parseGameMsg(google::protobuf::Message* p, pb_flag_type flag)
{

}