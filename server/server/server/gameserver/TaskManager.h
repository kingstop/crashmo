#pragma once
#include <map>
#include <iostream>
class TaskManager : public EventableObject
{
public:
	typedef std::map<int, message::TaskInfoConfig> TASKCONFIGS;
public:
	void LoadConfig(DBQuery* p);
	void AddTask(const message::TaskInfoConfig& info);
	bool DelTask(int task_id);
	const TASKCONFIGS* GetTaskConfigs();
	const message::TaskInfoConfig* GetTaskConfig(int id);
	void Save();
public:
	TaskManager();
	virtual ~TaskManager();
protected:
	TASKCONFIGS _task_configs;
};

