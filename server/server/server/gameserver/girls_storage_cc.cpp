/*
 * =====================================================================================
 *
 *       Filename:  girls_storage.cpp
 *
 *    Description:  <F2>
 *
 *        Version:  1.0
 *        Created:  07/04/2015 05:10:17 AM
 *       Revision:  none
 *       Compiler:  gcc
 *
 *         Author:  YOUR NAME (), 
 *   Organization:  
 *
 * =====================================================================================
 */
#include "stdafx.h"
#include "girls_storage.h"
#include "time.h"
#include "girl.h"
#include "session.h"
struct center_account
{
	center_account(u64 req_acc, u64 acc, const char* card, const char* password)
	{
		req_acount_ = req_acc;
		account_ = acc;
		card_ = card;
		password_ = password;
	}
	u64 req_acount_;
	u64 account_;
	std::string card_;
	std::string password_;
};

struct modify_password_db 
{
	modify_password_db(u64 req_acc, const char* card, const char* password)
	{
		req_account_ = req_acc;
		card_ = card;
		password_ = password;

	}
	u64 req_account_;
	std::string card_;
	std::string password_;
};
static bool sort_daily_performance(girl_info* entry_1, girl_info* entry_2)
{
	if (entry_1!= NULL && entry_2 != NULL)
	{
		if (entry_2->current_performance_ > entry_1->current_performance_)
		{
			return false;
		}
		else if (entry_2->current_performance_ == entry_1->current_performance_)
		{
			return false;
		}
	}
	return true;
}



#define _SAVE_GIRL_TIME_  (10 * 10 * _TIME_SECOND_MSEL_)

girls_storage::~girls_storage( )
{
     
}

girls_storage::girls_storage( )
{
	gEventMgr.addEvent(this, &girls_storage::save_girls, EVENT_SAVE_PLAYER_DATA_, _SAVE_GIRL_TIME_, 0, 0);

}



void girls_storage::Load_girls(DBQuery* p)
{
	srand(gGSServerTime);
	DBQuery& query = *p;
	query << "SELECT * FROM girls;";
	query.parse();
	SDBResult result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		girl_info* girl_entry = new girl_info();
		girl_entry->account_ = row["account"];
		girl_entry->name_ = GBKToUtf8(row["name"].c_str()).c_str();
		girl_entry->addr_ = GBKToUtf8(row["addr"].c_str()).c_str();
		girl_entry->phone_number_ = GBKToUtf8(row["phone_number"].c_str()).c_str();
		girl_entry->score_ = row["score"];
		girl_entry->performance_ = row["performance"];
		girl_entry->bonus_ = row["bonus"];
		girl_entry->reservation_car_ = row["reservation_car"];
		girl_entry->last_month_rank_ = row["last_month_rank"];
		girl_entry->current_month_rank_ = row["current_month_rank"];
		girl_entry->total_sign_count_ = row["total_sign_count"];
		girl_entry->continuous_sign_ = row["continuous_sign"];
		girl_entry->last_sign_time_ = row["last_sign_time"];
		girl_entry->current_performance_ = row["performance_current"];
		girl_entry->performance_last_mon_ = row["performance_last_mon"];
		girl_entry->card_ = GBKToUtf8(row["card"].c_str()).c_str();
		girl_entry->gm_flag_ = row["gm_flag"];
		_girls.insert(GIRLS::value_type(girl_entry->account_, girl_entry));
	}
	query.reset();
	result.clear();
	query << "SELECT * FROM guest_reservation;";
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		account_type entry_account = row["account"];
		u64 send_time_entry = row["send_time"];
		GIRLS::iterator it  = _girls.find(entry_account);
		if (it != _girls.end())
		{
			girl_info* girl_entry = it->second;
			RESERVATIONS::iterator it_reservation = girl_entry->reservation_infos_.find(send_time_entry);
			if (it_reservation == girl_entry->reservation_infos_.end())
			{
				reservation_info* entry_reservation = new reservation_info();
				entry_reservation->send_time_ = send_time_entry;
				entry_reservation->receive_time_ = row["receive_time"];
				entry_reservation->guest_number_	= row["guest_number"];
				entry_reservation->guest_name_ = GBKToUtf8(row["guest_name"].c_str()).c_str();
				entry_reservation->guest_request_ = GBKToUtf8(row["guest_request"].c_str()).c_str();
				girl_entry->reservation_infos_.insert(RESERVATIONS::value_type(entry_reservation->send_time_, entry_reservation));

				std::pair<u64, u64> entry_pair;
				entry_pair.first = girl_entry->account_;
				entry_pair.second = entry_reservation->send_time_;
				_all_reservations[entry_pair] = entry_reservation;
			}			
		}
	}
	query.reset();
	result.clear();
	query << "SELECT * FROM girl_cdkey;";
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		account_type entry_account = row["account"];
		std::string cdkey = GBKToUtf8(row["cdkey"].c_str()).c_str();
		std::string good_name = GBKToUtf8(row["good_name"].c_str()).c_str();

		GIRLS::iterator it  = _girls.find(entry_account);
		if (it != _girls.end())
		{
			girl_info* girl_entry = it->second;
			girl_entry->cdkeys_[cdkey] = good_name;
		}
	}
	query.reset();
	query << "SELECT * FROM current_day_rank ORDER BY `rank` ASC";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		account_type entry_account = row["account"];
		//std::string cdkey = row["cdkey"].c_str();
		//std::string good_name = row["good_name"].c_str();
		GIRLS::iterator it  = _girls.find(entry_account);
		if (it != _girls.end())
		{
			_girls_rank.push_back(it->second);
		}
	}
	query.reset();
	query << "SELECT * FROM last_mon_rank ORDER BY `rank` ASC";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		account_type entry_account = row["account"];
		//std::string cdkey = row["cdkey"].c_str();
		//std::string good_name = row["good_name"].c_str();

		GIRLS::iterator it  = _girls.find(entry_account);
		if (it != _girls.end())
		{
			_last_mon_girls_rank.push_back(it->second);
		}
	}

	query.reset();
	query << "SELECT `card`, `advice`, unix_timestamp(sender_time) FROM girls_advices";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		message::AdviceInfo advice;
		advice.set_sender_card(GBKToUtf8(row["card"].c_str()).c_str());
		advice.set_advice(GBKToUtf8(row["advice"].c_str()).c_str());
		advice.set_sender_time(row["unix_timestamp(sender_time)"]);
		_advices.push_back(advice);	
	}

	
	int tme_sec = get_next_day_update();
	gEventMgr.addEvent(this, &girls_storage::day_update, EVENT_DAILY_RANK_, tme_sec, 0, 0);
	day_update();
	//save_girls();
	mon_update();
	save_girls();

}

reservation_info* girls_storage::get_reservation(account_type account, u64 send_time)
{
	reservation_info* ret =  NULL;
	std::pair<account_type, u64> entry;
	entry.first = account;
	entry.second = send_time;
	ALLRESERVATIONS::iterator it = _all_reservations.find(entry);
	if (it != _all_reservations.end())
	{
		ret = it->second;
	}
	return ret;
}

ALLRESERVATIONS& girls_storage::get_all_reservations()
{
	return _all_reservations;
}

const std::vector<girl_info*>& girls_storage::get_current_rank_list()
{
	return _girls_rank;
}

const std::vector<girl_info*>& girls_storage::get_last_mon_rank_list()
{
	return _last_mon_girls_rank;
}

const GIRLS& girls_storage::get_girls()
{
	return _girls;
}

std::string girls_storage::get_time(time_t cur_time)
{
	time_t timep;
	if (cur_time == 0)
	{
		time(&timep); /*获取time_t类型的当前时间*/
	}
	else
	{
		timep = cur_time;
	}
	

	struct tm* cur = localtime(&timep);
	char sz_time[256];
	sprintf(sz_time, "%d-%d-%d %d:%d:%d",cur->tm_year + 1900, cur->tm_mon + 1, cur->tm_mday + 1, cur->tm_hour, cur->tm_min, cur->tm_sec);
	return std::string(sz_time);
}

void girls_storage::set_news(Session* modifier, const char* news, int repeated_count, int intveral_time)
{
	_news = news;
	_repeated_count = repeated_count;
	if (intveral_time > 10)
	{
		if (gEventMgr.hasEvent(this, EVENT_NEWS_))
		{
			gEventMgr.modifyEventTime(this,EVENT_NEWS_, intveral_time * _TIME_SECOND_MSEL_);
		}
		else
		{
			gEventMgr.addEvent(this,&girls_storage::news_update, EVENT_NEWS_, intveral_time * _TIME_SECOND_MSEL_,repeated_count,0);
		}
	}
	message::ModifyNewsACK msg;
	modifier->sendPBMessage(&msg);
}

void girls_storage::news_update()
{
	message::notifyNews msg;
	msg.set_news(_news.c_str());
	ONLINEGIRLS::iterator it = _online_girls.begin();
	for (; it != _online_girls.end(); ++ it)
	{
		girl* entry = it->second;
		entry->sendPBMessage(&msg);
	}
	_repeated_count --;
	if (_repeated_count <= 0)
	{
		gEventMgr.removeEvents(this,EVENT_NEWS_);
	}

}

void girls_storage::day_update()
{
	_girls_rank.clear();
	GIRLS::iterator it = _girls.begin();
	for (; it != _girls.end(); ++ it)
	{
		_girls_rank.push_back(it->second);
	}

	std::sort(_girls_rank.begin(), _girls_rank.end(), sort_daily_performance);	
	time_t timep;

	struct tm *p;

	time(&timep);
	std::vector<girl_info*>::iterator vc_it = _girls_rank.begin();
	for (int i = 1; vc_it != _girls_rank.end(); ++ vc_it, i ++)
	{
		girl_info* entry = (*vc_it);
		entry->current_month_rank_ = i;
	}
	p = localtime(&timep); //取得当地时间

	int mon_day = p->tm_mday;
	if (mon_day == 1)
	{
		mon_update();
	}
	int tme_sec = get_next_day_update();
	gEventMgr.modifyEventTime(this, EVENT_DAILY_RANK_, tme_sec);

	//gEventMgr.addEvent(this, &girls_storage::day_update, EVENT_DAILY_RANK_, tme_sec, 0, 0);
}

void girls_storage::mon_update()
{
	_last_mon_girls_rank.clear();
	_last_mon_girls_rank = _girls_rank;
	std::vector<girl_info*>::iterator vc_it = _girls_rank.begin();
	for (int i = 1; vc_it != _girls_rank.end(); ++ vc_it, i ++)
	{
		girl_info* entry = (*vc_it);
		entry->last_month_rank_ = i;
		entry->performance_last_mon_ = entry->current_performance_;
		//entry->current_performance_ = 0;
	}
}


int girls_storage::get_next_day_update()
{
	time_t timep;
	struct tm *p;
	time(&timep);
	p = localtime(&timep); //取得当地时间
	int mon_day = p->tm_mday;
	int hour = p->tm_hour;
	int min = p->tm_min;
	int sec = p->tm_sec;
	int need_hour = 0;
	if (hour < 3)
	{
		need_hour = 3 - hour -1;
	}
	else
	{
		need_hour = 24 + 3 - hour - 1;
	}
	int need_sec = 60 - sec;
	int need_min = 50 - min;
	int temp_sec = need_hour * 60 * 60 + need_sec * 60 + sec;
	return temp_sec;
}

girl_info* girls_storage::get_girl(account_type  card )
{
    girl_info* entry = NULL;
    GIRLS::iterator it =  _girls.find(card);
    if(it != _girls.end( ))
    {
        entry = it->second;     
    }
    return entry;
}


void girls_storage::dbCallNothing(const SDBResult* , const void* d, bool s)
{


}

char sz[] = {'a','b','c','d','e','f','g','h','i','j','k','l','m','n','o','p','q','r','s','t','u','v','w','x','y','z'};

void girls_storage::dbDoQueryModifyPassword(DBQuery*p, const void* d)
{
	modify_password_db* pkparm = static_cast<modify_password_db*>(const_cast<void*>(d));
	DBQuery& query = *p;
	DBQParms parms;
	char sztemp[256];
	sprintf(sztemp, "SELECT * FROM account where name=\'%s\'", pkparm->card_.c_str());
	query << sztemp;
	query.parse();
	SDBResult sResult = query.store(parms);
	message::ModifyPasswordACK msg;
	if (sResult.size() != 0)
	{
		sprintf(sztemp, "update account set `password`=\'%s\' where `name`=\'%s\'",pkparm->password_.c_str(), pkparm->card_.c_str());
		msg.set_sucessful(true);
		gCenterDataBase.addSQueryTask(this, &girls_storage::dbCallNothing, sztemp, &parms, NULL, _SAVE_GIRL_);
	}
	else
	{
		msg.set_sucessful(false);
	}
	girl* entry = get_online_girl(pkparm->req_account_);
	if (entry)
	{
		entry->sendPBMessage(&msg);
	}
}
void girls_storage::dbCallQueryModifyPassword(const void* d, bool s)
{

}

void girls_storage::dbDoQueryCenter(DBQuery*p, const void* d)
{
	center_account* pkParm = static_cast<center_account*>(const_cast<void*>(d));
	DBQuery& query = *p;
	DBQParms parms;
	char sztemp[256];
	sprintf(sztemp, "SELECT MAX(`id`) FROM account");
	query << sztemp;
	query.parse();
	SDBResult sResult = query.store(parms);
	if (sResult.size() != 0)
	{
		DBRow& row = sResult[0];
		pkParm->account_ = row["MAX(`id`)"] + 1;
	}
	else
	{
		pkParm->account_ = 10;
		//get_girl(pkParm->req_acount_);
	}
	sprintf(sztemp, "%llu",pkParm->account_);
	pkParm->card_ = sztemp;
	std::string pass_word;
	for (int i = 0; i < 6; i ++)
	{
		int temp = rand()%26;
		sprintf(sztemp, "%d", temp);
		pass_word += sz[temp];
	}	
	time_t timep;
	time(&timep); /*获取time_t类型的当前时间*/

	pkParm->password_ = pass_word;
	girl_info* info_entry = new girl_info();
	info_entry->account_ = pkParm->account_;
	info_entry->card_ = pkParm->card_;
	

	//std::string time_create = asctime(gmtime(&timep));
	std::string time_cur = get_time();
	sprintf(sztemp, "insert into account(`name`, `password`, `createtime`, `id`) values(\'%s\',\'%s\',\'%s\', %llu);", 
		pkParm->card_.c_str(), pkParm->password_.c_str(), time_cur.c_str(), pkParm->account_);
	gCenterDataBase.addSQueryTask(this, &girls_storage::dbCallNothing, sztemp, &parms, NULL, _SAVE_GIRL_);
	boost::recursive_mutex::scoped_lock lock(m_mutex); 
	_girls.insert(GIRLS::value_type(info_entry->account_, info_entry));
	message::notifyAddGirlInfo notify_msg;
	notify_msg.set_allocated_info(info_entry->createGirlInfo());
	girl* entry = get_online_girl(pkParm->req_acount_);
	entry->sendPBMessage(&notify_msg);	
	message::CreateAccountACK ack;
	ack.set_card(pkParm->card_.c_str());
	ack.set_password(pkParm->password_.c_str());
	entry->sendPBMessage(&ack);	
	
	
}

girl* girls_storage::get_online_girl(account_type account)
{
	girl* entry = NULL;
	ONLINEGIRLS::iterator it = _online_girls.find(account);
	if (it != _online_girls.end())
	{
		entry = it->second;
	}
	return entry;
}
void girls_storage::add_online_girl(girl* girl_param)
{
	_online_girls[girl_param->get_account()] = girl_param;
}

void girls_storage::add_account(account_type account)
{
	gCenterDataBase.addBatchTask(this, &girls_storage::dbDoQueryCenter, &girls_storage::dbCallQueryCenter, new center_account(account, 0, "", ""), "create account");
}

void girls_storage::remove_online_girl(girl* girl_param)
{
	ONLINEGIRLS::iterator it  = _online_girls.find(girl_param->get_account());
	if (it != _online_girls.end())
	{
		_online_girls.erase(it);
	}
}

void girls_storage::add_advice(const char* card, const char* advice)
{
	message::AdviceInfo entry;
	entry.set_sender_time(gGSServerTime);
	entry.set_sender_card(card);
	entry.set_advice(advice);
	_advices.push_back(entry);
}


void girls_storage::modify_password(account_type account_req, const char* card, const char* password)
{
	gCenterDataBase.addBatchTask(this, &girls_storage::dbDoQueryModifyPassword, &girls_storage::dbCallQueryModifyPassword, 
		new modify_password_db(account_req, card, password), "create account");
}


std::vector<message::AdviceInfo>& girls_storage::get_advices()
{
	return _advices;
}

void girls_storage::RservationReceivedReq(u64 ReqAccount, u64 acount, u64 send_time)
{
	std::pair<u64, u64> entry_pair;
	entry_pair.first = acount;
	entry_pair.second = send_time;
	ALLRESERVATIONS::iterator it = _all_reservations.find(entry_pair);
	if (it != _all_reservations.end())
	{
		reservation_info* info = it->second;
		info->receive_time_ = gGSServerTime;
		message::NotifyReservationStateModify msg;
		msg.set_send_time(send_time);
		msg.set_receive_time(info->receive_time_);
		msg.set_account(acount);
		girl* entry = get_online_girl(ReqAccount);
		if (entry)
		{
			entry->sendPBMessage(&msg);
		}
	}
	
}

void girls_storage::ReservationReq(message::ReservationReq* msg, account_type account)
{
	reservation_info* info = new reservation_info();
	info->set_reservation(&msg->info());
	info->send_time_ = gGSServerTime;
	//info->type_ = message::Reservation_send;
	girl_info* entry = get_girl(account);
	if (entry)
	{
		entry->reservation_infos_[info->send_time_] = info;
		info->receive_time_ = 0;
	}
	std::pair<u64, u64> entry_pair;
	entry_pair.first = account;
	entry_pair.second = info->send_time_;
	_all_reservations[entry_pair] = info;
	

}

void girls_storage::dbCallQueryCenter(const void* d, bool s)
{

}

void girls_storage::save_girls()
{
	gGirlShop.save_goods();
	if (_girls.size() == 0)
	{
		return;
	}
	boost::recursive_mutex::scoped_lock lock(m_mutex); 
	GIRLS::iterator it = _girls.begin();
	std::string sql_head = "replace into girls(`account`,`card`,`name`, `addr`, `phone_number`, `back_card`, `score`, `performance`, `bonus`, \
						   `reservation_car`, `last_month_rank`, `current_month_rank`, `total_sign_count`, `continuous_sign`, \
						   `performance_current`, `performance_last_mon`, `gm_flag`,`last_sign_time`) values";
	std::string excute_str;
	char sz_temp[512];
	int max_save_count = 10;
	DBQParms parms;
	for (int i = 0; it != _girls.end(); ++ it, i ++)
	{
		if (i >= max_save_count)
		{
			gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
			i = 0;
			excute_str.clear();
		}
		if (i != 0)
		{
			excute_str += ",";
		}
		else
		{
			excute_str.clear();
			excute_str = sql_head;
		}
		u64 account_temp = it->first;
		memset(sz_temp, 0, sizeof(sz_temp));
		girl_info* info_entry = it->second;
		sprintf(sz_temp, "(%llu",
			account_temp);
		excute_str += sz_temp;
		int gm_flag = info_entry->gm_flag_;
		sprintf(sz_temp, ", \'%s\', \'%s\', \'%s\',\'%s\', %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %llu)",
			Utf8ToGBK(info_entry->card_.c_str()).c_str(),
			Utf8ToGBK(info_entry->name_.c_str()).c_str(), 
			Utf8ToGBK(info_entry->addr_.c_str()).c_str(),
			Utf8ToGBK(info_entry->phone_number_.c_str()).c_str(),
			info_entry->bank_card_, info_entry->score_, info_entry->performance_,
			info_entry->bonus_, info_entry->reservation_car_, info_entry->last_month_rank_,
			info_entry->current_month_rank_,info_entry->total_sign_count_, info_entry->continuous_sign_,
			info_entry->current_performance_, info_entry->performance_last_mon_, gm_flag, info_entry->last_sign_time_ );
		excute_str += sz_temp;

	}
	gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);


	excute_str.clear();
	it = _girls.begin();
	for (; it != _girls.end(); ++ it)
	{
		u64 account_temp = it->first;
		girl_info* info_entry = it->second;
		std::map<std::string, std::string>::iterator it_cdkey = info_entry->cdkeys_.begin();
		std::string temp_excute_str;
		char sz_temp[512];
		memset(sz_temp, 0, sizeof(sz_temp));
		sprintf(sz_temp, "delete from girl_cdkey where `account`=%llu;", account_temp);
		temp_excute_str += sz_temp;
		if (info_entry->cdkeys_.size() != 0)
		{
			temp_excute_str += "insert info girl_cdkey(`cdkey`,`good_name`,`account`) values";
		}
		temp_excute_str += sz_temp;
		for (int i = 0; it_cdkey != info_entry->cdkeys_.end(); ++ it_cdkey, i ++)
		{
			if (i != 0)
			{
				temp_excute_str += ",";
			}
			memset(sz_temp, 0, sizeof(sz_temp));
			sprintf(sz_temp, "(\'%s\', \'%s\',%llu)", Utf8ToGBK(it_cdkey->first.c_str()).c_str(),Utf8ToGBK(it_cdkey->second.c_str()).c_str(), account_temp );
			temp_excute_str += sz_temp;
		}
		gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, temp_excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
		temp_excute_str.clear();
	}
	excute_str.clear();

	excute_str+= "delete from current_day_rank;";
	if (_girls_rank.size()!= 0)
	{
		excute_str += "insert into current_day_rank(`account`,`rank`) values";
		std::vector<girl_info*>::iterator it_vc_rank = _girls_rank.begin();
		char sz_temp[512];
		for (int i = 0; it_vc_rank != _girls_rank.end(); ++ it_vc_rank, i ++)
		{
			girl_info* entry = (*it_vc_rank);
			if (i != 0)
			{
				excute_str += ",";
			}
			memset(sz_temp, 0, sizeof(sz_temp));
			sprintf(sz_temp, "(%llu, %d)",entry->account_, entry->current_month_rank_);	
			excute_str += sz_temp;
		}
	}
	gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
	excute_str.clear();


	excute_str+= "delete from last_mon_rank;";
	if (_last_mon_girls_rank.size()!= 0)
	{
		excute_str += "insert into last_mon_rank(`account`,`rank`) values";
		std::vector<girl_info*>::iterator it_vc_rank = _last_mon_girls_rank.begin();
		char sz_temp[512];
		for (int i = 0; it_vc_rank != _last_mon_girls_rank.end(); ++ it_vc_rank, i ++)
		{
			girl_info* entry = (*it_vc_rank);
			if (i != 0)
			{
				excute_str += ",";
			}
			memset(sz_temp, 0, sizeof(sz_temp));
			sprintf(sz_temp, "(%llu, %d)",entry->account_, entry->current_month_rank_);		
			excute_str+= sz_temp;
		}
	}
	gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
	excute_str.clear();

	excute_str += "delete from girls_advices;";
	if (_advices.size() != 0)
	{
		std::string time_cur;
		excute_str += "insert into girls_advices(`card`,`advice`,`sender_time`) values";
		std::vector<message::AdviceInfo>::iterator it = _advices.begin();
		for (int i = 0; it != _advices.end(); ++ it, i ++)
		{
			if (i != 0)
			{
				excute_str += ",";
			}
			message::AdviceInfo entry = (*it);
			time_cur = get_time(entry.sender_time());
			sprintf(sz_temp,"(\'%s\', \'%s\', \'%s\')", Utf8ToGBK(entry.sender_card().c_str()).c_str(), 
				Utf8ToGBK(entry.advice().c_str()).c_str(), Utf8ToGBK(time_cur.c_str()).c_str());
			excute_str += sz_temp;
		}
	}
	gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
	excute_str.clear();		
	excute_str = "delete FROM guest_reservation;";
	if (_all_reservations.size() != 0)
	{
		std::string excute_head = "insert into guest_reservation(`guest_name`, `guest_request`,`guest_number`, `send_time`, `receive_time`,`account`) values";
		ALLRESERVATIONS::iterator it_reservation =  _all_reservations.begin();
		for (int i = 0; it_reservation != _all_reservations.end(); ++ it_reservation, i ++)
		{
			if (i == 0)
			{
				excute_str += excute_head;
			}
			else
			{
				excute_str += ",";
			}
			reservation_info* info = it_reservation->second;
			u64 account = it_reservation->first.first;
			sprintf(sz_temp, "(\'%s\', \'%s\', %d, %llu, %llu, %llu)", 
				Utf8ToGBK(info->guest_name_.c_str()).c_str(), info->guest_request_.c_str(),  info->guest_number_, info->send_time_, info->receive_time_,account);
			excute_str += sz_temp;
			if (i > max_save_count)
			{
				gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
				i = 0;
				excute_str.clear();
			}
		}
	}

	if (excute_str.empty() == false)
	{
		gDBCharDatabase.addSQueryTask(this, &girls_storage::dbCallNothing, excute_str.c_str(), &parms, NULL, _SAVE_GIRL_);
	}
}