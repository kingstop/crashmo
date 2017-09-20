#pragma once
struct CrashMapState
{
	message::CrashMapData* map;
	bool rank_map;
	bool new_map;
};
typedef std::map<u64, message::CrashMapData*> CRASHMAPS;
typedef std::map<u64, int> CRASHMAPCOUNT;
class CrashMapManager
{
public:
	CrashMapManager();
	virtual ~CrashMapManager();
public:
	message::CrashMapData* CreateCrashMap(const message::CrashMapData* map);
	message::CrashMapData* GetCrashMap(u64 map_index);
	void AddCrashMap(const message::CrashMapData& map);
	void DelCrashMap(u64 map_index);
protected:
	CRASHMAPS _maps;
	CRASHMAPCOUNT _map_counts;

};

