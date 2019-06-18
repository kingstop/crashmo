#include "base_server.h"
#include "tcpsession.h"
#include "task_thread_pool.h"
#include "crypt.h"
#include <stdarg.h>
#include "io_service_pool.h"
#include "message_interface.h"
static unsigned int s_io_once_listen_session_count = 40;


base_server::base_server(int id) : m_id(id), m_poolcount(0), m_thread_buffer(NULL), m_thread_count(0), m_cur_thread_index(0),_port(0),
m_ttti_mode(false), m_accepting_count(0), m_limit_mode(false), m_connection_count(0), m_fp_connection_log(NULL), m_security(true), msg_component(true)
{
	m_ttp = new task_thread_pool;
	m_unix_time = (unsigned int)time(NULL);

	m_last_log_connection_time = m_unix_time;
	m_last_clean_idle_ip_time = m_unix_time;

}


base_server::~base_server()
{
}

bool base_server::create(unsigned short port, unsigned int poolcount, int thread_count)
{

	_port = port;
	m_ttp->startup(thread_count);
	if (m_thread_buffer)
		delete[] m_thread_buffer;
	m_thread_buffer = new char[THREAD_BUFFER_SIZE * thread_count];
	m_thread_count = thread_count;

	m_poolcount = poolcount;

	if (m_poolcount == 0)
	{
		m_poolcount = 20;
	}

	for (unsigned int i = 0; i < m_poolcount; ++i)
	{
		free_session(create_session());
	}

	return true;
}

bool base_server::handle_accept(base_session* p)
{
	bool ret = true;
	{
		//boost::mutex::scoped_lock lock(m_proc_mutex);
		//--m_accepting_count;
	}
	if (is_ban_ip(p->get_remote_address_ui()))
	{
		p->handle_close();
		free_session(p);
		ret = false;		
	}
	return ret;
}

void base_server::handle_accept(base_session* p, const boost::system::error_code& error)
{	
	if (handle_accept(p) == false)
	{
		return;
	}

	if (!error)
	{
		//p->get_io_service().post(boost::bind(&tcp_session::handle_accept, p, this));
	}
	else
	{
		p->handle_close();
		free_session(p);
	}

}

void base_server::free_session(base_session* p)
{
	boost::mutex::scoped_lock lock(m_proc_mutex);
	p->reset();
	p->set_valid(false);
	m_sessions.push_back(p);
}

/*
void base_server::push_message(message_t* msg)
{
	boost::mutex::scoped_lock lock(m_msg_mutex);
	m_queue_recv_msg[m_current_recv_queue].push(msg);
}
*/

void base_server::push_task(task* p)
{
	m_ttp->push_task(p);
}

char* base_server::get_thread_buffer(int index)
{
	return m_thread_buffer + index * THREAD_BUFFER_SIZE;
}

int base_server::generate_thread_index()
{
	boost::mutex::scoped_lock lock(m_proc_mutex);
	int i = m_cur_thread_index;
	if (++m_cur_thread_index >= m_thread_count)
		m_cur_thread_index = 0;

	if (i >= m_thread_count)
		return 0;
	else
		return i;
}

bool base_server::is_ban_ip(unsigned int addr)
{
	boost::mutex::scoped_lock lock(m_ban_mutex);
	std::map<unsigned int, std::pair<unsigned int, net_global::ban_reason_t> >::iterator it = m_banip.find(addr);
	if (it != m_banip.end())
	{
		if (it->second.first < m_unix_time)
		{
			m_banip.erase(it);
			return false;
		}
		else
			return true;
	}
	else
		return false;
}

void base_server::add_ban_ip(unsigned int addr, unsigned int sec, net_global::ban_reason_t br)
{
	char fn[64] = { 0 };
	sprintf(fn, "connection%d.log", m_id);
	m_fp_connection_log = fopen(fn, "a");
	if (m_fp_connection_log)
	{
		time_t tnow = m_unix_time;
		tm* ptm = localtime(&tnow);
		in_addr ad;
		ad.s_addr = addr;
		char* ip = inet_ntoa(ad);

		fprintf(m_fp_connection_log, "%04d-%02d-%02d %02d:%02d:%02d|Banned IP:[%s], reason:%d\n",
			ptm->tm_year + 1900, ptm->tm_mon + 1, ptm->tm_mday, ptm->tm_hour, ptm->tm_min, ptm->tm_sec,
			ip, br);

		fclose(m_fp_connection_log);
	}

	boost::mutex::scoped_lock lock(m_ban_mutex);
	std::map<unsigned int, std::pair<unsigned int, net_global::ban_reason_t> >::iterator it = m_banip.find(addr);
	if (it != m_banip.end())
	{
		if (it->second.first < sec)
		{
			it->second.first = sec;
			it->second.second = br;
		}
	}
	else
		m_banip[addr] = std::make_pair(m_unix_time + sec, br);
}

void base_server::add_ban_ip(const std::string& addr, unsigned int sec, net_global::ban_reason_t br)
{
	unsigned int iaddr = boost::asio::ip::address_v4::from_string(addr).to_ulong();
	add_ban_ip(iaddr, sec, br);
}

void base_server::remove_ban_ip(unsigned int addr)
{
	boost::mutex::scoped_lock lock(m_ban_mutex);
	m_banip.erase(addr);
}

void base_server::run()
{
	_real_run(true);
}

void base_server::run_no_wait()
{
	_real_run(false);
}

void base_server::stop()
{
	m_sessions.clear();


	if (m_ttp != NULL)
	{
		m_ttp->shutdown();
	}

	if (m_ttp)
	{
		delete m_ttp;
		m_ttp = NULL;
	}

	if (m_thread_buffer)
	{
		delete[] m_thread_buffer;
		m_thread_buffer = NULL;
	}

}

void base_server::_real_run(bool is_wait)
{
	m_unix_time = (unsigned int)time(NULL);
	/*

	{
		boost::mutex::scoped_lock lock(m_proc_mutex);
		while (m_accepting_count < s_io_once_listen_session_count)
		{
			if (m_sessions.size() == 0)
				break;
			base_session* p = m_sessions.front();
			p->set_valid(true);
			//ÐÂµÄÁ´½Ó

			//m_acceptor->async_accept(p->socket(), boost::bind(&tcp_server::handle_accept, this, p, boost::asio::placeholders::error));
			m_sessions.pop_front();
			++m_accepting_count;
		}
	}
	*/
	m_cb_mgr.poll();
	msg_component::run(is_wait);
	/**/

	//int proc_index = 0;
	//m_msg_mutex.lock();
	//if (m_queue_recv_msg[m_current_recv_queue].empty())
	//{
	//	m_msg_mutex.unlock();
	//	if (is_wait)
	//		cpu_wait();
	//	return;
	//}
	//proc_index = m_current_recv_queue;
	//m_current_recv_queue = !m_current_recv_queue;
	//m_msg_mutex.unlock();

	//while (!m_queue_recv_msg[proc_index].empty())
	//{
	//	message_t* msg = m_queue_recv_msg[proc_index].front();
	//	msg->from->_proc_message(*msg);
	//	/*
	//	if( msg->from->is_valid() && msg->from->is_connected() )
	//		msg->from->proc_message( *msg );
	//	*/
	//	net_global::free_message(msg);
	//	m_queue_recv_msg[proc_index].pop();
	//}
}
