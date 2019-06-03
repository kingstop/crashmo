#include "client_base.h"



client_base::client_base()
{
}


client_base::~client_base()
{
}


void client_base::_clear_recv_msg()
{
	boost::mutex::scoped_lock lock(m_msg_mutex);
	for (int i = 0; i < 2; ++i)
	{
		while (!m_queue_recv_msg[i].empty())
		{
			net_global::free_message(m_queue_recv_msg[i].front());
			m_queue_recv_msg[i].pop();
		}
	}
}

void client_base::push_message(message_t* msg)
{
	boost::mutex::scoped_lock lock(m_msg_mutex);
	m_queue_recv_msg[m_current_recv_queue].push(msg);
}

void client_base::run(bool wait_cpu /* = false */)
{

	m_cb_mgr.poll();
	int proc_index = 0;
	m_msg_mutex.lock();	
	if (m_queue_recv_msg[m_current_recv_queue].empty())
	{		
		m_msg_mutex.unlock();
		if (wait_cpu)
		{
			cpu_wait();
		}		
		return;
	}
	proc_index = m_current_recv_queue;
	m_current_recv_queue = !m_current_recv_queue;
	m_msg_mutex.unlock();

	while (!m_queue_recv_msg[proc_index].empty())
	{
		message_t* msg = m_queue_recv_msg[proc_index].front();
		proc_message(*msg);
		net_global::free_message(msg);
		m_queue_recv_msg[proc_index].pop();
	}
}