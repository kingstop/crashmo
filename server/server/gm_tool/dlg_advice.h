#pragma once
#include "afxwin.h"


// dlg_advice dialog

class dlg_advice : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_advice)

public:
	dlg_advice(CWnd* pParent = NULL);   // standard constructor
	virtual ~dlg_advice();

// Dialog Data
	enum { IDD = IDD_DIALOG_ADVICE };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	DECLARE_MESSAGE_MAP()
public:
	void clear_list();
	void load_advice();
	void update_cur_sel();
	std::map<std::string, u64> _time_pair;
public:
	CListBox _advice_list;
	afx_msg void OnLbnSelchangeListAdvice();
};
