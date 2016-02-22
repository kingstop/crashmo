/*
 * =====================================================================================
 *
 *       Filename:  girl.h
 *
 *    Description:  
 *
 *        Version:  1.0
 *        Created:  07/04/2015 07:24:00 AM
 *       Revision:  none
 *       Compiler:  gcc
 *
 *         Author:  YOUR NAME (), 
 *   Organization:  
 *
 * =====================================================================================
 */
#ifndef GIRL_H 
#define GIRL_H 
#include "message/girls.pb.h"
struct girl_info;
class girl 
{
public:
    girl(Session* session);
    ~girl( );
public:
    void set_info( girl_info* info);
	void modify_info(message::ModifyInfoRequest* msg);
	void sign_in();
    message::GirlInfo* createGirlInfo();
	void sendPBMessage(google::protobuf::Message* p);
	void buygood(int good_id);
	void require_goods_list();
	void require_rank_list();
	void modify_performance(int m);
	account_type get_account();
	const char* get_card();
protected:
    girl_info* _info;
	Session* _session;
    
};

#endif 
