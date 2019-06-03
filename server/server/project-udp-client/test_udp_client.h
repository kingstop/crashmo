#pragma once
#include "udp_client.h"
#include "protoc_common.h"

class test_udp_client :
	public udp_client
{
public:
	test_udp_client();
	virtual ~test_udp_client();
	virtual void _proc_message(const message_t& msg);
	virtual void proc_message(const message_t& msg);
	void parseGameMsg(google::protobuf::Message* p, pb_flag_type flag);
	static void initPBModule();
};

