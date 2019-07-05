#ifndef ASIO_TCPSERVER_HEAD
#define ASIO_TCPSERVER_HEAD


#include "asiodef.h"
#include "base_server.h"
class tcp_session;

struct task;
class task_thread_pool;

class tcp_server : public base_server
{
public:
	tcp_server( int id );
	virtual ~tcp_server();
public:
	virtual bool create( unsigned short port, unsigned int poolcount, int thread_count );
	void handle_accept( tcp_session* p, const boost::system::error_code& error );
public:
	virtual void stop();
protected:
	void _real_run( bool is_wait );
	tcp::acceptor* m_acceptor;

};

#endif
