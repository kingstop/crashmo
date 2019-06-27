#pragma once
#include "message_interface.h"
#include "asiodef.h"
#include "common_type.h"
class base_server;


class base_session
{
public:
	base_session();
	virtual ~base_session();
public:
	virtual void _proc_message(const message_t& msg) = 0;
	virtual void proc_message(const message_t& msg) = 0;
	inline bool is_connected() const { return m_isconnected; }
	virtual void reset();
	unsigned int get_remote_address_ui() const;
	
	inline int get_thread_index() const { return m_thread_index; }
	virtual void close();
	virtual void handle_close();
	void set_base64(bool b) { _base64 = b; };
	bool get_base64() { return _base64; }
protected:
	virtual message_t* _compress_message(const void* data, message_len len, int t_idx, bool base64);
	virtual message_t* _make_message(const void* data, message_len len, bool base64);
	virtual void send_message(const void* data, const unsigned int len, bool base64);
	virtual bool _try_send_message(message_t* msg);
	virtual void handle_read_some(std::size_t bytes_transferred);
	virtual std::string get_remote_address_string() const;
	virtual bool _uncompress_message(char* data) = 0;
	void close_and_ban();
	virtual void push_message(message_t* msg);
	virtual bool _read_some(const char* buff, std::size_t bytes_transferred);
	virtual bool _write_message(std::size_t& len);
	virtual call_back_mgr* _get_cb_mgr() = 0;
public:
	boost::mutex m_proc_mutex;
	std::map<unsigned int, unsigned int> m_idleip;
	bool is_valid();
	void set_valid(bool b);
protected:
	friend struct compress_send_task;
	friend struct optimized_compress_send_task;
	unsigned char m_send_crypt_key;
	unsigned char m_recv_crypt_key;

	//boost::asio::io_service& m_io_service;
	volatile bool m_isconnected;
	base_server* m_father;
	std::queue<message_t*> m_queue_send_msg;
	char m_recv_buffer[MAX_MESSAGE_LEN * 2];
	char buffer_[MAX_MESSAGE_LEN];
	std::size_t m_recive_buffer_pos;
	bool m_iscompress;

	volatile boost::uint32_t m_isvalid;
	bool m_isclosing;
	int m_thread_index;
	std::string m_remote_ip_str;
	unsigned int m_remote_ip_ui;
	boost::mutex m_mutex;
	char m_sending_data[MAX_MESSAGE_LEN];
	volatile bool m_is_sending_data;
	volatile long m_not_sent_size;
	bool _base64;
};

