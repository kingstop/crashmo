#pragma once
typedef std::map<int, std::map<int, message::CrashMapData> > OFFICILMAPLIST;
typedef std::map<int, std::string> CHAPTERSNAMES;
class OfficilMapManager : public EventableObject
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void init(DBQuery* p);
	void getOfficilMap(CrashPlayer* p, int page);
	const OFFICILMAPLIST* getOfficilMap();
	const message::CrashMapData* getOfficilMap(int chapter_id, int section_id);
	void saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p);
	void modifySectionName(int section, const char* name, CrashPlayer* player);
	const CHAPTERSNAMES& getSectionNames();
protected:
	void saveOfficilMap();


protected:
	OFFICILMAPLIST _officilmap;
	CHAPTERSNAMES _chapter_names;

};

