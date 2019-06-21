#include "udp_client_manager.h"
#include "asiodef.h"
#include "udp_client.h"


udp_client_manager::udp_client_manager()
{

}

udp_client_manager::~udp_client_manager()
{

}

bool udp_client_manager::have_client(udp_client* client)
{
	
	if (_clients.count(client))
	{
		return true;
	}

	if (client->get_peer())
	{
		if (_connected_clients.count(client->get_peer()))
		{
			return true;
		}

		if (_connect_clients.count(client->get_peer()))
		{
			return true;
		}
	}

	return false;
	
}

void udp_client_manager::add_client(udp_client* client)
{
	
	if (have_client(client) == false)
	{
		_clients.insert(client);
	}
}
void udp_client_manager::remove_client(udp_client* client)
{
	_clients.erase(client);
	if (client->get_peer())
	{
		_connected_clients.erase(client->get_peer());
		_connect_clients.erase(client->get_peer());
	}
}
void udp_client_manager::on_try_connect(udp_client* client)
{
	remove_client(client);
	_connect_clients[client->get_peer()] = client;
}


void udp_client_manager::init(int client_count)
{
	net_global::udp_net_init(nullptr, client_count, 2);
}

void udp_client_manager::on_enet_connected(ENetEvent& event)
{
	auto it = _connect_clients.find(event.peer);
	if (it != _connect_clients.end())
	{
		udp_client* client = it->second;
		_connect_clients.erase(it);
		_clients.erase(client);
		_connected_clients[client->get_peer()] = client;
		client->handle_connect();
	}
}

void udp_client_manager::on_enet_receive(ENetEvent& event)
{
	auto it = _connected_clients.find(event.peer);
	if (it != _connected_clients.end())
	{
		udp_client* client = it->second;
		client->receive((char*)event.packet->data, event.packet->dataLength);
	}

}
void udp_client_manager::on_enet_disconnect(ENetEvent& event)
{
	auto it = _connected_clients.find(event.peer);
	if (it != _connected_clients.end())
	{
		udp_client* client = it->second;
		client->handle_close();
		_connected_clients.erase(it);
		_connect_clients.erase(client->get_peer());
		if (_clients.count(client) == 0)
		{
			_clients.insert(client);
		}		
	}
}
void udp_client_manager::extra_process(bool is_wait)
{
	for (auto entity : _connected_clients)
	{
		entity.second->extra_process(false);
	}

	for (auto entity : _connect_clients)
	{
		entity.second->extra_process(false);
	}
	if (is_wait)
	{
		cpu_wait();
	}
}

ENetHost* udp_client_manager::get_host()
{
	return net_global::get_enet_host();
}