// dlg_login.cpp : implementation file
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_login.h"
#include "afxdialogex.h"
#include "gm_toolDlg.h"

// dlg_login dialog

IMPLEMENT_DYNAMIC(dlg_login, CDialogEx)

dlg_login::dlg_login(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_login::IDD, pParent)
{

}

dlg_login::~dlg_login()
{
}

void dlg_login::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}

BOOL dlg_login::PreTranslateMessage(MSG* pMsg)
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

BEGIN_MESSAGE_MAP(dlg_login, CDialogEx)
	ON_BN_CLICKED(IDOK, &dlg_login::OnBnClickedOk)
	ON_BN_CLICKED(IDCANCEL, &dlg_login::OnBnClickedCancel)
END_MESSAGE_MAP()


// dlg_login message handlers


void dlg_login::OnBnClickedOk()
{
	CString account_temp;
	GetDlgItemText(IDC_EDIT_ACCOUNT, account_temp);
	account_temp = GBKToUtf8(account_temp);
	CString password_temp;
	GetDlgItemText(IDC_EDIT_PASS_WORD, password_temp);
	CString ip_temp;
	GetDlgItemText(IDC_EDIT_IP, ip_temp);
	if (ip_temp.GetLength() != NULL)
	{
		g_client->set_ip(ip_temp);
	}
	password_temp = GBKToUtf8(password_temp);
	g_client->set_account(account_temp);
	g_client->set_password(password_temp);
	set_notify("登录中...");
	if (g_client->is_connected() == false)
	{
		g_client->reConnect();
	}
	enable(false);
	//((CEdit*)GetDlgItem(IDC_EDIT_ACCOUNT))->SetReadOnly(TRUE);
	//((CEdit*)GetDlgItem(IDC_EDIT_PASS_WORD))->SetReadOnly(TRUE);
	 //CButton*
	//g_dlg->update_tab_status();
	// TODO: 在此添加控件通知处理程序代码
	//CDialogEx::OnOK();
}


void dlg_login::enable(bool b)
{
	((CEdit*)GetDlgItem(IDC_EDIT_ACCOUNT))->EnableWindow(b);
	((CEdit*)GetDlgItem(IDC_EDIT_PASS_WORD))->EnableWindow(b);
	GetDlgItem(IDCANCEL)->EnableWindow(b);
	GetDlgItem(IDOK)->EnableWindow(b);

}

void dlg_login::set_notify(const char* notify)
{
	SetDlgItemText(IDC_EDIT_NOTIFY, notify);
}


void dlg_login::OnBnClickedCancel()
{
	g_dlg->OnBnClickedCancel();
	// TODO: Add your control notification handler code here
	//CDialogEx::OnCancel();
	//GetParent()->
}
