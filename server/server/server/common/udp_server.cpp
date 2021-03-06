#include "udp_server.h"
#include "udpsession.h"



udp_server::udp_server(int id):base_server(id), _connect_count(0),_address()
{
}


udp_server::~udp_server()
{
	enet_host_destroy(get_host());
}


u32 udp_server::generateID()
{
	_connect_count++;
	return _connect_count;
}

void udp_server::_write_completed()
{
	if (get_host())
	{
		enet_host_flush(get_host());
	}
}

bool udp_server::handle_accept(base_session* p)
{
	if (base_server::handle_accept(p))
	{
		udp_session* udp_p = dynamic_cast<udp_session*>(p);
		if (udp_p)
		{
			udp_p->set_father(this);
		}
		return true;
	}
	return false;
}

void udp_server::on_enet_connected(ENetEvent& event)
{
	ENetAddress remote = event.peer->address; //远程地址
	char ip[256];
	unsigned connect_index = generateID();
	enet_address_get_host_ip(&remote, ip, 256);
	
	//event.peer->data = (void*)connect_index;
	u32 connect_id = event.peer->connectID;
	boost::mutex::scoped_lock lock(m_proc_mutex);
	if (m_sessions.empty() == false)
	{
		base_session* p = m_sessions.front();
		if (p)
		{
			udp_session* pudp = dynamic_cast<udp_session*>(p);
			if (pudp)
			{
				auto it_ = _connected_sessions.find(pudp->get_peer());
				if (it_ != _connected_sessions.end())
				{
					pudp->close();
					pudp->handle_close();

				}
				if (handle_accept(p))
				{
					pudp->handle_connect(event.peer, connect_index, remote.host, remote.port, ip);
					_connected_sessions[pudp->get_peer()] = pudp;
					event.peer->data = const_cast<u32*>(pudp->get_connect_index_data());
				}
				else
				{
					pudp->close();
					pudp->handle_close();
				}
			}
			//std::cout << "ip[" << ip << "] 已经连接,序号[" << connect_index <<"] 目前连接数["<< _connected_sessions.size() <<"]" <<std::endl;
			
		}
		m_sessions.pop_front();
	}		
}
void udp_server::on_enet_receive(ENetEvent& event)
{
	std::cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << std::endl;
	std::cout << "数据大小:" << event.packet->dataLength << std::endl;
	std::cout << "数据:" << (char*)event.packet->data << std::endl;
	auto it_ = _connected_sessions.find(event.peer);
	if (it_ != _connected_sessions.end())
	{
		udp_session* p_udp = it_->second;
		p_udp->receive((char*)event.packet->data, event.packet->dataLength);
	}
	//enet_packet_destroy(event.packet);    //注意释放空间
	std::cout << std::endl;
}
void udp_server::on_enet_disconnect(ENetEvent& event)
{
	std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
	auto it_ = _connected_sessions.find(event.peer);
	if (it_ != _connected_sessions.end())
	{
		udp_session* p_udp = it_->second;
		p_udp->close();
		p_udp->handle_close();
		_connected_sessions.erase(it_);
		free_session(p_udp);
	}
	std::cout << "目前连接数[" << _connected_sessions.size() << "]" << std::endl;
	
}
ENetHost* udp_server::get_host() 
{
	return net_global::get_enet_host();
}

void udp_server::extra_process(bool is_wait)
{
	base_server::_real_run(is_wait);
	auto it_ = _connected_sessions.begin();
	for (; it_ != _connected_sessions.end(); ++it_)
	{
		udp_session* p_udp = it_->second;
		p_udp->_write_message();
	}

}

char* udp_server::get_uncompress_buffer()
{
	return _uncompress_buffer;
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
	m_poolcount = poolcount;

	if (m_poolcount == 0)
	{
		m_poolcount = 20;
	}

	for (unsigned int i = 0; i < m_poolcount; ++i)
	{
		free_session(create_session());
	}
	net_global::udp_net_init(&_address, poolcount, 2);
	net_global::start_enet_thread(this);
	return true;
}


//void udp_server::run()
//{
	////下面开始收数据等
	//ENetEvent event;
	//while (enet_host_service(get_host(), &event, 1000) >= 0)
	//{
	//	switch (event.type)
	//	{
	//	case ENET_EVENT_TYPE_CONNECT:
	//	{
	//		ENetAddress remote = event.peer->address; //远程地址
	//		char ip[256];
	//		static unsigned connect_index = generateID();
	//		enet_address_get_host_ip(&remote, ip, 256);
	//		std::cout << "ip:" << ip << " 已经连接,序号:" << connect_index << std::endl;
	//		event.peer->data = (void*)connect_index;
	//		u32 connect_id = event.peer->connectID;
	//		boost::mutex::scoped_lock lock(m_proc_mutex);
	//		if (m_sessions.empty() == false)
	//		{
	//			base_session* p = m_sessions.front();
	//			udp_session* pudp = dynamic_cast<udp_session*>(p);
	//			if (pudp)
	//			{
	//				pudp->on_connect(event.peer, connect_index, remote.host, remote.port, ip);
	//				if (handle_accept(p))
	//				{
	//					auto it_ = _connected_sessions.find(pudp->get_connect_index());
	//					if (it_ != _connected_sessions.end())
	//					{
	//						it_->second->close();

	//					}
	//					_connected_sessions[pudp->get_connect_index()] = pudp;
	//				}
	//				else
	//				{
	//					pudp->close();
	//				}
	//			}
	//			m_sessions.pop_front();
	//		}

	//	}
	//	break;

	//	case ENET_EVENT_TYPE_RECEIVE:
	//	{
	//		std::cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << std::endl;
	//		std::cout << "数据大小:" << event.packet->dataLength << std::endl;
	//		std::cout << "数据:" << (char*)event.packet->data << std::endl;
	//		auto it_ = _connected_sessions.find((u32)event.peer->data);
	//		if (it_ != _connected_sessions.end())
	//		{
	//			udp_session* p_udp = it_->second;
	//			p_udp->receive((char*)event.packet->data, event.packet->dataLength);
	//		}
	//		enet_packet_destroy(event.packet);    //注意释放空间
	//		std::cout << std::endl;
	//	}
	//	break;
	//	case ENET_EVENT_TYPE_DISCONNECT:
	//	{
	//		std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
	//		auto it_ = _connected_sessions.find((u32)event.peer->data);
	//		if (it_ != _connected_sessions.end())
	//		{
	//			udp_session* p_udp = it_->second;
	//			p_udp->close();
	//		}
	//	}
	//	default:
	//		break;
	//	}
	//	auto it_ = _connected_sessions.begin();
	//	for (; it_ != _connected_sessions.end(); ++it_)
	//	{
	//		udp_session* p_udp = it_->second;
	//		p_udp->_write_message();
	//	}
	//}

//}