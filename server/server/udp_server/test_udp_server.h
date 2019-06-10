#pragma once
#include "udp_server.h"
class test_udp_server :
	public udp_server
{
public:
	test_udp_server(int id);
	virtual ~test_udp_server();
	virtual base_session* create_session();
};

