#include "basesession.h"
#include "asiodef.h"
#include "base_server.h"
#include "task_thread_pool.h"
class base_session;
struct compress_send_task : public task
{
	compress_send_task(const void* src, unsigned short l, base_session* s, int ti, bool base64) : len(l), session(s), thread_index(ti), _base64(base64)
	{
		data = (char*)malloc(len);
		memcpy(data, src, len);
	}
	virtual void execute()
	{
		if (session->is_valid())
			result = session->_compress_message(data, len, get_thread_index(), _base64);
	}

	virtual void end()
	{
		if (session->is_valid() && session->is_connected())
			session->_send_message(result);
	}

	~compress_send_task()
	{
		free(data);
	}

	virtual int get_thread_index()
	{
		return thread_index;
	}

private:
	char* data;
	unsigned short len;
	message_t* result;
	base_session* session;
	int thread_index;
	bool _base64;
};


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
			_send_message(_make_message(data, len, base64));
	}
	else
	{
		_send_message(_compress_message(data, len, -1, base64));
	}
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

}

void base_session::close()
{

}

void base_session::_send_message(message_t* msg)
{
	if (!is_connected())
		return;

	{
		boost::mutex::scoped_lock lock(m_mutex);
		m_queue_send_msg.push(msg);
		m_not_sent_size += msg->len;
		if (m_father && m_father->is_limit_mode())
		{
			if (m_not_sent_size >= QUEUE_MESSAGE_LIMIT_SIZE)
			{
				close();
				//net_global::write_close_log( "IP:[%s] close, message queue is full", this->get_remote_address_string().c_str() );
				return;
			}
		}
	}

	//m_io_service.post(boost::bind(&tcp_session::_write_message, this, 0));
}
