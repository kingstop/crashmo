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
	_map_counts[new_map_index] = 1;
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


void CrashMapManager::DelCrashMap(u64 map_index)
{
	CRASHMAPCOUNT::iterator it_count = _map_counts.find(map_index);
	int count = 0;
	if (it_count != _map_counts.end())
	{
		count = it_count->second;
	}

	count--;
	if (count <= 0)
	{
		_map_counts.erase(map_index);
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

	}
	
}

void CrashMapManager::AddCrashMap(const message::CrashMapData& map)
{
	message::CrashMapData* entry = new message::CrashMapData(map);
	u64 map_index = map.data().map_index();
	_maps[map_index] = entry;
	CRASHMAPCOUNT::iterator it = _map_counts.find(map_index);
	int count = 0;
	if (it != _map_counts.end())
	{
		count = it->second;
	}
	count++;
	_map_counts[map_index] = count;
	
}