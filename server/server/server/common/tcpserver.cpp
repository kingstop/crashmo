#include "tcpserver.h"
#include "tcpsession.h"
#include "task_thread_pool.h"
#include "crypt.h"
#include <stdarg.h>
#include "io_service_pool.h"
#include "message_interface.h"
#include "enet_component.h"
#include "udp_client_manager.h"


static volatile boost::uint32_t s_asio_thread_count = 0;
static io_service_pool* s_io_service_pool = NULL;
static bool net_service_stoped = false;
static unsigned int s_io_once_listen_session_count = 40;
static bool init_udp_service = false;
ENetHost* enet_host = nullptr;
enet_component* g_enent_comment = nullptr;
bool enet_update_start = false;
udp_client_manager* g_udp_client_manager = nullptr;
boost::thread* g_enet_thread = nullptr;

void net_global::start_client_thread()
{
	start_enet_thread(g_udp_client_manager);
}
void net_global::udp_init_client_manager(int client_count)
{
	g_udp_client_manager = new udp_client_manager();
	g_udp_client_manager->init(client_count);
	
}
udp_client_manager* net_global::get_udp_client_manager()
{
	return g_udp_client_manager;
}

void net_global::start_enet_thread(enet_component* component)
{
	if (enet_update_start == false)
	{
		g_enent_comment = component;
		g_enet_thread = new boost::thread(&update_udp_service);
		enet_update_start = true;
	}
}
void net_global::update_net_service()
{
	if( s_io_service_pool )
		s_io_service_pool->update();
	if (g_udp_client_manager)
	{
		g_udp_client_manager->update();
	}
}



void net_global::update_udp_service()
{
	while (g_enent_comment->is_exit() == false)
	{
		ENetEvent event;
		while (enet_host_service(enet_host, &event, 200) >= 0 && g_enent_comment->is_exit() == false)
		{
			switch (event.type)
			{
			case ENET_EVENT_TYPE_CONNECT:
			{
				g_enent_comment->on_enet_connected(event);
			}
			break;

			case ENET_EVENT_TYPE_RECEIVE:
			{
				g_enent_comment->on_enet_receive(event);
				enet_packet_destroy(event.packet);    //×¢ÒâÊÍ·Å¿Õ¼ä				
			}
			break;
			case ENET_EVENT_TYPE_DISCONNECT:
			{
				g_enent_comment->on_enet_disconnect(event);
			}
			default:
				break;
			}
			g_enent_comment->extra_process(g_enent_comment->is_cpu_wait());
		}

	}	
}


boost::asio::io_service* net_global::get_io_service()
{
	return &s_io_service_pool->get_io_service();
}


bool net_global::udp_net_deinit()
{
	if (g_enent_comment)
	{
		g_enent_comment->set_exit(true);
	}

	if (g_enet_thread)
	{
		g_enet_thread->join();
	}

	if (g_udp_client_manager)
	{
		delete g_udp_client_manager;
		g_udp_client_manager = nullptr;
	}
	init_udp_service = false;
	enet_deinitialize();
	return true;
}

ENetHost* net_global::get_enet_host()
{
	return enet_host;
}
bool net_global::udp_net_init(const ENetAddress* adress, int connect_count, int channels, int up, int  down)
{
	if (init_udp_service)
	{
		return true;
	}

	if (enet_initialize() != 0)
	{
		fprintf(stderr, "An error occurred while initializing ENet.\n");
		return false;
	}		
	enet_host = enet_host_create(adress /* create a client host */,

		connect_count /* only allow 1 outgoing connection */,

		channels /* allow up 2 channels to be used, 0 and 1 */,

		down / 8 /* 56K modem with 56 Kbps downstream bandwidth */,

		up / 8 /* 56K modem with 14 Kbps upstream bandwidth */);

	init_udp_service = true;

	return true;
}
void net_global::init_net_service( int thread_count, int proc_interval, compress_strategy* cs_imp, bool need_max_speed, int msg_pool_size )
{
	message_interface::messageInit(cs_imp);
	net_service_stoped = false;
	s_io_service_pool = new io_service_pool( thread_count );
	s_io_service_pool->run();
    s_io_once_listen_session_count = proc_interval;
}


message_t* net_global::get_message(message_len size, base_session* from, bool base64)
{
	return message_interface::createMessage(from, size, base64);
}

void net_global::free_message( message_t* p )
{
	message_interface::releaseMessage(p);
}

void net_global::stop_net_service()
{
	if( !net_service_stoped )
	{
		net_service_stoped = true;

		s_io_service_pool->stop();
		delete s_io_service_pool;
		s_io_service_pool = NULL;
	}
}

void net_global::free_net_service()
{
	stop_net_service();
}

long net_global::get_asio_thread_alive_count()
{
	return s_asio_thread_count;
}

void net_global::write_close_log( const char* txt, ... )
{
#ifndef _WIN32
    FILE* fp = fopen( "close_reason.log", "a" );
    if( fp )
    {
        va_list ap;
        char buffer[4096];
        va_start(ap, txt);
        vsnprintf(buffer, 4096, txt, ap);
        va_end(ap);

        unsigned int t = (unsigned int)time( NULL );
        int y, m, d, h, min, s;
        convert_unix_time( t, &y, &m, &d, &h, &min, &s );
        fprintf( fp, "[%d-%d-%d %d:%d:%d] %s\n", y, m, d, h, min, s, buffer );
        fclose( fp );
    }
#endif
}

tcp_server::tcp_server( int id ):base_server(id),m_acceptor(nullptr)
{
}

tcp_server::~tcp_server()
{

}

bool tcp_server::create( unsigned short port, unsigned int poolcount, int thread_count )
{
	tcp::endpoint ep( tcp::v4(), port );
	if( m_acceptor )
	{
		delete m_acceptor;
		m_acceptor = NULL;
	}
	try
	{
		boost::asio::io_service* is = net_global::get_io_service();
		m_acceptor = new tcp::acceptor( *is );
		m_acceptor->open( ep.protocol() );

		boost::asio::socket_base::reuse_address option( true );
		boost::asio::socket_base::linger optiontemp;
		boost::asio::socket_base::linger option2( true, 0 );
		m_acceptor->get_option( optiontemp );
		m_acceptor->set_option( option2 );
		m_acceptor->get_option( optiontemp );

		m_acceptor->set_option( option );
		m_acceptor->set_option( option2 );
		m_acceptor->bind( ep );
		m_acceptor->listen();
	}
	catch( boost::system::system_error& e )
	{
		// error code == 10048, connot listen the same port at a time.
        printf("handle tcp server error code:[%s] port[%d]\n", e.what(), port);
		return false;
	}
	base_server::create(port, poolcount, thread_count);
	return true;
}

void tcp_server::handle_accept( tcp_session* p, const boost::system::error_code& error )
{
	--m_accepting_count;
	if( !error )
	{
		if (base_server::handle_accept(p))
		{
			p->get_io_service().post(boost::bind(&tcp_session::handle_accept, p, this));
		}		
	}
	else
	{
		p->handle_close();
		free_session( p );
	}
}


void tcp_server::stop()
{
	if (m_acceptor)
	{
		delete m_acceptor;
		m_acceptor = NULL;
	}
	base_server::stop();
}

void tcp_server::_real_run( bool is_wait )
{
	m_unix_time = (unsigned int)time( NULL );
	
	{
		boost::mutex::scoped_lock lock( m_proc_mutex );
		while( m_accepting_count < s_io_once_listen_session_count )
		{
			if( m_sessions.size() == 0 )
				break;
			tcp_session* p =  dynamic_cast<tcp_session*>( m_sessions.front());
			if (p)
			{
				p->set_valid(true);
				m_acceptor->async_accept(p->socket(), boost::bind(&tcp_server::handle_accept, this, p, boost::asio::placeholders::error));
				m_sessions.pop_front();
				++m_accepting_count;
			}
		}
	}
	base_server::_real_run(is_wait);
}
