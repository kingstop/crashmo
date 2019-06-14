#pragma once
#include "udpsession.h"
#include "google/protobuf/message.h"
#include "protoc_common.h"
class test_udp_session :
	public udp_session, public ProtocMsgBase<test_udp_session>
{
public:
	test_udp_session();
public:
	virtual void _proc_message(const message_t& msg);
	virtual void proc_message(const message_t& msg);
	static void registerPBCall();
	void parseLogin(google::protobuf::Message* p, pb_flag_type flag);
	void parseGameMsg(google::protobuf::Message* p, pb_flag_type flag);

};

