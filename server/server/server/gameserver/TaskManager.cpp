#include "stdafx.h"
#include "TaskManager.h"
#define _SAVE_TASK_TIME_  (20 * _TIME_SECOND_MSEL_)
TaskManager::TaskManager()
{
}


TaskManager::~TaskManager()
{
}

void TaskManager::AddTask(const message::TaskInfoConfig& info)
{
	_task_configs[info.task_id()] = info;
}

bool TaskManager::DelTask(int task_id)
{
	bool ret = false;
	TASKCONFIGS::iterator it = _task_configs.find(task_id);
	if (it != _task_configs.end())
	{
		_task_configs.erase(it);
		ret = true;
	}
	return ret;
}

const TaskManager::TASKCONFIGS* TaskManager::GetTaskConfigs()
{
	return &_task_configs;
}

void TaskManager::LoadConfig(DBQuery* p)
{
	p->reset();
	DBQuery& query = *p;
	DBQParms parms;
	query << "SELECT * FROM task_config";
	query.parse();
	SDBResult result = query.store(parms);
	
	for (size_t i = 0; i < result.num_rows(); i++)
	{
		DBRow row = result[i];
		message::TaskInfoConfig entry;
		entry.set_task_id(row["task_id"]);
		entry.set_name(row["name"].c_str());
		entry.set_describe(row["describe"].c_str());
		entry.set_required_pass_chapter_id(row["required_chapter_id"]);
		entry.set_required_pass_section_id(row["required_section_id"]);
		entry.set_required_complete_task_count(row["required_complete_count"]);
		for (size_t j = 1; j <= 3; j++)
		{
			message::TaskConditionTypeConfig* conditionConfig = entry.add_conditions();
			message::TaskRewardConfig* rewardConfig = entry.add_rewards();
			char szTemp[255];
			sprintf(szTemp, "condition_type_%d", j);
			int condition_type = row[szTemp];
			conditionConfig->set_condition((message::ConditionType) condition_type);
			sprintf(szTemp, "condition_argu_%d_1", j);
			conditionConfig->set_argu_1(row[szTemp]);
			sprintf(szTemp, "condition_argu_%d_2", j);
			conditionConfig->set_argu_2(row[szTemp]);
			sprintf(szTemp, "reward_type_%d", j);
			int reward_type = row[szTemp];
			rewardConfig->set_resource_type((message::ResourceType) reward_type);
			sprintf(szTemp, "reward_argu_%d", j);
			rewardConfig->set_count(row[szTemp]);
		}
		_task_configs[entry.task_id()] = entry;
	}

	if (gEventMgr.hasEvent(this, EVENT_SAVE_TASK) == false)
	{
		gEventMgr.addEvent(this, &TaskManager::Save, EVENT_SAVE_TASK, _SAVE_TASK_TIME_, -1, 0);
	}
}

void TaskManager::Save()
{
	message::gs2dbWorldDatabaseSql msg;
	std::string sql = "delete from task_config;";
	int save_count = 10;
	int count = 0;
	char sz_temp[2048];
	TASKCONFIGS::iterator it = _task_configs.begin();
	for (; it != _task_configs.end(); ++ it, count ++)
	{
		const message::TaskInfoConfig& task_entry = it->second;
		if (count == 0)
		{
			sql += "insert into task_config(`task_id`, `name`,\
			 `describe`, `required_chapter_id`,\
			 `required_section_id`, `required_complete_count`,\
			 `condition_type_1`, `condition_argu_1_1`,`condition_argu_1_2`,\
			 `condition_type_2`, `condition_argu_2_1`,`condition_argu_2_2`,\
			 `condition_type_3`, `condition_argu_3_1`,`condition_argu_3_2`,\
			 `reward_type_1`, `reward_argu_1`,\
			 `reward_type_2`, `reward_argu_2`,\
			 `reward_type_3`, `reward_argu_3`) values";
		}
		else
		{
			sql += ",";
		}

		sprintf(sz_temp, "(%d, '%s', '%s', %d, %d, %d, %d, %d,\
			 %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d, %d)",
			task_entry.task_id(), task_entry.name().c_str(), task_entry.describe().c_str(),
			task_entry.required_pass_chapter_id(), task_entry.required_pass_section_id(),
			task_entry.required_complete_task_count(),
			(int)task_entry.conditions(0).condition(), task_entry.conditions(0).argu_1(), task_entry.conditions(0).argu_2(),
			(int)task_entry.conditions(1).condition(), task_entry.conditions(1).argu_1(), task_entry.conditions(1).argu_2(),
			(int)task_entry.conditions(2).condition(), task_entry.conditions(2).argu_1(), task_entry.conditions(2).argu_2(),
			(int)task_entry.rewards(0).resource_type(),task_entry.rewards(0).count(),
			(int)task_entry.rewards(1).resource_type(), task_entry.rewards(1).count(),
			(int)task_entry.rewards(2).resource_type(), task_entry.rewards(2).count());		
		sql += sz_temp;

		if (count >= save_count)
		{
			msg.set_sql(sql.c_str());
			gGSDBClient.sendPBMessage(&msg, 0);
			count = 0;
			sql.clear();
		}
	}
	if (sql.empty() == false)
	{
		msg.set_sql(sql.c_str());
		gGSDBClient.sendPBMessage(&msg, 0);
	}
}