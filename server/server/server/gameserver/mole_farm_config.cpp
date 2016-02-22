#include "stdafx.h"
#include "mole_farm_config.h"
struct intpair
{
	int level;
	int count;
};


//注意：当字符串为空时，也会返回一个空字符串  
void split(std::string& s, std::string& delim,std::vector< std::string >* ret)  
{  
	size_t last = 0;  
	size_t index=s.find_first_of(delim,last);  
	while (index!=std::string::npos)  
	{  
		ret->push_back(s.substr(last,index-last));  
		last=index+1;  
		index=s.find_first_of(delim,last);  
	}  
	if (index-last>0)  
	{  
		ret->push_back(s.substr(last,index-last));  
	}  
}  

mole_farm_config::mole_farm_config(void)
{
}


mole_farm_config::~mole_farm_config(void)
{
}


action_info* mole_farm_config::get_action_info(int id)
{
	action_info* entry = NULL;
	MAPACTIONINFOS::iterator it = _actions.find(id);
	if (it != _actions.end())
	{
		entry = it->second;
	}
	return entry;
}


const construction_data* mole_farm_config::get_construction_data(int config_id)
{
	construction_data* entry = NULL;
	MAPCONSTRUCTIONS::iterator it = _constructions.find(config_id);
	if (it != _constructions.end())
	{
		entry = it->second;
	}
	return entry;
}

const action_info* mole_farm_config::get_construction_action(int config_id, int level, int t)
{
	action_info* entry = NULL;

	MAPGAINS::iterator it = _gains.find(config_id);
	if (it != _gains.end())
	{
		construction_gain* gain_entry = it->second;
		MAPSEED::iterator it_seed = gain_entry->gains_.find(t);
		if (it_seed != gain_entry->gains_.end())
		{
			MAPINT::iterator it_level = it_seed->second.find(level);
			if (it_level != it_seed->second.end())
			{
				int _action_id = it_level->second;
				entry = get_action_info(_action_id);
			}
		}
	}
	return entry;
}

const construction_level_use_data* mole_farm_config::get_construction_level_up(int config_id, int Level)
{
	construction_level_up_data* entry = NULL;
	MAP_CONSTRUCTION_LEVEL_UP_DATAS::iterator it = _constructions_level_up_data.find(config_id);
	if (it != _constructions_level_up_data.end())
	{
		entry = it->second;
	}

	construction_level_use_data* level_entry = NULL;
	if (entry)
	{
		MAP_LEVEL_USE_DATAS::iterator it_level = entry->levels_.find(Level);
		if (it_level != entry->levels_.end() )
		{
			level_entry = it_level->second;
		}
	}
	return level_entry;
}

int mole_farm_config::get_center_construction_config_id()
{
	return _center_construction_config_id;
}

int mole_farm_config::get_max_farm_labour_config_id()
{
	return _max_farm_labour_config_id;
}


int mole_farm_config::get_farm_house_config_id()
{
	return _farm_house_config_id;
}

int mole_farm_config::get_farm_labour_config_id()
{
	return _farm_labour_config_id;
}

int mole_farm_config::get_max_resource_config_id()
{
	return _max_resource_config_id;
}

int mole_farm_config::get_max_construction_level()
{
	return _max_construction_level;
}

int mole_farm_config::get_max_construction_count(int config_id, int level)
{
	int count_temp = 0;
	MAPSEED::iterator it = _constructions_config.find(config_id);
	if (it != _constructions_config.end())
	{
		MAPINT::iterator it_count = it->second.find(level);
		count_temp = it_count->second;
	}
	return count_temp;
}

int mole_farm_config::get_begin_recycle_grid()
{
	return _begin_recycle_grid;
}
int mole_farm_config::get_exp_config_id()
{
	return _exp_config_id;
}

int mole_farm_config::get_max_level()
{
	return _max_level;
}

void mole_farm_config::prepare_constructions_config()
{
	std::vector<intpair> vc_temp;
	MAPSEED::iterator it = _constructions_config.begin();
	for (; it != _constructions_config.end(); ++ it)
	{
		vc_temp.clear();
		intpair entry;
		entry.level = -1;
		MAPINT::iterator it_int = it->second.begin();
		for (; it_int!= it->second.end(); ++ it_int)
		{			
			entry.level = it_int->first;
			entry.count = it_int->second;
			vc_temp.push_back(entry);
		}
		int size_temp = vc_temp.size();
		for (int i = 0; i < size_temp; i ++)
		{
			int begin_number = vc_temp[i].level + 1;
			int end_number = 0;
			if (i == (size_temp -1))
			{
				end_number = get_max_construction_level();			
			}
			else
			{
				end_number = vc_temp[(i + 1)].level;
			}
			int count = vc_temp[i].count;
			for (int j = begin_number; j <= end_number; j ++)
			{
				 it->second.insert(MAPINT::value_type(j, count));
			}
		}
	}
}


void mole_farm_config::load_config(DBQuery* p)
{
	p->reset();
	DBQuery& query = *p;
	DBQParms parms;
	query << "SELECT * FROM construction_config";
	query.parse();

	SDBResult result = query.store(parms);
	std::vector<std::string> list_entry;
	std::string split_temp = ";";
	int list_size = 0;
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		construction_data* entry = new construction_data();
		DBRow row = result[i];
		entry->id_ = row["config_id"];
		entry->width_ = row["width"];
		entry->heigth_ = row["height"];
		entry->begin_level_ = row["begin_level"];
		entry->max_level_ = row["max_level"];
		entry->area_level_ = row["area_level"];
		entry->can_recycle_ = row["can_recycle"];
		entry->modify_flag_ = row["modify_tile_flag"];
		//entry->require_flag_ = 0;

		std::string build_required_tile_ = row["build_required_tile"].c_str();
		
		std::string split_temp = ";";
		
		list_entry.clear();
		if (build_required_tile_.empty() == false)
		{
			split(build_required_tile_, split_temp, &list_entry);
			list_size = list_entry.size();

			for (int i = 0; i < list_size; i ++)
			{
				u32 flag_temp = 1 << atoi(list_entry[i].c_str());
				entry->require_flags_.push_back(flag_temp);
				//entry->require_flag_ |= 1 << atoi(list_entry[i].c_str());
			}
			//build_required_tile_
		}
		//std::string action_ = row["action_"].c_str();
		//list_entry.clear();
		//entry->action_flag_ = 0;
		//if (action_.empty() == false)
		//{
		//	split(action_, split_temp, &list_entry);
		//	list_size = list_entry.size();

		//	for (int i = 0; i < list_size; i ++)
		//	{
		//		entry->action_flag_ |= 1 << atoi(list_entry[i].c_str());
		//	}
		//}

		_constructions.insert(MAPCONSTRUCTIONS::value_type(entry->id_, entry));
	}
	query.reset();
	query << "SELECT * FROM construction_level_up_config";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		//construction_data* entry = new construction_data();
		DBRow row = result[i];
		int config_id_entry = row["config_id"];
		construction_level_up_data* entry = NULL;
		MAP_CONSTRUCTION_LEVEL_UP_DATAS::iterator it  = _constructions_level_up_data.find(config_id_entry);
		if (it != _constructions_level_up_data.end())
		{
			entry = it->second;
		}
		else
		{
			entry = new construction_level_up_data();
			entry->config_id_ = config_id_entry;
			_constructions_level_up_data.insert(MAP_CONSTRUCTION_LEVEL_UP_DATAS::value_type(config_id_entry, entry));
		}

		int level = row["level"];
		MAP_LEVEL_USE_DATAS::iterator it_level = entry->levels_.find(level);
		if (it_level != entry->levels_.end())
		{

		}
		else
		{
			construction_level_use_data* entry_level = new construction_level_use_data();
			entry_level->level_ = level;
			entry_level->action_id_ = row["action_id"];
			/*
			std::string resource_temp = row["use_resource"];
			std::vector<std::string> temp_array;

			if (resource_temp.empty() == false)
			{
				split_temp = ";";
				split(resource_temp, split_temp, &list_entry);
				list_size = list_entry.size();
				std::string temp_entry;
				for (int j = 0; j < list_size; j ++)
				{
					temp_entry = list_entry[j];
					split_temp = ":";
					split(temp_entry, split_temp, &temp_array);
					int array_size = temp_array.size();
					if (array_size == 2)
					{
						int id_entry = atoi(temp_array[0].c_str());
						int number_entry = atoi(temp_array[1].c_str());
						entry_level->use_resouce_.insert(MAPINT::value_type(id_entry, number_entry));
					}					
				}				
			}
			*/
			entry->levels_.insert(MAP_LEVEL_USE_DATAS::value_type(level, entry_level));
		}		
	}


	query.reset();
	query << "SELECT * FROM action_config";
	query.parse();
	result.clear();
	result = query.store();
	char temp_entry[256];
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		int action_id_entry = row["action_id"];
		MAPACTIONINFOS::iterator it = _actions.find(action_id_entry);
		if (it == _actions.end())
		{
			action_info* entry_temp = new action_info();
			entry_temp->id_ = action_id_entry;
			for (int j = 1; j <= 5; j ++)
			{
				action_require_data entry_require_data;
				sprintf(temp_entry, "require_type_%d", j);
				entry_require_data.type_ = row[temp_entry];
				if (entry_require_data.type_ == 0)
				{
					continue;
				}
				sprintf(temp_entry, "require_argument_%d", j);
				entry_require_data.argument_ = row[temp_entry];
				sprintf(temp_entry, "require_remove_%d", j);
				entry_require_data.remove_ = row[temp_entry];
				sprintf(temp_entry, "require_argument_%d_1", j);
				entry_require_data.argument_1_ = row[temp_entry];
				entry_temp->requires_.push_back(entry_require_data);
			}
			entry_temp->duration_time_ = row["duration_time"];

			for (int j = 1; j <= 3; j ++)
			{
				action_result_data entry_result_data;
				sprintf(temp_entry, "result_type_%d", j);
				entry_result_data.type_ = row[temp_entry];
				if (entry_result_data.type_ == 0)
				{
					continue;
				}
				sprintf(temp_entry, "result_argument_%d", j);
				entry_result_data.result_argument_ = row[temp_entry];
				sprintf(temp_entry, "result_argument_%d_1", j);
				entry_result_data.result_argument_1_ = row[temp_entry];
				entry_temp->results_.push_back(entry_result_data);
			}
			_actions.insert(MAPACTIONINFOS::value_type(entry_temp->id_, entry_temp));
		}
	}

	query.reset();
	query << "SELECT * FROM construction_gain";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		int config_id = row["config_id"];
		MAPGAINS::iterator it = _gains.find(config_id);
		construction_gain* construction_gain_entry = NULL;
		if (it != _gains.end())
		{
			construction_gain_entry = it->second;
		}
		else
		{
			construction_gain_entry = new construction_gain();
			construction_gain_entry->config_id_ = config_id;
			_gains.insert(MAPGAINS::value_type(construction_gain_entry->config_id_, construction_gain_entry));
		}
		int seed_type_temp = row["seed_type"];
		MAPSEED::iterator it_map_seed = construction_gain_entry->gains_.find(seed_type_temp);
		int level_temp = row["level"];
		int action_id_temp = row["action_id"];
		if (it_map_seed != construction_gain_entry->gains_.end())
		{
			if (it_map_seed->second.find(level_temp) != it_map_seed->second.end())
			{
			}
			else
			{
				it_map_seed->second.insert(MAPINT::value_type(level_temp, action_id_temp));
			}			
		}
		else
		{
			MAPINT map_seed;
			map_seed.insert(MAPINT::value_type(level_temp, action_id_temp));
			construction_gain_entry->gains_.insert(MAPSEED::value_type(seed_type_temp, map_seed));
		}
	}

	query.reset();
	query << "SELECT * FROM game_value_config";
	query.parse();
	result.clear();
	result = query.store();
	if (result.num_rows() > 0)
	{
		DBRow row = result[0];
		_center_construction_config_id = row["center_construction_config_id"];
		_farm_house_config_id = row["farm_house_config_id"];
		_farm_labour_config_id = row["farm_labour_config_id"];
		_max_resource_config_id = row["max_resource_config_id"];
		_max_farm_labour_config_id = row["max_farm_labour_config_id"];
		_max_construction_level = row["max_construction_level"];
		_begin_recycle_grid = row["begin_recycle_grid"];
		_exp_config_id = row["exp_config_id"];
		_max_level = row["max_level"];
	}

	query.reset();
	query << "SELECT * FROM construction_count_config";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		DBRow row = result[i];
		int level = row["level"];
		int config_id = row["config_id"];
		int config_count = row["count"];

		MAPSEED::iterator it_config = _constructions_config.find(config_id);
		if (it_config == _constructions_config.end())
		{			
			MAPINT MAP_ENTRY;
			MAP_ENTRY.insert(MAPINT::value_type(level, config_count));
			_constructions_config.insert(MAPSEED::value_type(config_id, MAP_ENTRY));
		}
		else
		{
			it_config->second.insert(MAPINT::value_type(level, config_count));
		}
	}


	query.reset();
	query << "SELECT * FROM main_exp_config";
	query.parse();
	result.clear();
	result = query.store();
	for (u16 i = 0 ; i < result.num_rows(); ++i)
	{
		level_config entry;
		DBRow row = result[i];
		int level = row["level"];
		entry.exp_ = row["exp"];
		entry.recycle_grid_ = row["recycle_grid"];
		_level_configs[level] = entry;
	}
	//construction_count_config
}


int mole_farm_config::get_recycle_grid(int level)
{
	int ret = 0;
	MAP_LEVEL_CONFIGS::iterator it = _level_configs.find(level);
	if (it != _level_configs.end())
	{
		ret = it->second.recycle_grid_;
	}
	return ret;

}
int mole_farm_config::get_exp(int level)
{
	int ret = 0;
	if (level <= 1)
	{
		ret = get_begin_recycle_grid();
	}
	else
	{

		MAP_LEVEL_CONFIGS::iterator it = _level_configs.find(level);
		if (it != _level_configs.end())
		{
			ret = it->second.exp_;
		}
	}
	return ret;
}
