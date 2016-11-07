using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using message;

using common.Sockets;

enum u3dclient_state
{
    disconnect,
    connect_login,
    login_check_sucessed,
    connect_gate,
    login_game,


}
public class u3dclient 
{

    private SocketClient socket_client = null;
    private static u3dclient _instance = null;
    u3dclient_state _connect_state = u3dclient_state.disconnect;
    private String _gate_ip;
    private int _gate_port;
    private UInt64 _user_account;
    public u3dclient()
    {
        register_function();
    }

	protected SocketClient  CreateSocket()
	{
		return new SocketClient(1048576, 1048576, 131072, null, new MessageHandler(MessageClient), 
			new CloseHandler(CloseClient), new ErrorHandler(ErrorClient), new ConnectHandler(on_connect));
	}
    public static u3dclient Instance 
    { 
          get  
          { 
             if (_instance == null) 
             {
                 _instance = new u3dclient(); 
             } 
             return _instance; 
          } 
     } 

   public delegate bool ProcessDelegate(System.IO.MemoryStream stream, SocketClient socketclient);

   public Dictionary<string, ProcessDelegate> _MessageFun = new Dictionary<string, ProcessDelegate>();

    public void CloseClient(SocketBase socket)
   {

   }


    public void ErrorClient(SocketBase socket, Exception exception)
    {

    }
   public void MessageClient(SocketBase socket, int iNumberOfBytes)
   {
       try
       {
           SocketClient pSocket = ((SocketClient)socket);
           Byte[] bytes = new Byte[iNumberOfBytes];
           Array.Copy(socket.MsgBuffer, bytes, iNumberOfBytes);
           callFunction(bytes, pSocket);
       }
       catch (Exception pException)
       {
           Debug.Log(pException.Message);
       }
   }


    public void connect()
   {
		socket_client = CreateSocket ();
		socket_client.Connect("114.55.116.251", 20002);
   }

    public void register_account(string acc, string pwd)
    {
        connect();
        RegisterAccountRequest req = new RegisterAccountRequest();
        req.name = acc;
        req.pwd = pwd;
        send(req);
    }
    public void on_connect(SocketBase socket)
    {
        switch(_connect_state)
        {
            case  u3dclient_state.connect_gate:
                {
                    LoginGame msg = new LoginGame();
                    msg.user_account = (uint)_user_account;
                    send(msg);
                }
                break;                
        }
        
       // System.IO.MemoryStream mem = new System.IO.MemoryStream();

    }


    public void update()
    {
       // socket_client.Receive();
    }

    public void send(global::ProtoBuf.IExtensible base_msg)
    {
        String name = base_msg.ToString();
        System.IO.MemoryStream mem = new System.IO.MemoryStream();
        ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(mem, base_msg);
        UInt32 length = (UInt32)mem.Length;
        byte[] bytes = new byte[length];
        Array.Copy(mem.GetBuffer(), bytes, length);
        UInt32 pb_flag = 0;
        UInt16 mask = 0;
        UInt32 crc32 = 0;
        crc32 = (UInt32)CRC32.GetCRC32(bytes.ToString());
        byte[] byteArray = System.Text.Encoding.Default.GetBytes(name);
        UInt32 name_length = (UInt32)byteArray.Length;
        UInt32 max_length = sizeof(UInt16) + sizeof(UInt32) + sizeof(UInt32) + sizeof(UInt32) + name_length + length;
        byte[] sendbuff = new byte[max_length];


        UInt32 write_length = sizeof(UInt16);
        UInt32 write_pos = 0;
       
       
        Array.Copy(BitConverter.GetBytes(mask), sendbuff, write_length);
        write_pos += write_length;

        write_length = sizeof(UInt32);

        Array.Copy(BitConverter.GetBytes(crc32), 0, sendbuff, write_pos, write_length);

        write_pos += write_length;
        write_length = sizeof(UInt32);
        Array.Copy(BitConverter.GetBytes(pb_flag), 0, sendbuff, write_pos, write_length);
        write_pos += write_length;
        write_length = sizeof(UInt32);

        Array.Copy(BitConverter.GetBytes(name_length),0, sendbuff, write_pos,write_length);
        write_pos += write_length;
        write_length = name_length;
        Array.Copy(System.Text.Encoding.UTF8.GetBytes(name), 0, sendbuff, write_pos, write_length);
        write_pos += write_length;
        write_length = length;
        Array.Copy(bytes, 0, sendbuff, write_pos, write_length);
        socket_client.Send(sendbuff);
       
    }

    public void callFunction(byte[] bytes, SocketClient socketclient)
    {
        UInt32 max_length = (UInt32)bytes.Length;
        UInt32 ptr = 0;
        UInt32 pb_flag_type = System.BitConverter.ToUInt32(bytes, (int)ptr);        
        ptr += sizeof(UInt32);
        UInt32 pb_name_type_length = System.BitConverter.ToUInt32(bytes, (int)ptr);
        ptr += sizeof(UInt32);
        if(pb_name_type_length == 0)
        {
            return;
        }
        if(bytes.Length - ptr < pb_name_type_length)
        {
            return;
        }
        String name = System.Text.Encoding.UTF8.GetString(bytes, (int)ptr, (int)pb_name_type_length);
        ptr += pb_name_type_length;        
        String[] temp_arry = name.Split('.');
        name = temp_arry[1];
        Debug.Log("function call [" + name + "]");
        System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes, (int)ptr, (int)(max_length - ptr));
        if(_MessageFun.ContainsKey(name))
        {
            _MessageFun[name](stream, socketclient);
            stream.Close();
        }
        else
        {
            global_instance.Instance._client_session.addmsg(name, stream);            
        }
    }

    private void register_function()
    {
        _MessageFun.Add("LoginResponse", login_respone);
    }

    private bool register_failed(System.IO.MemoryStream stream, SocketClient socketclient)
    {
        RegisterAccountFaildACK msg = ProtoBuf.Serializer.Deserialize<RegisterAccountFaildACK>(stream);
        return true;
    }
    private bool login_respone(System.IO.MemoryStream stream, SocketClient socketclient)
    {
        message.LoginResponse msg = ProtoBuf.Serializer.Deserialize<message.LoginResponse>(stream);
        if (msg.result == enumLoginResult.enumLoginResult_Success)
        {
            socket_client.Disconnect();
            _gate_ip = msg.gate_ip;
            _gate_port = (int)msg.gate_port;
            _user_account = msg.user_account;
            _connect_state = u3dclient_state.connect_gate;
			socket_client = CreateSocket ();
            socket_client.Connect(_gate_ip, _gate_port);
        }
        return true;
    }
}
