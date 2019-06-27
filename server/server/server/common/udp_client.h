#pragma once
#include "udpsession.h"
#include "msg_component.h"
#include "enet_component.h"

class udp_client_manager;
//#include "client_base.h"
class udp_client :
	public udp_session , public msg_component
{
public:
	udp_client();
	virtual ~udp_client();
	inline void set_reconnect(bool b) { m_isreconnect = b; }
	virtual void on_connect();

	void connect(const char* address, unsigned short port);
	ENetPeer* get_peer();
	virtual void extra_process(bool is_wait);
	void handle_connect();
protected:
	void reconnect_check();
	void reconnect();
	virtual ENetHost* get_host();
	virtual void handle_connect(ENetPeer* peer, u32 connect_index,
		u32 remote_host, u16 remote_port, const char* ip);
 	virtual char* get_uncompress_buffer();
protected:
	virtual void push_message(message_t* msg);
	virtual call_back_mgr* _get_cb_mgr();

	volatile bool m_isreconnect;
	unsigned int m_reconnect_time;
	volatile bool m_isinitconncted;
	ENetAddress _address;	

	unsigned short _connect_port;
	std::string _connect_address;
	volatile unsigned int m_last_reconnect_time;
	volatile bool m_isconnecting;
	
	udp_client_manager* _client_manager;
};

