#pragma once
typedef std::map<int, std::map<int, message::CrashMapData>> OFFICILMAPLIST;
class OfficilMapManager
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void getOfficilMap(CrashPlayer* p, int page);


protected:
	OFFICILMAPLIST _officilmap;

};

