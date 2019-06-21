#pragma once
#include "enet_component.h"
class udp_client;
class client_enet_component :
	public enet_component
{
public:
	client_enet_component();
	virtual ~client_enet_component();
	//virtual void enet_run(bool is_wait);
protected:
	virtual void on_enet_connected(ENetEvent& event);
	virtual void on_enet_receive(ENetEvent& event);
	virtual void on_enet_disconnect(ENetEvent& event);
protected:
  	virtual udp_client* get_udp_client(ENetPeer* peer) = 0;
	

};

