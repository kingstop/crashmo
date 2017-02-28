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

	//gOfficilMapManager.init(p);
	
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
		entry.set_chapter(row["chapter"]);
		entry.set_section(row["section"]);
		entry.set_gold(row["gold"]);
		entry.set_creatername(row["creater_name"].c_str());
		message::CrashmoMapBaseData* data = entry.mutable_data();
		std::string sql_map_data = row["map_data"].c_str();
		data->ParseFromString(sql_map_data);
		std::pair<int, int> temp_key;
		temp_key.first = entry.chapter();
		temp_key.second = entry.section();
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
		_map_config.day_refrash_time_ =  row["day_refrash_config_time"];
		
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


bool GameConfig::isInToday(u32 time)
{
	//std::string time_str;
	//std::string time_cur;
	//build_unix_time_to_string(time, time_str);
	//build_unix_time_to_string(g_server_time, time_cur);
	//Mylog::log_server(LOG_INFO, "server time[%s]  time1[%s]!", time_str.c_str(), time_cur.c_str());
	int day_offset_time = _map_config.day_refrash_time_ * 60 * 60;
	time_t _t1 = (time_t)g_server_time;
	tm* p1 = localtime(&_t1);
	p1->tm_min = 0;
	p1->tm_sec = 0;
	p1->tm_hour = _map_config.day_refrash_time_;
	time_t today_refresh_time = mktime(p1); //utf Ê±¼ä²î
	time_t Tomorrow_refresh_time = today_refresh_time + (24 * 60 * 60);
	bool ret = false;
	if (time < Tomorrow_refresh_time && time >= today_refresh_time)
	{
		ret = true;
	}
	return ret;
}
