#pragma once
typedef std::map<int, std::map<int, message::CrashMapData>> OFFICILMAPLIST;
class OfficilMapManager
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void getOfficilMap(CrashPlayer* p, int page);
	void saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p);


protected:
	OFFICILMAPLIST _officilmap;

};

