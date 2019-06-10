#include "test_udp_session.h"
#include "protoc_common.h"
#include "login.pb.h"

void test_udp_session::_proc_message(const message_t& msg)
{

}
void test_udp_session::proc_message(const message_t& msg)
{

}

void test_udp_session::parseLogin(google::protobuf::Message* p, pb_flag_type flag)
{

}
//这里负责注册消息
void test_udp_session::registerPBCall()
{

	registerCBFun(PROTOCO_NAME(message::LoginRequest), &test_udp_session::parseLogin);
	


}