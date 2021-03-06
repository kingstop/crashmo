#pragma once
typedef std::map<int, std::map<int, u64> > OFFICILMAPLIST;
typedef std::map<int, std::string> CHAPTERSNAMES;

class OfficilMapManager : public EventableObject
{
public:
	OfficilMapManager();
	virtual ~OfficilMapManager();
public:
	void init(DBQuery* p);
	void LoadMaps(DBQuery* p);
	void getOfficilMap(CrashPlayer* p, int page);
	const OFFICILMAPLIST* getOfficilMap();
	const message::CrashMapData* getOfficilMap(int chapter_id, int section_id);
	void saveMapOfficilMap(const message::CrashMapData* map_data, CrashPlayer* p);
	void modifySectionName(int section, const char* name, CrashPlayer* player);
	const CHAPTERSNAMES& getSectionNames();
protected:
	void loadMap(DBQuery* p, const char* argu);
protected:
	void saveOfficilMap();
	u64 generateMapIndex();
protected:
	OFFICILMAPLIST _officilmap;
	CHAPTERSNAMES _chapter_names;
	std::list<u64> _ids;
	

};

