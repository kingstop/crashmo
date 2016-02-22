// dlg_reservation.cpp : implementation file
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_reservation.h"
#include "afxdialogex.h"
#include "girls_manager.h"


// dlg_reservation dialog

IMPLEMENT_DYNAMIC(dlg_reservation, CDialogEx)

dlg_reservation::dlg_reservation(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_reservation::IDD, pParent)
{

}

dlg_reservation::~dlg_reservation()
{
}

BOOL dlg_reservation::OnInitDialog()
{
	enable_edit(false);
	return CDialogEx::OnInitDialog();
	
}

void dlg_reservation::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_RESERVATION, _reservation_list);
}



void dlg_reservation::clear_reservation_list()
{
	int size_temp = _reservation_list.GetCount();
	for (int i = 0; i < size_temp; i ++)
	{
		_reservation_list.DeleteString(i);
	}
}


void dlg_reservation::clear_edits()
{
	SetDlgItemText(IDC_EDIT_CARD, "");
	SetDlgItemText(IDC_EDIT_SEND_TIME, "");

	SetDlgItemText(IDC_EDIT_RECEIVE_TIME, "");
	SetDlgItemText(IDC_EDIT_GUEST_NUMBER, "");

	SetDlgItemText(IDC_EDIT_GUEST_NAME, "");
	SetDlgItemText(IDC_EDIT_GUEST_REQUEST, "");

}

void dlg_reservation::enable_edit(bool b)
{
	GetDlgItem(IDC_EDIT_CARD)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_SEND_TIME)->EnableWindow(b);

	GetDlgItem(IDC_EDIT_RECEIVE_TIME)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GUEST_NUMBER)->EnableWindow(b);

	GetDlgItem(IDC_EDIT_GUEST_NAME)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GUEST_REQUEST)->EnableWindow(b);
}

void dlg_reservation::load_reservation()
{
	_time_pair.clear();
	clear_reservation_list();
	MAPGirlsReservation::iterator it = g_girls_manager._Reservation.begin();
	char sztemp[256];
	for (; it != g_girls_manager._Reservation.end(); ++ it)
	{
		std::string time_data = get_time(it->first.second);
		_time_pair[time_data] =it->first.second;
		sprintf(sztemp, "%s,%lu", time_data.c_str(), it->first.first);
		_reservation_list.AddString(sztemp);
	}
	enable(true);
}


BEGIN_MESSAGE_MAP(dlg_reservation, CDialogEx)
	ON_LBN_SELCHANGE(IDC_LIST_RESERVATION, &dlg_reservation::OnLbnSelchangeListReservation)
	ON_BN_CLICKED(IDC_BTN_RECEIVED, &dlg_reservation::OnBnClickedBtnReceived)
	ON_BN_CLICKED(IDC_BUTTON_NEWS, &dlg_reservation::OnBnClickedButtonNews)
	ON_BN_CLICKED(IDC_BUTTON_UPDATE, &dlg_reservation::OnBnClickedButtonUpdate)
END_MESSAGE_MAP()


// dlg_reservation message handlers


BOOL dlg_reservation::PreTranslateMessage(MSG* pMsg)
{
	if(pMsg->message==WM_KEYDOWN && pMsg->wParam==VK_ESCAPE )
	{
		return TRUE;
	}
	else
	{
		return CDialogEx::PreTranslateMessage(pMsg);
	}
}


void dlg_reservation::enable(bool b)
{
	//GetDlgItem(IDC_EDIT_NEW_PERFORMANCE)->EnableWindow(b);
	//GetDlgItem(IDC_BTN_MODIFY_PERFORMANCE)->EnableWindow(b);
	//GetDlgItem(IDC_BTN_ADD_ACOUNT)->EnableWindow(b);
	//GetDlgItem(IDC_BUTTON3)->EnableWindow(b);
	GetDlgItem(IDC_BTN_RECEIVED)->EnableWindow(b);
	GetDlgItem(IDC_BUTTON_UPDATE)->EnableWindow(b);
}

void dlg_reservation::set_edits(message::GirlsReservationInfo* reservation_info)
{
	SetDlgItemInt(IDC_EDIT_CARD, reservation_info->account());
	SetDlgItemText(IDC_EDIT_SEND_TIME, get_time(reservation_info->info().send_time()).c_str());
	u64 receive_time = reservation_info->info().receive_time();
	if (receive_time == 0)
	{
		SetDlgItemText(IDC_EDIT_RECEIVE_TIME, "未接受订单");
	}
	else
	{
		SetDlgItemText(IDC_EDIT_RECEIVE_TIME, get_time(reservation_info->info().receive_time()).c_str());
	}
	
	SetDlgItemInt(IDC_EDIT_GUEST_NUMBER, reservation_info->info().guest_number());

	SetDlgItemText(IDC_EDIT_GUEST_NAME, Utf8ToGBK(reservation_info->info().guest_name().c_str()));
	SetDlgItemText(IDC_EDIT_GUEST_REQUEST, Utf8ToGBK(reservation_info->info().guest_request().c_str()));
}
message::GirlsReservationInfo* dlg_reservation::get_current_reservation()
{
	message::GirlsReservationInfo* reservation_info = NULL;
	int cur_sel = _reservation_list.GetCurSel();
	if (cur_sel != -1)
	{
		CString temp;
		_reservation_list.GetText(cur_sel, temp);
		std::string entry = temp;
		std::vector<std::string> vc_str = split(entry, ",");
		if (vc_str.size() == 2)
		{
			account_type account;
			sscanf(vc_str[1].c_str(),"%I64u",&account);
			time_t t = _time_pair[vc_str[0]];			 
			reservation_info =  g_girls_manager.get_reservation(account, t);
		}
	}
	return reservation_info;
}
void dlg_reservation::update_edits()
{
	clear_edits();
	message::GirlsReservationInfo* reservation_info =get_current_reservation();
	if (reservation_info!= NULL)
	{
		set_edits(reservation_info);
	}

}

void dlg_reservation::OnLbnSelchangeListReservation()
{
	update_edits();
	// TODO: Add your control notification handler code here
}


void dlg_reservation::OnBnClickedBtnReceived()
{
	message::GirlsReservationInfo* reservation_info =get_current_reservation();
	if (reservation_info)
	{
		g_dlg->set_title("接收订单中");
		g_client->receive_reservation(reservation_info->account(), reservation_info->info().send_time());
	}

	// TODO: Add your control notification handler code here
}


void dlg_reservation::OnBnClickedButtonNews()
{
	CString news_content;
	GetDlgItemText(IDC_EDIT_NEWS, news_content);
	news_content = GBKToUtf8(news_content);
	int interval_time = GetDlgItemInt(IDC_EDIT_INTERVAL_TIME);
	int repeated_count = GetDlgItemInt(IDC_EDIT_REPEATED_COUNT);
	g_client->send_news(news_content, repeated_count, interval_time);
	// TODO: Add your control notification handler code here
}


void dlg_reservation::OnBnClickedButtonUpdate()
{
	enable(false);
	g_client->send_reservation_req();
	// TODO: Add your control notification handler code here
}
