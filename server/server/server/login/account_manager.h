#ifndef _account_manager_h_
#define _account_manager_h_
class UserLoginSession;
struct CheckAcct
{
	enum
	{
		_check_pass_,
		_not_found_,
		_error_name_pwd,
		_already_have_acc

	};
	enum
	{
		_login_check_,
		_new_acc_check_

	};
	CheckAcct( const std::string& name, const std::string& password, u8 t,UserLoginSession* p)
	{
		acct = INVALID_ACCOUNT;
		str = name;
		pwd = password;
		ower = p;
		result = _check_pass_;
		check_type = t;
	}
	std::string str;
	std::string pwd;
	u8 result;
	UserLoginSession* ower;
	u32 acct;
	u8 check_type;

};
struct Account : PUBLIC_BASE_OBJECT(Account)
{
	REGISTER_POOL_INFO(Account, 2000, 0)
	
	Account()
	{
		nId = INVALID_ACCOUNT;
		nGateId = INVALID_GATE_ID;
		nTranId = INVALID_TRANS_ID;
	}

    u16 nGateId;
	tran_id_type nId;
	account_type nTranId;
};


struct BanAndMute : PUBLIC_BASE_OBJECT(BanAndMute)
{
	REGISTER_POOL_INFO(BanAndMute, 500, 0)

    BanAndMute()
    {
        nId = INVALID_ACCOUNT;
        nMute = 0;
        nBan = 0;
    }
	
    u32 nId;
	u32 nMute;
	u32 nBan;
};
class UserLoginSession;
class AccountManager
{
public:
	AccountManager();
	~AccountManager();

	bool loadBanCall(const UDBResult& r);
	void checkAccount(const std::string&strName, const std::string& strPwd, UserLoginSession* p , u16 t, const char* s);
	
	void batchQuery(DBQuery* p, const void* data);
	void CreateNewAccount(const void* data, bool sucess);
	void checkAccountCall(const void* data, bool sucess);
	u32  makeTransId(std::string& str);
	bool isBanned(u32 nAccountId);
	void removeAccount(u32 nAccountId);
	bool addAccount(u32 account, u32 tranid, u16 gate);
    Account* getAccount(u32 nAccId);
protected:
	obj_ptr_map<u32, Account> m_Accts;
	obj_ptr_map<u32, BanAndMute> m_BanAndMute;
};
#endif