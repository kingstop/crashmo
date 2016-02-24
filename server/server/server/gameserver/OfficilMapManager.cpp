#include "stdafx.h"
#include "CrashPlayer.h"
#include "OfficilMapManager.h"


OfficilMapManager::OfficilMapManager()
{
}


OfficilMapManager::~OfficilMapManager()
{
}


void OfficilMapManager::getOfficilMap(CrashPlayer* p, int page)
{
	message::MsgOfficilMapACK msg;
	OFFICILMAPLIST::iterator it = _officilmap.find(page);
	if (it != _officilmap.end())
	{
		std::map<int, message::CrashMapData>::iterator it_page = it->second.begin();
		for (; it_page != it->second.end(); ++ it_page)
		{
			message::CrashMapData* map_temp = msg.add_maps();
			map_temp->CopyFrom(it_page->second);
		}
	}
	p->sendPBMessage(&msg);
}

void OfficilMapManager::addOfficilMap(CrashPlayer* p, message::CrashMapData* msg)
{
	OFFICILMAPLIST::iterator it = _officilmap.find(msg->section());
	if (it == _officilmap.end())
	{
		std::map<int, message::CrashMapData> temp;
		_officilmap[msg->section()] = temp;		
	}
	message::CrashMapData temp_entry;
	temp_entry.CopyFrom(*msg);
	_officilmap[msg->section()][msg->number()] = temp_entry;
	message::MsgAddOfficilMapACK MsgACK;
	MsgACK.mutable_map()->CopyFrom(*msg);
	p->sendPBMessage(&MsgACK);
	
}