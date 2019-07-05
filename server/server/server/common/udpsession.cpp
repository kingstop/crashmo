
#include "udpsession.h"
#include "asiodef.h"
#include "base_server.h"
#include "enet/enet.h"
#include "udp_server.h"

udp_session::udp_session(): _connect_id(0), _connect_index(0), _port(0), _peer(nullptr)
{
	//memset(_uncompress_buffer, 0, sizeof(_uncompress_buffer));
}

udp_session::~udp_session()
{
}

void udp_session::close()
{
	if (_peer)
	{
		enet_peer_disconnect(_peer, 0);
	}
	
}

void udp_session::reset()
{
	base_session::reset();

}

void udp_session::on_close()
{

}
void udp_session::on_connect()
{

}

void udp_session::handle_close()
{
	auto _call_back = _get_cb_mgr();
	if (_call_back)
	{
		_call_back->add_cb(&udp_session::on_close, this);
	}

	m_isconnected = false;
}

void udp_session::handle_connect(ENetPeer* peer, u32 connect_index,
	u32 remote_host, u16 remote_port, const char* ip)
{
	_peer = peer;
	_connect_id = _peer->connectID;
	_connect_index = connect_index;
	m_remote_ip_ui = remote_host;
	_port = remote_port;
	m_remote_ip_str = ip;
	set_valid(true);
	m_isconnected = true;
	auto _call_back = _get_cb_mgr();
	if (_call_back)
	{
		_call_back->add_cb(&udp_session::on_connect, this);
	}
}

void udp_session::receive(const char* receive_data, std::size_t length)
{
	_read_some(receive_data, length);	
}

char* udp_session::get_uncompress_buffer() 
{
	if (m_father)
	{
		udp_server* server = dynamic_cast<udp_server*>(m_father);
		if (server)
		{
			return server->get_uncompress_buffer();
		}
	}
	return nullptr;
}

bool udp_session::_uncompress_message(char* data)
{
	message_len size = MAX_MESSAGE_LEN;
	message_t* msg = message_interface::uncompress(this, data, &m_recv_crypt_key, get_uncompress_buffer(), size);
	if (NULL == msg)
	{
		if (m_father)
		{
			m_father->add_ban_ip(this->get_remote_address_ui(), 120, net_global::BAN_REASON_WRONG_CHECK_SUM);
		}
		this->close();
		return false;
	}
	push_message(msg);
	return true;
}

void udp_session::_send_message(message_t* msg)
{
	_try_send_message(msg);
}
call_back_mgr* udp_session::_get_cb_mgr()
{
	if (m_father)
	{
		return m_father->get_cb_mgr();
	}
	return nullptr;
}

ENetHost* udp_session::get_host()
{
	ENetHost* host = nullptr;
	if (m_father)
	{
		udp_server* pserver = dynamic_cast<udp_server*>(m_father);
		host = pserver->get_host();
	}
	return host;
}

void udp_session::_write_message()
{

	std::size_t len = 0;
	
	if (base_session::_write_message(len))
	{
		unsigned int head_len = strlen("packet") + 1;
		ENetPacket * packet = enet_packet_create(m_sending_data,

			len,

			ENET_PACKET_FLAG_RELIABLE);
		int ret = enet_peer_send(_peer, 1, packet);

		enet_host_flush(get_host());
	}
	
} 
