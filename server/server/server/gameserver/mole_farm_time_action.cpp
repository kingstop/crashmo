#include "stdafx.h"
/*
#include "mole_farm_time_action.h"
#include "Player_mole_farm.h"
#include "mole_farm_data.h"

pDoAction action_handler[action_type_max] = {
	&mole_farm_time_action::default_action,
	&mole_farm_time_action::construction_level_up,
	&mole_farm_time_action::do_action
};
mole_farm_time_action::mole_farm_time_action(Player_mole_farm* owner): _owner(owner)
{
	_action_type = action_type_null;

}


mole_farm_time_action::~mole_farm_time_action(void)
{
}



message::enum_farm_error_code  mole_farm_time_action::init(u64 construction_id, int seed_type)
{
	bool ret = false;
	_construction_id = construction_id;
	_action_type = action_type_do_action;
	message::enum_farm_error_code error_entry;
	error_entry = message::farm_error_no_error;
	mole_construction_data* construction_entry = _owner->get_mole_construction(construction_id);
	_resource_modify_type = message::resource_modify_type_plant;
	if (construction_entry != NULL)
	{
		const action_info* action_entry = gGSMoleFarmConfig.get_construction_action(construction_entry->config_id_, construction_entry->level_, seed_type);
		if (action_entry != NULL)
		{
			error_entry = init_action(action_entry->id_);
		}
		else
		{
			Mylog::log_server(LOG_ERROR,"mole farm <%s> construction config id<%d> level<%d> not found seed<%d> ",
				_owner->get_guid(), construction_entry->config_id_, construction_entry->level_, seed_type);
			error_entry = message::farm_error_not_found_seed;
		}
	}
	else
	{
		Mylog::log_server(LOG_ERROR,"mole farm <%s> construction <%lu> not found", _owner->get_guid(), construction_id);
	}

	if (error_entry == message::farm_error_no_error)
	{
		action_begin();
	}
	return error_entry;
	
	
}
message::enum_farm_error_code mole_farm_time_action::init(u64 construction_id)
{
	bool ret = false;
	_action_type = action_type_do_action;
	_construction_id = construction_id;
	message::enum_farm_error_code error_entry;
	error_entry = message::farm_error_no_error;
	_resource_modify_type = message::resource_modify_type_construction_level_up;
	mole_construction_data* construction_entry = _owner->get_mole_construction(_construction_id);
	if (construction_entry != NULL)
	{
		int level_target = construction_entry->level_ + 1;
		const construction_level_use_data* use_data = gGSMoleFarmConfig.get_construction_level_up(construction_entry->config_id_,  level_target);
		if (use_data != NULL)
		{
			error_entry = init_action(use_data->action_id_);
		}				
		else
		{
			Mylog::log_server(LOG_ERROR,"mole farm <%s> not found construction <%lu> level<%d> up config<%d>",
				_owner->get_guid(), construction_id, level_target, construction_entry->config_id_);
			error_entry = message::farm_error_not_found_target_config_when_level_up;
		}
	}
	else
	{
		Mylog::log_server(LOG_ERROR,"mole farm <%s> not found construction <%lu>",_owner->get_guid(), construction_id);
		error_entry = message::farm_error_not_found_construction_when_level_up;
	}

	if (error_entry == message::farm_error_no_error)
	{
		action_begin();
	}
	return error_entry;
}

message::enum_farm_error_code mole_farm_time_action::init_action(int action_id, bool new_action)
{
	message::enum_farm_error_code error_entry;
	error_entry = message::farm_error_no_error;
	_action_id = action_id;
	const action_info* action_entry = gGSMoleFarmConfig.get_action_info(_action_id);
	if (action_entry != NULL)
	{
		
		if (_owner->have_mole_action(_construction_id))
		{
			error_entry = message::farm_error_construction_already_have_action;
		}
		else
		{
			bool check_requires = false;
			if (new_action == false)
			{
				check_requires = true;
			}
			else
			{
				check_requires = _owner->check_action_require(_construction_id, action_entry->requires_, _resource_modify_type);
			}
			if (check_requires)
			{
				
				if (new_action == true)
				{
					_action_begin_time = gGSServerTime;
				}
				if (_owner->add_construction_action(_construction_id, this) == true)
				{
					//_action_begin_time = gGSServerTime;
					u64 execute_time = _action_begin_time + _duration_time;
					int duration_timme = 0;
					if (gGSServerTime < execute_time)
					{
						duration_timme = 10000;
					}
					else
					{
						_duration_time = gGSServerTime - execute_time;
						if (_duration_time < 10000)
						{
							duration_timme = 10000;
						}
					}
					if (new_action)
					{
						duration_timme = action_entry->duration_time_ * 1000;
					}
					
					gEventMgr.addEvent(this,&mole_farm_time_action::execute, EVENT_CONSTRUCTION_ACTION_, duration_timme, 1, EVENT_FLAG_DELETES_OBJECT);
				}
				else
				{
					error_entry = message::farm_error_construction_already_have_action;
				}
				
				error_entry = message::farm_error_no_error;
			}	
			else
			{
				error_entry = message::farm_error_resource_not_enough;
			}
		}
	}
	else
	{
		Mylog::log_server(LOG_ERROR,"mole farm <%s> not found action <%d>",_owner->get_guid(), _action_id);
		error_entry = message::farm_error_not_found_action;
	}
	return error_entry;
}


message::enum_farm_error_code  mole_farm_time_action::init(const message::ActionInfo* ac)
{
	_action_type = (action_type)ac->action_type();
	_duration_time = ac->duration_time();
	_action_begin_time = ac->start_time();
	_resource_modify_type =(message::resource_modify_type) ac->argument();
	return init_action(ac->action_id(), false);
}

void mole_farm_time_action::execute()
{
	(*this.*action_handler[ _action_type ])( );
	action_complete();
}

void mole_farm_time_action::action_complete()
{
	_owner->construction_action_complete(this);
}

void mole_farm_time_action::action_begin()
{
	_owner->construction_action_begin(_construction_id, _action_id);

}


void mole_farm_time_action::construction_level_up()
{
	_owner->construction_level_up(_construction_id);
}

void mole_farm_time_action::do_action()
{
	const action_info* entry_action = gGSMoleFarmConfig.get_action_info(_action_id);
	_owner->action_results(_construction_id, entry_action->results_);
}

void mole_farm_time_action::default_action()
{

}

void mole_farm_time_action::createActionInfo(message::ActionInfo* entry_action)
{
	u64 duration_time = _duration_time;
	entry_action->set_action_type(_action_type);
	entry_action->set_start_time(_action_begin_time);
	entry_action->set_duration_time(duration_time);
	entry_action->set_argument(_resource_modify_type);
	entry_action->set_action_id(_action_id);
	entry_action->set_construction_id(_construction_id);
}
*/
