#pragma once
#include "asiodef.h"

class msg_component
{
public:
	msg_component(bool is_server);
	virtual ~msg_component();
	virtual void run(bool wait_cpu = false);
	virtual void proc_message(const message_t& msg);
	virtual void push_message(message_t* msg);

protected:
	std::queue<message_t*> m_queue_recv_msg[2];
	int m_current_recv_queue;
	void _clear_recv_msg();
	boost::mutex m_msg_mutex;
	bool _is_server;
};

		