#include "stdafx.h"
/*
#include "Player_mole_farm.h"
#include "mole_farm_data.h"
#include "session.h"
#include "message/farm.pb.h"
#include "mole_farm_time_action.h"

#define _SAVE_PLAYER_FARM_TIME_  (10 * 10 * _TIME_SECOND_MSEL_)

Player_mole_farm::Player_mole_farm(Session* p, const message::MoleFarmInfoFull * msg):_Session(p)
{
	_max_construction_id = 0;
	
	_farm_data = new mole_farm_data(msg,0);
	_guid = _farm_data->guid_;
	for (int i = 0; i < MAXTILE; i ++)
	{
		for (int j = 0; j < MAPSIZE; j ++)
		{
			memset(_mole_build[i][j],true , sizeof(_mole_build[i][j]));
		}
	}
	_account_id = msg->extra_info().account();
	
	const message::MoleFarmInfo base_info = msg->base_info();
	int action_size = base_info.actions().size();
	for (int i = 0; i < action_size; i ++)
	{
		const message::ActionInfo action_entry = base_info.actions(i);
		mole_farm_time_action* time_action = new mole_farm_time_action(this);
		message::enum_farm_error_code error_entry = time_action->init(&action_entry);
		if (error_entry != message::farm_error_no_error)
		{
			Mylog::log_player(LOG_ERROR, "create farm [%s] error construction [%lu] add action failed", get_guid(), action_entry.construction_id());
		}

	}

	//int size_construction = _farm_data->constructions_.size();
	std::map<u64, mole_construction_data*>::iterator it_constructions = _farm_data->constructions_.begin();
	for (; it_constructions != _farm_data->constructions_.end();  ++ it_constructions)
	{
		mole_construction_data* entry_data = it_constructions->second;
		u64 id = it_constructions->first;
		if (entry_data->recycle_)
		{
			_recycle_constructions[id] = entry_data;
		}
		else
		{
			if (build_construction_check(entry_data->config_id_, entry_data->pos_x_, entry_data->pos_y_, true))
			{

			}
			else
			{
				Mylog::log_player(LOG_ERROR, "create farm [%s] error construction config[%d] can not build at pos_x[%d] pos_y[%d]",
					get_guid(), entry_data->config_id_, entry_data->pos_x_, entry_data->pos_y_);
			}
		}
	}

	/*
	int resource_size = base_info.resource_size();
	for (int i = 0; i < resource_size; i ++)
	{
		const message::int32pair entry_pair32 = base_info.resource(i);
		_farm_data->resource_[entry_pair32.number_1()] = entry_pair32.number_2();
	}
	_guid = base_info.guid();
	int construction_size = base_info.construstions().size();
	for (int i = 0; i < construction_size; i ++)
	{
		const message::MoleConstrustion construction_entry = base_info.construstions(i);
		const construction_data* config_data_entry = gGSMoleFarmConfig.get_construction_data(construction_entry.config_id());
		if (config_data_entry)
		{
			if (build_construction_check(construction_entry.config_id(), construction_entry.pos_x(), construction_entry.pos_y(), true))
			{
				mole_construction* mole_entry = new mole_construction();
				mole_entry->id_ = construction_entry.sid();
				mole_entry->config_id_ = construction_entry.config_id();
				mole_entry->level_ = construction_entry.level();
				mole_entry->pos_x_ = construction_entry.pos_x();
				mole_entry->pos_y_ = construction_entry.pos_y();
				if (_max_construction_id < mole_entry->id_)
				{
					_max_construction_id = mole_entry->id_;
				}
				_farm_data->constructions_[mole_entry->id_] = mole_entry;//insert(std::map<u64, mole_construction_data*>::value_type(mole_entry->id_, mole_entry));
			}
			else
			{
				Mylog::log_player(LOG_ERROR, "create farm [%s] error construction config[%d] can not build at pos_x[%d] pos_y[%d]",
					get_guid(), construction_entry.config_id(), construction_entry.pos_x(), construction_entry.pos_y());
			}
		}
		else
		{
			Mylog::log_player(LOG_ERROR, "create farm [%s] error construction config[%d] not found", get_guid(), construction_entry.config_id());
		}		
	}
	/ */

	/*/
	int unlock_areas_size = base_info.unlock_areas().size();
	for (int i = 0; i < unlock_areas_size; i ++)
	{
		 int unlock = base_info.unlock_areas(i);
		 _unlock_areas.push_back(unlock);
	}

	int counstruction_resource_size = base_info.construction_resource().size();
	for (int i = 0; i < counstruction_resource_size; i ++)
	{
		const message::ConstructionResource entry = base_info.construction_resource(i);
		u64 construction_id = entry.construction_id();
		int r_size = entry.resource().size();
		MAPINT temp_map;
		for (int j = 0; j < r_size; j ++)
		{
			temp_map[entry.resource(j).number_1()] = entry.resource(j).number_2();
		}

		_constructions_resource[construction_id] = temp_map;
	}

	*/
/*
	gEventMgr.addEvent(this, &Player_mole_farm::save_farm, EVENT_SAVE_FARM_DATA, _SAVE_PLAYER_FARM_TIME_, 0, 0);

	//base_info.construction_resource()
}


Player_mole_farm::~Player_mole_farm(void)
{
	if (_farm_data)
	{
		delete _farm_data;
		_farm_data = NULL;
	}
}



int Player_mole_farm::get_max_farm_labour()
{
	int max_number = 0;
	if (_farm_data)
	{
		std::map<int, int>::iterator it = _farm_data->resource_.find(gGSMoleFarmConfig.get_max_farm_labour_config_id());
		if (it != _farm_data->resource_.end())
		{
			max_number = it->second;
		}
	}
	return max_number;
}

int Player_mole_farm::get_center_counstruction_level()
{
	int level = 0;
	std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(gGSMoleFarmConfig.get_center_construction_config_id());
	if (it != _farm_data->constructions_.end())
	{
		level = it->second->level_;
	}
	return level;
}

int Player_mole_farm::get_max_resource()
{
	int max_number = 0;
	if (_farm_data)
	{
		std::map<int, int>::iterator it = _farm_data->resource_.find(gGSMoleFarmConfig.get_max_resource_config_id());
		if (it != _farm_data->resource_.end())
		{
			max_number = it->second;
		}
	}
	return max_number;
}

void Player_mole_farm::send_construction_error(u64 construction_id, message::enum_farm_error_code error)
{
	message::construction_error msg;
	msg.set_error_code(error);
	msg.set_construction_id(construction_id);
	sendPBMessage(&msg);
}

void Player_mole_farm::send_construction_resource_update(u64 construction_id, const MAPINT& resources)
{

	message::construction_resource_update msg;
	msg.set_construction_id(construction_id);
	MAPINT::const_iterator it_int_pair = resources.begin();
	for (; it_int_pair != resources.end(); ++ it_int_pair)
	{
		message::int32pair* pair_entry = msg.add_gain_resource();
		pair_entry->set_number_1(it_int_pair->first);
		pair_entry->set_number_2(it_int_pair->second);
	}
	sendPBMessage(&msg);
}

void Player_mole_farm::save_farm()
{
	message::MsgGS2DBSaveMoleFarmReq* msg_to_db = create_mole_farm_to_db();
	gGSDBClient.sendPBMessage(msg_to_db);
	delete msg_to_db;	
}


void Player_mole_farm::setReconnet(Session* pksession)
{
	if (_Session != NULL)
	{
		if (pksession != _Session)
		{
			Mylog::log_player(LOG_ERROR, "player [%s], old session[%u], new [%u] quit game", _farm_data->guid_.c_str(), _Session->getTranId(), _Session->getTranId());
			assert(false);
		}
	}

	_Session = pksession;
}

const char* Player_mole_farm::get_guid()
{
	return _guid.c_str();
}


message::MsgGS2DBSaveMoleFarmReq * Player_mole_farm::create_mole_farm_to_db()
{
	message::MsgGS2DBSaveMoleFarmReq * mole_farm_entry = new message::MsgGS2DBSaveMoleFarmReq();
	mole_farm_entry->set_allocated_mole_farm_info(create_mole_farm_info(true));
	return mole_farm_entry;
}


message::MoleFarmInfoFull* Player_mole_farm::create_mole_farm_info(bool to_save)
{
	_farm_data->clear_action();
	MAP_MOLE_FARM_TIME_ACTIONS::iterator it_actions = _constructions_actions.begin();
	for (; it_actions != _constructions_actions.end(); ++ it_actions)
	{
		mole_farm_time_action* action_entry = it_actions->second;
		mole_construction_action_data* action_data = new mole_construction_action_data();

		std::map<u64, mole_construction_action_data*>::iterator it_farm_data = _farm_data->constructions_actions_.begin();
		if (it_farm_data != _farm_data->constructions_actions_.end())
		{
			delete it_farm_data->second;
			_farm_data->constructions_actions_.erase(it_farm_data);
		}
		action_data->construction_id_ = action_entry->get_construction_id();
		action_data->action_id_ = action_entry->get_action_id();
		action_data->action_type_ = action_entry->get_action_type();
		action_data->argument_ = action_entry->get_resource_modify_type();
		action_data->start_time_ = action_entry->get_start_time();
		_farm_data->constructions_actions_[action_data->construction_id_] = action_data;	
	}
	return _farm_data->create_message_info();
	/*
	message::MoleFarmInfo* mole_farm_entry = new message::MoleFarmInfo();
	mole_farm_entry->set_guid(_guid.c_str());
	std::map<u64, mole_construction_data*>::iterator it_construction =  _farm_data->constructions_.begin();
	for (; it_construction != _farm_data->constructions_.end(); ++ it_construction)
	{
		mole_construction* construction = it_construction->second;
		message::MoleConstrustion* msg_mole_construction = mole_farm_entry->add_construstions();
		msg_mole_construction->set_sid(it_construction->first);
		msg_mole_construction->set_config_id(construction->config_id_);
		msg_mole_construction->set_level(construction->level_);
		msg_mole_construction->set_pos_x(construction->pos_x_);
		msg_mole_construction->set_pos_y(construction->pos_y_);
		msg_mole_construction->set_recycle(construction->recycle_);	
	
	}

	MAP_MOLE_FARM_TIME_ACTIONS::iterator it_construction_action = _constructions_actions.begin();
	for (; it_construction_action != _constructions_actions.end(); ++ it_construction_action)
	{
		u64 construction_id = it_construction_action->first;
		mole_farm_time_action* action_entry = it_construction_action->second;
		message::ActionInfo* msg_action = mole_farm_entry->add_actions();
		action_entry->createActionInfo(msg_action);	
	}

	MAP_CONSTRUCTIONS_RESOURCE::iterator it_resource = _constructions_resource.begin();
	for (; it_resource != _constructions_resource.end(); ++ it_resource)
	{
		u64 construction_id = it_resource->first;
		message::ConstructionResource* construction_resource_entry = mole_farm_entry->add_construction_resource();
		construction_resource_entry->set_construction_id(construction_id);
		MAPINT::iterator it_map_int = it_resource->second.begin();
		for (; it_map_int != it_resource->second.end(); ++ it_map_int)
		{
			int resource_id = it_map_int->first;
			int resource_count = it_map_int->second;
			message::int32pair* msg_int32pair = construction_resource_entry->add_resource();
			msg_int32pair->set_number_1(resource_id);
			msg_int32pair->set_number_2(resource_count);
		}
		
	}

	MAPINT::iterator it_farm_resource =  _farm_data->resource_.begin();
	for (; it_farm_resource != _farm_data->resource_.end(); ++ it_farm_resource)
	{
		int resource_id = it_farm_resource->first;
		int resource_count = it_farm_resource->second;
		message::int32pair* msg_int32pair = mole_farm_entry->add_resource();
		msg_int32pair->set_number_1(resource_id);
		msg_int32pair->set_number_2(resource_count);
	}

	VC_INTS::iterator it_unlock_area = _unlock_areas.begin();
	for (; it_unlock_area != _unlock_areas.end(); ++ it_unlock_area)
	{
		int unlock_area = *it_unlock_area;
		mole_farm_entry->add_unlock_areas(unlock_area);
	}

	return mole_farm_entry;
	*/
    /*
}

mole_construction_data* Player_mole_farm::get_mole_construction(u64 construction_id)
{
	mole_construction_data* entry = NULL;
	std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(construction_id);
	if (it != _farm_data->constructions_.end())
	{
		entry = it->second;
	}
	return entry;
}


bool Player_mole_farm::have_resource(const MAPINT& temp)
{
	MAPINT::const_iterator it = temp.begin();
	for (; it != temp.end(); ++ it)
	{
		int config_number = (int)it->first;
		MAPINT::iterator it_find = _farm_data->resource_.find(config_number);
		if (it_find != _farm_data->resource_.end())
		{
			int number_value = it_find->second + it->second;
			int max_config_value = 0;
			if (config_number == gGSMoleFarmConfig.get_farm_labour_config_id())
			{
				max_config_value =get_max_farm_labour();
			}
			else
			{
				max_config_value = get_max_resource();
			}

			if (number_value < 0 || number_value > max_config_value)
			{
				return false;
			}

		}
		else
		{
			return false;
		}
	}
	return true;
}

u64 Player_mole_farm::generate_construction_id()
{
	_max_construction_id = _max_construction_id + 1;
	return _max_construction_id;
}


bool Player_mole_farm::ready_gain_resource(u64 construction_id, const MAPINT& temp)
{
	MAP_CONSTRUCTIONS_RESOURCE::iterator it = _farm_data->constructions_resource_.find(construction_id);
	if (it != _farm_data->constructions_resource_.end())
	{
		_farm_data->constructions_resource_.erase(it);
	}

	MAPINT gains = temp;
	send_construction_resource_update(construction_id, temp);

	_farm_data->constructions_resource_.insert(MAP_CONSTRUCTIONS_RESOURCE::value_type(construction_id, gains));
	
	return true;
}

bool Player_mole_farm::modify_resource(u64 construction_id, const MAPINT& temp, message::resource_modify_type type)
{
	if (have_resource(temp) == false)
	{
		return false;
	}

	message::resource_modify msg;
	msg.set_construction_id(construction_id);
	msg.set_modify_type(type);
	MAPINT modify;
	MAPINT::const_iterator it = temp.begin();
	for (; it != temp.end(); ++ it)
	{
		MAPINT::iterator it_find = _farm_data->resource_.find((int)it->first);
		if (it_find != _farm_data->resource_.end())
		{
			if (it_find->second > it->second)
			{				
				it_find->second -= it->second;
				modify[it_find->first] = it_find->second;
			}
		}
	}

	MAPINT::iterator it_temp = modify.begin();
	for (; it_temp != modify.end(); ++ it_temp)
	{
		message::int32pair* pair_entry = msg.add_modify_resource();
		pair_entry->set_number_1(it_temp->first);
		pair_entry->set_number_2(it_temp->second);
	}
	sendPBMessage(&msg);

	return true;

}

bool Player_mole_farm::construction_level_up(u64 construction_id)
{
	bool b = false;
	int level = 0;
	std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(construction_id);
	if (it != _farm_data->constructions_.end())
	{		
		mole_construction_data* mole_construction_entry = it->second;

		mole_construction_entry->level_ += 1;
		//mole_construction_entry->set_modify();
		b = true;
	}

	if (b)
	{
		message::construction_level_up msg;
		msg.set_construction_id(construction_id);
		msg.set_construction_level(it->second->level_);
		sendPBMessage(&msg);
	}
	else
	{
		send_construction_error(construction_id, message::farm_error_not_found_construction);
		Mylog::log_server(LOG_ERROR,"Player_mole_farm<%s> construction level up not found construction<%lu>",get_guid(), construction_id);
	}

	return b;
}

bool Player_mole_farm::action_results(u64 construction_id, const ACTIONRESULTDATAS& results)
{

	ACTIONRESULTDATAS::const_iterator it = results.begin();
	MAPINT resource_temp;
	for (; it != results.end(); ++ it)
	{
		action_result_data entry = *it;
		switch(entry.type_)
		{
		case result_type_add_resource:
			{

				if (entry.result_argument_ == gGSMoleFarmConfig.get_exp_config_id())
				{
					increase_exp(entry.result_argument_1_);
				}
				else
				{
					MAPINT::iterator it_temp = resource_temp.find(entry.result_argument_);		
					if (it_temp == resource_temp.end())
					{
						resource_temp[entry.result_argument_] = 0;
					}
					resource_temp[entry.result_argument_] += entry.result_argument_1_;		
				}

			}
			break;
		case  result_type_modify_construction_level:
			{
				construction_level_up(construction_id);
			}
			break;
		}
	}

	if (resource_temp.size() != 0)
	{
		ready_gain_resource(construction_id, resource_temp);
	}
	
	return true;
}

void Player_mole_farm::sendPBMessage(google::protobuf::Message* p)
{
	if (_Session)
	{
		_Session->sendPBMessage(p);
	}
}

bool Player_mole_farm::check_pos_valid(int pos)
{
	if (pos <0 || pos >= MAPSIZE)
	{
		return false;
	}
	return true;
}

construction_build_area Player_mole_farm::get_construction_build_area(int config_id, int pos_x, int pos_y)
{
	construction_build_area ret;
	ret.b_ = false;
	const construction_data* entry = gGSMoleFarmConfig.get_construction_data(config_id);
	if (entry != NULL)
	{
		if (entry->area_level_ < 0 || entry->area_level_ >= 2)
		{
			
		}
		else
		{
			int max_width = pos_x + entry->width_;
			int temp_y = pos_y - entry->heigth_;
			int max_height = pos_y;
			pos_y = temp_y;
			
			if (check_pos_valid(pos_x) && check_pos_valid(pos_y) && check_pos_valid(max_width) && check_pos_valid(max_height))
			{
				ret.b_ = true;
				ret.begin_x_ = pos_x;
				ret.max_x_ = max_width;
				ret.begin_y_ = pos_y;
				ret.max_y_ = max_height;
				ret.config_ = entry;
				//ret.require_flags_ = entry->require_flags_;
			}
			
		}
	}
	return ret;

}
bool Player_mole_farm::build_construction_check(int config_id, int pos_x, int pos_y, bool set_flags)
{
	construction_build_area build_entry = get_construction_build_area(config_id, pos_x, pos_y);
	if (build_entry.b_ == true)
	{
		bool check_can_build = true;
		for (int i = pos_x; i < build_entry.max_x_; i ++ )
		{
			for (int j = pos_y; j < build_entry.max_y_; j ++)
			{
				int area_level_ = build_entry.config_->area_level_;
				if (_mole_build[area_level_][i][j] == false)
				{
					check_can_build = false;
					break;
				}
				int index_x = i - pos_x;
				int index_y = j - pos_y;

				bool grid_check = false;
				std::vector<u32>::const_iterator it = build_entry.config_->require_flags_.begin();
				for (; it != build_entry.config_->require_flags_.end(); ++ it)
				{
					u32 flag_area = 0;
					if (area_level_ == 1)
					{
						flag_area = gGSMoleFarmMapStorage.get_area_flag(i, j);

					}
					else
					{
						flag_area = _mole_farm_config[i][j];
						if (flag_area == 0)
						{
							flag_area = gGSMoleFarmMapStorage.get_area_flag(i, j);
						}

					}

					u32 require_entry_flag = (*it);
					if (require_entry_flag | flag_area)
					{
						grid_check = true;
						break;
					}
				}			
				if (grid_check == false)
				{
					check_can_build = false;
					break;
				}

			}
			if (check_can_build == false)
			{
				break;
			}
		}
	}
	const construction_data* entry = gGSMoleFarmConfig.get_construction_data(config_id);
	if (entry != NULL)
	{
		if (entry->area_level_ < 0 || entry->area_level_ >= 2)
		{
			return false;
		}
		int max_width = pos_x + entry->width_;
		int temp_y = pos_y - entry->heigth_;
		int max_height = pos_y;
		pos_y = temp_y;
		bool check_can_build = true;
		if (check_pos_valid(pos_x) && check_pos_valid(pos_y) && check_pos_valid(max_width) && check_pos_valid(max_height))
		{
			
			for (int i = pos_x; i < max_width; i ++ )
			{
				for (int j = pos_y; j < max_height; j ++)
				{
					if (_mole_build[entry->area_level_][i][j] == false)
					{
						check_can_build = false;
						break;
					}
					int index_x = i - pos_x;
					int index_y = j - pos_y;
					
					bool grid_check = false;
					std::vector<u32>::const_iterator it = entry->require_flags_.begin();
					for (; it != entry->require_flags_.end(); ++ it)
					{
						u32 flag_area = 0;
						if (entry->area_level_ == 1)
						{
							flag_area = gGSMoleFarmMapStorage.get_area_flag(i, j);

						}
						else
						{
							flag_area = _mole_farm_config[i][j];
							if (flag_area == 0)
							{
								flag_area = gGSMoleFarmMapStorage.get_area_flag(i, j);
							}

						}

						u32 require_entry_flag = (*it);
						if (require_entry_flag | flag_area)
						{
							grid_check = true;
							break;
						}
					}			
					if (grid_check == false)
					{
						check_can_build = false;
						break;
					}
					
				}
				if (check_can_build == false)
				{
					break;
				}
			}

		}
		else
		{
			check_can_build = false;
		}
		if (set_flags && check_can_build)
		{
			for (int i = pos_x; i < max_width; i ++)
			{
				for (int j = pos_y; j <max_height; j ++)
				{
					_mole_build[entry->area_level_][i][j] = false;
				}
			}
		}
		return check_can_build;
	}
	return false;
}

bool Player_mole_farm::check_action_require(u64 construction_id, const ACTIONREQUIREDATAS& requirs, message::resource_modify_type type)
{
	MAPINT consume_resource_temp;
	ACTIONREQUIREDATAS::const_iterator it = requirs.begin();
	for (; it != requirs.end(); ++ it)
	{
		action_require_data entry = *it;
		switch(entry.type_)
		{
		case require_type_resource_count:
			{
				MAPINT::iterator it = _farm_data->resource_.find(entry.argument_);
				if (it != _farm_data->resource_.end())
				{
					if (it->second > entry.argument_1_)
					{
						MAPINT::iterator it_temp = consume_resource_temp.find(it->first);
						if (it_temp == consume_resource_temp.end())
						{
							consume_resource_temp[it->first] = 0;
						}
						consume_resource_temp[it->first] += entry.argument_1_;
					}
					else
					{
						return false;
					}					
				}
				else
				{
					return false;
				}
			}
			break;
		}
	}

	if (consume_resource_temp.size() != 0)
	{
		if (modify_resource(construction_id, consume_resource_temp, type) == false)
		{
			return false;
		}
	}
	return true;
}


void Player_mole_farm::construction_action_complete(mole_farm_time_action* act)
{
	MAP_MOLE_FARM_TIME_ACTIONS::iterator it = _constructions_actions.begin();
	for (; it != _constructions_actions.end(); ++ it)
	{
		if (it->second == act)
		{
			_constructions_actions.erase(it);
			delete act;
			break;
		}
	}
}

void Player_mole_farm::construction_action_begin(u64  construction_id, int action_id)
{
	message::construction_action_start msg;
	msg.set_construction_id(construction_id);
	msg.set_action_id(action_id);
	sendPBMessage(&msg);
}

bool Player_mole_farm::have_mole_action(u64 constuction_id)
{
	MAP_MOLE_FARM_TIME_ACTIONS::iterator it = _constructions_actions.find(constuction_id);
	if (it != _constructions_actions.end())
	{
		return true;
	}
	return false;
}

void Player_mole_farm::build_construction_req(message::construction_build_req* msg)
{

	int config_id = msg->config_id();
	message::enum_farm_error_code entry_error;
	const construction_data* entry_config = gGSMoleFarmConfig.get_construction_data(config_id);
	if (entry_config)
	{
		int pos_x = msg->pos_x();
		int pos_y = msg->pos_y();
		if (build_construction_check(config_id, pos_x, pos_y, false))
		{
			u64 id = generate_construction_id();
			mole_construction_data* entry = new mole_construction_data();
			entry->id_ = id;
			entry->level_ = entry_config->begin_level_;
			entry->pos_x_ = pos_x;
			entry->pos_y_ = pos_y;
			entry->config_id_ = config_id;
			//entry->set_create();
			
			_farm_data->constructions_.insert(std::map<u64, mole_construction_data*>::value_type(id, entry));
			if (have_mole_action(id) == false)
			{
				mole_farm_time_action* mole_farm_action = new mole_farm_time_action(this);
				entry_error = mole_farm_action->init(id);
				if (entry_error == message::farm_error_no_error)
				{
					build_construction_check(config_id, pos_x, pos_y, true);
					message::construction_build_ACK msg;
					msg.set_level(entry->level_);
					msg.set_construction_id(entry->id_);
					msg.set_pos_x(entry->pos_x_);
					msg.set_pos_y(entry->pos_y_);
					msg.set_config_id(entry->config_id_);
					sendPBMessage(&msg);
				}
				else
				{
					std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(id);
					if (it != _farm_data->constructions_.end())
					{
						mole_construction_data* p = it->second;
						delete p;
						_farm_data->constructions_.erase(it);
					}
					delete mole_farm_action;
				}
			}
			else
			{
				entry_error = message::farm_error_construction_already_have_action;
			}
		}
		else
		{
			entry_error = message::farm_error_can_not_build_construction;
		}
	}
	else
	{
		entry_error = message::farm_error_not_found_construction_config;
	}

	if (entry_error != message::farm_error_no_error)
	{
		message::construction_build_error msg_error;
		msg_error.set_config_id(config_id);
		msg_error.set_pos_x(msg->pos_x());
		msg_error.set_pos_y(msg->pos_y());
		msg_error.set_error_code(entry_error);
		sendPBMessage(&msg_error);
	}
}

void Player_mole_farm::construction_level_req(message::construction_level_up_req* msg)
{
	u64 construction_id = msg->construction_id();
	message::enum_farm_error_code entry_error;
	if (have_mole_action(construction_id) == false)
	{
		mole_farm_time_action* mole_farm_action = new mole_farm_time_action(this);
		message::enum_farm_error_code entry_error;
		entry_error = mole_farm_action->init(construction_id);
		if (entry_error != message::farm_error_no_error)
		{	
			delete mole_farm_action;
		}	
	}
	else
	{
		entry_error = message::farm_error_construction_already_have_action;
	}

	if (entry_error == message::farm_error_no_error)
	{
		message::construction_level_begin msg;
		msg.set_construction_id(construction_id);
		sendPBMessage(&msg);
	}
	else
	{
		send_construction_error(construction_id, entry_error);		
	}
}

void Player_mole_farm::farm_construction_action_req(message::construction_action_req* msg)
{
	u64 construction_id = msg->construction_id();
	int seed_type_temp =  msg->seed_type();
	message::enum_farm_error_code entry_error;
	if (have_mole_action(construction_id) == false)
	{
		mole_farm_time_action* mole_farm_action = new mole_farm_time_action(this);
		entry_error = mole_farm_action->init(construction_id, seed_type_temp);
		if (entry_error != message::farm_error_no_error)
		{
			delete mole_farm_action;
		}
	}

	if (entry_error == message::farm_error_no_error)
	{
		message::construction_action_ACK msg;
		msg.set_construction_id(construction_id);
		msg.set_seed_type(seed_type_temp);
		sendPBMessage(&msg);
	}
	else
	{
		send_construction_error(construction_id, entry_error);
	}
}


bool Player_mole_farm::add_construction_action(u64 construction_id, mole_farm_time_action* time_action)
{
	bool b_ret = false;
	if (have_mole_action(construction_id) == false)
	{
		b_ret = true;
		_constructions_actions.insert(MAP_MOLE_FARM_TIME_ACTIONS::value_type(construction_id, time_action));
	}
	return b_ret;
}

void Player_mole_farm::construction_remove(message::construction_remove_req* msg)
{
	u64 construction_id = msg->construction_id();
	message::enum_farm_error_code error = message::farm_error_no_error;
	int max_recycle = gGSMoleFarmConfig.get_recycle_grid(_farm_data->level_);
	if (_recycle_constructions.size() >= max_recycle)
	{
		error = message::farm_error_recycle_constructions_count_reach_max;
	}
	else
	{
		std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(msg->construction_id());
		if (it != _farm_data->constructions_.end())
		{
			mole_construction_data* entry_data = it->second;
			const construction_data* entry_config = gGSMoleFarmConfig.get_construction_data(entry_data->config_id_);
			if (entry_config->can_recycle_ == 0)
			{
				if (entry_data->recycle_ == true)
				{
					error = message::farm_error_construction_already_been_recycled;
				}
				else
				{
					entry_data->recycle_ = true;
					_recycle_constructions[entry_data->id_] = entry_data;
				}
			}
			else
			{
				error = message::farm_error_construction_can_not_been_recycled;
			}

		}
		else
		{
			error = message::farm_error_not_found_construction_when_recycle;
		}
	}

	if (error == message::farm_error_no_error)
	{
		message::construction_remove_ACK send_msg;
		send_msg.set_construction_id(construction_id);
		sendPBMessage(&send_msg);
	}
	else
	{
		send_construction_error(construction_id, error);
	}
}


void Player_mole_farm::construction_move(u64 construction_id, int pos_x, int pos_y)
{
	message::enum_farm_error_code error = message::farm_error_no_error;
	std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(construction_id);
	if (it != _farm_data->constructions_.end())
	{
		mole_construction_data* mole_construction = it->second;
		int current_pos_x = mole_construction->pos_x_;
		int current_pos_y = mole_construction->pos_y_;
		if (pos_x == current_pos_x && pos_y == current_pos_y)
		{
			error = message::farm_error_can_not_move_to_same_pos;
		}
		else
		{
			if (remove_construction(construction_id))
			{
				message::construction_move_ACK msg_entry;
				msg_entry.set_construction_id(construction_id);
				if (build_construction_check(mole_construction->config_id_, pos_x, pos_y, true))
				{
					msg_entry.set_pos_x(pos_x);
					msg_entry.set_pos_y(pos_y);
					sendPBMessage(&msg_entry);
				}
				else
				{
					if (build_construction_check(mole_construction->config_id_, current_pos_x, current_pos_y, true))
					{
						msg_entry.set_pos_x(current_pos_x);
						msg_entry.set_pos_y(current_pos_y);
						sendPBMessage(&msg_entry);
					}
					else
					{
						error = message::farm_error_unknow;
					}
				}
			}
			else
			{
				error = message::farm_error_can_not_remove_construction;
			}
		}
	}
	else
	{
		error = message::farm_error_not_found_construction;
	}
	if (error != message::farm_error_no_error)
	{
		send_construction_error(construction_id, error);
	}

}


void Player_mole_farm::increase_exp(int exp)
{
	int current_exp = _farm_data->exp_;
	int current_level = _farm_data->level_;
	int max_level = gGSMoleFarmConfig.get_max_level();
	if (current_level >= max_level)
	{

	}
	else
	{

		bool need_modify = false;
		int add_exp = exp;
		while(add_exp)
		{
			int target_level = _farm_data->level_ + 1;
			if (target_level > max_level)
			{
				break;
			}
			int need_exp =  gGSMoleFarmConfig.get_exp(target_level);
			current_exp = _farm_data->exp_ + add_exp;
			if (current_exp > need_exp)
			{
				add_exp = 0;
				_farm_data->exp_ = current_exp;
			}
			else
			{
				add_exp = current_exp - need_exp;
				_farm_data->exp_ = 0;
				_farm_data->level_ = target_level;
				need_modify = true;
			}
		}

		if (need_modify)
		{
			message::farm_level_modify_msg msg;
			msg.set_level(_farm_data->level_);
			msg.set_exp(_farm_data->exp_);
			sendPBMessage(&msg);
		}
	}
	
}

bool Player_mole_farm::remove_construction(u64 construction_id)
{
	bool can_remove = true;
	std::map<u64, mole_construction_data*>::iterator it = _farm_data->constructions_.find(construction_id);
	if (it != _farm_data->constructions_.end())
	{
		mole_construction_data* mole_construction = it->second;
		construction_build_area entry_build = get_construction_build_area(mole_construction->config_id_, mole_construction->pos_x_, mole_construction->pos_y_);
		if (entry_build.b_ == true)
		{
			int area_level_ = entry_build.config_->area_level_;
			for (int x = entry_build.begin_x_; x < entry_build.max_x_; x ++)
			{
				for (int y = entry_build.begin_y_; y < entry_build.max_y_; y ++)
				{
					if (_mole_build[area_level_][x][y] != true)
					{
						can_remove = false;
						break;
					}
				}
				if (can_remove == false)
				{
					break;
				}
			}

			if (can_remove == true)
			{
				for (int x = entry_build.begin_x_; x < entry_build.max_x_; x ++)
				{
					for (int y = entry_build.begin_y_; y < entry_build.max_y_; y ++)
					{
						_mole_build[area_level_][x][y] = false;
					
						if (area_level_ == 1)
						{
							_mole_farm_config[x][y] = 0;
						}
					}					
				}

			}
		}
		else
		{
			can_remove = false;
		}

	}
	else
	{
		can_remove = false;
	}
	return can_remove;
}

void Player_mole_farm::gain_resource(u64 construction_id)
{
	MAP_CONSTRUCTIONS_RESOURCE::iterator it = _farm_data->constructions_resource_.find(construction_id);
	if (it != _farm_data->constructions_resource_.end())
	{
		MAPINT& resources = it->second;
		MAPINT::iterator it_resource = resources.begin();
		MAPINT temp_resource;
		for (; it_resource != resources.end();  ++ it_resource)
		{
			int config_id = it_resource->first;
			int config_value = it_resource->second;
			MAPINT::iterator it_self = _farm_data->resource_.find(config_id);
			if (it_self == _farm_data->resource_.end())
			{
				_farm_data->resource_[config_id] = 0;
			}

			if (_farm_data->resource_[config_id] >= get_max_resource())
			{
				break;
			}
			else
			{
				int add_value_temp = 0;
				if (_farm_data->resource_[config_id] + config_value > get_max_resource())
				{
					add_value_temp = get_max_resource() - _farm_data->resource_[config_id];
					if (add_value_temp < 0)
					{
						add_value_temp = 0;
					}
				}
				if (add_value_temp != 0)
				{
					temp_resource[config_id] = add_value_temp;
					config_id -= add_value_temp;
					if (config_id == 0)
					{
						resources.erase(it_resource);
					}
					
				}
			}

			modify_resource(construction_id, temp_resource, message::resource_modify_type_gain);
			send_construction_resource_update(construction_id, resources);
			if (resources.size() == 0)
			{
				_farm_data->constructions_resource_.erase(it);
			}
		}
	}
	else
	{
		send_construction_error(construction_id, message::farm_error_not_have_construction_resource_to_gain);
	}

}
*/
