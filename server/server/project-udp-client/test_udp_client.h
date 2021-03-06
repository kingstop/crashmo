#pragma once
#include "udp_client.h"
#include "protoc_common.h"
#include "google/protobuf/message.h"

class test_udp_client :
	public udp_client, public ProtocMsgBase<test_udp_client>
{
public:
	test_udp_client();
	virtual ~test_udp_client();
	virtual void on_connect();
	virtual void on_close();
	virtual void _proc_message(const message_t& msg);
	virtual void proc_message(const message_t& msg);
	void parseGameMsg(google::protobuf::Message* p, pb_flag_type flag);	
	void parseLogin(google::protobuf::Message* p, pb_flag_type flag);
	static void initPBModule();
private:
	int _send_count;
	int _send_max_count;
};

