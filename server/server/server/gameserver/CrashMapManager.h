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
	message::CrashMapData* CreateCrashMap(const message::CrashMapData* map);
	message::CrashMapData* GetCrashMap(u64 map_index);
	void AddCrashMap(message::CrashMapData& map);
	bool DelCrashMap(u64 map_index);
protected:
	CRASHMAPS _maps;
};

