
// gm_tool.h : PROJECT_NAME Ӧ�ó������ͷ�ļ�
//

#pragma once

#ifndef __AFXWIN_H__
	#error "�ڰ������ļ�֮ǰ������stdafx.h�������� PCH �ļ�"
#endif

#include "resource.h"		// ������


// Cgm_toolApp:
// �йش����ʵ�֣������ gm_tool.cpp
//

class Cgm_toolApp : public CWinApp
{
public:
	Cgm_toolApp();

// ��д
public:
	virtual BOOL InitInstance();

// ʵ��

	DECLARE_MESSAGE_MAP()
};

extern Cgm_toolApp theApp;