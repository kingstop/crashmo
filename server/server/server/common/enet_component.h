#pragma once
#include "enet/enet.h"

class enet_component
{
public:
	enet_component();
	~enet_component();
	void enet_run(bool is_wait);
protected:
	virtual void on_enet_connected(ENetEvent& event) = 0;
	virtual void on_enet_receive(ENetEvent& event) = 0;
	virtual void on_enet_disconnect(ENetEvent& event) = 0;
	virtual ENetHost* get_host() = 0;
	virtual void extra_process(bool is_wait);
protected:
	

	
};

