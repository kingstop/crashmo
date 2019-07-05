#pragma once
#include "tcpclient.h"
#include "protoc_common.h"
class my_tcp_client :
	public tcp_client , public ProtocMsgBase<my_tcp_client>
{
public:
	my_tcp_client();
	~my_tcp_client();
	void parseGameMsg(google::protobuf::Message* p, pb_flag_type flag);
	void parseLoginGame(google::protobuf::Message* p, pb_flag_type flag);
	virtual void on_connect_failed(boost::system::error_code error);
	virtual void on_connect();
	virtual void proc_message(const message_t& msg);
	static void initPBModule();
};

