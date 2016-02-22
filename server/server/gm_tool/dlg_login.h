#pragma once


// dlg_login dialog

class dlg_login : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_login)

public:
	dlg_login(CWnd* pParent = NULL);   // standard constructor
	virtual ~dlg_login();

// Dialog Data
	enum { IDD = IDD_DIALOG_LOGIN };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL PreTranslateMessage(MSG* pMsg);  
	DECLARE_MESSAGE_MAP()
	
public:
	void enable(bool b);
	void set_notify(const char* notify);
	afx_msg void OnBnClickedOk();
	afx_msg void OnBnClickedCancel();
};
