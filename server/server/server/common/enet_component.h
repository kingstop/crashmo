#pragma once
#include "enet/enet.h"

class enet_component
{
public:
	enet_component();
	virtual ~enet_component();
	virtual void enet_run(bool is_wait);
	virtual void run_nowait();
	virtual void run();
	void set_exit(bool exit);
	bool is_exit();
	virtual void on_enet_connected(ENetEvent& event) = 0;
	virtual void on_enet_receive(ENetEvent& event) = 0;
	virtual void on_enet_disconnect(ENetEvent& event) = 0;
	virtual void extra_process(bool is_wait);
	bool is_cpu_wait();
protected:

	virtual ENetHost* get_host() = 0;
	
	
protected:
	volatile bool _exit;
	bool _cpu_wait;

	
};

