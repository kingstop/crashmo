#pragma once
#include "udpsession.h"
class test_udp_session :
	public udp_session
{
public:
	virtual void _proc_message(const message_t& msg);
	virtual void proc_message(const message_t& msg);
};

