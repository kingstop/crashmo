#include "stdafx.h"
#include "OfficilMapManager.h"
#include "CrashPlayer.h"


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
		std::map<int, message::CrashMapData>::iterator it_temp = it->second.begin();
		for (; it_temp != it->second.end(); ++ it_temp)
		{
			message::CrashMapData* temp = msg.add_maps();
			temp->CopyFrom(it_temp->second);
		}
	}
	p->sendPBMessage(&msg);
}

void OfficilMapManager::saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p)
{
	OFFICILMAPLIST::iterator it = _officilmap.find(map_data->section());
	if (it != _officilmap.end())
	{
		std::map<int, message::CrashMapData> temp_map;
		_officilmap.insert(OFFICILMAPLIST::value_type(map_data->section(), temp_map));
	}

	message::CrashMapData entry;
	entry.CopyFrom(*map_data);
	_officilmap[map_data->section()][map_data->number()] = entry;
	message::MsgSaveMapACK msgACK;
	msgACK.set_map_name(map_data->mapname().c_str());
	msgACK.set_save_type(message::OfficeMap);
	msgACK.set_error(message::ServerError_NO);
	message::CrashMapData* map_temp = msgACK.mutable_map();
	map_temp->CopyFrom(entry);
	p->sendPBMessage(&msgACK);
}