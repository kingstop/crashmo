#pragma once
#include "afxwin.h"


// dlg_girls 对话框

class dlg_girls : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_girls)

public:
	dlg_girls(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~dlg_girls();

// 对话框数据
	enum { IDD = IDD_DIALOG_INFO_EDIT };
public:
	girl_info* get_edits();
	girl_info* get_cur_sel_info();
protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	virtual BOOL PreTranslateMessage(MSG* pMsg);  
	void clear_list();
	void set_edits(girl_info* info);
	void clear_edits();

	DECLARE_MESSAGE_MAP()
public:
	CListBox _girls_list;
	void load_girls();
	void update_edit();
	void enable(bool b);
	afx_msg void OnLbnSelchangeListGirls();
	afx_msg void OnBnClickedBtnModifyPerformance();
	afx_msg void OnBnClickedButton3();
	afx_msg void OnBnClickedBtnAddAcount();
};
