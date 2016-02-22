/*
 * =====================================================================================
 *
 *       Filename:  girl.cpp
 *
 *    Description:  
 *
 *        Version:  1.0
 *        Created:  07/04/2015 07:26:50 AM
 *       Revision:  none
 *       Compiler:  gcc
 *
 *         Author:  YOUR NAME (), 
 *   Organization:  
 *
 * =====================================================================================
 */
#include "stdafx.h"
#include "girl.h"
#include "girls_storage.h"
#include "session.h"
#include "message/girls.pb.h"
girl::girl(Session* session):_session(session)
{
    _info = NULL; 
}

girl::~girl( )
{

} 

 void girl::set_info( girl_info* info)
{
    _info = info;
}
 message::GirlInfo* girl::createGirlInfo()
{
    if(_info)
    {
        return _info->createGirlInfo( );
    }
    return NULL;
}


void girl::require_rank_list()
{
	message::RankInfoListACK msg;
	const std::vector<girl_info*> current_rank_list = gGSGirlsStorage.get_current_rank_list();
	std::vector<girl_info*>::const_iterator it = current_rank_list.begin();
	for (int i = 0; it != current_rank_list.end(); ++  it, i ++)
	{
		const girl_info* entry = (*it);
		message::RankInfo* entry_msg = msg.add_current_list();
		entry_msg->set_name(entry->name_.c_str());
		entry_msg->set_performance(entry->current_performance_);
		entry_msg->set_last_mon_performance(entry->performance_last_mon_);
		entry_msg->set_card(entry->card_.c_str());
		entry_msg->set_rank(entry->current_month_rank_);
		if (i >= 30)
		{
			break;
		}
	}


	const std::vector<girl_info*> last_mon_rank_list = gGSGirlsStorage.get_last_mon_rank_list();
	std::vector<girl_info*>::const_iterator it_last = last_mon_rank_list.begin();
	for (int i = 0; it_last != last_mon_rank_list.end(); ++ it_last, i ++)
	{
		const girl_info* entry = (*it_last);
		message::RankInfo* entry_msg = msg.add_last_mon_list();
		entry_msg->set_name(entry->name_.c_str());
		entry_msg->set_performance(entry->current_performance_);
		entry_msg->set_last_mon_performance(entry->performance_last_mon_);
		entry_msg->set_card(entry->card_.c_str());
		entry_msg->set_rank(entry->last_month_rank_);
		if (i >= 30)
		{
			break;
		}

	}
	sendPBMessage(&msg);
}

void girl::modify_performance(int m)
{
	_info->current_performance_ += m;
	_info->performance_ += m;
}

account_type girl::get_account()
{
	return _info->account_;
}

const char* girl::get_card()
{
	return _info->card_.c_str();
}

void girl::modify_info(message::ModifyInfoRequest* msg)
 {
	 _info->addr_ = msg->info().addr();
	 _info->bank_card_ = msg->info().bank_card();
	 _info->name_ = msg->info().name();
	 _info->name_icon_ = msg->info().name_icon();
	 _info->phone_number_ = msg->info().phone_number();
	 message::ModifyInfoACK ack;
	 message::GirlBaseInfo* base_info = new message::GirlBaseInfo();		
	 _info->get_girl_base_info(base_info);
	 ack.set_allocated_info(base_info);
	 sendPBMessage(&ack);
 }

 void girl::sign_in()
 {
	 if (_info)
	 {
		 time_t last_day_time = _info->last_sign_time_;
		 if (is_in_today(&last_day_time))
		 {
			 message::ServerError msg;
			 msg.set_error(message::already_signed);
			 sendPBMessage(&msg);
		 }
		 else
		 {
			 if (is_in_current_mon(&last_day_time) == false)
			 {
				 _info->total_sign_count_ = 0;
			 }
			 _info->total_sign_count_ ++;
			 _info->score_ += gGirlShop.get_action_score(1);
			 time_t timep;
			 time(&timep);
			 _info->last_sign_time_ =timep;
			 message::ServerSignACK msg;
			 msg.set_score(_info->score_);
			 msg.set_total_sign_count(_info->total_sign_count_);
			 sendPBMessage(&msg);
		 }
	 }
	 
 }



void girl::sendPBMessage(google::protobuf::Message* p)
{
	if (_session)
	{
		_session->sendPBMessage(p);
	}	
}


void girl::require_goods_list()
{
	message::GoodListACK msg;
	GOODS::iterator it_good = gGirlShop._goods.begin();
	for (; it_good != gGirlShop._goods.end(); ++ it_good)
	{
		goodsinfo entry = it_good->second;
		CDKEYS::iterator it_cdkey = entry.cdkeys_.begin();
		message::goodinfo* info = msg.add_goods_list();
		info->CopyFrom(entry.info_);

	}
	char sztemp[1024];
	msg.SerializeToArray(sztemp, sizeof(sztemp));

	sendPBMessage(&msg);
}

void girl::buygood(int good_id)
{
	message::server_girls_error error_temp = message::no_error;
	GOODS::iterator it_good = gGirlShop._goods.find(good_id);
	if (it_good == gGirlShop._goods.end())
	{
		error_temp = message::not_found_goods;
	}
	else
	{
		std::string cdkey;
		int price_  = 0 ; 
		std::string name_temp;
		CDKEYS::iterator it_cdkey = it_good->second.cdkeys_.begin();
		for (; it_cdkey != it_good->second.cdkeys_.end(); ++ it_cdkey)
		{
			if (it_cdkey->second == exchange_type_no)
			{
				cdkey = it_cdkey->first;
				it_cdkey->second = exchange_type_exchanged;
				price_ = it_good->second.info_.price();
				name_temp = it_good->second.info_.name();
				break;
			}			
		}

		if (cdkey.empty())
		{
			error_temp = message::goods_is_sell_out;
		}
		else
		{
			if (_info->score_ < price_)
			{
				error_temp = message::not_enough_score;
			}
			else
			{
				_info->score_ -= price_;
				message::BuyGoodACK msg;
				msg.set_cdkey(cdkey);
				msg.set_good_id(good_id);
				msg.set_score(_info->score_);
				msg.set_good_name(name_temp);
				_info->cdkeys_[cdkey] = name_temp;
				sendPBMessage(&msg);
			}
			
		}
	}
	if (error_temp != message::no_error)
	{
		message::ServerError msg;
		msg.set_error(error_temp);
		sendPBMessage(&msg);
	}
	
}
