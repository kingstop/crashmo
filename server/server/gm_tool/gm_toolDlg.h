
// gm_toolDlg.h : 头文件
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

// Cgm_toolDlg 对话框
class Cgm_toolDlg : public CDialogEx
{
// 构造
public:
	Cgm_toolDlg(CWnd* pParent = NULL);	// 标准构造函数

// 对话框数据
	enum { IDD = IDD_GM_TOOL_DIALOG };

	protected:
	virtual void DoDataExchange(CDataExchange* pDX);	// DDX/DDV 支持
// 实现
protected:
	HICON m_hIcon;
	// 生成的消息映射函数
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
