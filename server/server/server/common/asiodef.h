#ifndef ASIO_DEF_HEAD
#define ASIO_DEF_HEAD
#include "utilities.h"
#include "mt_buffer.h"
#include "call_back.h"
#include "message_interface.h"
#include <enet\enet.h>
using boost::asio::ip::tcp;
class udp_client_manager;

class tcp_session;
class base_session;
class enet_component;
static const int MAX_MESSAGE_LEN = 65000;
static const int QUEUE_MESSAGE_LIMIT_SIZE = MAX_MESSAGE_LEN * 30;



struct net_global
{
	// external
	static boost::asio::io_service* get_io_service();

	static bool udp_net_init(const ENetAddress* adress, int connect_count, int channels, int up = 14400, int  down = 57600);
	static ENetHost* get_enet_host();
	static bool udp_net_deinit();
	static void init_net_service( int thread_count, int proc_interval, compress_strategy* cs_imp, bool need_max_speed, int msg_pool_size );
	static void free_net_service();
	static void stop_net_service();
	static message_t* get_message(message_len size, base_session* from, bool base64);
	static void free_message( message_t* p );
	static long get_asio_thread_alive_count();
	static void write_close_log( const char* txt, ... );
	static void update_net_service();
	static void update_udp_service();
	static void update_udp_event_service();
	static void start_enet_thread(enet_component* component);
	static void udp_init_client_manager(int client_count);
	static void start_client_thread();
	static udp_client_manager* get_udp_client_manager();
	
	enum ban_reason_t
	{
		BAN_REASON_WRONG_CHECK_SUM,
		BAN_REASON_TOO_MANY_AUTH_FAIL,
		BAN_REASON_UNCOMPRESS_FAIL,
		BAN_REASON_HACK_PACKET,
		BAN_REASON_TOO_MANY_PACKET,
		BAN_REASON_TOO_MANY_IDLE_CONNECTION,
		BAN_REASON_SHIFTING_GEAR,
	};
};

//using namespace net_global;

#endif
