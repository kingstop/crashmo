#pragma once
#include "asiodef.h"

class msg_component
{
public:
	msg_component(bool is_server);
	virtual void run(bool wait_cpu = false);
	virtual void proc_message(const message_t& msg);
	virtual void push_message(message_t* msg);

protected:
	//volatile unsigned int m_last_reconnect_time;
	//volatile bool m_isconnecting;
	std::queue<message_t*> m_queue_recv_msg[2];
	int m_current_recv_queue;
	void _clear_recv_msg();
	boost::mutex m_msg_mutex;
	bool _is_server;

	//boost::condition m_conn_cond;
	//boost::mutex m_conn_mutex;
	//call_back_mgr m_cb_mgr;
};

		