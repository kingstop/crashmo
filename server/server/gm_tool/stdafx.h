
// stdafx.h : ��׼ϵͳ�����ļ��İ����ļ���
// ���Ǿ���ʹ�õ��������ĵ�
// �ض�����Ŀ�İ����ļ�

#pragma once

#ifndef _SECURE_ATL
#define _SECURE_ATL 1
#endif

#ifndef VC_EXTRALEAN
#define VC_EXTRALEAN            // �� Windows ͷ���ų�����ʹ�õ�����
#endif

#include "targetver.h"

#define _ATL_CSTRING_EXPLICIT_CONSTRUCTORS      // ĳЩ CString ���캯��������ʽ��

// �ر� MFC ��ĳЩ�����������ɷ��ĺ��Եľ�����Ϣ������
#define _AFX_ALL_WARNINGS

#include <afxwin.h>         // MFC ��������ͱ�׼���
#include <afxext.h>         // MFC ��չ


#include <afxdisp.h>        // MFC �Զ�����



#ifndef _AFX_NO_OLE_SUPPORT
#include <afxdtctl.h>           // MFC �� Internet Explorer 4 �����ؼ���֧��
#endif
#ifndef _AFX_NO_AFXCMN_SUPPORT
#include <afxcmn.h>             // MFC �� Windows �����ؼ���֧��
#endif // _AFX_NO_AFXCMN_SUPPORT

#include <afxcontrolbars.h>     // �������Ϳؼ����� MFC ֧��

#include <iostream>
#include <string>
#include "girls_manager.h"
class Cgm_toolDlg;
extern girls_manager g_girls_manager;
extern Cgm_toolDlg* g_dlg;






#ifdef _UNICODE
#if defined _M_IX86
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='x86' publicKeyToken='6595b64144ccf1df' language='*'\"")
#elif defined _M_X64
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='amd64' publicKeyToken='6595b64144ccf1df' language='*'\"")
#else
#pragma comment(linker,"/manifestdependency:\"type='win32' name='Microsoft.Windows.Common-Controls' version='6.0.0.0' processorArchitecture='*' publicKeyToken='6595b64144ccf1df' language='*'\"")
#endif
#endif#include <time.h>#include "task_thread_pool.h"#include "io_service_pool.h"
#include "tcpsession.h"#include "tcpclient.h"
#include "tcpserver.h"
#include "net_type.h"
#include "my_log.h"
#include "crypt.h"
#include "memory.h"
#include "database.h"
#include "event_table_object.h"
#include "event_manager.h"
#include "server_frame.h"
#include "message/server_define.h"
#include "message/login.pb.h"
#include "message/msg_gate_login.pb.h"
#include "message/msgs2s.pb.h"
#include "message/girls.pb.h"
#include "protoc_common.h"
#include "version.h"
#include "client.h"

extern Client* g_client;
#include "gm_toolDlg.h"
#include "dlg_girls.h"
#include "dlg_login.h"
std::string get_time(time_t cur_time);
std::vector<std::string> split(std::string str,std::string pattern);
//CString Convert(CString str, int sourceCodepage, int targetCodepage);

char* Utf8ToGBK(const char* strUtf8);

char* GBKToUtf8(const char* strGBK);