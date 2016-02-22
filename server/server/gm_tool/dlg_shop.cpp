// dlg_shop.cpp : implementation file
//

#include "stdafx.h"
#include "gm_tool.h"
#include "dlg_shop.h"
#include "afxdialogex.h"


// dlg_shop dialog

IMPLEMENT_DYNAMIC(dlg_shop, CDialogEx)

dlg_shop::dlg_shop(CWnd* pParent /*=NULL*/)
	: CDialogEx(dlg_shop::IDD, pParent)
{

}

dlg_shop::~dlg_shop()
{
}

void dlg_shop::DoDataExchange(CDataExchange* pDX)
{
	CDialogEx::DoDataExchange(pDX);
	DDX_Control(pDX, IDC_LIST1, _good_id_list);
	DDX_Control(pDX, IDC_LIST2, _cd_key_list);
}

BOOL dlg_shop::PreTranslateMessage(MSG* pMsg)
{
	if(pMsg->message==WM_KEYDOWN && pMsg->wParam==VK_ESCAPE )
	{
		return TRUE;
	}
	else
	{
		return CDialogEx::PreTranslateMessage(pMsg);
	}
}


void dlg_shop::clear_cdkey_list()
{
	int count = 0;
	count = _cd_key_list.GetCount();
	for (int i = 0; i < count; i ++)
	{
		_cd_key_list.DeleteString(i);
	}
}

void dlg_shop::clear_good_list()
{
	int count = 0;
	count = _good_id_list.GetCount();
	for (int i = 0; i < count; i ++)
	{
		_good_id_list.DeleteString(i);
	}

	//count = _cd_key_list.GetCount();
	//for (int i = 0; i < count; i ++)
	//{
	//	_cd_key_list.DeleteString(i);
	//}
}

void dlg_shop::add_single_item(int id)
{
	char temp[256];
	sprintf(temp, "%d", id);
	_good_id_list.AddString(temp);
}

void dlg_shop::load_goods()
{
	clear_cdkey_list();
	clear_good_list();
	MAPGOODSCDKEY::iterator it_goods = g_girls_manager._good_cdkey.begin();
	for (; it_goods != g_girls_manager._good_cdkey.end(); ++it_goods)
	{
		message::GoodsCDKEYInfo entry = it_goods->second;
		int good_id_temp = it_goods->first;
		char temp[256];
		sprintf(temp, "%d", good_id_temp);
		_good_id_list.AddString(temp);
	}
	OnLbnSelchangeList1();
}


BEGIN_MESSAGE_MAP(dlg_shop, CDialogEx)
	ON_LBN_SELCHANGE(IDC_LIST1, &dlg_shop::OnLbnSelchangeList1)
	ON_BN_CLICKED(IDC_BUTTON_ADD_GOODS, &dlg_shop::OnBnClickedButtonAddGoods)
END_MESSAGE_MAP()


void dlg_shop::enable(bool b)
{
	GetDlgItem(IDC_EDIT_GOOD_ID)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GOOD_PRICE)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GOOD_NAME)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GOOD_DESCRIP)->EnableWindow(b);
	GetDlgItem(IDC_EDIT_GOOD_COUNT)->EnableWindow(b);
	GetDlgItem(IDC_BUTTON_ADD_GOODS)->EnableWindow(b);
}

void dlg_shop::set_edits(message::GoodsCDKEYInfo* info_entry)
{
	if (info_entry != NULL)
	{
		SetDlgItemInt(IDC_EDIT_GOOD_ID, info_entry->info().good_id());
		SetDlgItemInt(IDC_EDIT_GOOD_PRICE, info_entry->info().price());
		SetDlgItemText(IDC_EDIT_GOOD_NAME, Utf8ToGBK(info_entry->info().name().c_str()));
		SetDlgItemText(IDC_EDIT_GOOD_DESCRIP, Utf8ToGBK(info_entry->info().description().c_str()));
		SetDlgItemInt(IDC_EDIT_GOOD_COUNT, info_entry->cdkeys_size());
	}
}

void dlg_shop::clear_edits()
{
	SetDlgItemText(IDC_EDIT_GOOD_ID, "");
	SetDlgItemText(IDC_EDIT_GOOD_PRICE, "");
	SetDlgItemText(IDC_EDIT_GOOD_NAME, "");
	SetDlgItemText(IDC_EDIT_GOOD_DESCRIP, "");
	SetDlgItemText(IDC_EDIT_GOOD_COUNT, "");
}

// dlg_shop message handlers

void dlg_shop::update_edit_cdkey_list()
{
	int cur_sel = _good_id_list.GetCurSel();
	if (cur_sel != -1)
	{
		CString text;
		_good_id_list.GetText(cur_sel, text);
		int good_id = atoi(text);
		message::GoodsCDKEYInfo* info_entry = g_girls_manager.get_good_cdkey(good_id);
		char sz_cdkey[256];
		if (info_entry)
		{
			clear_cdkey_list();
			int cd_key_size = info_entry->cdkeys_size();
			for (int i = 0; i < cd_key_size; i ++)
			{
				message::string_bool_pair pair_entry = info_entry->cdkeys(i);
				sprintf(sz_cdkey, "%s|%d", pair_entry.key().c_str(), pair_entry.exchanged());
				_cd_key_list.AddString(sz_cdkey);
			}
			set_edits(info_entry);
		}
		
	}
}


void dlg_shop::OnLbnSelchangeList1()
{
	int count = _good_id_list.GetCount();
	update_edit_cdkey_list();
	// TODO: Add your control notification handler code here
}


void dlg_shop::OnBnClickedButtonAddGoods()
{
	int goods_id = GetDlgItemInt(IDC_EDIT_GOOD_ID);
	int goods_pricce = GetDlgItemInt(IDC_EDIT_GOOD_PRICE);
	CString name_goods;
	GetDlgItemText(IDC_EDIT_GOOD_NAME, name_goods);
	name_goods = GBKToUtf8(name_goods);
	CString descrip_goods;
	GetDlgItemText(IDC_EDIT_GOOD_DESCRIP, descrip_goods);
	descrip_goods  = GBKToUtf8(descrip_goods);
	int goods_count = GetDlgItemInt(IDC_EDIT_GOOD_COUNT);
	if (g_girls_manager.have_goods_key(goods_id))
	{
		g_dlg->set_title("已经有这个物品id");
	}
	else
	{
		if (goods_count <= 0 && goods_count >=100)
		{
			g_dlg->set_title("物品数量不得低于0,不得大于99");
		}
		else
		{
			if (goods_pricce <= 0)
			{
				g_dlg->set_title("物品价格不得低于0");
			}
			else
			{
				if (name_goods.GetLength() == 0 || descrip_goods.GetLength() == 0)
				{
					g_dlg->set_title("必须得填写物品名字 和 介绍");
				}
				else
				{
					enable(false);
					g_client->add_goods_cdkey(goods_id, goods_pricce, goods_count, name_goods, descrip_goods);
				}
			}
		}
	}
	// TODO: Add your control notification handler code here
}
