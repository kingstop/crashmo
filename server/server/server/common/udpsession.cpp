/*
#include "udpsession.h"
#include "asiodef.h"
static char static_compress_buffer[RECEIVEBUFFLENGTH];
udp_session::udp_session(u32 connect_id, u32 connect_index,
	u32 remote_host, u16 remote_port, const char* ip) :
	_recive_buffer_pos(0), m_not_sent_size(0)
{
	memset(_recive_buff, 0, sizeof(_recive_buff));
	_connect_id = connect_id;
	_connect_index = connect_index;
	_host = remote_host;
	_port = remote_port;
	_ip = ip;


}

udp_session::~udp_session()
{
}

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
