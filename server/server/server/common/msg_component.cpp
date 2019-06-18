#include "msg_component.h"
#include "message_interface.h"
#include "basesession.h"

msg_component::msg_component(bool is_server):m_current_recv_queue(0), _is_server(is_server)
{

}
void msg_component::_clear_recv_msg()
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

void msg_component::push_message(message_t* msg)
{
	boost::mutex::scoped_lock lock(m_msg_mutex);
	m_queue_recv_msg[m_current_recv_queue].push(msg);
}
void msg_component::proc_message(const message_t& msg)
{

}

void msg_component::run(bool wait_cpu /* = false */)
{
	//m_cb_mgr.poll();
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
		if (_is_server)
		{
			msg->from->_proc_message(*msg);
		}
		else
		{
			proc_message(*msg);
		}
		net_global::free_message(msg);
		m_queue_recv_msg[proc_index].pop();
	}
}