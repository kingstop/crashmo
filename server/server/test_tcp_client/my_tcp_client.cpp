#include "my_tcp_client.h"
#include "login.pb.h"


my_tcp_client::my_tcp_client():tcp_client(*net_global::get_io_service()), ProtocMsgBase<my_tcp_client>(this)
{

}

my_tcp_client::~my_tcp_client()
{

}


void my_tcp_client::parseGameMsg(google::protobuf::Message* p, pb_flag_type flag)
{

}

void my_tcp_client::parseLoginGame(google::protobuf::Message* p, pb_flag_type flag)
{

}

void my_tcp_client::on_connect()
{
	tcp_client::on_connect();
	message::LoginRequest msg;
	sendPBMessage(&msg, 0);
}

void my_tcp_client::initPBModule()
{
	ProtocMsgBase<my_tcp_client>::registerSDFun(&my_tcp_client::send_message, &my_tcp_client::parseLoginGame);
	ProtocMsgBase<my_tcp_client>::registerCBFun(PROTOCO_NAME(message::LoginRequest), &my_tcp_client::parseGameMsg);
}

void my_tcp_client::on_connect_failed(boost::system::error_code error)
{
	//tcp_client::on_connect_failed(error);
}

void my_tcp_client::proc_message(const message_t& msg)
{
	//tcp_client::proc_message(msg);
}