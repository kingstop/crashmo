#include "stdafx.h"
#include "CrashMapManager.h"


CrashMapManager::CrashMapManager()
{
}


CrashMapManager::~CrashMapManager()
{
}


message::CrashMapData* CrashMapManager::CreateCrashMap(const message::CrashMapData* map)
{
	u64 new_map_index = gGameConfig.GenerateMapIndex();
	message::CrashMapData* entry = new message::CrashMapData(*map);
	entry->mutable_data()->set_map_index(new_map_index);
	_maps[new_map_index] = entry;
	return entry;
	
}
message::CrashMapData* CrashMapManager::GetCrashMap(u64 map_index)
{
	message::CrashMapData* entry = NULL;
	CRASHMAPS::iterator it = _maps.find(map_index);
	if (it != _maps.end())
	{
		entry = it->second;
	}
	return entry;

}


bool CrashMapManager::DelCrashMap(u64 map_index)
{
	bool ret = false;
	message::CrashMapData* entry = NULL;
	CRASHMAPS::iterator it = _maps.find(map_index);
	if (it != _maps.end())
	{
		entry = it->second;
		delete entry;
		_maps.erase(it);
		ret = true;
	}
	return ret;
}

void CrashMapManager::AddCrashMap(message::CrashMapData& map)
{
	message::CrashMapData* entry = new message::CrashMapData(map);
	_maps[map.data().map_index()] = entry;
}