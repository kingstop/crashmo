#include "enet_component.h"
#include "enet/enet.h"

enet_component::enet_component()
{

}
enet_component::~enet_component()
{

}

void enet_component::extra_process(bool is_wait)
{

}
void enet_component::enet_run(bool is_wait)
{
	if (get_host() != nullptr)
	{
		ENetEvent event;
		while (enet_host_service(get_host(), &event, 1000) >= 0)
		{
			switch (event.type)
			{
			case ENET_EVENT_TYPE_CONNECT:
			{
				on_enet_connected(event);
				/*
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
				*/
			}
			break;

			case ENET_EVENT_TYPE_RECEIVE:
			{
				on_enet_receive(event);
				enet_packet_destroy(event.packet);    //注意释放空间
				/*
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
				*/
			}
			break;
			case ENET_EVENT_TYPE_DISCONNECT:
			{
				on_enet_disconnect(event);
				/*
				std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
				auto it_ = _connected_sessions.find((u32)event.peer->data);
				if (it_ != _connected_sessions.end())
				{
					udp_session* p_udp = it_->second;
					p_udp->close();
				}
				*/
			}
			default:
				break;
			}

			extra_process(is_wait);
		}
	}


}
