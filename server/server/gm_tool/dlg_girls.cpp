// dlg_girls.cpp : 实现文件
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_girls.h"
#include "afxdialogex.h"
#include "dlg_modify_password.h"
#include "girls_manager.h"

// dlg_girls 对话框

IMPLEMENT_DYNAMIC(dlg_girls, CDialogEx)

dlg_girls::dlg_girls(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_girls::IDD, pParent)
{

}

dlg_girls::~dlg_girls()
{
}

void dlg_girls::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_GIRLS, _girls_list);
}


BOOL dlg_girls::PreTranslateMessage(MSG* pMsg)
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


void dlg_girls::clear_list()
{
	int a = _girls_list.GetCount();//总列数

	for(int i=0;i<a;i++)
	{
		_girls_list.DeleteString(i);//删除ListBox 当前列

	}
}

void dlg_girls::load_girls()
{
	clear_list();
	MAPGIRLS::iterator it = g_girls_manager._girls.begin();
	for (; it != g_girls_manager._girls.end(); ++it)
	{
		girl_info* entry = it->second;
		_girls_list.AddString(entry->card_.c_str());
	}
	update_edit();
}

BEGIN_MESSAGE_MAP(dlg_girls, CDialogEx)
	ON_LBN_SELCHANGE(IDC_LIST_GIRLS, &dlg_girls::OnLbnSelchangeListGirls)
	ON_BN_CLICKED(IDC_BTN_MODIFY_PERFORMANCE, &dlg_girls::OnBnClickedBtnModifyPerformance)
	ON_BN_CLICKED(IDC_BUTTON3, &dlg_girls::OnBnClickedButton3)
	ON_BN_CLICKED(IDC_BTN_ADD_ACOUNT, &dlg_girls::OnBnClickedBtnAddAcount)
END_MESSAGE_MAP()


// dlg_girls 消息处理程序


void dlg_girls::update_edit()
{
	clear_edits();
	CString string_entry;
	if (_girls_list.GetCurSel() != -1)
	{
		_girls_list.GetText(_girls_list.GetCurSel(), string_entry);
		std::string cur_sel = string_entry;
		account_type account_temp = atoi(cur_sel.c_str());
		girl_info* info_entry = g_girls_manager.get_girl(account_temp);
		
		if (info_entry)
		{
			set_edits(info_entry);
		}
	}	
}

void dlg_girls::enable(bool b)
{
	_girls_list.EnableWindow(b);
	//GetDlgItem(IDC_EDIT_CARD)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_NAME)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_ADDR)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_PHONE_NUMBER)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_BANK_CARD)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_SCORE)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_PERFORMANCE)->EnableWindow(b);
	//GetDlgItem(IDC_EDIT_BONUS)->EnableWindow(b);

	GetDlgItem(IDC_BTN_MODIFY_PERFORMANCE)->EnableWindow(b);
	GetDlgItem(IDC_BUTTON3)->EnableWindow(b);
	GetDlgItem(IDC_BTN_ADD_ACOUNT)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_NEW_PERFORMANCE)->EnableWindow(b);

}


girl_info* dlg_girls::get_cur_sel_info()
{
	girl_info* info_entry = NULL;
	CString string_entry;
	int cur_cel = _girls_list.GetCurSel();
	if (cur_cel != -1)
	{
		_girls_list.GetText(cur_cel, string_entry);
		std::string cur_sel = string_entry;
		account_type account_temp = atoi(cur_sel.c_str());
		info_entry = g_girls_manager.get_girl(account_temp);		
	}
	return info_entry;

}
void dlg_girls::OnLbnSelchangeListGirls()
{
	
	girl_info* info_entry = get_cur_sel_info();
	clear_edits();
	if (info_entry)
	{
		set_edits(info_entry);
	}	
	
	// TODO: 在此添加控件通知处理程序代码
}

girl_info* dlg_girls::get_edits()
{
	girl_info* info = new girl_info();
	CString temp;
	GetDlgItemText(IDC_EDIT_CARD, temp);
	info->card_ = temp;
	GetDlgItemText(IDC_EDIT_NAME, temp);
	temp = GBKToUtf8(temp);
	info->name_ = temp;
	GetDlgItemText(IDC_EDIT_ADDR, temp);
	temp = GBKToUtf8(temp);
	info->addr_ = temp;
	GetDlgItemText(IDC_EDIT_PHONE_NUMBER, temp);
	temp = GBKToUtf8(temp);
	info->phone_number_ = temp;
	GetDlgItemText(IDC_EDIT_BANK_CARD, temp);
	temp = GBKToUtf8(temp);
	info->bank_card_ = temp;
	info->score_ = GetDlgItemInt(IDC_EDIT_SCORE);
	info->performance_ = GetDlgItemInt(IDC_EDIT_PERFORMANCE);
	info->bonus_ = GetDlgItemInt(IDC_EDIT_BONUS);	
	return info;
}

void dlg_girls::clear_edits()
{
	SetDlgItemText(IDC_EDIT_CARD, "");
	SetDlgItemText(IDC_EDIT_NAME, "");
	SetDlgItemText(IDC_EDIT_ADDR, "");
	SetDlgItemText(IDC_EDIT_PHONE_NUMBER, "");
	SetDlgItemText(IDC_EDIT_BANK_CARD, "");
	SetDlgItemInt(IDC_EDIT_SCORE, 0);
	SetDlgItemInt(IDC_EDIT_PERFORMANCE, 0);
	SetDlgItemInt(IDC_EDIT_BONUS, 0);
}

void dlg_girls::set_edits(girl_info* info)
{
	SetDlgItemText(IDC_EDIT_CARD, Utf8ToGBK(info->card_.c_str()));
	SetDlgItemText(IDC_EDIT_NAME, Utf8ToGBK(info->name_.c_str()));
	SetDlgItemText(IDC_EDIT_ADDR, Utf8ToGBK(info->addr_.c_str()));
	SetDlgItemText(IDC_EDIT_PHONE_NUMBER, Utf8ToGBK(info->phone_number_.c_str()));
	SetDlgItemText(IDC_EDIT_BANK_CARD, Utf8ToGBK(info->bank_card_.c_str()));
	SetDlgItemInt(IDC_EDIT_SCORE, info->score_);
	SetDlgItemInt(IDC_EDIT_PERFORMANCE, info->performance_);
	SetDlgItemInt(IDC_EDIT_BONUS, info->bonus_);
}

void dlg_girls::OnBnClickedBtnModifyPerformance()
{
	int performance_number = GetDlgItemInt(IDC_EDIT_NEW_PERFORMANCE);
	girl_info* info = get_cur_sel_info();
	if (info)
	{
		int modify = performance_number - info->performance_;
		g_client->modify_performacine(info->account_, modify);
		enable(false);
	}
	// TODO: 在此添加控件通知处理程序代码
}


void dlg_girls::OnBnClickedButton3()
{
	dlg_modify_password dlg;
	dlg.set_type(dlg_type_modify_password);	
	dlg.DoModal();
	// TODO: 在此添加控件通知处理程序代码
}


void dlg_girls::OnBnClickedBtnAddAcount()
{
	message::CreateAccountReq msg_create_req;
	
	g_client->sendPBMessage(&msg_create_req);
	enable(false);
	// TODO: Add your control notification handler code here
}
