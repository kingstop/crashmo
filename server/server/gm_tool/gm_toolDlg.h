
// gm_toolDlg.h : ͷ�ļ�
//

#pragma once
#include "afxcmn.h"
#include "resource.h"

class dlg_login;
class dlg_girls;
class dlg_advice;
class dlg_reservation;
class dlg_shop;

//class dlg_info_check;
enum en_status
{
	status_info_check,
	status_reservation,
	status_advice,
	status_shop
};

// Cgm_toolDlg �Ի���
class Cgm_toolDlg : public CDialogEx
{
// ����
public:
	Cgm_toolDlg(CWnd* pParent = NULL);	// ��׼���캯��

// �Ի�������
	enum { IDD = IDD_GM_TOOL_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV ֧��
// ʵ��
protected:
	HICON m_hIcon;
	// ���ɵ���Ϣӳ�亯��
	virtual BOOL OnInitDialog();
	afx_msg void OnPaint();
	afx_msg HCURSOR OnQueryDragIcon();
	DECLARE_MESSAGE_MAP()

public:
	void create_account_sucessful(const char* account, const char* password);
	void set_title(const char* title);
	afx_msg void OnBnClickedOk();
	dlg_login* _dlg_login;
	dlg_girls* _dlg_girls;
	dlg_advice* _dlg_advice;
	dlg_reservation* _dlg_reservation;
	dlg_shop* _dlg_shop;
	//dlg_info_check* _dlg_info_check;
	afx_msg void OnMoving(UINT fwSide, LPRECT pRect);
	afx_msg void OnMove(int x, int y);
	void update_tab_status();
	CTabCtrl _tab_status;
	en_status _tab_status_type;
	afx_msg void OnTimer(UINT_PTR nIDEvent);
	afx_msg void OnBnClickedCancel();
	afx_msg void OnTcnSelchangeTab2(NMHDR *pNMHDR, LRESULT *pResult);
};
