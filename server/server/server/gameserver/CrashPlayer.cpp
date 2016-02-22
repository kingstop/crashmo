#include "StdAfx.h"
#include "CrashPlayer.h"
#include "session.h"
#define _SAVE_PLAYER_FARM_TIME_  (10 * 10 * _TIME_SECOND_MSEL_)

CrashPlayer::CrashPlayer(Session* session):_session(session)
{
}


CrashPlayer::~CrashPlayer(void)
{
}


void CrashPlayer::SetInfo(message::CrashPlayerInfo info)
{
	_info = info;
}


message::CrashPlayerInfo CrashPlayer::GetInfo()
{
	return _info;
}

void CrashPlayer::SetSession(Session* session)
{
	_session = session;
}

Session* CrashPlayer::GetSession()
{
	return _session;
}

void CrashPlayer::SaveCrashInfo()
{
	gGSDBClient.sendPBMessage(&_info, _session->getTranId());
}

void CrashPlayer::StartSave()
{
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == false)
	{
		gEventMgr.addEvent(this,&CrashPlayer::SaveCrashInfo, EVENT_SAVE_PLAYER_DATA_, _SAVE_PLAYER_FARM_TIME_, 1, 0);
	}
}

void CrashPlayer::StopSave()
{
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == true)
	{
		gEventMgr.removeEvents(this, EVENT_SAVE_PLAYER_DATA_);
	}
}
