
// gm_toolDlg.cpp : ʵ���ļ�
//

#include "stdafx.h"
#include "gm_tool.h"
#include "gm_toolDlg.h"
#include "afxdialogex.h"
#include "dlg_login.h"
#include "dlg_girls.h"
#include "client.h"
#include "dlg_modify_password.h"
#include "dlg_advice.h"
#include "dlg_reservation.h"
#include "dlg_shop.h"
#ifdef _DEBUG
#define new DEBUG_NEW
#endif


// Cgm_toolDlg �Ի���


Client* g_client = NULL;

Cgm_toolDlg::Cgm_toolDlg(CWnd* pParent /*=NULL*/)
	: CDialogEx(Cgm_toolDlg::IDD, pParent),_dlg_login(NULL),_dlg_girls(NULL)
{
	m_hIcon = AfxGetApp()->LoadIcon(IDR_MAINFRAME);
	g_dlg = this;
}

void Cgm_toolDlg::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);

	DDX_Control(pDX, IDC_TAB2, _tab_status);
}

BEGIN_MESSAGE_MAP(Cgm_toolDlg, CDialogEx)
	ON_WM_PAINT()
	ON_WM_QUERYDRAGICON()
	ON_BN_CLICKED(IDOK, &Cgm_toolDlg::OnBnClickedOk)
	ON_WM_MOVING()
	ON_WM_MOVE()
	ON_WM_TIMER()
	ON_BN_CLICKED(IDCANCEL, &Cgm_toolDlg::OnBnClickedCancel)
	ON_NOTIFY(TCN_SELCHANGE, IDC_TAB2, &Cgm_toolDlg::OnTcnSelchangeTab2)
END_MESSAGE_MAP()


// Cgm_toolDlg ��Ϣ�������

BOOL Cgm_toolDlg::OnInitDialog()
{
	CDialogEx::OnInitDialog();
	service_config conf;
	if(!ServerFrame::loadServiceConfig(conf, SERVER_CONFIG))
	{  
		return false;
	}
	SetTimer(1,1000,NULL);
	net_global::init_net_service( conf.thread_count, conf.proc_interval, NULL, conf.speed_, conf.msg_pool_size);
	net_info net_temp;
	Client::initPBModule();
	g_client = new Client(net_temp);
	// ���ô˶Ի����ͼ�ꡣ��Ӧ�ó��������ڲ��ǶԻ���ʱ����ܽ��Զ�
	//  ִ�д˲���
	SetIcon(m_hIcon, TRUE);			// ���ô�ͼ��
	SetIcon(m_hIcon, FALSE);		// ����Сͼ��
	_tab_status.InsertItem(0 ,(LPCTSTR)"��Ϣ��ѯ");
	_tab_status.InsertItem(1 ,(LPCTSTR)"ԤԼ��ѯ");
	_tab_status.InsertItem(2 ,(LPCTSTR)"�����ѯ");
	_tab_status.InsertItem(3 ,(LPCTSTR)"�̵���Ϣ");
	// TODO: �ڴ���Ӷ���ĳ�ʼ������
	_dlg_login = new dlg_login();
	_dlg_login->Create(IDD_DIALOG_LOGIN,this);
	_tab_status_type = status_info_check;
	//_dlg_login->SetWindowPos(&wndTopMost, 0, 0, 0, 0, SWP_NOMOVE|SWP_NOSIZE);
	_dlg_login->ShowWindow(SW_SHOW);
	_dlg_girls = new dlg_girls();
	_dlg_girls->Create(IDD_DIALOG_INFO_EDIT, this);
	_tab_status_type = status_info_check;
	_dlg_advice = new dlg_advice( );
	_dlg_advice->Create(IDD_DIALOG_ADVICE, this);
	_dlg_reservation = new dlg_reservation( );
	_dlg_reservation->Create(IDD_DIALOG_RESERVATION, this);
	_dlg_shop = new dlg_shop();
	_dlg_shop->Create(IDD_DIALOG_SHOP,this);
	

	return TRUE;  // ���ǽ��������õ��ؼ������򷵻� TRUE
}

// �����Ի��������С����ť������Ҫ����Ĵ���
//  �����Ƹ�ͼ�ꡣ����ʹ���ĵ�/��ͼģ�͵� MFC Ӧ�ó���
//  �⽫�ɿ���Զ���ɡ�

void Cgm_toolDlg::OnPaint()
{
	if (IsIconic())
	{
		CPaintDC dc(this); // ���ڻ��Ƶ��豸������

		SendMessage(WM_ICONERASEBKGND, reinterpret_cast<WPARAM>(dc.GetSafeHdc()), 0);

		// ʹͼ���ڹ����������о���
		int cxIcon = GetSystemMetrics(SM_CXICON);
		int cyIcon = GetSystemMetrics(SM_CYICON);
		CRect rect;
		GetClientRect(&rect);
		int x = (rect.Width() - cxIcon + 1) / 2;
		int y = (rect.Height() - cyIcon + 1) / 2;

		// ����ͼ��
		dc.DrawIcon(x, y, m_hIcon);
	}
	else
	{
		CDialogEx::OnPaint();
	}
}

//���û��϶���С������ʱϵͳ���ô˺���ȡ�ù��
//��ʾ��
HCURSOR Cgm_toolDlg::OnQueryDragIcon()
{
	return static_cast<HCURSOR>(m_hIcon);
}



void Cgm_toolDlg::OnBnClickedOk()
{
	// TODO: Add your control notification handler code here
	CDialogEx::OnOK();
}

void Cgm_toolDlg::set_title(const char* title)
{
	SetDlgItemText(IDC_EDIT_TITLE, title);
}

void Cgm_toolDlg::create_account_sucessful(const char* account, const char* password)
{
	set_title("�����˻��ɹ�");
	dlg_modify_password dlg;
	dlg.set_account(account);
	dlg.set_password(password);
	dlg.set_type(dlg_type_notify_add_account);	
	dlg.DoModal();
}

void Cgm_toolDlg::OnMoving(UINT fwSide, LPRECT pRect)
{
	CDialogEx::OnMoving(fwSide, pRect);

	// TODO: Add your message handler code here
}

void Cgm_toolDlg::update_tab_status()
{
	switch(_tab_status_type)
	{
	case  status_info_check:
		{
			_dlg_girls->ShowWindow(TRUE);
			_dlg_advice->ShowWindow(FALSE);
			_dlg_reservation->ShowWindow(FALSE);
			_dlg_shop->ShowWindow(FALSE);
		}
		break;
	case status_advice:
		{
			_dlg_girls->ShowWindow(FALSE);
			_dlg_advice->ShowWindow(TRUE);
			_dlg_reservation->ShowWindow(FALSE);
			_dlg_shop->ShowWindow(FALSE);
		}
		break;

	case status_reservation:
		{
			_dlg_girls->ShowWindow(FALSE);
			_dlg_advice->ShowWindow(FALSE);
			_dlg_shop->ShowWindow(FALSE);
			_dlg_reservation->ShowWindow(TRUE);
		}
		break;
	case status_shop:
		{
			_dlg_girls->ShowWindow(FALSE);
			_dlg_advice->ShowWindow(FALSE);
			_dlg_shop->ShowWindow(TRUE);
			_dlg_reservation->ShowWindow(FALSE);
		}
	}
}

void Cgm_toolDlg::OnMove(int x, int y)
{
	CDialogEx::OnMove(x, y);
	if (_dlg_login)
	{
		CRect temp_rect;
		GetWindowRect(&temp_rect);	
		CRect login_rect;
		login_rect.top = temp_rect.top +30;
		login_rect.left = temp_rect.left + 10;
		login_rect.right = temp_rect.right - 10;
		login_rect.bottom = temp_rect.bottom - 10;

		
		//login_rect.top = temp_rect.top - 30;
		_dlg_login->MoveWindow(&login_rect);
		_tab_status.GetWindowRect(temp_rect);
		CRect tab_rect;
		tab_rect.top = temp_rect.top +20;
		tab_rect.left = temp_rect.left + 10;
		tab_rect.right = temp_rect.right - 10;
		tab_rect.bottom = temp_rect.bottom -10;
		_dlg_girls->MoveWindow(tab_rect);
		_dlg_advice->MoveWindow(tab_rect);
		_dlg_reservation->MoveWindow(tab_rect);
		_dlg_shop->MoveWindow(tab_rect);

	}

	// TODO: Add your message handler code here
}


void Cgm_toolDlg::OnTimer(UINT_PTR nIDEvent)
{
	// TODO: Add your message handler code here and/or call default
	g_client->update(0);
	CDialogEx::OnTimer(nIDEvent);
}


void Cgm_toolDlg::OnBnClickedCancel()
{
	
	// TODO: Add your control notification handler code here
	CDialogEx::OnCancel();
}


void Cgm_toolDlg::OnTcnSelchangeTab2(NMHDR *pNMHDR, LRESULT *pResult)
{
	// TODO: Add your control notification handler code here
	*pResult = 0;
	int tab = _tab_status.GetCurSel();
	_tab_status_type = (en_status)tab;
	update_tab_status();
}
