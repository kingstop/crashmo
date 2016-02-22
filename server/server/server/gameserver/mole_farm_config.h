#pragma once

enum require_type
{
	require_type_null,
	require_type_resource_count,
	require_type_construction_level,
};

enum result_type
{
	result_type_null,
	result_type_add_resource,
	result_type_modify_construction_level,
};

struct action_require_data
{
	int type_;
	int argument_;
	int argument_1_;
	bool remove_;
};

struct action_result_data
{
	int type_;
	int result_argument_;
	int result_argument_1_;
};



typedef std::vector<action_require_data> ACTIONREQUIREDATAS;
typedef std::vector<action_result_data> ACTIONRESULTDATAS;
struct action_info
{
	int id_;
	int duration_time_;
	ACTIONREQUIREDATAS requires_;
	ACTIONRESULTDATAS results_;
};


struct construction_data
{
	int id_;
	int width_;
	int heigth_;
	std::vector<u32> require_flags_;
	//u32 require_flag_;
	u32 action_flag_;
	int max_level_;
	int begin_level_;
	int area_level_;
	u32 modify_flag_;
	std::string construction_name_;
	std::string construction_describe_;
	int can_recycle_;
};
typedef std::map<int, int> MAPINT;

struct construction_level_use_data
{
	int level_;
	int action_id_;

};
typedef std::map<int, construction_level_use_data*> MAP_LEVEL_USE_DATAS;
struct construction_level_up_data
{
	int config_id_;
	MAP_LEVEL_USE_DATAS levels_;
};

typedef std::map<int, MAPINT> MAPSEED;
struct construction_gain
{
	int config_id_;
	MAPSEED gains_;
};

struct level_config
{
	int exp_;
	int recycle_grid_;
};


class mole_farm_config
{
public:
	typedef std::map<int, action_info*> MAPACTIONINFOS;
	typedef std::map<int, construction_data*> MAPCONSTRUCTIONS;
	typedef std::map<int, construction_gain*> MAPGAINS;
	typedef std::map<int, construction_level_up_data*> MAP_CONSTRUCTION_LEVEL_UP_DATAS;
	typedef std::map<int, level_config> MAP_LEVEL_CONFIGS;
public:
	mole_farm_config(void);
	virtual ~mole_farm_config(void);
	void load_config(DBQuery* p);
	
	const construction_level_use_data* get_construction_level_up(int config_id, int level);
	const action_info* get_construction_action(int config_id, int level, int t);
	action_info* get_action_info(int id);
	const construction_data* get_construction_data(int config_id);
	int get_recycle_grid(int level);
	int get_exp(int level);

	int get_center_construction_config_id();
	int get_farm_house_config_id();
	int get_farm_labour_config_id();
	int get_max_farm_labour_config_id();
	int get_max_resource_config_id();
	int get_max_construction_level();
	int get_begin_recycle_grid();
	int get_exp_config_id();
	int get_max_level();
	int get_max_construction_count(int config_id, int level);
protected:
	void prepare_constructions_config();

protected:
	MAPACTIONINFOS _actions;
	MAPCONSTRUCTIONS _constructions;
	MAP_CONSTRUCTION_LEVEL_UP_DATAS _constructions_level_up_data;
	MAPGAINS _gains;
	MAPSEED _constructions_config;
	MAP_LEVEL_CONFIGS _level_configs;

	int _center_construction_config_id;
	int _farm_house_config_id;
	int _farm_labour_config_id;
	int _max_farm_labour_config_id;
	int _max_resource_config_id;
	int _max_construction_level;
	int _begin_recycle_grid;
	int _exp_config_id;
	int _max_level;
};

