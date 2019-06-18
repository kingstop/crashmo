#pragma once
#include "udpsession.h"
#include "msg_component.h"
#include "enet_component.h"
//#include "client_base.h"
class udp_client :
	public udp_session , public msg_component , public enet_component
{
public:
	udp_client();
	virtual ~udp_client();
	inline void set_reconnect(bool b) { m_isreconnect = b; }
	virtual void on_connect(ENetPeer* peer, u32 connect_index,
		u32 remote_host, u16 remote_port, const char* ip);
	void connect(const char* address, unsigned short port);
	virtual void run();
	virtual void run_no_wait();
protected:
	void try_create_client();
	void reconnect_check();
	void reconnect();

	virtual void on_enet_connected(ENetEvent& event);
	virtual void on_enet_receive(ENetEvent& event);
	virtual void on_enet_disconnect(ENetEvent& event);
	virtual ENetHost* get_host();
	virtual void extra_process(bool is_wait);
protected:
	virtual void push_message(message_t* msg);
public:
	volatile bool m_isreconnect;
	unsigned int m_reconnect_time;
	volatile bool m_isinitconncted;
	ENetAddress _address;	
	ENetHost * _client;
	unsigned short _connect_port;
	std::string _connect_address;
	volatile unsigned int m_last_reconnect_time;
	volatile bool m_isconnecting;
};

