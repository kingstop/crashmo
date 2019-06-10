#include "test_udp_session.h"
#include "protoc_common.h"
void test_udp_session::_proc_message(const message_t& msg)
{

}
void test_udp_session::proc_message(const message_t& msg)
{

}


//这里负责注册消息
void test_udp_session::registerPBCall()
{

	//registerCBFun(PROTOCO_NAME(message::MsgSaveMapReq), &Session::parseSaveMap);
	


}