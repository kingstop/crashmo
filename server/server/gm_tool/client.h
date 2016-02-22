#ifndef __client_h__
#define __client_h__
#include "tcpclient.h"
#include "protoc_common.h"
#include "event_table_object.h"

enum
{
	_client_init_,
	_client_wait_login_,
	_client_conn_login_,     
	_client_wait_gate_,   
	_client_connet_gate_,
	_client_close_,
};

class Client : public tcp_client, public ProtocMsgBase<Client>, public EventableObject
{
public:
	Client(const net_info& info);
	~Client();
	bool reConnect();
	static void initPBModule();
	void parseLoginResult(google::protobuf::Message* p, pb_flag_type flag);
	void parseGirlList(google::protobuf::Message* p, pb_flag_type flag);
	void parseCreateAccount(google::protobuf::Message* p, pb_flag_type flag);
	void parseModfiyPerformanceACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseNotifyAddGirl(google::protobuf::Message* p, pb_flag_type flag);
	void parseGirlInit(google::protobuf::Message* p, pb_flag_type flag);
	void parseAdviceList(google::protobuf::Message* p, pb_flag_type flag);
	void parseGirlsReservationsList(google::protobuf::Message* p, pb_flag_type flag);
	void parseModifyPerformanceACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseModifyPasswordACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseClientInit(google::protobuf::Message* p, pb_flag_type flag);
	void parseClientChar(google::protobuf::Message* p, pb_flag_type flag);
	void parseReceiveReservationACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseModifyNewsACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseGoodsCDKEYInfoListACK(google::protobuf::Message* p, pb_flag_type flag);
	void parseAddGoodsACK(google::protobuf::Message* p, pb_flag_type flag);

	virtual void on_connect();
	virtual void on_close( const boost::system::error_code& error );
	virtual void on_connect_failed( boost::system::error_code error );
	virtual void proc_message( const message_t& msg );
	void update(u32 nDiff);
	void moveRand();
	void moveSend();
	bool IsOK(){return m_MoveStateEnable;}
	bool IsGetTrans(){return m_transid > 0;}
	void set_account(const char* account);
	void set_password(const char* password);
public:
	void modify_performacine(account_type account, int performance);
	void modify_pass_word(const char* card, const char* pw);
	void receive_reservation(account_type account, u64 send_time);
	void send_news(const char* content, int repeated_count, int interval_time);
	void add_goods_cdkey(int goods_id, int goods_price, int goods_count, const char* name, const char* descrip);
	void send_reservation_req();
	const char* get_ip();
	void set_ip(const char* ip);
protected:
private:
	//u32 m_testIndex;
	u8  m_client_state;
	u32 m_transid;
	net_info m_info;
	std::string _account;
	std::string _password;
	bool _first_login;
	//message::ClientInit m_PlayerData;

	bool m_MoveStateEnable;
};
#endif