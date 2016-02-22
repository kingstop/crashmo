
#pragma once 
/* 
struct action_info;
struct construction_level_up_data;
class Player_mole_farm;
enum action_type
{
	action_type_null,
	action_type_construction_level_up,
	action_type_do_action,
	action_type_max
};


class mole_farm_time_action : public EventableObject
{
public:
	mole_farm_time_action(Player_mole_farm* owner);
	virtual ~mole_farm_time_action(void);
public:
	message::enum_farm_error_code  init(u64 construction_id, int seed_type);
	message::enum_farm_error_code init(u64 construction_id);
	message::enum_farm_error_code  init(const message::ActionInfo* ac);
public:
	void construction_level_up();
	void do_action();
	void default_action();
	void execute();

	void createActionInfo(message::ActionInfo* entry_action);
	u64 get_construction_id(){return _construction_id;}
	int get_action_id(){return _action_id;}
	action_type get_action_type(){return _action_type;}
	u32 get_start_time(){return _action_begin_time;}
	message::resource_modify_type get_resource_modify_type(){return _resource_modify_type;}

protected:
	message::enum_farm_error_code init_action(int action_id, bool new_action = true);
	void action_complete();
	void action_begin();
protected:
	Player_mole_farm* _owner;
	u64 _construction_id;
	int _action_id;
	u32 _duration_time;
	u32 _action_begin_time;
	message::resource_modify_type _resource_modify_type;
	action_type _action_type;
};

typedef void(mole_farm_time_action::*pDoAction)( );
*/
