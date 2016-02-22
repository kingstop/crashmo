// dlg_advice.cpp : implementation file
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_advice.h"
#include "afxdialogex.h"


// dlg_advice dialog

IMPLEMENT_DYNAMIC(dlg_advice, CDialogEx)

dlg_advice::dlg_advice(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_advice::IDD, pParent)
{

}

dlg_advice::~dlg_advice()
{
}
BOOL dlg_advice::PreTranslateMessage(MSG* pMsg)
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

void dlg_advice::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST_ADVICE, _advice_list);
}



void dlg_advice::clear_list()
{
	int count = _advice_list.GetCount();
	for (int i = 0; i < count; i ++)
	{
		_advice_list.DeleteString(i);
	}
}

void dlg_advice::update_cur_sel()
{
	SetDlgItemText(IDC_EDIT_ADVICE, "");
	int cur_sel = _advice_list.GetCurSel();
	if (cur_sel != -1)
	{
		CString list_data;
		_advice_list.GetText(cur_sel, list_data);
		std::string entry_temp = list_data;
		std::vector<std::string> vc_entry = split(entry_temp, "|");		
		message::AdviceInfo* info_entry = g_girls_manager.get_advice(vc_entry[0].c_str(), _time_pair[vc_entry[1]]);
		if (info_entry)
		{
			SetDlgItemText(IDC_EDIT_ADVICE, Utf8ToGBK(info_entry->advice().c_str()));
		}				
	}
}

void dlg_advice::load_advice()
{
	clear_list();
	ADVICESLIST advices = g_girls_manager.get_advices();
	ADVICESLIST::iterator it = advices.begin();
	std::string advice_entry;
	for (; it != advices.end(); ++ it)
	{
		message::AdviceInfo entry = (*it);
		u64 sender_time = entry.sender_time(); 
		std::string str_time = get_time(sender_time);
		_time_pair[str_time] = sender_time;
		advice_entry = entry.sender_card() + "|" + str_time;
		_advice_list.AddString(advice_entry.c_str());		 
	}
}

BEGIN_MESSAGE_MAP(dlg_advice, CDialogEx)
	ON_LBN_SELCHANGE(IDC_LIST_ADVICE, &dlg_advice::OnLbnSelchangeListAdvice)
END_MESSAGE_MAP()


// dlg_advice message handlers


void dlg_advice::OnLbnSelchangeListAdvice()
{
	update_cur_sel();
	// TODO: Add your control notification handler code here
}
