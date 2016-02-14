using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common.Sockets
{
    public class MsgCompoment
    {
        //public delegate void MessageFun(MsgHead message, SocketClient socketclient);
       // public  Dictionary<int, MessageFun> MsgFunction = new Dictionary<int, MessageFun>();

        protected MsgCompoment()
        {

        }
        //public  MsgHead MsgParse(byte[] bytes)
        //{
        //    MsgHead head = BaseData.DeSerialize<MsgHead>(bytes);
        //    return head;
        //}

        public virtual void CreateFuctions()
        {

        }

        //public void callFunction(byte[] bytes, SocketClient socketclient)
        //{ 
        //    MsgHead head = MsgParse(bytes);
        //    if (MsgFunction.ContainsKey(head.msgType))
        //    {
        //        callFunction(head, socketclient);
        //        MsgFunction[head.msgType](head, socketclient);
        //    }
        //    else
        //    {
        //        waringNotFoundMsg(head.msgType, socketclient);
        //    }
        //}

        //public virtual void callFunction(MsgHead msg, SocketClient socketclient)
        //{

        //}

        //public virtual void waringNotFoundMsg(int n, SocketClient socketclient)
        //{

        //}
    }
    
}
