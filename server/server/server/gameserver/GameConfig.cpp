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

	query.reset();
	result.clear();
	query << "SELECT * FROM global_config";
	query.parse();
	result = query.store();
	if (result.num_rows() != 0)
	{
		DBRow row = result[0];
		_map_config.config_width_ = row["map_config_width"];
		_map_config.config_heigth_ = row["map_config_height"];
		_map_config.config_count_ = row["map_config_count"];
		_map_config.config_width_max_ =  row["map_config_max_width"];
		_map_config.config_heigth_max_ =  row["map_config_max_height"];
		_map_config.config_count_max_ =  row["map_config_max_count"];
		_map_config.gold_ = row["gold"];
	}

	query.reset();
	result.clear();
	query << "SELECT * FROM group_config";
	query.parse();
	result = query.store(parms);
	for (u16 i = 0; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		int config_id = row["group_config_id"];
		int config_count = row["group_config_count"];
		int config_count_max = row["group_config_max_count"];
		_map_config.group_config_count_[config_id] = config_count;
		_map_config.group_config_max_count_[config_id] = config_count_max;
	}
}

const MapConfig* GameConfig::getMapConfig()
{
	return &_map_config;
}