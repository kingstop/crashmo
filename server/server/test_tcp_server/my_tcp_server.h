#pragma once
#include <tcpserver.h>
class my_tcp_server :
	public tcp_server
{
public:
	my_tcp_server();
	~my_tcp_server();
public:
	virtual base_session* create_session();
	virtual void run();
};

