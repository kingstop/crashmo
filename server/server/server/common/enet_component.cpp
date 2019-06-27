#include "enet_component.h"
#include "enet/enet.h"

enet_component::enet_component():_exit(false),_cpu_wait(false)
{

}
enet_component::~enet_component()
{

}

bool enet_component::is_exit()
{
	return _exit;
}

void enet_component::extra_process(bool is_wait)
{

}

bool enet_component::is_cpu_wait()
{
	return _cpu_wait;
}

void enet_component::set_exit(bool exit)
{
	_exit = exit;
}

void enet_component::run_nowait()
{
	enet_run(false);
}
void enet_component::run()
{
	enet_run(true);
}

void enet_component::enet_run(bool is_wait)
{
	if (get_host() != nullptr)
	{
		ENetEvent event;
		while (enet_host_service(get_host(), &event, 1000) >= 0 && _exit == false)
		{
			switch (event.type)
			{
			case ENET_EVENT_TYPE_CONNECT:
			{ 
				on_enet_connected(event);
			}
			break;

			case ENET_EVENT_TYPE_RECEIVE:
			{
				on_enet_receive(event);
							
				ENetPacket* packet1 = enet_packet_create((char*)event.packet->data, event.packet->dataLength, ENET_PACKET_FLAG_RELIABLE);
				//strcpy((char*)packet1->data, "��ð����Ǻ�");
				enet_peer_send(event.peer, 2, packet1);
				enet_host_flush(get_host()); //����ʹ�������������enet_host_service��ʹ���ݷ���ȥ

				enet_packet_destroy(event.packet);    //ע���ͷſռ�



			}
			break;
			case ENET_EVENT_TYPE_DISCONNECT:
			{
				on_enet_disconnect(event);
			}
			default:
				break;
			}

			extra_process(is_wait);
		}
	}


}
