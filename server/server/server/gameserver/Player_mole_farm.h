#pragma once
#define MAPSIZE 150
#define MAXTILE 2
typedef std::map<int, int> MAPINT;

/*

struct mole_farm_data;
class mole_farm_time_action;
class mole_construction_data;

//typedef std::map<u64, mole_construction*>  MAP_MOLE_CONSTRUCTIONS;
typedef std::map<u64, mole_farm_time_action*> MAP_MOLE_FARM_TIME_ACTIONS;
typedef std::map<u64, MAPINT> MAP_CONSTRUCTIONS_RESOURCE;
typedef std::vector<int> VC_INTS;
struct gs_farm_data;

struct construction_build_area
{
	int begin_x_;
	int begin_y_;
	int max_x_;
	int max_y_;
	const construction_data* config_;
	//int level_;
	//int area_level_;
	bool b_;
	//std::vector<u32> require_flags_;
};
class Player_mole_farm : PUBLIC_BASE_OBJECT(Player_mole_farm) ,public EventableObject
{
	REGISTER_POOL_INFO(Player_mole_farm, 100, 0)
public:
	Player_mole_farm(Session* p, const message::MoleFarmInfoFull * msg);
	virtual ~Player_mole_farm(void);
public:
	void build_construction_req(message::construction_build_req* msg);
	void construction_level_req(message::construction_level_up_req* msg);
	void farm_construction_action_req(message::construction_action_req* msg);
	void construction_remove(message::construction_remove_req* msg);
public:
	void setReconnet(Session* pksession);
	mole_construction_data* get_mole_construction(u64 construction_id);
	bool have_resource(const MAPINT& temp);
	bool modify_resource(u64 construction_id, const MAPINT& temp,  message::resource_modify_type type);
	bool ready_gain_resource(u64 construction_id, const MAPINT& temp);
	bool construction_level_up(u64 construction_id);
	bool check_action_require(u64 construction_id, const ACTIONREQUIREDATAS& requirs, message::resource_modify_type type);
	bool action_results(u64 construction_id, const ACTIONRESULTDATAS& results);
	void sendPBMessage(google::protobuf::Message* p);
	bool build_construction_check(int config_id, int pos_x, int pos_y, bool set_flags);
	bool check_pos_valid(int pos);
	u64 generate_construction_id();
	bool have_mole_action(u64 constuction_id);
	bool add_construction_action(u64 construction_id, mole_farm_time_action* time_action);
	void gain_resource(u64 construction_id);
	void construction_move(u64 construction_id, int pos_x, int pos_y);


public:
	void construction_action_complete(mole_farm_time_action* act);
	void construction_action_begin(u64  construction_id, int action_id);
	
	const char* get_guid();

	message::MoleFarmInfoFull* create_mole_farm_info(bool to_save = false);
	message::MsgGS2DBSaveMoleFarmReq * create_mole_farm_to_db();
	void save_farm();

	int get_max_farm_labour();
	int get_max_resource();
	int get_center_counstruction_level();

protected:
	void send_construction_error(u64 construction_id, message::enum_farm_error_code error);
	void send_construction_resource_update(u64 construction_id, const MAPINT& resources);
	construction_build_area get_construction_build_area(int config_id, int pos_x, int pos_y);
	bool remove_construction(u64 construction_id);
	void increase_exp(int exp);
protected:
	mole_farm_data* _farm_data;
	Session* _Session;
	std::map<u64, mole_construction_data*> _recycle_constructions;
	/*
	MAP_MOLE_CONSTRUCTIONS _constructions;
	
	MAP_CONSTRUCTIONS_RESOURCE _constructions_resource;
	VC_INTS _unlock_areas;
	MAPINT _resource;

	MAP_MOLE_FARM_TIME_ACTIONS _constructions_actions;
	u32 _mole_farm_config[MAPSIZE][MAPSIZE];
	bool _mole_build[MAXTILE][MAPSIZE][MAPSIZE];
	u64 _max_construction_id;
	std::string _guid;
	u64 _account_id;

};
*/

