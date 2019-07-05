#include "my_tcp_session.h"
#include "login.pb.h"


void my_tcp_session::initPBModule()
{
	ProtocMsgBase<my_tcp_session>::registerSDFun(&my_tcp_session::send_message, NULL);
	ProtocMsgBase<my_tcp_session>::registerCBFun(PROTOCO_NAME(message::LoginRequest), &my_tcp_session::parseLoginGame);
	
}

my_tcp_session::my_tcp_session():tcp_session(*net_global::get_io_service()), ProtocMsgBase<my_tcp_session>(this)
{

}
my_tcp_session::~my_tcp_session()
{

}

void my_tcp_session::proc_message(const message_t& msg)
{
	parsePBMessage(msg.data, msg.len, msg.base64);
}

void my_tcp_session::parseLoginGame(google::protobuf::Message* p, pb_flag_type flag)
{
	message::LoginRequest* msg = dynamic_cast<message::LoginRequest*>(p);
	if (msg)
	{
		sendPBMessage(msg, 0);
	}
}