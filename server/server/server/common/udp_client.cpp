#include "udp_client.h"
#include "udp_client_manager.h"


udp_client::udp_client():msg_component(false), m_isreconnect(false),
m_reconnect_time(0), m_isinitconncted(false),m_last_reconnect_time(0), m_isconnecting(false), _connect_port(0), _client_manager(net_global::get_udp_client_manager())
{	
	_client_manager->add_client(this);
}


ENetHost* udp_client::get_host()
{
	ENetHost* host = net_global::get_enet_host();

	return net_global::get_enet_host();
}


void udp_client::reconnect()
{
	connect(_connect_address.c_str(), _connect_port);
}


void udp_client::reconnect_check()
{
	{
		boost::mutex::scoped_lock lock(m_mutex);
		if (!m_isconnected && m_isreconnect && !m_isconnecting && m_isinitconncted)
		{
			unsigned int now = (unsigned int)time(NULL);
			if (now - m_last_reconnect_time > m_reconnect_time)
				reconnect();
		}
	}

}



udp_client::~udp_client()
{

	
}

void udp_client::on_connect()
{
	on_connect(_peer, 0, _address.host, _address.host, _connect_address.c_str());
}

void udp_client::extra_process(bool is_wait)
{
	_write_message();
	msg_component::run(is_wait);

}

void udp_client::on_connect(ENetPeer* peer, u32 connect_index,
	u32 remote_host, u16 remote_port, const char* ip)
{
	udp_session::on_connect(peer, connect_index, remote_host, remote_port, ip);
	m_isconnected = true;
	set_valid(true);
}

call_back_mgr* udp_client::_get_cb_mgr()
{
	return &m_cb_mgr;
}


void udp_client::push_message(message_t* msg)
{
	msg_component::push_message(msg);
}



ENetPeer* udp_client::get_peer()
{
	return _peer;
}

void udp_client::connect(const char* address, unsigned short port)
{
	_connect_port = port;
	_connect_address = address;
	//net_global::udp_net_init(nullptr, 2, 2);
	ENetEvent event;
	enet_address_set_host(&_address, address);
	_address.port = port;
	/* Initiate the connection, allocating the two channels 0 and 1. */
	_peer = enet_host_connect(get_host(), &_address, 3, 0);

	if (_peer == NULL)
	{
		fprintf(stderr,
			"No available peers for initiating an ENet connection.\n");
		exit(EXIT_FAILURE);
	}
	else
	{
		_client_manager->on_try_connect(this);
	}
	
	///* Wait up to 5 seconds for the connection attempt to succeed. */
	//if (enet_host_service(get_host(), &event, 5000) > 0 &&
	//	event.type == ENET_EVENT_TYPE_CONNECT && event.peer == _peer)
	//{
	//	//puts("Connection to some.server.net:1234 succeeded.");
	//	on_connect(_peer, 0, _address.host, _address.host, address);
	//}

	//else
	//{
	//	/* Either the 5 seconds are up or a disconnect event was */
	//	/* received. Reset the peer in the event the 5 seconds   */
	//	/* had run out without any significant event.            */
	//	enet_peer_reset(_peer);
	//	//puts("Connection to some.server.net:1234 failed.");
	//}



}
