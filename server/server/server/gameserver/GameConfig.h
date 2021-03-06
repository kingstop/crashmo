#pragma once
#include <map>

struct MapConfig
{
	int config_width_;
	int config_heigth_;
	int config_count_;
	int config_width_max_;
	int config_heigth_max_;
	int config_count_max_;
	int gold_;
	std::map<int, int> group_config_count_;
	std::map<int, int> group_config_max_count_;
	int day_refrash_time_;
	int publish_map_cd;
};

class GameConfig
{
public:
	typedef std::map<std::pair<int, int>, message::CrashMapData> OFFICIAL_MAP;
public:
	GameConfig();
	~GameConfig();
public:
	void LoadGameConfig(DBQuery* p);
	const MapConfig* getMapConfig();
	bool isInToday(u32 time);
	int GetServerOpenPassedTime(u32 time);
	void SetServerOpenTime(u64 time);
	void SetMaxMapIndex(u64 index);
	u64 GetMaxMapIndex();
	u64 GenerateMapIndex();
protected:
	OFFICIAL_MAP _offcial_map;
	MapConfig _map_config;
	u64 _server_open_time;
	u64 _current_max_map_index;
};