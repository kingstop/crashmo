#include "udp_client.h"



udp_client::udp_client()
{
	


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

void udp_client::_write_completed()
{
	if (_client)
	{
		enet_host_flush(_client);
	}	
}

void udp_client::try_create_client()
{
	if (_client == nullptr)
	{
		_client = enet_host_create(NULL /* create a client host */,

			1 /* only allow 1 outgoing connection */,

			2 /* allow up 2 channels to be used, 0 and 1 */,

			57600 / 8 /* 56K modem with 56 Kbps downstream bandwidth */,

			14400 / 8 /* 56K modem with 14 Kbps upstream bandwidth */);

		if (_client == NULL)

		{

			fprintf(stderr,

				"An error occurred while trying to create an ENet client host.\n");

			exit(EXIT_FAILURE);

		}
	}
}

udp_client::~udp_client()
{
	if (_client)
	{
		enet_host_destroy(_client);
	}
	
}

void udp_client::on_connect(ENetPeer* peer, u32 connect_index,
	u32 remote_host, u16 remote_port, const char* ip)
{
	udp_session::on_connect(peer, connect_index, remote_host, remote_port, ip);
	m_isconnected = true;
	set_valid(true);
}


void udp_client::push_message(message_t* msg)
{
	client_base::push_message(msg);
}

void udp_client::run()
{
	client_base::run(true);
	_write_message();
}
void udp_client::run_no_wait()
{
	client_base::run(false);
	_write_message();
}

void udp_client::connect(const char* address, unsigned short port)
{
	_connect_port = port;
	_connect_address = address;
	try_create_client();
	
	ENetEvent event;
	enet_address_set_host(&_address, address);
	_address.port = port;
	/* Initiate the connection, allocating the two channels 0 and 1. */
	_peer = enet_host_connect(_client, &_address, 3, 0);
	if (_peer == NULL)
	{
		fprintf(stderr,
			"No available peers for initiating an ENet connection.\n");
		exit(EXIT_FAILURE);
	}
	/* Wait up to 5 seconds for the connection attempt to succeed. */
	if (enet_host_service(_client, &event, 5000) > 0 &&
		event.type == ENET_EVENT_TYPE_CONNECT)
	{
		//puts("Connection to some.server.net:1234 succeeded.");
		on_connect(_peer, 0, _address.host, _address.host, address);
	}
	else
	{
		/* Either the 5 seconds are up or a disconnect event was */
		/* received. Reset the peer in the event the 5 seconds   */
		/* had run out without any significant event.            */
		enet_peer_reset(_peer);
		//puts("Connection to some.server.net:1234 failed.");
	}
}
