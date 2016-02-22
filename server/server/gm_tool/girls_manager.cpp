#include "StdAfx.h"
#include "girls_manager.h"
#include "gm_toolDlg.h"
#include "dlg_girls.h"
#include "dlg_advice.h"
#include "dlg_reservation.h"
#include "gm_toolDlg.h"
#include "dlg_shop.h"


girls_manager::girls_manager(void)
{
}


girls_manager::~girls_manager(void)
{
}

girl_info* girls_manager::get_girl(account_type account)
{
	girl_info* entry = NULL;
	MAPGIRLS::iterator it = _girls.find(account);
	if (it != _girls.end())
	{
		entry = it->second;
	}
	return entry;
}


message::AdviceInfo* girls_manager::get_advice(const char* card, u64 sender_time)
{
	message::AdviceInfo* entry = NULL;
	ADVICESLIST::iterator it = _advices.begin();
	for (; it != _advices.end(); ++it)
	{
		if ((*it).sender_card() == card && (*it).sender_time() == sender_time)
		{
			entry = &(*it);
		}
	}
	return entry;

}

message::GirlsReservationInfo* girls_manager::get_reservation(account_type account, u64 send_time)
{
	message::GirlsReservationInfo* ret = NULL;
	std::pair<account_type, u64> entry;
	entry.first = account;
	entry.second = send_time;
	MAPGirlsReservation::iterator it = _Reservation.find(entry);
	if (it != _Reservation.end())
	{
		ret = &it->second;
	}
	return ret;
}

void girls_manager::clear_girls()
{
	MAPGIRLS::iterator it = _girls.begin();
	for (; it != _girls.end(); ++ it)
	{
		delete it->second;
	}
	_girls.clear();
}

void girls_manager::load_girls(message::GirlListACK* msg)
{
	if (msg->begin())
	{
		clear_girls();
	}
	
	int size = msg->girls_size();
	for (int i = 0; i < size; i ++)
	{
		const message::GirlInfo info_entry = msg->girls(i);
		girl_info* info = new girl_info();
		info->set(&info_entry);
		_girls[info->account_] = info;
	}
	if (msg->end())
	{
		g_dlg->_dlg_girls->load_girls();
	}
	
}

ADVICESLIST& girls_manager::get_advices()
{
	return _advices;
}

message::GoodsCDKEYInfo* girls_manager::get_good_cdkey(int id)
{
	message::GoodsCDKEYInfo* entry = NULL;
	MAPGOODSCDKEY::iterator it = _good_cdkey.find(id);
	if (it != _good_cdkey.end())
	{
		entry = &it->second;
	}
	return entry;
}

void girls_manager::add_girl_info(message::notifyAddGirlInfo* msg)
{
	girl_info* info = new girl_info();
	info->set(&msg->info());
	_girls[info->account_] = info;	
	g_dlg->_dlg_girls->load_girls();
}

void girls_manager::add_advice(message::AdviceListACK* msg)
{
	if (msg->begin())
	{
		_advices.clear();
	}
	int size_temp = msg->advices_size();
	for (int i = 0; i < size_temp; i ++)
	{
		message::AdviceInfo info = msg->advices(i);
		_advices.push_back(info);		
	}
	if (msg->end())
	{
		g_dlg->_dlg_advice->load_advice();
	}
}

void girls_manager::add_girls_reservation_info_list_ACK(message::GirlsReservationInfoListACK* msg)
{
	if (msg->begin())
	{
		_Reservation.clear();
	}
	
	int size_temp = msg->girl_reservations_size();
	for (int i = 0; i < size_temp; i ++)
	{
		message::GirlsReservationInfo info = msg->girl_reservations(i);
		std::pair<account_type, u64> entry;
		entry.first = info.account();
		entry.second = info.info().send_time();
		_Reservation.insert(MAPGirlsReservation::value_type(entry, info));
	}
	if (msg->end())
	{
		g_dlg->_dlg_reservation->load_reservation();
	
	}	
}

void girls_manager::add_goods_cdkey(message::GoodsCDKEYInfoListACK* msg)
{
	if (msg->begin())
	{
		_good_cdkey.clear();
	}
	int count = msg->infos_size();
	for (int i = 0; i < count; i ++)
	{
		message::GoodsCDKEYInfo entry_info = msg->infos(i);
		message::GoodsCDKEYInfo temp_info;
		temp_info.CopyFrom(entry_info);
		_good_cdkey[temp_info.info().good_id()] = temp_info;
	}
	if (msg->end())
	{
		g_dlg->_dlg_shop->load_goods();
	}
}

void girls_manager::add_goods_cdkey(message::AddGoodsACK* msg)
{
	int good_temp_id = msg->info().info().good_id();
	message::GoodsCDKEYInfo temp_info;
	temp_info.CopyFrom(msg->info());
	_good_cdkey[temp_info.info().good_id()] = temp_info;
	g_dlg->_dlg_shop->add_single_item(good_temp_id);
}

bool girls_manager::have_goods_key(int id)
{
	MAPGOODSCDKEY::iterator it = _good_cdkey.find(id);
	if (it != _good_cdkey.end())
	{
		return true;
	}
	return false;
}
