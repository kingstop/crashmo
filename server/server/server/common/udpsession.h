#include "basesession.h"
#include "enet/enet.h"

class udp_session : public base_session
{

public:
	udp_session();
	virtual ~udp_session();
	virtual void on_connect();

	inline u32 get_connect_id() { return _connect_id; }
	inline u32 get_connect_index() { return _connect_index; }
	void receive(const char* receive_data, std::size_t length);
	inline void set_father(base_server* father) { m_father = father; }
	inline const u32* get_connect_index_data() { return &_connect_index; }
	inline ENetPeer* get_peer() {  return _peer; }
	virtual void close();
	virtual void handle_close();
	virtual void on_close();
	virtual void reset();

public:
	void _write_message();
	virtual void handle_connect(ENetPeer* peer, u32 connect_index,
		u32 remote_host, u16 remote_port, const char* ip);
protected:
	virtual bool _uncompress_message(char* data);
	virtual void _send_message(message_t* msg);
	virtual ENetHost* get_host();
	virtual call_back_mgr* _get_cb_mgr();
	virtual char* get_uncompress_buffer();
protected:
	std::string _ip;
	u32 _connect_id;
	u32 _connect_index;
	u16 _port;
	ENetPeer * _peer;
};
