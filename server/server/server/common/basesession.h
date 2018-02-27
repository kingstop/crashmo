#pragma once
#include "message_interface.h"
class base_session
{
public:
	base_session();
	virtual ~base_session();
public:
	virtual void _proc_message(const message_t& msg) = 0;
	virtual void proc_message(const message_t& msg) = 0;
protected:
	unsigned char m_send_crypt_key;
	unsigned char m_recv_crypt_key;
};

