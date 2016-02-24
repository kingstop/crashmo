#pragma once
typedef std::map<int, std::map<int, message::CrashMapData>> OFFICILMAPLIST;
class OfficilMapManager
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void getOfficilMap(CrashPlayer* p, int page);
	void addOfficilMap(CrashPlayer* p, message::CrashMapData* msg);

protected:
	OFFICILMAPLIST _officilmap;

};

