
// stdafx.cpp : 只包括标准包含文件的源文件
// gm_tool.pch 将作为预编译头
// stdafx.obj 将包含预编译类型信息

#include "stdafx.h"


girls_manager g_girls_manager;
Cgm_toolDlg* g_dlg;

std::string get_time(time_t cur_time)
{
	time_t timep;
	if (cur_time == 0)
	{
		time(&timep); /*获取time_t类型的当前时间*/
	}
	else
	{
		timep = cur_time;
	}
	struct tm* cur = localtime(&timep);
	char sz_time[256];
	sprintf(sz_time, "%d-%d-%d %d:%d:%d",cur->tm_year + 1900, cur->tm_mon + 1, cur->tm_mday, cur->tm_hour, cur->tm_min, cur->tm_sec);

	struct tm ycur_time;
	sscanf(sz_time, "%d-%d-%d %d:%d:%d",
		&ycur_time.tm_year, &ycur_time.tm_mon, &ycur_time.tm_mday, &ycur_time.tm_hour, &ycur_time.tm_min, &ycur_time.tm_sec);

	time_t t = mktime(&ycur_time);
	return std::string(sz_time);
}

char* Utf8ToGBK(const char* strUtf8)
{
	int len=MultiByteToWideChar(CP_UTF8, 0, (LPCTSTR)strUtf8, -1, NULL,0); 
	unsigned short * wszGBK = new unsigned short[len+1];       
	memset(wszGBK, 0, len * 2 + 2); 
	MultiByteToWideChar(CP_UTF8, 0, (LPCTSTR)strUtf8, -1, (LPWSTR)wszGBK, len);
	len = WideCharToMultiByte(CP_ACP, 0, (LPCWSTR)wszGBK, -1, NULL, 0, NULL, NULL);
	char *szGBK=new char[len + 1]; 
	memset(szGBK, 0, len + 1); 
	WideCharToMultiByte (CP_ACP, 0, (LPCWSTR)wszGBK, -1, (LPSTR)szGBK, len, NULL,NULL);
	return szGBK; 
}

char* GBKToUtf8(const char* strGBK)
{ 
	int len=MultiByteToWideChar(CP_ACP, 0, (LPCTSTR)strGBK, -1, NULL,0); 
	unsigned short * wszUtf8 = new unsigned short[len+1]; 
	memset(wszUtf8, 0, len * 2 + 2); 
	MultiByteToWideChar(CP_ACP, 0, (LPCTSTR)strGBK, -1, (LPWSTR)wszUtf8, len);
	len = WideCharToMultiByte(CP_UTF8, 0, (LPCWSTR)wszUtf8, -1, NULL, 0, NULL, NULL);
	char *szUtf8=new char[len + 1]; 
	memset(szUtf8, 0, len + 1); 
	WideCharToMultiByte (CP_UTF8, 0, (LPCWSTR)wszUtf8, -1, (LPSTR)szUtf8, len, NULL,NULL);
	return szUtf8; 
}
std::vector<std::string> split(std::string str,std::string pattern)
{
	std::string::size_type pos;
	std::vector<std::string> result;
	str+=pattern;//扩展字符串以方便操作
	int size=str.size();

	for(int i=0; i<size; i++)
	{
		pos=str.find(pattern,i);
		if(pos<size)
		{
			std::string s=str.substr(i,pos-i);
			result.push_back(s);
			i=pos+pattern.size()-1;
		}
	}
	return result;
}
