#include "udpserver.h"
#include <iostream>


int InitEnet()
{
	return enet_initialize();
}

void DeinitEnet()
{
	enet_deinitialize();
}

udp_server::udp_server() :_connect_index(0)
{

}


udp_server::~udp_server()
{

}
bool udp_server::Init(u32 host, s32 port)
{
	_address.host = host;
	_address.port = port;
	_server = enet_host_create(&_address /* the address to bind the server host to */,
		32      /* allow up to 32 clients and/or outgoing connections */,
		2      /* allow up to 2 channels to be used, 0 and 1 */,
		0      /* assume any amount of incoming bandwidth */,
		0      /* assume any amount of outgoing bandwidth */);

	if (_server == NULL)
	{
		DeinitEnet();
		return false;
	}

	return true;
}

void udp_server::Run()
{

	//下面开始收数据等
	ENetEvent event;
	while (enet_host_service(_server, &event, 5000) >= 0)
	{
		if (event.type == ENET_EVENT_TYPE_CONNECT) //有客户机连接成功
		{
			_netPeers[event.peer->connectID] = event.peer;

			//static unsigned int num = 0;

			ENetAddress remote = event.peer->address; //远程地址
			char ip[256];
			enet_address_get_host_ip(&remote, ip, 256);
			std::cout << "ip:" << ip << " 已经连接,序号:" << _connect_index << std::endl;
			_connect_index++;
			event.peer->data = (void*)_connect_index;
		}
		else if (event.type == ENET_EVENT_TYPE_RECEIVE) //收到数据
		{
			std::cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << std::endl;
			std::cout << "数据大小:" << event.packet->dataLength << std::endl;
			std::cout << "数据:" << (char*)event.packet->data << std::endl;
			enet_packet_destroy(event.packet);    //注意释放空间
			std::cout << std::endl;
		}
		else if (event.type == ENET_EVENT_TYPE_DISCONNECT) //失去连接
		{
			std::cout << "序号" << event.peer->data << "远程已经关闭连接" << std::endl;
		}
	}

}