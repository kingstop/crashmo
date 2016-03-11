#include "stdafx.h"
#include "GameConfig.h"


GameConfig::GameConfig()
{


}

GameConfig::~GameConfig()
{


}
void GameConfig::LoadGameConfig(DBQuery* p)
{

	gOfficilMapManager.init(p);
	
	p->reset();
	DBQuery& query = *p;
	DBQParms parms;
	query << "SELECT * FROM offical_map";
	query.parse();

	SDBResult result = query.store(parms);	
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		message::CrashMapData entry;
		entry.set_mapname(row["map_name"].c_str());
		entry.set_number(row["map_point"]);
		entry.set_section(row["map_section"]);
		message::CrashmoMapBaseData* data = entry.mutable_data();
		std::string sql_map_data = row["map_data"].c_str();
		data->ParseFromString(sql_map_data);
		std::pair<int, int> temp_key;
		temp_key.first = entry.section();
		temp_key.second = entry.number();
		_offcial_map[temp_key] = entry;
	}

}