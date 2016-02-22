#pragma once
#include "afxwin.h"


// dlg_shop dialog

class dlg_shop : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_shop)

public:
	dlg_shop(CWnd* pParent = NULL);   // standard constructor
	virtual ~dlg_shop();

// Dialog Data
	enum { IDD = IDD_DIALOG_SHOP };

protected:
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	void clear_good_list();
	void clear_cdkey_list();
	virtual BOOL PreTranslateMessage(MSG* pMsg);  

	DECLARE_MESSAGE_MAP()
public:
	CListBox _good_id_list;
	CListBox _cd_key_list;
	void update_edit_cdkey_list();
	void load_goods();
	void add_single_item(int id);
	void set_edits(message::GoodsCDKEYInfo* info_entry);
	void clear_edits();
	void enable(bool b);
	afx_msg void OnLbnSelchangeList1();
	
	afx_msg void OnBnClickedButtonAddGoods();
};
