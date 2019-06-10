#pragma once
#include "base_server.h"
#include <iostream>
#include <enet/enet.h>
#include <unordered_map>
#include "common_type.h"

class udp_session;
class udp_server : public base_server
{
public:
	udp_server(int id);
	virtual ~udp_server();
	u32 generateID();
	virtual bool create(unsigned short port, unsigned int poolcount, int thread_count);
	virtual void _real_run(bool is_wait);
private:
	void run();
protected:
	ENetAddress _address;
	ENetHost * _server;
	u32  _connect_count;
	std::unordered_map<u32, udp_session*> _connected_sessions;

};

