#pragma once
typedef std::map<int, std::map<int, message::CrashMapData>> OFFICILMAPLIST;
typedef std::map<int, std::string> SECTIONSNAMES;
class OfficilMapManager
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void getOfficilMap(CrashPlayer* p, int page);
	void saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p);
	void modifySectionName(int section, const char* name, CrashPlayer* player);
	const SECTIONSNAMES& getSectionNames();


protected:
	OFFICILMAPLIST _officilmap;
	SECTIONSNAMES _sections_names;

};

