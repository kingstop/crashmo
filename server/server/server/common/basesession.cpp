#include "basesession.h"
#include "asiodef.h"
#include "base_server.h"
#include "task_thread_pool.h"


base_session::base_session() :m_send_crypt_key(0), m_recv_crypt_key(0)
{
}


base_session::~base_session()
{
}

static char static_compress_buffer[MAX_MESSAGE_LEN];

void base_session::send_message(const void* data, const unsigned int len, bool base64)
{
	if (!is_valid() || !m_isconnected || !data || !len || len > MAX_MESSAGE_LEN - 1 /*|| m_send_crypt_key == 0*/)
		return;

	if (m_father)
	{
		if (m_iscompress)
		{
			int ti = 0;
			if (m_father->is_thread_task_transfer_id_mode())
			{
				unsigned int transferid = *(unsigned int*)((const char*)data + 4);
				ti = transferid % m_father->get_task_thread_count();
			}
			else
				ti = get_thread_index();

			task* t = NULL;
			t = new compress_send_task(data, len, this, ti, base64);
			m_father->push_task(t);
		}
		else
			_try_send_message(_make_message(data, len, base64));
	}
	else
	{
		_try_send_message(_compress_message(data, len, -1, base64));
	}
}

std::string base_session::get_remote_address_string() const
{
	if (m_remote_ip_str.length())
		return m_remote_ip_str;
	return "";
}


void base_session::close_and_ban()
{
	if (m_father)
		m_father->add_ban_ip(this->get_remote_address_ui(), 5, net_global::BAN_REASON_HACK_PACKET);
	this->close();
	net_global::write_close_log("IP:[%s] recv hack packet. ban for 5 seconds", this->get_remote_address_string().c_str());
}


void base_session::push_message(message_t* msg)
{
	if (m_father)
		m_father->push_message(msg);
}


bool base_session::_write_message(std::size_t& len)
{
	boost::mutex::scoped_lock lock(m_mutex);
	//m_not_sent_size -= sent_size;
	len = 0;
	if (!is_connected())
		return false;

	// optimize for sending-data frequently
	if (m_is_sending_data)
		return false;

	if (m_queue_send_msg.empty())
		return false;

	
	while (!m_queue_send_msg.empty())
	{
		message_t* msg = m_queue_send_msg.front();
		if (len + msg->len <= MAX_MESSAGE_LEN)
		{
			memcpy(m_sending_data + len, msg->data, msg->len);
			len += msg->len;
			m_queue_send_msg.pop();
			net_global::free_message(msg);
		}
		else
			break;
	}
	m_not_sent_size -= len;
	return true;
}

bool base_session::_read_some(const char* buff, std::size_t bytes_transferred)
{	
	if (is_valid() == false)
	{
		return false;
	}
	memcpy(m_recv_buffer + m_recive_buffer_pos, buff, bytes_transferred);
	m_recive_buffer_pos += bytes_transferred;
	std::size_t now_pos = 0;
	if (m_recive_buffer_pos >= MESSAGE_HEAD_LEN)
	{
		message_len len = *(message_len *)m_recv_buffer;
		if (len < MESSAGE_HEAD_LEN || len > MAX_MESSAGE_LEN)
		{
			close_and_ban();
			return false;
		}
		while (m_recive_buffer_pos - now_pos >= len)
		{
			if (!_uncompress_message(m_recv_buffer + now_pos))
			{
				close_and_ban();
				return false;
			}
			now_pos += len;
			if (m_recive_buffer_pos - now_pos >= MESSAGE_HEAD_LEN)
			{
				len = *(message_len*)(m_recv_buffer + now_pos);

				if (len < MESSAGE_HEAD_LEN || len > MAX_MESSAGE_LEN)
				{
					close_and_ban();
					return false;
				}
			}
			else
				break;
		}
		if (now_pos > 0 && m_recive_buffer_pos > now_pos)
			memmove(m_recv_buffer, m_recv_buffer + now_pos, m_recive_buffer_pos - now_pos);
		m_recive_buffer_pos -= now_pos;
	}
	return true;
}

void base_session::handle_read_some(std::size_t bytes_transferred)
{
	if (is_valid() == false)
	{
		return;
	}
	_read_some(buffer_, bytes_transferred);
}

message_t* base_session::_compress_message(const void* data, message_len len, int t_idx, bool base64)
{
	message_len size = MAX_MESSAGE_LEN;
	char* buffptr = static_compress_buffer;
	return message_interface::compress(this, (const char*)data, len, buffptr, size, &m_send_crypt_key, base64);

}
message_t* base_session::_make_message(const void* data, message_len len, bool base64)
{
	return message_interface::makeMessage(this, (const char*)data, len, &m_send_crypt_key, false, base64);
}

unsigned int base_session::get_remote_address_ui() const
{
	if (m_remote_ip_ui)
		return m_remote_ip_ui;
	return 0;
}

void base_session::reset()
{
	m_recive_buffer_pos = 0;
	m_isconnected = false;
	m_father = NULL;
	m_isvalid = false;
	m_thread_index = 0;
	m_is_sending_data = false;
	m_isclosing = false;
	m_remote_ip_str.clear();
	m_remote_ip_ui = 0;

	m_not_sent_size = 0;
}



bool base_session::is_valid()
{
	return interlocked_read(&m_isvalid) != 0;
}

void base_session::set_valid(bool b)
{
	interlocked_write(&m_isvalid, b ? 1 : 0);
}

void base_session::handle_close()
{
	m_isconnected = false;
	m_thread_index = 0;
}

void base_session::close()
{

}

bool base_session::_try_send_message(message_t* msg)
{
	if (!is_connected())
		return false;

	{
		boost::mutex::scoped_lock lock(m_mutex);
		m_queue_send_msg.push(msg);
		m_not_sent_size += msg->len;
		if (m_father && m_father->is_limit_mode())
		{
			if (m_not_sent_size >= QUEUE_MESSAGE_LIMIT_SIZE)
			{
				close();
				net_global::write_close_log( "IP:[%s] close, message queue is full", this->get_remote_address_string().c_str() );
				return false;
			}
		}
	}
	return true;
}
