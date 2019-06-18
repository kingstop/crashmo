// project-udp-client.cpp : 此文件包含 "main" 函数。程序执行将在此处开始并结束。
//

#include "pch.h"

#include <iostream>
#include <string.h>
#include "enet/enet.h"
/*
using namespace std;

int main()
{
//#if 0
	//初始化enet库
	if (enet_initialize())
	{
		cout << "初始化失败" << endl;
		return -1;
	}

	//创建本地HOST对象
	ENetHost* client = enet_host_create(NULL,
		1,   //只允许连接一个服务器
		0,
		0,
		0);
	if (client == NULL)
	{
		cout << "创建客户端失败" << endl;
		return -1;
	}


	//连接到服务器
	ENetAddress svraddr;
	enet_address_set_host(&svraddr, "127.0.0.1");
	svraddr.port = 777;

	ENetPeer* server = enet_host_connect(client, &svraddr, 3, 0); //client连接到svraddr对象，共分配三个通道
	if (server == NULL)
	{
		//cout << "连接服务器失败" << endl;
		return -1;
	}

	ENetEvent event;
	//连接成功后必须调用enet_host_service来最终确认
	if (enet_host_service(client, &event, 5000) > 0 &&
		event.type == ENET_EVENT_TYPE_CONNECT)
	{
		//cout << "连接服务器成功" << endl;

	}
	else
	{
		enet_peer_reset(server);
		//cout << "连接服务器失败" << endl;
		return -1;
	}


	//下面开始发数据
	ENetPacket* packet = enet_packet_create(NULL, 78, ENET_PACKET_FLAG_RELIABLE); //创建包
	strcpy((char*)packet->data, "hi,哈哈");
	enet_peer_send(server, 1, packet);

	ENetPacket* packet1 = enet_packet_create(NULL, 86, ENET_PACKET_FLAG_RELIABLE); //创建包
	strcpy((char*)packet1->data, "你好啊，呵呵");
	enet_peer_send(server, 2, packet1);

	enet_host_flush(client); //必须使用这个函数或是enet_host_service来使数据发出去


	//关闭连接
	enet_peer_disconnect(server, 0);
	//等待关闭成功
	while (enet_host_service(client, &event, 5000) > 0)
	{
		switch (event.type)
		{
		case ENET_EVENT_TYPE_RECEIVE:
			enet_packet_destroy(event.packet);
			break;

		case ENET_EVENT_TYPE_DISCONNECT:
			cout << "已经成功断开连接" << endl;
			enet_deinitialize();
			return -1;
		}
	}

	//这里就是关闭失败，强制重置
	enet_peer_reset(server);
	cout << "强制重置" << endl;

	enet_deinitialize();
//#endif

	char k = 0;

	for (int i = 0; i < 127; i++)
	{
		k += i & 3;
		printf("<ycs>k:%d\n", k);
	}
	printf("k:%d\n", k);
	return 0;


}
*/
//



#include <iostream>
#include "asiodef.h"
#include "test_udp_client.h"
#include "login.pb.h"

#include <boost/bind.hpp>  
#include <boost/thread/thread_pool.hpp> 

class my_task_thread
{
public:
	my_task_thread(int index):m_index(index), m_exit(false), _client(nullptr)
	{
		_client = new test_udp_client();
		_client->connect("127.0.0.1", 777);
		message::LoginRequest msg;
		msg.set_name(std::to_string(m_index));
		msg.set_pwd(std::to_string(m_index));
		_client->sendPBMessage(&msg, 0);

		m_thread = new boost::thread(boost::bind(&my_task_thread::run, this));

		
	}
	~my_task_thread()
	{
		m_exit = true;
		m_thread->join();
		delete m_thread;
		delete _client;
	}

public:	
	void run()
	{
		_client->run_no_wait();
	}
private:

	boost::thread* m_thread;
	boost::mutex m_mutex;
	volatile bool m_exit;
	int m_index;
	test_udp_client* _client;
};


int main()
{
	
	net_global::udp_net_init();
	test_udp_client::initPBModule();
	/*
	std::vector<my_task_thread*> vc;
	for (size_t i = 0; i < 5; i++)
	{
		vc.push_back(new my_task_thread(i));

	}
	*/

	test_udp_client client;
	
	client.connect("127.0.0.1", 777);
	message::LoginRequest msg;
	msg.set_name("12345");
	msg.set_pwd("54321");
	client.sendPBMessage(&msg, 0);

	while (true)
	{
		
		client.run_no_wait();
	}
	
	return 0;

    //std::cout << "Hello World!\n"; 
}

// 运行程序: Ctrl + F5 或调试 >“开始执行(不调试)”菜单
// 调试程序: F5 或调试 >“开始调试”菜单

// 入门提示: 
//   1. 使用解决方案资源管理器窗口添加/管理文件
//   2. 使用团队资源管理器窗口连接到源代码管理
//   3. 使用输出窗口查看生成输出和其他消息
//   4. 使用错误列表窗口查看错误
//   5. 转到“项目”>“添加新项”以创建新的代码文件，或转到“项目”>“添加现有项”以将现有代码文件添加到项目
//   6. 将来，若要再次打开此项目，请转到“文件”>“打开”>“项目”并选择 .sln 文件
