#pragma once

#include <iostream>
#include <string>
#include <map>
#include "message/girls.pb.h"
#include "stdint.h"
#include "common_type.h"
#include "message/server_define.h"

struct girl_info 
{
	account_type account_;
	std::string name_;
	std::string addr_;
	std::string phone_number_;
	std::string bank_card_;
	std::string card_;
	int score_;
	int performance_;
	int bonus_;
	void set(const message::GirlInfo* info)
	{
		account_ = atoi(info->info().card().c_str());
		name_ = info->info().name();
		addr_ = info->info().addr();
		phone_number_ = info->info().phone_number();
		bank_card_ = info->info().bank_card();
		card_ = info->info().card().c_str();
		score_ = info->scoure();
		performance_ = info->performance();
		bonus_ = info->bonus();
	}

};
typedef std::map<account_type, girl_info*> MAPGIRLS;
typedef std::vector<message::AdviceInfo> ADVICESLIST;
typedef std::map<std::pair<account_type, u64>, message::GirlsReservationInfo> MAPGirlsReservation;
typedef std::map<int, message::GoodsCDKEYInfo> MAPGOODSCDKEY;
class girls_manager
{
public:
	girls_manager(void);
	virtual ~girls_manager(void);
public:
	void clear_girls();
	girl_info* get_girl(account_type account);
	message::GirlsReservationInfo* get_reservation(account_type account, u64 send_time);
	message::AdviceInfo* get_advice(const char* card, u64 sender_time);
	message::GoodsCDKEYInfo* get_good_cdkey(int id);
	ADVICESLIST& get_advices();
	void load_girls(message::GirlListACK* msg);
	void add_girl_info(message::notifyAddGirlInfo* msg);
	void add_advice(message::AdviceListACK* msg);
	void add_girls_reservation_info_list_ACK(message::GirlsReservationInfoListACK* msg);
	void add_goods_cdkey(message::GoodsCDKEYInfoListACK* msg);
	void add_goods_cdkey(message::AddGoodsACK* msg);
	bool have_goods_key(int id);

public:
	MAPGIRLS _girls;
	ADVICESLIST _advices;
	MAPGirlsReservation _Reservation;
	MAPGOODSCDKEY _good_cdkey;
};

