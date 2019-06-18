#include "basesession.h"
#include "enet/enet.h"

class udp_session : public base_session
{

public:
	udp_session();
	virtual ~udp_session();
	virtual void on_connect(ENetPeer* peer, u32 connect_index,
		u32 remote_host, u16 remote_port, const char* ip);
	inline u32 get_connect_id() { return _connect_id; }
	inline u32 get_connect_index() { return _connect_index; }
	void receive(const char* receive_data, std::size_t length);
	inline void set_father(base_server* father) { m_father = father; }
	inline const u32* get_connect_index_data() { return &_connect_index; }
	
public:

	void _write_message();

protected:
	virtual bool _uncompress_message(char* data);
	virtual void _send_message(message_t* msg);
	virtual ENetHost* get_host();
protected:
	std::string _ip;
	u32 _connect_id;
	u32 _connect_index;
	u16 _port;
	ENetPeer * _peer;
	char _uncompress_buffer[MAX_MESSAGE_LEN];
};



/*


#pragma once
#include <iostream>
#include <queue>
#include "asiodef.h"
#include "common_type.h"
#define  MAXSINGLEUDPBUFF 65535
#define  RECEIVEBUFFLENGTH MAXSINGLEUDPBUFF * 2
#include "basesession.h"
class udp_session : public base_session
{

public:
	udp_session(u32 connect_id, u32 connect_index,
		u32 remote_host, u16 remote_port, const char* ip);
	virtual ~udp_session();
public:
	void Receive(const char* receive_data, u64 length);
	void Closed();
	virtual void _proc_message(const message_t& msg);
	void send_message(const void* data, const unsigned int len, bool base64);
	bool is_connected();
protected:
	void _ReadSome();
protected:
	bool _uncompress_message(char* data);
	message_t* _make_message(const void* data, message_len len, bool base64);
	message_t* _compress_message(const void* data, message_len len, int t_idx, bool base64);
	void _send_message(message_t* msg);
	void _write_message();
	void _send();

protected:
	unsigned int _connect_id;
	std::string _ip;
	char _recive_buff[RECEIVEBUFFLENGTH];
	char _send_buff[MAXSINGLEUDPBUFF];
	std::size_t _recive_buffer_pos;
	unsigned long _connect_index;
	unsigned int _host;
	unsigned short _port;
	std::queue<message_t*> m_queue_send_msg;
	boost::mutex m_mutex;
	volatile long m_not_sent_size;
};
*/
