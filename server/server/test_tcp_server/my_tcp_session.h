#pragma once
#include <tcpsession.h>
#include "protoc_common.h"

class my_tcp_session :
	public tcp_session, public ProtocMsgBase<my_tcp_session>
{
public:
	my_tcp_session();
	virtual ~my_tcp_session();
public:
	virtual void proc_message(const message_t& msg);
	void parseLoginGame(google::protobuf::Message* p, pb_flag_type flag);
	static void initPBModule();
};

