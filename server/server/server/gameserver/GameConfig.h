#pragma once
#include <map>


class GameConfig
{
public:
	typedef std::map<std::pair<int, int>, message::CrashMapData> OFFICIAL_MAP;
public:
	GameConfig();
	~GameConfig();
public:
	void LoadGameConfig(DBQuery* p);


protected:
	OFFICIAL_MAP _offcial_map;

};