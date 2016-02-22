#pragma once

// dlg_modify_password 对话框

enum modify_dlg_type
{
	dlg_type_modify_password,
	dlg_type_notify_add_account,
};
class dlg_modify_password : public CDialogEx
{
	DECLARE_DYNAMIC(dlg_modify_password)

public:
	dlg_modify_password(CWnd* pParent = NULL);   // 标准构造函数
	virtual ~dlg_modify_password();

// 对话框数据
	enum { IDD = IDD_DIALOG_MODIFY_PASSWORD };

protected:
	virtual BOOL OnInitDialog();
	virtual void DoDataExchange(CDataExchange* pDX);    // DDX/DDV 支持
	modify_dlg_type _type;
	DECLARE_MESSAGE_MAP()
	std::string _pass_word;
	std::string _account;
public:
	void set_title(const char* title);
	void set_type(modify_dlg_type type);
	void set_account(const char* acc);
	void set_password(const char* pw);
	
	afx_msg void OnBnClickedOk();
};
