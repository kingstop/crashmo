#include "client_enet_component.h"
#include "udp_client.h"


client_enet_component::client_enet_component()
{

}

client_enet_component::~client_enet_component()
{

}



void client_enet_component::on_enet_connected(ENetEvent& event)
{
	udp_client* udp_client = get_udp_client(event.peer);
	if (udp_client)
	{
		udp_client->on_connect();
	}
	
	
}
void client_enet_component::on_enet_receive(ENetEvent& event)
{
	udp_client* udp_client = get_udp_client(event.peer);
	if (udp_client)
	{
		udp_client->receive((char*)event.packet->data, event.packet->dataLength);
	}
}
void client_enet_component::on_enet_disconnect(ENetEvent& event)
{
	udp_client* udp_client = get_udp_client(event.peer); 
	if (udp_client)
	{
		udp_client->handle_close();
	}
	
}
