// dlg_modify_password.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_modify_password.h"
#include "afxdialogex.h"


// dlg_modify_password �Ի���

IMPLEMENT_DYNAMIC(dlg_modify_password, CDialogEx)

dlg_modify_password::dlg_modify_password(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_modify_password::IDD, pParent)
{

}

dlg_modify_password::~dlg_modify_password()
{
}

void dlg_modify_password::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
}
BOOL dlg_modify_password::OnInitDialog()
{
	CDialogEx::OnInitDialog();

	switch(_type)
	{
	case dlg_type_modify_password:
		{
			set_title("���޸�����");
		}
		break;
	case dlg_type_notify_add_account:
		{
			set_title("�µĿ�������");
			((CEdit*)GetDlgItem(IDC_EDIT_CARD))->SetReadOnly(TRUE);
			((CEdit*)GetDlgItem(IDC_EDIT_PASSWORD))->SetReadOnly(TRUE);
			SetDlgItemText(IDC_EDIT_CARD, _account.c_str());
			SetDlgItemText(IDC_EDIT_PASSWORD, _pass_word.c_str());
		}
		break;
	}
	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}


void dlg_modify_password::set_title(const char* title)
{
	SetDlgItemText(IDC_EDIT_TITLE, title);
}

void dlg_modify_password::set_type(modify_dlg_type type)
{
	_type = type;
}

void dlg_modify_password::set_account(const char* acc)
{
	_account = acc;
}
void dlg_modify_password::set_password(const char* pw)
{
	_pass_word = pw;
}


BEGIN_MESSAGE_MAP(dlg_modify_password, CDialogEx)
	ON_BN_CLICKED(IDOK, &dlg_modify_password::OnBnClickedOk)
END_MESSAGE_MAP()


// dlg_modify_password ��Ϣ�������


void dlg_modify_password::OnBnClickedOk()
{
	CString pass_word;
	CString card;
	GetDlgItemText(IDC_EDIT_CARD,card);
	GetDlgItemText(IDC_EDIT_PASSWORD,pass_word);
	g_client->modify_pass_word(card, pass_word);
	// TODO: Add your control notification handler code here
	CDialogEx::OnOK();
}
