// udp_server.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include "test_udp_server.h"


#include <iostream>
#include "enet/enet.h"
using namespace std;

int main()
{
	//初始化enet库
	if (enet_initialize())
	{
		cout << "初始化失败" << endl;
		return -1;
	}

	//下面创建服务器
	ENetAddress address;
	address.host = ENET_HOST_ANY;
	address.port = 1234;

	ENetHost* server;
	server = enet_host_create(&address, //主机地址
		64,        //允许的连接数
		0,
		0,
		0);        //自动管理带宽
	if (server == NULL)
	{
		cout << "创建主机对象失败" << endl;
		enet_deinitialize();
		return -1;
	}


	//下面开始收数据等
	ENetEvent event;
	while (enet_host_service(server, &event, 5000) >= 0)
	{
		if (event.type == ENET_EVENT_TYPE_CONNECT) //有客户机连接成功
		{
			static unsigned int num = 0;
			ENetAddress remote = event.peer->address; //远程地址
			char ip[256];
			enet_address_get_host_ip(&remote, ip, 256);
			cout << "ip:" << ip << " 已经连接,序号:" << num << endl;
			event.peer->data = (void*)num++;
		}
		else if (event.type == ENET_EVENT_TYPE_RECEIVE) //收到数据
		{
			cout << "收到序号" << event.peer->data << "的数据,从" << event.channelID << "通道发送" << endl;
			cout << "数据大小:" << event.packet->dataLength << endl;
			cout << "数据:" << (char*)event.packet->data << endl;
			enet_packet_destroy(event.packet);    //注意释放空间
			cout << endl;
		}
		else if (event.type == ENET_EVENT_TYPE_DISCONNECT) //失去连接
		{
			cout << "序号" << event.peer->data << "远程已经关闭连接" << endl;
		}
	}

	enet_deinitialize();
}


/*
int main()
{
	net_global::udp_net_init();
	test_udp_server udp_server(1);
	udp_server.create(777, 5, 2);
	while (true)
	{
		udp_server.run_no_wait();
	}
	

    //std::cout << "Hello World!\n";
}
*/

