#pragma once
#include "base_server.h"
#include <iostream>
#include <enet/enet.h>
#include <unordered_map>
#include "common_type.h"
#include "enet_component.h"

class udp_session;
class udp_server : public base_server ,public enet_component
{
public:
	udp_server(int id);
	virtual ~udp_server();
	u32 generateID();
	virtual bool create(unsigned short port, unsigned int poolcount, int thread_count);
	virtual void _real_run(bool is_wait);	
	virtual ENetHost* get_host();
	char* get_uncompress_buffer();
protected:
	virtual void _write_completed();
	virtual bool handle_accept(base_session* p);
	virtual void on_enet_connected(ENetEvent& event);
	virtual void on_enet_receive(ENetEvent& event);
	virtual void on_enet_disconnect(ENetEvent& event);	
	virtual void extra_process(bool is_wait);
private:
	void run();
protected:
	ENetAddress _address;
	u32  _connect_count;
	std::unordered_map<u32, udp_session*> _connected_sessions;
	char _uncompress_buffer[MAX_MESSAGE_LEN];
};

