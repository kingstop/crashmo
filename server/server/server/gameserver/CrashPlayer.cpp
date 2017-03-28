#include "stdafx.h"
#include "CrashPlayer.h"
#include "session.h"
#include "CrashPlayerManager.h"
#define _SAVE_PLAYER_TIME_  (10 * _TIME_SECOND_MSEL_)
#define _DELETE_PLAYER_TIME_ (100 * _TIME_SECOND_MSEL_)

CrashPlayer::CrashPlayer(Session* session):_session(session)
{
	_ping_count = 0;
}
CrashPlayer::CrashPlayer(Session* session, account_type acc) : _session(session)
{
	_info.set_name("name");

	_info.set_isadmin(true);
	_info.set_account(acc);
	_ping_count = 0;
}

CrashPlayer::~CrashPlayer(void)
{
}

void CrashPlayer::LoadConfig()
{
	const MapConfig* config = gGameConfig.getMapConfig();
	google::protobuf::RepeatedPtrField< ::message::intPair >* resources = _info.mutable_resources();
	resources->Clear();
	std::map<int, int>::const_iterator const_it = config->group_config_count_.begin();
	for (; const_it != config->group_config_count_.end(); ++ const_it)
	{
		message::intPair* pair_entry = resources->Add();
		pair_entry->set_number_1(const_it->first);
		pair_entry->set_number_2(const_it->second);

	}
	_info.set_gold(config->gold_);
	_info.set_map_count(config->config_count_);
	_info.set_map_width(config->config_width_);
	_info.set_map_height(config->config_heigth_);
}

void CrashPlayer::OnLogin()
{
	DayUpdate();
}

void CrashPlayer::reqMsgAddBlogMap(const message::MsgC2SReqAddMapBolg* msg)
{
	int chapter_id = msg->chapter_id();
	int section_id = msg->section_id();
}

void CrashPlayer::PassOfficilMap(int chapter_id, int section_id, int use_step, int use_time)
{
	message::ServerError error = message::ServerError_PassOfficilMapFailedTheMapIsLock;
	bool first_pass = false;
	int length = _info.passed_record_size();
	message::MsgS2CPassOfficilMapACK msg;
	int add_gold = 0;
	int add_jewel = 0;
	
	std::vector<u32> vc_task_ids;
	const message::CrashMapData* MapData = gOfficilMapManager.getOfficilMap(chapter_id, section_id);
	if (MapData != NULL)
	{
		for (size_t i = 0; i < length; i++)
		{
			message::intPair* pair_entry = _info.mutable_passed_record(i);

			if (pair_entry->number_1() == chapter_id)
			{
				error = message::ServerError_NO;
				int temp = pair_entry->number_2() + 1;
				if (section_id > temp)
				{
					error = message::ServerError_PassOfficilMapFailedTheMapIsLock;
				}
				else if (temp == section_id)
				{
					first_pass = true;
					pair_entry->set_number_2(temp);
				}
				else
				{

				}
				break;
			}
		}

		
		if (error == message::ServerError_NO)
		{
			if (first_pass)
			{
				add_gold = MapData->gold();
			}

			google::protobuf::RepeatedPtrField< ::message::TaskInfo >* map_task = _info.mutable_current_task();
			google::protobuf::RepeatedPtrField< ::message::TaskInfo >::iterator it_task = map_task->begin();
			for (; it_task != map_task->end(); ++ it_task)
			{
				message::TaskInfo& entry = (*it_task);
				int task_id = entry.task_id();
				const message::TaskInfoConfig*  task_config = gTaskManager.GetTaskConfig(task_id);
				bool ret = true;
				if (task_config != NULL)
				{
					int length = task_config->conditions_size();
					for (size_t i = 0; i < length; i++)
					{
						const message::TaskConditionTypeConfig& config =  task_config->conditions(i);
						switch (config.condition())
						{
						case message::ConditionType_PassOfficilGame:
						{
							if (config.argu_1() == chapter_id && config.argu_2() == section_id)
							{

							}
							else
							{
								ret = false;
								break;
							}
							
						}
						break;
						case message::ConditionType_LimitedTime:
						{
							if (use_time > config.argu_1())
							{
								ret = false;
								break;
							}
						}
						break;
						case message::ConditionType_LimitedStep:
						{
							if (use_step > config.argu_1())
							{
								ret = false;
								break;
							}
						}
						break;
						case message::ConditionType_PassUserGame:
						{
							ret = false;
							break;
						}
						break;
						default:
							break;
						}

					}
				}
				vc_task_ids.push_back(task_id);
				
				
			}
		}
	}
	else
	{
		error = message::ServerError_PassOfficilMapFailedTheMapNotFound;
	}

	if (error == message::ServerError_NO)
	{
		length = vc_task_ids.size();
		for (size_t i = 0; i < length; i++)
		{
			int task_id = vc_task_ids[i];
			int complete_count = _info.complete_task_count();
			complete_count += 1;
			_info.set_complete_task_count(complete_count);
			gTaskManager.GetTaskConfig(task_id);
			const message::TaskInfoConfig*  task_config = gTaskManager.GetTaskConfig(task_id);
			if (task_config != NULL)
			{
				message::MsgTaskReward* reward = msg.add_complete_task();
				reward->set_task_id(task_id);
				int reward_length = task_config->rewards_size();
				reward->set_task_name(task_config->name().c_str());
				for (size_t j = 0; j < reward_length; j++)
				{
					const message::TaskRewardConfig reward_config = task_config->rewards(j);										
					reward->add_rewards()->CopyFrom(reward_config);
					if (reward_config.resource_type() >= message::ResourceType_0 && reward_config.resource_type() <= message::ResourceType_7)
					{
						bool found = false;
						int resource_type = reward_config.resource_type() - message::ResourceType_0;
						int resource_length = _info.resources_size();
						for (size_t b = 0; b < resource_length; b++)
						{
							message::intPair* pair_entry = _info.mutable_resources(b);
							if (pair_entry->number_1() == resource_type)
							{
								int temp_number = pair_entry->number_2() + reward_config.count();
								pair_entry->set_number_2(temp_number);
								found = true;
								break;
							}
						}
						if (found == false)
						{
							message::intPair* pair_entry = _info.add_resources();
							pair_entry->set_number_1(resource_type);
							pair_entry->set_number_2(reward_config.count());
						}
					}
					else 
					{
						if (reward_config.resource_type() == message::ResourceType_gold)
						{
							int temp_gold = _info.gold() + reward_config.count();
							_info.set_gold(temp_gold);
						}
						else if (reward_config.resource_type() == message::ResourceType_jewel)
						{
							int temp_jewel = _info.jewel() + reward_config.count();
							_info.set_jewel(temp_jewel);
						}
						else
						{

						}
					}
				}
			}

			
		}
	}

	msg.set_chapter_id(chapter_id);
	msg.set_section_id(section_id);
	msg.set_error(error);
	msg.set_add_gold(add_gold);
	msg.set_current_gold(_info.gold());
	length = _info.resources_size();
	for (size_t i = 0; i < length; i++)
	{
		msg.add_add_resource()->CopyFrom(_info.resources(i));
	}
	sendPBMessage(&msg);
}

void CrashPlayer::SetInfo(message::CrashPlayerInfo info)
{
	_info = info;
}


message::CrashPlayerInfo CrashPlayer::GetInfo()
{
	return _info;
}

void CrashPlayer::SetSession(Session* session)
{
	_session = session;
}

void CrashPlayer::Destroy()
{
	SaveCrashInfo();
	_session->setPlayer(NULL);
	gPlayerManager.DestroyPlayer(this);
	
}

account_type CrashPlayer::getAccount()
{
	return _info.account();
}

void CrashPlayer::StartDeleteClock()
{
	if (gEventMgr.hasEvent(this, EVENT_DELETE_PLAYER_) == false)
	{
		gEventMgr.addEvent(this, &CrashPlayer::Destroy, EVENT_DELETE_PLAYER_, _DELETE_PLAYER_TIME_, 1, 0);
	}
	else
	{
		gEventMgr.modifyEventTime(this, EVENT_DELETE_PLAYER_, _DELETE_PLAYER_TIME_);
	}
}

void CrashPlayer::StopDeleteClock()
{
	if (gEventMgr.hasEvent(this, EVENT_DELETE_PLAYER_) == true)
	{
		gEventMgr.removeEvents(this, EVENT_DELETE_PLAYER_);
	}
}



Session* CrashPlayer::GetSession()
{
	return _session;
}

void CrashPlayer::SaveCrashInfo()
{

	std::string last_accept_task_time;
	u32 entry_time = _info.last_accept_task_time();
	build_unix_time_to_string(entry_time, last_accept_task_time);

	//message::ReqSaveCharacterData msg;

	char temp_sz[5012];
	message::ReqSaveCharacterDBSql msg;
	
	account_type acc_temp = _info.account();
	char sql[40960];
	//DBQParms parms;
	google::protobuf::RepeatedPtrField< ::message::intPair >* resources = _info.mutable_resources();
	google::protobuf::RepeatedPtrField< ::message::intPair >::const_iterator it = resources->begin();
	std::string resource_str;
	char sz_resource[256];
	std::string officil_game_record;
	int length = _info.passed_record_size();
	char sz_record[256];
	for (size_t i = 0; i < length; i++)
	{
		if (i != 0)
		{
			officil_game_record += ";";
		}
		const message::intPair entry = _info.passed_record(i);
		sprintf(sz_record, "%d,%d", entry.number_1(), entry.number_2());
		officil_game_record += sz_record;
	}

	char sz_task[256];
	std::string current_task;

	length = _info.current_task_size();
	for (size_t i = 0; i < length; i++)
	{
		if (i != 0)
		{
			current_task += ";";
		}
		const message::TaskInfo entry = _info.current_task(i);

		sprintf(sz_task, "%d,%d,%d,%d", entry.task_id(), entry.argu_1(), entry.argu_2(), entry.argu_3());
		current_task += sz_task;
	}
	for (; it != resources->end(); it++)
	{
		if (resource_str.empty() == false)
		{
			resource_str += ";";
		}
		sprintf(sz_resource, "%d,%d", it->number_1(), it->number_2());
		resource_str += sz_resource;
	}
	std::string complete_map_index;
	length = _info.completemap_size();
	char sz_index[256];
	for (size_t i = 0; i < length; i++)
	{
		if (i != 0)
		{
			complete_map_index += ",";
		}
		u64 map_index = _info.completemap(i);
		sprintf(sz_index, "llu", map_index);
		complete_map_index += sz_index;
	}
	std::string incomplete_map_index;
	length = _info.incompletemap_size();
	for (size_t i = 0; i < length; i++)
	{
		if (i != 0)
		{
			incomplete_map_index += ",";
		}
		u64 map_index = _info.incompletemap(i);
		sprintf(sz_index, "llu", map_index);
		incomplete_map_index += sz_index;
	}

	sprintf(sql, "replace into `character`(`account`, `pass_chapter`, `pass_section`,`name`, `isadmin`, `map_width`, `map_height`, `map_count`, `group_count`, `gold`,`jewel`, `task`, `complete_task_count`,`officil_game_record`, `incomplete_map_index`, `complete_map_index`, `last_accept_task_time`) values\
	 (%llu, %d, %d, '%s', %d, %d, %d, %d, '%s', %d, %d, '%s', %d, '%s', '%s')",
		acc_temp, 0, 0, _info.name().c_str(), (int)_info.isadmin(),
		_info.map_width(), _info.map_height(), _info.map_count(), resource_str.c_str(), _info.gold(),
		_info.jewel(), current_task.c_str(), _info.complete_task_count(), officil_game_record.c_str(), complete_map_index.c_str(), incomplete_map_index.c_str(), last_accept_task_time.c_str());
	msg.set_sql(sql);
	sendPBMessage(&msg);
	
	//gGSDBClient.sendPBMessage(&msg, _session->getTranId());
}


void CrashPlayer::SendPing()
{
	_ping_count++;
	message::MsgS2CNotifyPing msg;
	msg.set_time_stamp(g_server_time);
	msg.set_ping_count(_ping_count);
	sendPBMessage(&msg);
}

void CrashPlayer::StartSave()
{
	StopDeleteClock();
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == false)
	{
		gEventMgr.addEvent(this,&CrashPlayer::SaveCrashInfo, EVENT_SAVE_PLAYER_DATA_, _SAVE_PLAYER_TIME_, -1, 0);
	}
}

void CrashPlayer::StopSave()
{
	if (gEventMgr.hasEvent(this, EVENT_SAVE_PLAYER_DATA_) == true)
	{
		gEventMgr.removeEvents(this, EVENT_SAVE_PLAYER_DATA_);
	}
}
//
//bool CrashPlayer::havemapname(const char* mapname)
//{
//	int size_ar = _info.incompletemap_size();
//	std::string map_name;
//	for (int i = 0; i < size_ar; i++)
//	{
//		map_name = _info.incompletemap(i).mapname();
//		if (map_name == mapname)
//		{
//			return true;
//		}
//	}
//
//	size_ar = _info.completemap_size();
//	for (int i = 0; i < size_ar; i++)
//	{
//		map_name = _info.completemap(i).mapname();
//		if (map_name == mapname)
//		{
//			return true;
//		}
//	}
//
//	return false;
//}
//

void CrashPlayer::sendPBMessage(google::protobuf::Message* p)
{
	if (_session)
	{
		_session->sendPBMessage(p);
	}
}

void CrashPlayer::DelMap(message::MsgDelMapReq* msg)
{
	message::MsgDelMapACK msgACK;
	int size_ar = _info.incompletemap_size();
	u64 del_map_index = msg->map_index();
	msgACK.set_map_index(del_map_index);
	//msgACK.set_map_name(mapname);
	std::string map_name;
	bool del = false;
	
	::google::protobuf::RepeatedField< ::google::protobuf::uint64 >* temp_list = _info.mutable_completemap();
	::google::protobuf::RepeatedField< ::google::protobuf::uint64 >::iterator it = temp_list->begin();
	message::MapType temp_type = message::CompleteMap;
	for (it = temp_list->begin(); it != temp_list->end(); ++ it)
	{

		if ((*it) == del_map_index)
		{
			temp_type = message::CompleteMap;
			temp_list->erase(it);
			del = true;
			break;
		}
	}

	if (del == false)
	{
		temp_list = _info.mutable_incompletemap();
		for (it = temp_list->begin(); it != temp_list->end(); ++it)
		{
			if ((*it) == del_map_index)
			{
				temp_type = message::ImcompleteMap;
				temp_list->erase(it);
				del = true;
				break;
			}
		}
	}
	msgACK.set_map_type(temp_type);
	if (del == true)
	{
		gCrashMapManager.DelCrashMap(del_map_index);
	}
	else
	{
		msgACK.set_error(message::ServerError_NotFoundMapNameWhenDel);
	}
	
	sendPBMessage(&msgACK);
}

void CrashPlayer::DayUpdate()
{
	if (gGameConfig.isInToday(_info.last_accept_task_time()) == false)
	{
		if (_info.current_task_size() < 3)
		{
			const TaskManager::TASKCONFIGS*  task_configs = gTaskManager.GetTaskConfigs();
			TaskManager::TASKCONFIGS::const_iterator it = task_configs->begin();
			for (; it != task_configs->end(); ++it)
			{
				const message::TaskInfoConfig& entry = it->second;
				int required_pass_chapter_id = entry.required_pass_chapter_id();
				int required_pass_section_id = entry.required_pass_section_id();
				bool can_accept = true;
				if (required_pass_chapter_id != 0 && required_pass_section_id != 0)
				{
					can_accept = false;
					int pass_record_length = _info.passed_record_size();
					for (size_t i = 0; i < pass_record_length; i++)
					{
						message::intPair pair = _info.passed_record(i);
						if (pair.number_1() == required_pass_chapter_id)
						{
							if (pair.number_1() > pair.number_2())
							{
								can_accept = true;
							}
							break;
						}
					}
				}

				if (can_accept)
				{
					if (_info.complete_task_count() < entry.required_complete_task_count())
					{
						can_accept = false;
					}
				}
				if (can_accept)
				{
					message::TaskInfo* task_info = _info.add_current_task();
					task_info->set_task_id(entry.task_id());
					task_info->set_argu_1(0);
					task_info->set_argu_2(0);
					task_info->set_argu_3(0);
					task_info->set_describe(entry.describe().c_str());
					task_info->set_name(entry.name().c_str());
					message::MsgS2CNewTaskNotify msgACK;
					msgACK.mutable_info()->CopyFrom(*task_info);
					sendPBMessage(&msgACK);
					_info.set_last_accept_task_time(g_server_time);
					break;
				}
			}
		}
	}
}

void CrashPlayer::ReqPublishMap(const message::MsgC2SReqPlayerPublishMap* msg)
{
	u64 passed_time = g_server_time - _info.last_publish_map_time();
	u64 need_passed_time = gGameConfig.getMapConfig()->publish_map_cd * 60 * 60;
	if (need_passed_time > passed_time)
	{
		message::MsgServerErrorNotify msg;
		msg.set_error(message::ServerError_FailedToPublishMapTheTimeIsInCD);
		sendPBMessage(&msg);
	}
	else
	{
		gRankMapManager.PublishMap(msg, this);
		_info.set_last_publish_map_time(g_server_time);
	}
}



void CrashPlayer::SaveMap(message::MsgSaveMapReq* msg)
{
	message::MsgSaveMapACK msgACK;
	
	msgACK.set_save_type(msg->save_type());
	msgACK.set_error(message::ServerError_NO);
	message::CrashMapData* temp = gCrashMapManager.CreateCrashMap(&msg->map());
	u64 map_index = temp->data().map_index();
	switch (msg->save_type())
	{
	case  message::ImcompleteMap:
		_info.add_incompletemap(map_index);		
		break;
	case  message::CompleteMap:
		_info.add_completemap(map_index);
		break;
	default:
		break;
	}
	if (temp != NULL)
	{
		temp->CopyFrom(msg->map());
		message::CrashMapData* temp_map = msgACK.mutable_map();
		temp_map->CopyFrom(msg->map());
	}
	else
	{
		msgACK.set_error(message::ServerError_Unknow);
	}
	/*
	if (havemapname(msg->map().mapname().c_str()) == false)
	{

	}
	else
	{
		msgACK.set_error(message::ServerError_HaveSameName);
	}
	*/
	sendPBMessage(&msgACK);	
}

