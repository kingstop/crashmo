#pragma once
#include "enet_component.h"
#include <iostream>
#include <unordered_map>
#include <set>
#include "call_back.h"

class udp_client;
class udp_client_manager :
	public enet_component
{
public:
	udp_client_manager();
	virtual ~udp_client_manager();
	void init(int client_count);
	virtual void on_enet_connected(ENetEvent& event);
	virtual void on_enet_receive(ENetEvent& event);
	virtual void on_enet_disconnect(ENetEvent& event);
	virtual void extra_process(bool is_wait);
	virtual ENetHost* get_host();
	void add_client(udp_client* client);
	void remove_client(udp_client* client);
	void on_try_connect(udp_client* client);	
	bool have_client(udp_client* client);
	call_back_mgr* get_cb_mgr() { return &m_cb_mgr; }
protected:
	std::unordered_map<ENetPeer*, udp_client*> _connected_clients;
	std::unordered_map<ENetPeer*, udp_client*> _connect_clients;
	std::set<udp_client*> _clients;
	call_back_mgr m_cb_mgr;

};

