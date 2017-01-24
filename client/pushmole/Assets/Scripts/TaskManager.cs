using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
public enum enTaskLoadState
{
    NO,
    LOADING,
    LOADED,
}
public class TaskManager
{
    public Dictionary<int, message.TaskInfoConfig> _tasks = new Dictionary<int, message.TaskInfoConfig>();
    protected enTaskLoadState _state;

    public enTaskLoadState GetTaskManagerState()
    {
        return _state;
    }
    public TaskManager()
    {
        _state = enTaskLoadState.NO;
    }
    public void LoadTask()
    {
        if(_state == enTaskLoadState.NO)
        {
            message.MsgC2SReqLoadTaskConfigs msg = new message.MsgC2SReqLoadTaskConfigs();
            msg.begin_id = -1;
            msg.load_count = 20;
            global_instance.Instance._net_client.send(msg);
            _state = enTaskLoadState.LOADING;
        }

    }

    public void ParseLoadTask(message.MsgS2CLoadTaskConfigsACK msg)
    {
        int max_id = 0;
        List<message.TaskInfoConfig> list = msg.task_configs;
        foreach(message.TaskInfoConfig entry in list)
        {
            if(entry.task_id > max_id)
            {
                max_id = entry.task_id;
            }
            _tasks[entry.task_id] = entry;
        }
        if(_tasks.Count == msg.total_task_count)
        {
            _state = enTaskLoadState.LOADED;
        }
        else
        {
            message.MsgC2SReqLoadTaskConfigs msgReq = new message.MsgC2SReqLoadTaskConfigs();
            msgReq.begin_id = max_id;
            msgReq.load_count = 20;
            global_instance.Instance._net_client.send(msgReq);
        }
    }
}