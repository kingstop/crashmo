#pragma once
#include "afxwin.h"


// dlg_reservation dialog

class dlg_reservation : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_reservation)

public:
	dlg_reservation(CWnd* pParent = NULL);   // standard constructor
	virtual ~dlg_reservation();

// Dialog Data
	enum { IDD = IDD_DIALOG_RESERVATION };
public:
	void clear_reservation_list();
	void load_reservation();
	void enable(bool b);
	void clear_edits();
	void set_edits(message::GirlsReservationInfo* reservation_info);
	void update_edits();
	message::GirlsReservationInfo* get_current_reservation();
protected:
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV support
	virtual BOOL PreTranslateMessage(MSG* pMsg);
	void enable_edit(bool b);
	DECLARE_MESSAGE_MAP()
public:
	CListBox _reservation_list;
	afx_msg void OnLbnSelchangeListReservation();
	afx_msg void OnBnClickedBtnReceived();
	std::map<std::string, u64> _time_pair;
	afx_msg void OnBnClickedButtonNews();
	afx_msg void OnBnClickedButtonUpdate();
};
