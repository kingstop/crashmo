#pragma once
typedef std::map<int, std::map<int, message::CrashMapData> > OFFICILMAPLIST;
typedef std::map<int, std::string> SECTIONSNAMES;
class OfficilMapManager : public EventableObject
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void init(DBQuery* p);
	void getOfficilMap(CrashPlayer* p, int page);
	const OFFICILMAPLIST* getOfficilMap();
	void saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p);
	void modifySectionName(int section, const char* name, CrashPlayer* player);
	const SECTIONSNAMES& getSectionNames();
protected:
	void saveOfficilMap();


protected:
	OFFICILMAPLIST _officilmap;
	SECTIONSNAMES _sections_names;

};

