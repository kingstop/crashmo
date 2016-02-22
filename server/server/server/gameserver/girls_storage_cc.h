/*
 * =====================================================================================
 *
 *       Filename:  girls_storage.h
 *
 *    Description:  
 *
 *        Version:  1.0
 *        Created:  07/04/2015 04:47:24 AM
 *       Revision:  none
 *       Compiler:  gcc
 *
 *         Author:  YOUR NAME (), 
 *   Organization:  
 *
 * =====================================================================================
 */

#ifndef GIRLS_STORAGE_H
#define GIRLS_STORAGE_H 
#include <map>
#include "common_type.h"
#include "message/server_define.h"
#include "message/girls.pb.h"
#include "event_table_object.h"
struct reservation_info
{
    std::string guest_name_;
    int guest_number_; 
    std::string guest_request_;
    u64 send_time_;
    u64 receive_time_;
	//message::Reservation_type type_;
    void get_reservation(message::Reservation* info)
    {
        info->set_guest_name( guest_name_.c_str( ));
        info->set_guest_number( guest_number_);
        info->set_guest_request( guest_request_.c_str( ));
        info->set_send_time(send_time_ );
		info->set_receive_time(receive_time_);
		if (receive_time_ != 0)
		{
			info->set_type(message::Reservation_accepted);
		}
		else
		{
			info->set_type(message::Reservation_send);
		}
		
    }
	void set_reservation(const message::Reservation* info)
	{
		guest_request_ = info->guest_request();
		guest_number_ = info->guest_number();
		guest_name_ = info->guest_name();
		send_time_ = info->send_time();
		receive_time_ = info->receive_time();
		//type_ = info->type();
	}
};

struct base_girl
{
	base_girl()
	{
		
		bank_card_ =0;
		name_ = "name";
		name_icon_ = "";
		addr_ = "addr";
		phone_number_ = "phone number";
	}
    std::string name_;
    std::string card_;
    std::string name_icon_;
    std::string addr_;
    std::string phone_number_;
    int bank_card_;
	u64 account_;
    void get_girl_base_info(message::GirlBaseInfo* info)
    {
        info->set_name(name_.c_str());
        info->set_card( card_.c_str());
        info->set_name_icon( name_icon_.c_str( ));
        info->set_addr( addr_.c_str( ));
        info->set_phone_number( phone_number_.c_str( ));
        info->set_bank_card( bank_card_);
    }
};

typedef std::map<u64, reservation_info*> RESERVATIONS; 
struct girl_info : public base_girl
{
	girl_info()
	{
		score_ = 0; //积分 
		performance_ = 0; //业绩 
		bonus_ = 0; //奖金
		reservation_car_ = 0; //预约电话
		last_month_rank_ = 0;//上月业绩排名
		current_month_rank_ = 0;  //本月业绩排名
		continuous_sign_ = 0;
		total_sign_count_ = 0;
		last_sign_time_ = 0;
		current_performance_ = 0;
		performance_last_mon_ = 0;
		gm_flag_ = false;
	}

  
  int score_; //积分 
  int performance_; //业绩 
  int bonus_; //奖金
  int reservation_car_; //预约电话
  int last_month_rank_;//上月业绩排名
  int current_month_rank_;  //本月业绩排名
  int continuous_sign_;
  int total_sign_count_;
  int current_performance_;
  int performance_last_mon_;
  u64 last_sign_time_;
  u64 account_;
  bool gm_flag_;
  RESERVATIONS reservation_infos_;
  std::map<std::string, std::string> cdkeys_;
  message::GirlInfo* createGirlInfo( ) 
  {
      message::GirlInfo* entry = new message::GirlInfo( );
      entry->set_scoure( score_);
      entry->set_performance( performance_);
      entry->set_bonus( bonus_);
	  entry->set_last_month_rank(last_month_rank_);
	  entry->set_current_month_rank(current_month_rank_);
	  entry->set_continuous_sign(continuous_sign_);
	  entry->set_total_sign_count(total_sign_count_);
	  entry->set_reservation_car(reservation_car_);
	  entry->set_current_performance(current_performance_);
	  entry->set_last_mon_performance(performance_last_mon_);
	  entry->set_gm_flag(gm_flag_);
      RESERVATIONS::iterator it = reservation_infos_.begin();
      for(; it != reservation_infos_.end( ); ++ it)
      {
           reservation_info* info_entry = it->second; 
           message::Reservation* info = entry->add_reservation( );
           info_entry->get_reservation(info);
      }
	  std::map<std::string, std::string>::iterator it_cdkey = cdkeys_.begin();
	  for (; it_cdkey != cdkeys_.end(); ++ it_cdkey)
	  {
		  message::cdkey_good* temp_cdkey = entry->add_cdkeys();
		  temp_cdkey->set_cdkey(it_cdkey->first.c_str());
		  temp_cdkey->set_good_name(it_cdkey->second.c_str());

	  }
     message::GirlBaseInfo* base_info_entry =  entry->mutable_info( ); 
     get_girl_base_info(base_info_entry);
	 return entry;
  }
};
class girl;
typedef std::map<account_type, girl_info*> GIRLS;
typedef std::map<account_type, girl*> ONLINEGIRLS;
typedef std::map<std::pair<u64, u64>, reservation_info*> ALLRESERVATIONS; 
class girls_storage : public EventableObject
{
public: 
 
public:
    girls_storage( );
    ~girls_storage( );
public:
	void Load_girls(DBQuery* p);
	void save_girls();
    girl_info* get_girl(account_type card);
	int get_next_day_update();
	void day_update();
	void mon_update();
	void news_update();
	const std::vector<girl_info*>& get_current_rank_list();
	const std::vector<girl_info*>& get_last_mon_rank_list();
	reservation_info* get_reservation(account_type account, u64 send_time);
	ALLRESERVATIONS& get_all_reservations();
	void dbCallNothing(const SDBResult* , const void* d, bool s);
	const GIRLS& get_girls();
	girl* get_online_girl(account_type account);
	void add_online_girl(girl* girl_param);
	void add_account(account_type account);
	void remove_online_girl(girl* girl_param);
	void add_advice(const char* card, const char* advice);
	void modify_password(account_type account_req, const char* card, const char* password);
public:
	void dbDoQueryCenter(DBQuery*p, const void* d);
	void dbCallQueryCenter(const void* d, bool s);
	void dbDoQueryModifyPassword(DBQuery*p, const void* d);
	void dbCallQueryModifyPassword(const void* d, bool s);
	void ReservationReq(message::ReservationReq* msg, account_type account);
	void RservationReceivedReq(u64 ReqAccount, u64 acount, u64 send_time);
	std::vector<message::AdviceInfo>& get_advices();
	void set_news(Session* modifier, const char* news, int repeated_count, int intveral_time);
	std::string get_time(time_t cur_time = 0);

protected:         
    GIRLS _girls;
	ONLINEGIRLS _online_girls;
	std::vector<girl_info*> _girls_rank;
	std::vector<girl_info*> _last_mon_girls_rank;
	std::vector<message::AdviceInfo> _advices;
	ALLRESERVATIONS _all_reservations;
	boost::recursive_mutex m_mutex; 
	std::string _news;
	int _repeated_count;
};
#endif 
