#ifndef ASIO_TCPSESSION_HEAD
#define ASIO_TCPSESSION_HEAD
#include "basesession.h"
#include "asiodef.h"

class tcp_server;


class tcp_session : public base_session
{
public:
	tcp_session( boost::asio::io_service& is );
	virtual ~tcp_session();
public:
	virtual void on_accept( tcp_server* p );
	virtual void on_close( const boost::system::error_code& error );
	virtual void _proc_message(const message_t& msg);
	virtual void reset();
	virtual void run();
public:
	inline tcp::socket& socket() { return *m_socket; }
	inline int get_thread_index() const { return m_thread_index; }
	inline boost::asio::io_service& get_io_service() { return m_io_service; }
	virtual std::string get_remote_address_string() const;
	unsigned int get_remote_address_ui() const;
	unsigned short get_remote_port() const;
	virtual void close();
public:
	void handle_read_some( const boost::system::error_code& error, std::size_t bytes_transferred );
	void handle_write( const boost::system::error_code& error, std::size_t size, int block_idx );
	void handle_accept( tcp_server* p );
	virtual void handle_close();
	virtual bool _write_message(std::size_t& len);
protected:
	virtual call_back_mgr* _get_cb_mgr();
	friend struct compress_send_task;
	friend struct optimized_compress_send_task;
	virtual bool _try_send_message(message_t* msg);
	void begin_read_message();
	bool _uncompress_message( char* data );
	message_t* _compress_message( const void* data, message_len len, int t_idx, bool base64);
	void _clear_send_msg();
	void _on_close( const boost::system::error_code& error );
	boost::asio::io_service& m_io_service;
	tcp::socket* m_socket;
};

#endif
