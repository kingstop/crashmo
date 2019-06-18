
#include "udpsession.h"
#include "asiodef.h"
#include "base_server.h"
#include "enet/enet.h"
#include "udp_server.h"

udp_session::udp_session(): _connect_id(0), _connect_index(0), _port(0), _peer(nullptr)
{
	memset(_uncompress_buffer, 0, sizeof(_uncompress_buffer));
}

udp_session::~udp_session()
{
}


void udp_session::on_connect(ENetPeer* peer, u32 connect_index,
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

}

void udp_session::receive(const char* receive_data, std::size_t length)
{
	_read_some(receive_data, length);	
}

bool udp_session::_uncompress_message(char* data)
{
	message_len size = MAX_MESSAGE_LEN;
	message_t* msg = message_interface::uncompress(this, data, &m_recv_crypt_key, _uncompress_buffer, size);
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

	//下面开始发数据
	//ENetPacket* packet = enet_packet_create(NULL, 78, ENET_PACKET_FLAG_RELIABLE); //创建包
	//strcpy((char*)packet->data, "hi,哈哈");
	//enet_peer_send(_peer, 1, packet);

	//ENetPacket* packet1 = enet_packet_create(NULL, 86, ENET_PACKET_FLAG_RELIABLE); //创建包
	//strcpy((char*)packet1->data, "你好啊，呵呵");
	//enet_peer_send(_peer, 2, packet1);
	//_write_completed();
	//enet_host_flush(client); //必须使用这个函数或是enet_host_service来使数据发出去


	std::size_t len = 0;
	
	if (base_session::_write_message(len))
	{
		unsigned int head_len = strlen("packet") + 1;
		ENetPacket * packet = enet_packet_create(m_sending_data,

			len,

			ENET_PACKET_FLAG_RELIABLE);

		/* Extend the packet so and append the string "foo", so it now */

		/* contains "packetfoo\0" */

		//enet_packet_resize(packet, head_len + len);
		//memcpy(&packet->data[strlen("packet")], m_sending_data, len);

		

		/* Send the packet to the peer over channel id 0. */

		/* One could also broadcast the packet by */

		/* enet_host_broadcast (host, 0, packet); */

		enet_peer_send(_peer, 1, packet);

		enet_host_flush(get_host());
		//_write_completed();
		//enet_host_service()
		//enet_host_flush(host);
	}
	
} 


/*
bool udp_session::is_connected()
{
	return true;
}


void udp_session::_write_message()
{
	if (!is_connected())
		return;
	boost::mutex::scoped_lock lock(m_mutex);
	u32 send_size = 0;
	u32 current_size = 0;
	while (!m_queue_send_msg.empty())
	{
		message_t* msg = m_queue_send_msg.front();
		current_size = msg->len + send_size;
		if (current_size < MAXSINGLEUDPBUFF)
		{
			memcpy(_send_buff + send_size, msg->data, msg->len);
			send_size = current_size;
			m_queue_send_msg.pop();
			net_global::free_message(msg);
		}
		else
		{
			_send();
			send_size = 0;
		}
	}

	if (send_size != 0)
	{
		_send();
	}
}

void udp_session::_send()
{

}

void udp_session::_send_message(message_t* msg)
{
	if (!is_connected())
		return;
	{
		boost::mutex::scoped_lock lock(m_mutex);
		m_queue_send_msg.push(msg);
		m_not_sent_size += msg->len;
		if (m_not_sent_size >= QUEUE_MESSAGE_LIMIT_SIZE)
		{
			Closed();
		}
	}
}

message_t* udp_session::_compress_message(const void* data, message_len len, int t_idx, bool base64)
{
	char* buffptr = NULL;
	message_len size = MAX_MESSAGE_LEN;
	buffptr = static_compress_buffer;
	return message_interface::compress(this, (const char*)data, len, buffptr, size, &m_send_crypt_key, base64);

}

void udp_session::send_message(const void* data, const unsigned int len, bool base64)
{
	_send_message(_compress_message(data, len, -1, base64));
}

void udp_session::Receive(const char* receive_data, u64 length)
{
	std::size_t target_pos = _recive_buffer_pos + length;

	if (target_pos < RECEIVEBUFFLENGTH)
	{
		memcpy(_recive_buff + _recive_buffer_pos, receive_data, length);
		_recive_buffer_pos = target_pos;
		_ReadSome();
	}
	else
	{

	}
}
void udp_session::Closed()
{

}

void udp_session::_ReadSome()
{
	std::size_t now_pos = 0;
	//memcpy(m_recv_buffer + m_recive_buffer_pos, buffer_, bytes_transferred);
	//m_recive_buffer_pos += bytes_transferred;

	if (_recive_buffer_pos >= MESSAGE_HEAD_LEN)
	{
		message_len len = *(message_len *)_recive_buff;
		if (len < MESSAGE_HEAD_LEN || len > MAX_MESSAGE_LEN)
		{
			//close_and_ban();
			return;
		}
		while (_recive_buffer_pos - now_pos >= len)
		{
			if (!_uncompress_message(_recive_buff + now_pos))
			{
				//_on_close(error);
				return;
			}
			now_pos += len;
			if (_recive_buffer_pos - now_pos >= MESSAGE_HEAD_LEN)
			{
				len = *(message_len*)(_recive_buff + now_pos);

				if (len < MESSAGE_HEAD_LEN || len > MAX_MESSAGE_LEN)
				{
					//close_and_ban();
					return;
				}
			}
			else
				break;
		}
		if (now_pos > 0 && _recive_buffer_pos > now_pos)
			memmove(_recive_buff, _recive_buff + now_pos, _recive_buffer_pos - now_pos);
		_recive_buffer_pos -= now_pos;
	}

	//_recive_buff
}


void udp_session::_proc_message(const message_t& msg)
{
	proc_message(msg);
}



message_t* udp_session::_make_message(const void* data, message_len len, bool base64)
{
	return message_interface::makeMessage(this, (const char*)data, len, &m_send_crypt_key, false, base64);
}

bool udp_session::_uncompress_message(char* data)
{
	message_len size = MAXSINGLEUDPBUFF;
	//多线程的时候需要改进
	message_t* msg = message_interface::uncompress(this, data, &m_recv_crypt_key, static_compress_buffer, size);
	if (NULL == msg)
	{
		return false;
	}

	return true;
}

*/
