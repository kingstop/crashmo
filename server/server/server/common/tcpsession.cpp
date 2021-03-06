#include "tcpsession.h"
#include "tcpserver.h"
#include "crypt.h"
#include "task_thread_pool.h"
#include "io_service_pool.h"
#include "message_interface.h"
class base_server;
class  tcp_session;
//
//struct compress_send_task : public task
//{
//	compress_send_task( const void* src, unsigned short l, tcp_session* s, int ti, bool base64 ) : len( l ), session( s ), thread_index( ti ), _base64(base64)
//	{
//		data = (char*)malloc( len );
//		memcpy( data, src, len );
//	}
//	virtual void execute()
//	{
//		if( session->is_valid() )
//			result = session->_compress_message( data, len, get_thread_index(), _base64 );
//	}
//
//	virtual void end()
//	{
//		if( session->is_valid() && session->is_connected() )
//			session->_send_message( result );
//	}
//
//	~compress_send_task()
//	{
//		free( data );
//	}
//
//	virtual int get_thread_index()
//	{
//		return thread_index;
//	}
//
//private:
//	char* data;
//	unsigned short len;
//	message_t* result;
//	tcp_session* session;
//	int thread_index;
//	bool _base64;
//};
/**/
tcp_session::tcp_session( boost::asio::io_service& is )
: m_socket( NULL ), m_io_service( is )
{
	memset(m_sending_data, 0, sizeof(m_sending_data));
	memset(m_recv_buffer, 0, sizeof(m_recv_buffer));
	memset(buffer_, 0, sizeof(buffer_));
}

//void tcp_session::set_base64(bool b)
//{
//	_base64 = b;
//}

tcp_session::~tcp_session()
{
	_clear_send_msg();
}

//bool tcp_session::is_valid()
//{
//	return interlocked_read( &m_isvalid ) != 0;
//}
//
//void tcp_session::set_valid( bool b )
//{
//	interlocked_write( &m_isvalid, b ? 1 : 0 );
//}

void tcp_session::on_accept( tcp_server* p )
{
}

void tcp_session::handle_accept( tcp_server* p )
{
	set_valid( true );
	m_recive_buffer_pos = 0;
	m_isconnected = true;
	m_father = p;

	//m_send_crypt_key = rand() % 255 + 1;
	//m_recv_crypt_key = ;
	//send_message( &m_send_crypt_key, 1 );
	begin_read_message();
	m_thread_index = p->generate_thread_index();

	m_remote_ip_str = get_remote_address_string();
	m_remote_ip_ui = get_remote_address_ui();

	_get_cb_mgr()->add_cb( &tcp_session::on_accept, this, p );

	static_cast<advanced_io_service&>( m_io_service ).add_session( this );
}

std::string tcp_session::get_remote_address_string() const
{
	if( m_remote_ip_str.length() )
		return m_remote_ip_str;

	try
	{
		return m_socket->remote_endpoint().address().to_string();
	}
	catch( ... )
	{
		return "";
	}
}

unsigned int tcp_session::get_remote_address_ui() const
{
	if( m_remote_ip_ui )
		return m_remote_ip_ui;

	try
	{
		return m_socket->remote_endpoint().address().to_v4().to_ulong();
	}
	catch( ... )
	{
		return 0;
	}
}

unsigned short tcp_session::get_remote_port() const
{
	try
	{
		return m_socket->remote_endpoint().port();
	}
	catch( ... )
	{
		return 0;
	}
}

//void tcp_session::send_message( const void* data, const unsigned int len, bool base64)
//{
//	if( !is_valid() || !m_isconnected || !data || !len || len > MAX_MESSAGE_LEN - 1 /*|| m_send_crypt_key == 0*/ )
//		return;
//
//	if( m_father )
//	{
//		if( m_iscompress )
//		{
//			int ti = 0;
//			if( m_father->is_thread_task_transfer_id_mode() )
//			{
//				unsigned int transferid = *(unsigned int*)( (const char*)data + 4 );
//				ti = transferid % m_father->get_task_thread_count();
//			}
//			else
//				ti = get_thread_index();
//
//			task* t = NULL;
//			t = new compress_send_task( data, len, this, ti, base64);
//			m_father->push_task( t );
//		}
//		else
//			_send_message( _make_message( data, len, base64) );
//	}
//	else
//	{	
//		_send_message(_compress_message( data, len, -1, base64) );
//	}
//}

void tcp_session::close()
{
	m_io_service.post( boost::bind( &tcp_session::handle_close, this ) );
	base_session::close();
}

//void tcp_session::close_and_ban()
//{
//	if( m_father )
//		m_father->add_ban_ip( this->get_remote_address_ui(), 5, net_global::BAN_REASON_HACK_PACKET );
//	this->close();
//	net_global::write_close_log( "IP:[%s] recv hack packet. ban for 5 seconds", this->get_remote_address_string().c_str() );
//}

void tcp_session::handle_close()
{
	base_session::handle_close();
	if (m_socket != NULL)
	{
		boost::system::error_code ignored_ec;
		m_socket->shutdown(boost::asio::ip::tcp::socket::shutdown_both, ignored_ec);

		try
		{
			m_socket->close();
		}
		catch (const boost::system::error_code&)
		{

		}
	}

	//m_send_crypt_key = 0;
	//m_recv_crypt_key = 0;


	//m_isconnected = false;
	//m_thread_index = 0;
}

void tcp_session::run()
{
}

call_back_mgr* tcp_session::_get_cb_mgr()
{
	if( m_father )
		return m_father->get_cb_mgr();
	else
	{
		//assert( 0 );
		return NULL;
	}
}

bool tcp_session::_try_send_message( message_t* msg )
{
	bool ret = false;
	if (base_session::_try_send_message(msg))
	{
		m_io_service.post(boost::bind(&tcp_session::_write_message, this, (size_t)0));
		ret = true;
	}
	return ret;
	//if( !is_connected() )
	//	return;

	//{
	//	boost::mutex::scoped_lock lock( m_mutex );
	//	m_queue_send_msg.push( msg );
	//	m_not_sent_size += msg->len;
	//	if( m_father && m_father->is_limit_mode() )
	//	{
	//		if( m_not_sent_size >= QUEUE_MESSAGE_LIMIT_SIZE )
	//		{
	//			close();
	//			//net_global::write_close_log( "IP:[%s] close, message queue is full", this->get_remote_address_string().c_str() );
	//			return;
	//		}
	//	}
	//}

	
}

void tcp_session::begin_read_message()
{
	m_recive_buffer_pos = 0;
	m_socket->async_read_some(boost::asio::buffer(buffer_, sizeof(buffer_)),
		boost::bind(&tcp_session::handle_read_some, this,
		boost::asio::placeholders::error,
		boost::asio::placeholders::bytes_transferred));
}

bool tcp_session::_uncompress_message( char* data )
{
	message_len size = MAX_MESSAGE_LEN;
	message_t* msg  = message_interface::uncompress(this, data, &m_recv_crypt_key, static_cast<advanced_io_service&>( m_io_service ).get_uncompress_buffer(), size );
	if (NULL == msg)
	{
		if( m_father )
		{
			m_father->add_ban_ip( this->get_remote_address_ui(), 120, net_global::BAN_REASON_WRONG_CHECK_SUM );
		}
		this->close();
		return false;
	}
	
	push_message( msg );
	return true;
}

static char static_compress_buffer[MAX_MESSAGE_LEN];

message_t* tcp_session::_compress_message( const void* data, message_len len, int t_idx, bool base64)
{
	message_len size = MAX_MESSAGE_LEN;
	char* buffptr = NULL;
	if( m_father && t_idx >= 0 )
		buffptr = m_father->get_thread_buffer( t_idx );
	else
		buffptr = static_compress_buffer;

	return message_interface::compress(this, (const char*)data, len, buffptr, size ,&m_send_crypt_key, base64);
}


void tcp_session::_clear_send_msg()
{
	boost::mutex::scoped_lock lock( m_mutex );
	while( !m_queue_send_msg.empty() )
	{
		net_global::free_message( m_queue_send_msg.front() );
		m_queue_send_msg.pop();
	}
	m_not_sent_size = 0;
}

void tcp_session::_on_close( const boost::system::error_code& error )
{
	//m_send_crypt_key = 0;
	//m_recv_crypt_key = 0;
	_clear_send_msg();

	boost::system::error_code ignored_ec;
	m_socket->shutdown(boost::asio::ip::tcp::socket::shutdown_both, ignored_ec);

	try
	{
		m_socket->close();
	}
	catch( const boost::system::error_code& )
	{
	}
	m_isconnected = false;

	if( _get_cb_mgr() )
	{
		_get_cb_mgr()->add_cb( &tcp_session::on_close, this, error );
	}
}

bool tcp_session::_write_message(std::size_t& len)
{
	bool ret = false;
	if (base_session::_write_message(len))
	{
		boost::asio::async_write(*m_socket,
			boost::asio::buffer(m_sending_data,
				len),
			boost::bind(&tcp_session::handle_write, this,
				boost::asio::placeholders::error, len, 0));

		m_is_sending_data = true;
		ret = true;
	}
	return ret;
}

void tcp_session::handle_read_some( const boost::system::error_code& error, std::size_t bytes_transferred )
{
	if (!error)
	{
		_read_some(buffer_, bytes_transferred);
		m_socket->async_read_some(boost::asio::buffer(buffer_, sizeof(buffer_)),
			boost::bind(&tcp_session::handle_read_some, this,
				boost::asio::placeholders::error,
				boost::asio::placeholders::bytes_transferred));
	}
	else
	{
		if (is_valid())
		{
			_on_close(error);
		}
	}
}
void tcp_session::handle_write( const boost::system::error_code& error, std::size_t size, int block_idx )
{
	if( !is_valid() )
		return;

	if( !error )
	{
		{
			boost::mutex::scoped_lock lock( m_mutex );
			m_is_sending_data = false;
		}
		_write_message( size );
	}
	else
		_on_close( error );
}

void tcp_session::on_close( const boost::system::error_code& error )
{
	if( m_father )
	{
		m_father->free_session( this );
		static_cast<advanced_io_service&>( m_io_service ).del_session( this );
	}
}



void tcp_session::reset()
{
	base_session::reset();
	_clear_send_msg();
	if( m_socket )
		delete m_socket;
	m_socket = new tcp::socket( m_io_service );
}

void tcp_session::_proc_message(const message_t& msg)
{
	if (is_valid() && is_connected())
	{
		proc_message(msg);
	}
		
}