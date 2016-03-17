#include "stdafx.h"
#include "CrashPlayer.h"
#include "session.h"
#define _SAVE_PLAYER_TIME_  (10 * _TIME_SECOND_MSEL_)

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
	message::ReqSaveCharacterData msg;
	message::CrashPlayerInfo* pl_data =  msg.mutable_data();
	pl_data->CopyFrom(_info);

	gGSDBClient.sendPBMessage(&msg, _session->getTranId());
}

void CrashPlayer::StartSave()
{
	
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == false)
	{
		gEventMgr.addEvent(this,&CrashPlayer::SaveCrashInfo, EVENT_SAVE_PLAYER_DATA_, _SAVE_PLAYER_TIME_, 99999999, 0);
	}
}

void CrashPlayer::StopSave()
{
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == true)
	{
		gEventMgr.removeEvents(this, EVENT_SAVE_PLAYER_DATA_);
	}
}

bool CrashPlayer::havemapname(const char* mapname)
{
	int size_ar = _info.incompletemap_size();
	std::string map_name;
	for (int i = 0; i < size_ar; i++)
	{
		map_name = _info.incompletemap(i).mapname();
		if (map_name == mapname)
		{
			return true;
		}
	}

	size_ar = _info.completemap_size();
	for (int i = 0; i < size_ar; i++)
	{
		map_name = _info.completemap(i).mapname();
		if (map_name == mapname)
		{
			return true;
		}
	}

	return false;
}


void CrashPlayer::sendPBMessage(google::protobuf::Message* p)
{
	if (_session)
	{
		_session->sendPBMessage(p);
	}
}

void CrashPlayer::DelMap(message::MsgDelMapReq* msg)
{
	message::MsgDelMapACK msgACK;
	int size_ar = _info.incompletemap_size();
	const char* mapname = msg->map_name().c_str();
	msgACK.set_map_name(mapname);
	std::string map_name;
	bool del = false;
	::google::protobuf::RepeatedPtrField< ::message::CrashMapData >* temp_list = _info.mutable_completemap();
	::google::protobuf::RepeatedPtrField< ::message::CrashMapData >::iterator it = temp_list->begin();
	message::MapType temp_type = message::CompleteMap;
	for (it = temp_list->begin(); it != temp_list->end(); ++ it)
	{
		if (it->mapname() == mapname)
		{
			temp_type = message::CompleteMap;
			temp_list->erase(it);
			del = true;
			break;
		}
	}

	if (del == false)
	{
		temp_list = _info.mutable_incompletemap();
		for (it = temp_list->begin(); it != temp_list->end(); ++it)
		{
			if (it->mapname() == mapname)
			{
				temp_type = message::ImcompleteMap;
				temp_list->erase(it);
				del = true;
				break;
			}
		}
	}
	msgACK.set_map_type(temp_type);
	if (del == false)
	{
		msgACK.set_error(message::ServerError_NotFoundMapNameWhenDel);
	}
	sendPBMessage(&msgACK);
}

void CrashPlayer::SaveMap(message::MsgSaveMapReq* msg)
{
	message::MsgSaveMapACK msgACK;
	msgACK.set_map_name(msg->map().mapname());
	msgACK.set_save_type(msg->save_type());
	msgACK.set_error(message::ServerError_NO);

	if (havemapname(msg->map().mapname().c_str()) == false)
	{
		message::CrashMapData* temp = NULL;
		switch (msg->save_type())
		{
		case  message::ImcompleteMap:
			temp = _info.add_incompletemap();
			break;
		case  message::CompleteMap:
			temp = _info.add_completemap();
			break;
		default:
			break;
		}
		if (temp != NULL)
		{
			temp->CopyFrom(msg->map());
			message::CrashMapData* temp_map = msgACK.mutable_map();
			temp_map->CopyFrom(msg->map());
		}
		else
		{
			msgACK.set_error(message::ServerError_Unknow);
		}
	}
	else
	{
		msgACK.set_error(message::ServerError_HaveSameName);
	}
	sendPBMessage(&msgACK);	
}

