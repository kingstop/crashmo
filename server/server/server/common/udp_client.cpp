#include "udp_client.h"



udp_client::udp_client():msg_component(false), m_isreconnect(false),
m_reconnect_time(0), m_isinitconncted(false),m_last_reconnect_time(0),_client(nullptr), m_isconnecting(false), _connect_port(0)
{


	 
}

void udp_client::on_enet_connected(ENetEvent& event)
{

}

void udp_client::on_enet_receive(ENetEvent& event)
{
	receive((char*)event.packet->data, event.packet->dataLength);	
}

void udp_client::on_enet_disconnect(ENetEvent& event)
{

}

void udp_client::extra_process(bool is_wait)
{
	msg_component::run(false);
	_write_message();
}

ENetHost* udp_client::get_host()
{
	return _client;
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



void udp_client::try_create_client()
{
	if (_client == nullptr)
	{
		_client = enet_host_create(NULL /* create a client host */,

			100 /* only allow 1 outgoing connection */,

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
	msg_component::push_message(msg);
}

void udp_client::run()
{
	enet_run(true);
	//msg_component::run(true);
	//_write_message();
}
void udp_client::run_no_wait()
{

	enet_run(false);

	////下面开始收数据等
	//ENetEvent event;
	//while (enet_host_service(_client, &event, 1000) >= 0)
	//{
	//	switch (event.type)
	//	{
	//	case ENET_EVENT_TYPE_CONNECT:
	//	{
	//		int i = 0;
	//		i++;
	//		/*
	//		ENetAddress remote = event.peer->address; //远程地址
	//		char ip[256];
	//		static unsigned connect_index = generateID();
	//		enet_address_get_host_ip(&remote, ip, 256);
	//		std::cout << "ip:" << ip << " 已经连接,序号:" << connect_index << std::endl;
	//		event.peer->data = (void*)connect_index;
	//		u32 connect_id = event.peer->connectID;
	//		boost::mutex::scoped_lock lock(m_proc_mutex);
	//		base_session* p = m_sessions.front();
	//		udp_session* pudp = dynamic_cast<udp_session*>(p);
	//		if (pudp)
	//		{
	//			pudp->on_connect(event.peer, connect_index, remote.host, remote.port, ip);
	//			if (handle_accept(p))
	//			{
	//				auto it_ = _connected_sessions.find(pudp->get_connect_index());
	//				if (it_ != _connected_sessions.end())
	//				{
	//					it_->second->close();

	//				}
	//				_connected_sessions[pudp->get_connect_index()] = pudp;
	//			}
	//			else
	//			{
	//				pudp->close();
	//			}
	//		}

	//		m_sessions.pop_front();
	//		*/
	//	}
	//	break;

	//	case ENET_EVENT_TYPE_RECEIVE:
	//	{
	//		std::cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << std::endl;
	//		std::cout << "数据大小:" << event.packet->dataLength << std::endl;
	//		std::cout << "数据:" << (char*)event.packet->data << std::endl;
	//		receive((char*)event.packet->data, event.packet->dataLength);
	//		enet_packet_destroy(event.packet);    //注意释放空间
	//		std::cout << std::endl;
	//	}
	//	break;
	//	case ENET_EVENT_TYPE_DISCONNECT:
	//	{
	//		std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
	//	}
	//	default:
	//		break;
	//	}

	//	msg_component::run(false);
	//	_write_message();
	//}

}

void udp_client::connect(const char* address, unsigned short port)
{
	_connect_port = port;
	_connect_address = address;
	try_create_client();
	for (size_t i = 0; i < 3; i++)
	{
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


}
