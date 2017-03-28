#pragma once
struct CrashMapState
{
	message::CrashMapData* map;
	bool rank_map;
	bool new_map;
};
typedef std::map<u64, message::CrashMapData*> CRASHMAPS;
class CrashMapManager
{
public:
	CrashMapManager();
	virtual ~CrashMapManager();
public:
	const message::CrashMapData* CreateCrashMap(const message::CrashMapData* map);
	const message::CrashMapData* GetCrashMap(u64 map_index);
	bool DelCrashMap(u64 map_index);
protected:
	CRASHMAPS _maps;
};

