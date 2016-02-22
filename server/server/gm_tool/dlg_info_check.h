#pragma once


// dlg_info_check dialog

class dlg_info_check : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_info_check)

public:
	dlg_info_check(CWnd* pParent = NULL);   // standard constructor
	virtual ~dlg_info_check();

// Dialog Data
	enum { IDD = IDD_DIALOG_INFO_EDIT };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support

	DECLARE_MESSAGE_MAP()
};
