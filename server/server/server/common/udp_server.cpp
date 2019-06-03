#include "udp_server.h"
#include "udpsession.h"



udp_server::udp_server(int id):base_server(id), _connect_count(0)
{
}


udp_server::~udp_server()
{
	enet_host_destroy(_server);
}


u32 udp_server::generateID()
{
	_connect_count++;
	return _connect_count;
}

bool udp_server::create(unsigned short port, unsigned int poolcount, int thread_count)
{
	base_server::create(port, poolcount, thread_count);


	/* Bind the server to the default localhost. */

	/* A specific host address can be specified by */

	/* enet_address_set_host (& address, "x.x.x.x"); */

	_address.host = ENET_HOST_ANY;

	/* Bind the server to port 1234. */

	_address.port = _port;
	_server = enet_host_create(&_address /* the address to bind the server host to */,

		32 /* allow up to 32 clients and/or outgoing connections */,

		2 /* allow up to 2 channels to be used, 0 and 1 */,

		0 /* assume any amount of incoming bandwidth */,

		0 /* assume any amount of outgoing bandwidth */);

	if (_server == NULL)

	{

		fprintf(stderr,

			"An error occurred while trying to create an ENet server host.\n");

		exit(EXIT_FAILURE);
		return false;
	}
	return true;
}


void udp_server::run()
{
	//下面开始收数据等
	ENetEvent event;



	while (enet_host_service(_server, &event, 5000) >= 0)
	{
		if (event.type == ENET_EVENT_TYPE_CONNECT) //有客户机连接成功
		{

			ENetAddress remote = event.peer->address; //远程地址
			char ip[256];
			static unsigned connect_index = generateID();
			enet_address_get_host_ip(&remote, ip, 256);
			std::cout << "ip:" << ip << " 已经连接,序号:" << connect_index << std::endl;
			event.peer->data = (void*)connect_index;
			u32 connect_id = event.peer->connectID;
			boost::mutex::scoped_lock lock(m_proc_mutex);
			base_session* p = m_sessions.front();
			udp_session* pudp = dynamic_cast<udp_session*>(p);
			if (pudp)
			{
				pudp->on_connect(event.peer, connect_index, remote.host, remote.port, ip);
				if (handle_accept(p))
				{
					auto it_ = _connected_sessions.find(pudp->get_connect_index());
					if (it_ != _connected_sessions.end())
					{
						it_->second->close();

					}
					_connected_sessions[pudp->get_connect_index()] = pudp;
				}
				else
				{
					pudp->close();
				}
			}

			m_sessions.pop_front();
		}
		else if (event.type == ENET_EVENT_TYPE_RECEIVE) //收到数据
		{
			std::cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << std::endl;
			std::cout << "数据大小:" << event.packet->dataLength << std::endl;
			std::cout << "数据:" << (char*)event.packet->data << std::endl;
			auto it_ = _connected_sessions.find((u32)event.peer->data);
			if (it_ != _connected_sessions.end())
			{
				udp_session* p_udp = it_->second;
				p_udp->receive((char*)event.packet->data, event.packet->dataLength);
			}
			enet_packet_destroy(event.packet);    //注意释放空间
			std::cout << std::endl;
		}
		else if (event.type == ENET_EVENT_TYPE_DISCONNECT) //失去连接
		{
			std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
			auto it_ = _connected_sessions.find((u32)event.peer->data);
			if (it_ != _connected_sessions.end())
			{
				udp_session* p_udp = it_->second;
				p_udp->close();
			}
		}

	}
	auto it_ = _connected_sessions.begin();
	for (; it_ != _connected_sessions.end(); ++ it_)
	{
		udp_session* p_udp = it_->second;
		p_udp->_write_message();
	}
}