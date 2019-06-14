// udp_server.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include <iostream>
#include "test_udp_server.h"
#include "test_udp_session.h"

/*
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

*/

int main()
{
	net_global::udp_net_init();
	test_udp_session::registerPBCall();
	test_udp_server udp_server(1);
	udp_server.create(777, 5, 2);
	while (true)
	{
		udp_server.run_no_wait();
	}
	

    //std::cout << "Hello World!\n";
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门使用技巧: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
