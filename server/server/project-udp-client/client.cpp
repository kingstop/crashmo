// project-udp-client.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"

#include <iostream>
#include <string.h>
#include "enet/enet.h"

//using namespace std;
//
//int main()
//{
////#if 0
//	//初始化enet库
//	if (enet_initialize())
//	{
//		cout << "初始化失败" << endl;
//		return -1;
//	}
//
//	//创建本地HOST对象
//	ENetHost* client = enet_host_create(NULL,
//		1,   //只允许连接一个服务器
//		0,
//		0,
//		0);
//	if (client == NULL)
//	{
//		cout << "创建客户端失败" << endl;
//		return -1;
//	}
//
//
//	//连接到服务器
//	ENetAddress svraddr;
//	enet_address_set_host(&svraddr, "127.0.0.1");
//	svraddr.port = 777;
//
//	ENetPeer* server = enet_host_connect(client, &svraddr, 3, 1); //client连接到svraddr对象，共分配三个通道
//	if (server == NULL)
//	{
//		//cout << "连接服务器失败" << endl;
//		return -1;
//	}
//	//ENetPeer* server1 = enet_host_connect(client, &svraddr, 3, 2); //client连接到svraddr对象，共分配三个通道
//	//if (server1 == NULL)
//	//{
//	//	//cout << "连接服务器失败" << endl;
//	//	return -1;
//	//}
//
//	ENetEvent event;
//	//连接成功后必须调用enet_host_service来最终确认
//	if (enet_host_service(client, &event, 5000) > 0 &&
//		event.type == ENET_EVENT_TYPE_CONNECT)
//	{
//		//cout << "连接服务器成功" << endl;
//
//	}
//	else
//	{
//		enet_peer_reset(server);
//		//cout << "连接服务器失败" << endl;
//		return -1;
//	}
//
//
//	//下面开始发数据
//	ENetPacket* packet = enet_packet_create(NULL, 78, ENET_PACKET_FLAG_RELIABLE); //创建包
//	strcpy((char*)packet->data, "hi,哈哈");
//	enet_peer_send(server, 1, packet);
//
//	ENetPacket* packet1 = enet_packet_create(NULL, 86, ENET_PACKET_FLAG_RELIABLE); //创建包
//	strcpy((char*)packet1->data, "你好啊，呵呵");
//	enet_peer_send(server, 2, packet1);
//
//	enet_host_flush(client); //必须使用这个函数或是enet_host_service来使数据发出去
//
//
//	//关闭连接
//	//enet_peer_disconnect(server, 0);
//	//等待关闭成功
//	while (enet_host_service(client, &event, 5000) > 0)
//	{
//		switch (event.type)
//		{
//		case ENET_EVENT_TYPE_RECEIVE:
//		{
//			enet_packet_destroy(event.packet);
//			ENetPacket* packet3 = enet_packet_create(NULL, 86, ENET_PACKET_FLAG_RELIABLE); //创建包
//			strcpy((char*)packet3->data, "你好啊，呵呵");
//			enet_peer_send(event.peer, 2, packet1);
//			enet_host_flush(client); //必须使用这个函数或是enet_host_service来使数据发出去
//		}
//			break;
//
//		case ENET_EVENT_TYPE_DISCONNECT:
//			cout << "已经成功断开连接" << endl;
//			enet_deinitialize();
//			return -1;
//		}
//	}
//
//	//这里就是关闭失败，强制重置
//	enet_peer_reset(server);
//	cout << "强制重置" << endl;
//
//	enet_deinitialize();
////#endif
//
//	char k = 0;
//
//	for (int i = 0; i < 127; i++)
//	{
//		k += i & 3;
//		printf("<ycs>k:%d\n", k);
//	}
//	printf("k:%d\n", k);
//	return 0;
//
//
//}

//

//

#include <iostream>
#include "asiodef.h"
#include "test_udp_client.h"
#include "login.pb.h"

#include <boost/bind.hpp>  
#include <boost/thread/thread_pool.hpp> 

class test_client_manager {
public:
	test_client_manager(int count):_count(count) {

	}
	~test_client_manager() {
	}
	void create()
	{
		for (size_t i = 0; i < _count; i++)
		{
			test_udp_client* client = new test_udp_client();
			client->connect("127.0.0.1", 777);
		}
	}
protected:
	int _count;
	std::map<int, test_udp_client*> _clients;

};

int main()
{
	
	
	test_udp_client::initPBModule();
	net_global::udp_init_client_manager(100);
	test_client_manager manager(99);
	manager.create();
	//net_global::udp_net_init(nullptr, 1, 2, 57600, 14400);
	/*
	std::vector<my_task_thread*> vc;
	for (size_t i = 0; i < 5; i++)
	{
		vc.push_back(new my_task_thread(i));

	}
	*/
	

	net_global::start_client_thread();

	//test_udp_client client;	
	//client.connect("127.0.0.1", 777);
	//net_global::update_udp_event_service();
	
	

	//client.sendPBMessage(&msg, 0);

	//test_udp_client client1;
	//client1.connect("127.0.0.1", 777);
	//client1.sendPBMessage(&msg, 0);

	while (true)
	{
		net_global::update_net_service();
	}
	return 0;

    //std::cout << "Hello World!\n"; 
}
