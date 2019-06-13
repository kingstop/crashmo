#include "pch.h"
#include "test_udp_client.h"


test_udp_client::test_udp_client():ProtocMsgBase(this)
{
}


test_udp_client::~test_udp_client()
{
}
void test_udp_client::_proc_message(const message_t& msg)
{
	proc_message(msg);
}
void test_udp_client::proc_message(const message_t& msg)
{

}
void test_udp_client::parseGameMsg(google::protobuf::Message* p, pb_flag_type flag)
{

}

void test_udp_client::initPBModule()
{
	ProtocMsgBase<test_udp_client>::registerSDFun(&test_udp_client::send_message, &test_udp_client::parseGameMsg);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgServerRegister), &test_udp_client::parseGameRegister);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgGS2GTOnlines), &test_udp_client::parseGameOnlines);
	//ProtocMsgBase<test_udp_client>::registerCBFun(PROTOCO_NAME(message::MsgDB2GTChangeGS), &test_udp_client::parseChangeGS);

}