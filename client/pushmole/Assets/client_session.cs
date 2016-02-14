using UnityEngine;
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
    }

    ~client_session()
    {

    }
    
    public bool CrashmoClientInit(System.IO.MemoryStream stream)
    {
        CrashmoClientInit msg = ProtoBuf.Serializer.Deserialize<CrashmoClientInit>(stream);
        global_instance.Instance._player.SetInfo(msg.info);
        global_instance.Instance._ngui_edit_manager.show_main_panel();
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
