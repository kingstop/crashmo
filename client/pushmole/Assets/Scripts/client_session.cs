﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using message;

public class msgtg
{
    public System.IO.MemoryStream stream_;
    public string name_;
}
public class client_session
{
    public delegate bool ProcessDelegate(System.IO.MemoryStream stream);
    public Dictionary<string, ProcessDelegate> _MessageFun = new Dictionary<string, ProcessDelegate>();
    private Queue _msg_queue = new Queue();
    public client_session()
    {
        _MessageFun.Add("RegisterAccountFaildACK", RegisterFailed);
        _MessageFun.Add("CrashmoClientInit", CrashmoClientInit);
        _MessageFun.Add("MsgSaveMapACK", SaveMap);
        _MessageFun.Add("MsgDelMapACK", DelMap);
        _MessageFun.Add("MsgModifySectionNameACK", ModifySectionName);
        _MessageFun.Add("MsgS2COfficeStatusACK", ParseOfficeStatus);
        _MessageFun.Add("MsgS2COfficeMapACK", ParseOfficeMapACK);
		_MessageFun.Add ("MsgS2CLoadTaskConfigsACK", parseLoadTaskConfigsACK);
        _MessageFun.Add("MsgS2CPassOfficilMapACK", parseMsgS2CPassOfficilMapACK);
        _MessageFun.Add("MsgS2CNewTaskNotify", parseMsgS2CNewTaskNotify);
        _MessageFun.Add("MsgLoadUserMapACK", LoadUserMapACK);
        _MessageFun.Add("MsgS2CModifyTaskInfoACK", parseMsgS2CModifyTaskInfoACK);


    }

    ~client_session()
    {

    }


    public bool parseMsgS2CNewTaskNotify(System.IO.MemoryStream stream)
    {
        MsgS2CNewTaskNotify msg = ProtoBuf.Serializer.Deserialize<MsgS2CNewTaskNotify>(stream);                
        return true;
    }

    public bool parseMsgS2CModifyTaskInfoACK(System.IO.MemoryStream stream)
    {
        MsgS2CModifyTaskInfoACK msg = ProtoBuf.Serializer.Deserialize<MsgS2CModifyTaskInfoACK>(stream);
        global_instance.Instance._ngui_edit_manager._task_edit_panel.OnAddTask(msg.info);
        return true;
    }
    public bool parseMsgS2CPassOfficilMapACK(System.IO.MemoryStream stream)
    {
        global_instance.Instance._ngui_edit_manager._game_end.clear();
        MsgS2CPassOfficilMapACK msg = ProtoBuf.Serializer.Deserialize<MsgS2CPassOfficilMapACK>(stream);
        foreach(message.MsgTaskReward entry in msg.complete_task)
        {
            global_instance.Instance._ngui_edit_manager._game_end.AddTaskRewards(entry);
        }
        //global_instance.Instance._ngui_edit_manager._game_end.taskRewards_.SetGold((int)msg.add_gold);
        global_instance.Instance._ngui_edit_manager._game_end.SetGold((int)msg.add_gold);
        foreach (message.intPair entry in msg.add_resource)
        {
            global_instance.Instance._ngui_edit_manager._game_end.AddRewardCount(entry.number_1, entry.number_2);
        }
        global_instance.Instance._ngui_edit_manager.game_win();

        return true;
    }


    public bool parseLoadTaskConfigsACK(System.IO.MemoryStream stream)
	{
		MsgS2CLoadTaskConfigsACK msg = ProtoBuf.Serializer.Deserialize<MsgS2CLoadTaskConfigsACK>(stream);
		global_instance.Instance._taskManager.ParseLoadTask (msg);
		return true;
	}

    public bool ParseOfficeMapACK(System.IO.MemoryStream stream)
    {
        MsgS2COfficeMapACK msg = ProtoBuf.Serializer.Deserialize<MsgS2COfficeMapACK>(stream);
        global_instance.Instance._officilMapManager.parseOfficeChaper(msg);
        return true;
    }

    public bool ParseOfficeStatus(System.IO.MemoryStream stream)
    {
        MsgS2COfficeStatusACK msg = ProtoBuf.Serializer.Deserialize<MsgS2COfficeStatusACK>(stream);
        global_instance.Instance._officilMapManager.parseOfficeStatus(msg);
        return true;

    }




    public bool CrashmoClientInit(System.IO.MemoryStream stream)
    {
       
        CrashmoClientInit msg = ProtoBuf.Serializer.Deserialize<CrashmoClientInit>(stream);
        global_instance.Instance._player.SetInfo(msg.info);
        foreach(message.MsgIntStringProto key_pair in msg.chapter_names)
        {
			global_instance.Instance._officilMapManager.addChapterName(key_pair.intger_temp, key_pair.string_temp);
        }
        global_instance.Instance._officilMapManager.LoadSections();
        //global_instance.Instance._officilMapManager.LoadFromLocal();
        message.MsgLoadUserMapReq msgLoadUserMapReq = new message.MsgLoadUserMapReq();
        msgLoadUserMapReq.map_count = 10;
        global_instance.Instance._net_client.send(msgLoadUserMapReq);


        //if (global_instance.Instance._player.isadmin())
        //{
        //   global_instance.Instance._taskManager.LoadTask();
        //}        
        //global_instance.Instance._ngui_edit_manager._login_obj.SetActive(false);
        //global_instance.Instance._ngui_edit_manager.show_main_panel();
        return true;
    }

    public bool LoadUserMapACK(System.IO.MemoryStream stream)
    {
        MsgLoadUserMapACK msg = ProtoBuf.Serializer.Deserialize<MsgLoadUserMapACK>(stream);
        foreach(MsgUserMap entry in msg.maps)
        {
            MapType Type = MapType.ImcompleteMap;
            if(entry.complete)
            {
                Type = MapType.CompleteMap;
            }
            global_instance.Instance._player.addUserMap(entry.map, Type);
        }
        if(msg.end)
        {
            if (global_instance.Instance._player.isadmin())
            {
                global_instance.Instance._taskManager.LoadTask();
            }
        }
        return true;
    }


   
    public bool ModifySectionName(System.IO.MemoryStream stream)
    {
        MsgModifySectionNameACK msg = ProtoBuf.Serializer.Deserialize<MsgModifySectionNameACK>(stream);
		global_instance.Instance._officilMapManager.addChapterName(msg.section, msg.section_name);
        global_instance.Instance._ngui_edit_manager._main_panel.refrashCurrentPage(global_instance.Instance._ngui_edit_manager._main_panel._current_page);
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        return true;
    }


    private bool SaveMap(System.IO.MemoryStream stream)
    {
        MsgSaveMapACK msg = ProtoBuf.Serializer.Deserialize<MsgSaveMapACK>(stream);

        global_instance.Instance._player.addUserMap(msg.map, msg.save_type);
        global_instance.Instance._ngui_edit_manager._SaveMapPanel.SaveMapOk();               
        return true;
    }

    private bool DelMap(System.IO.MemoryStream stream)
    {
        MsgDelMapACK msg = ProtoBuf.Serializer.Deserialize<MsgDelMapACK>(stream);
        
        global_instance.Instance._player.delMap(msg.map_type, msg.map_index);           
        return true;
    }



    public void addmsg(string name, System.IO.MemoryStream stream)
    {
        msgtg msgtg = new msgtg();
        msgtg.name_ = name;
        msgtg.stream_ = stream;
        lock (_msg_queue)
        {
            _msg_queue.Enqueue(msgtg);
        }
    }

    public bool RegisterFailed(System.IO.MemoryStream stream)
    {
        RegisterAccountFaildACK msg = ProtoBuf.Serializer.Deserialize<RegisterAccountFaildACK>(stream);

        return true;
    }

    public void register_account(string acc, string pwd)
    {
        global_instance.Instance._net_client.connect();
        RegisterAccountRequest req = new RegisterAccountRequest();
        req.name = acc;
        req.pwd = pwd;
        send(req);
    }


    public void login(string acc, string pwd)
    {

        global_instance.Instance._net_client.connect();
        LoginRequest req = new LoginRequest();
        req.name = acc;
        req.pwd = pwd;
        send(req);
        global_instance.Instance._in_login = true;
    }
    public void send(global::ProtoBuf.IExtensible base_msg)
    {
        global_instance.Instance._net_client.send(base_msg);
    }
    public void update()
    {
        lock (_msg_queue)
        {
            while (_msg_queue.Count != 0)
            {
                msgtg msg = (msgtg)_msg_queue.Dequeue();
                if (_MessageFun.ContainsKey(msg.name_))
                {
                    _MessageFun[msg.name_](msg.stream_);
                }
                msg.stream_.Close();

            }
        }

    }

}
