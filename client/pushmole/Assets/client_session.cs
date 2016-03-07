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
    }

    ~client_session()
    {

    }

    public bool CrashmoClientInit(System.IO.MemoryStream stream)
    {
        CrashmoClientInit msg = ProtoBuf.Serializer.Deserialize<CrashmoClientInit>(stream);
        global_instance.Instance._player.SetInfo(msg.info);
        foreach(message.MsgIntStringProto key_pair in msg.sections_names)
        {
            global_instance.Instance._player.addSectionName(key_pair.intger_temp, key_pair.string_temp);
        }
        global_instance.Instance._ngui_edit_manager._login_obj.SetActive(false);
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        return true;
    }

	
    public bool ModifySectionName(System.IO.MemoryStream stream)
    {
        MsgModifySectionNameACK msg = ProtoBuf.Serializer.Deserialize<MsgModifySectionNameACK>(stream);
        global_instance.Instance._player.addSectionName(msg.section, msg.section_name);
        global_instance.Instance._ngui_edit_manager._main_panel.refrashCurrentPage(global_instance.Instance._ngui_edit_manager._main_panel._current_page);
        global_instance.Instance._ngui_edit_manager.show_main_panel();
        return true;
    }


    private bool SaveMap(System.IO.MemoryStream stream)
    {
        MsgSaveMapACK msg = ProtoBuf.Serializer.Deserialize<MsgSaveMapACK>(stream);
        if(msg.error == ServerError.ServerError_NO)
        {
            CrashPlayerInfo player_info = global_instance.Instance._player.GetInfo();
            switch(msg.save_type)
            {
                case MapType.CompleteMap:
                    {
                        player_info.CompleteMap.Add(msg.map);
                    }
                    break;
                case MapType.ImcompleteMap:
                    {
                        player_info.IncompleteMap.Add(msg.map);
                    }
                    break;
                case MapType.OfficeMap:
                    {
                        global_instance.Instance._player.add_map(msg.map);
                    }
                    break;
            }
            global_instance.Instance._player.SetInfo(player_info);
            global_instance.Instance._ngui_edit_manager._SaveMapPanel.SaveMapOk();
        }
        
        return true;
    }

    private bool DelMap(System.IO.MemoryStream stream)
    {
        MsgDelMapACK msg = ProtoBuf.Serializer.Deserialize<MsgDelMapACK>(stream);
        global_instance.Instance._player.delMap(msg.map_type, msg.map_name);           
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
