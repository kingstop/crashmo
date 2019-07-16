using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using message;
namespace ENetDemo
{
    public class ENnetSocketClient
    {


        public delegate bool ProcessDelegate(System.IO.MemoryStream stream);
        public Dictionary<string, ProcessDelegate> _MessageFun = new Dictionary<string, ProcessDelegate>();

        public delegate void call_back();
        public Queue<call_back> _callback_queue = new Queue<call_back>();
        public ENnetSocketClient()
        {
            _MessageFun.Add("LoginResponse", login_respone);
            _MessageFun.Add("LoginRequest", login_request);
        }

        public void on_connect()
        {
            //LoginRequest msg = new LoginRequest();
            //msg.name = "12345";
            //msg.pwd = "54321";
            //send(msg);
        }

        public void on_close()
        {

        }
        public void connect(string ip, int port)
        {
            _host.Initialize(null, 1);
            _peer = _host.Connect(ip, port, 1234, 200);
            _count = 0;
        }

        private bool login_respone(System.IO.MemoryStream stream)
        {
            //LoginResponse msg = ProtoBuf.Serializer.Deserialize<LoginResponse>(stream);
            //send(msg);
            return true;
        }

        Int32 _count = 0;
        private bool login_request(System.IO.MemoryStream stream)
        {
            _count++;
            LoginRequest msg = ProtoBuf.Serializer.Deserialize<LoginRequest>(stream);
            send(msg);
            Console.WriteLine("login_request count {0}.", _count);
            return true;
        }

        public void logic_update()
        {
            lock(_lock_call_back)
            {
                while(_callback_queue.Count != 0)
                {
                    call_back call_back = _callback_queue.Dequeue();
                    call_back();
                }
            }

            lock (_lock_obj)
            {
                while(messageQueue.Count != 0)
                {
                    byte[] bytes = (byte[])messageQueue.Dequeue();
                    UInt32 max_length = (UInt32)bytes.Length;
                    UInt32 ptr = 0;
                    UInt32 pb_flag_type = System.BitConverter.ToUInt32(bytes, (int)ptr);
                    ptr += sizeof(UInt32);
                    UInt32 pb_name_type_length = System.BitConverter.ToUInt32(bytes, (int)ptr);
                    ptr += sizeof(UInt32);
                    if (pb_name_type_length == 0)
                    {
                        return;
                    }
                    if (bytes.Length - ptr < pb_name_type_length)
                    {
                        return;
                    }
                    String name = System.Text.Encoding.UTF8.GetString(bytes, (int)ptr, (int)pb_name_type_length);
                    ptr += pb_name_type_length;
                    String[] temp_arry = name.Split('.');
                    name = temp_arry[1];
                    //Debug.Log("function call [" + name + "]");
                    System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes, (int)ptr, (int)(max_length - ptr));
                    if (_MessageFun.ContainsKey(name))
                    {
                        _MessageFun[name](stream);
                        stream.Close();
                    }
                }

                //messageQueue.Enqueue(msg);
            }

        }

        public void update()
        {
            while (true)
            {
                ENet.Event @event;

                if (_host.Service(15000, out @event))
                {
                    do
                    {
                        switch (@event.Type)
                        {
                            case ENet.EventType.Connect:

                                Console.WriteLine("Connected to server at IP/port {0}.", @event.Peer.GetRemoteAddress());
                                lock(_lock_call_back)
                                {
                                    _callback_queue.Enqueue(on_connect);
                                }                                
                                break;
                            case ENet.EventType.Receive:
                                byte[] data = @event.Packet.GetBytes();
                                receive(data);
                                @event.Packet.Dispose();
                                break;
                            case ENet.EventType.Disconnect:
                                {
                                    lock (_lock_call_back)
                                    {
                                        _callback_queue.Enqueue(on_close);
                                    }
                                }
                                break;
                            default:
                            
                                Console.WriteLine(@event.Type);
                                break;
                        }
                    }
                    while (_host.CheckEvents(out @event));
                }
            }
        }

        private void receive(byte[] data)
        {
            Array.Copy(data, 0, _read_buff, _read_pos, data.Length);
            _read_pos += data.Length;
            UInt32 length = 0;
            int ptr = 0;
            try
            {
                while (ptr < _read_pos)
                {
                    if (length == 0)
                    {

                        length = System.BitConverter.ToUInt32(_read_buff, ptr);
                        ptr += sizeof(UInt32);
                    }
                    else
                    {
                        int cur_ptr = ptr;

                        UInt16 mask = System.BitConverter.ToUInt16(_read_buff, cur_ptr);
                        cur_ptr += sizeof(UInt16);
                        UInt32 crc32 = System.BitConverter.ToUInt32(_read_buff, cur_ptr);
                        cur_ptr += sizeof(UInt32);
                        UInt32 data_length = length - 2 * (UInt32)sizeof(UInt32) - (UInt16)sizeof(UInt16);
                        byte[] msg = new byte[data_length];
                        Array.Copy(_read_buff, cur_ptr, msg, 0, data_length);
                        ptr = cur_ptr + (int)data_length;
                        length = 0;

                        if(ptr < _read_pos)
                        {
                            _read_pos -= ptr;
                            Array.Copy(_read_buff, ptr, _read_buff, 0, _read_pos);
                        }
                        lock (_lock_obj)
                        {
                            messageQueue.Enqueue(msg);
                        }
                            
                    }

                }
            }
            catch
            {
                Console.WriteLine("Error:SocketClient: Got Exception while ParseMessage");
            }
        }
        public byte[] getarray(global::ProtoBuf.IExtensible base_msg)
        {
            String name = base_msg.ToString();
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(name);
            UInt32 name_length = (UInt32)byteArray.Length;

            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(mem, base_msg);
            UInt32 length = (UInt32)mem.Length;
            byte[] bytes = new byte[length];
            Array.Copy(mem.GetBuffer(), bytes, length);
            UInt32 pb_flag = 0;
            UInt16 mask = 0;
            //mask = 1 << 2;
            UInt32 crc32 = 0;
            //crc32 = (UInt32)CRC32.GetCRC32(bytes);
            UInt32 source_base64_length = sizeof(UInt32) + sizeof(UInt32) + name_length + length;
            UInt32 head_length = sizeof(UInt16) + sizeof(UInt32);
            UInt32 max_length = 0;//= sizeof(UInt16) + sizeof(UInt32) + source_base64_length;
            UInt32 body_length = 0;
            byte[] base64_buff = new byte[source_base64_length];


            //////////
            UInt32 write_pos = 0;
            UInt32 write_length = sizeof(UInt32);


            Array.Copy(BitConverter.GetBytes(pb_flag), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = sizeof(UInt32);

            Array.Copy(BitConverter.GetBytes(name_length), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = name_length;
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(name), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = length;
            Array.Copy(bytes, 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            UInt32 char_base64_out_length = write_pos * 2;
            char[] base64_out = new char[char_base64_out_length];
            int base64_length = Convert.ToBase64CharArray(base64_buff, 0, (int)write_pos, base64_out, 0);
            string base64_str = new string(base64_out, 0, base64_length);


            byte[] body_bytes = base64_buff; // System.Text.Encoding.UTF8.GetBytes(base64_out, 0, base64_length);

            body_length = (UInt32)body_bytes.Length;
            max_length = head_length + body_length + sizeof(UInt32);
            byte[] sendbuff = new byte[max_length];

            write_pos = 0;
            write_length = sizeof(UInt32);
            Array.Copy(BitConverter.GetBytes(max_length), 0, sendbuff, write_pos, write_length);

            write_pos += write_length;
            write_length = sizeof(UInt16);
            Array.Copy(BitConverter.GetBytes(mask), 0,sendbuff, write_pos ,write_length);
            write_pos += write_length;

            write_length = sizeof(UInt32);
            crc32 = (UInt32)CRC32.GetCRC32(body_bytes);
            Array.Copy(BitConverter.GetBytes(crc32), 0, sendbuff, write_pos, write_length);

            write_pos += write_length;
            write_length = body_length;
            Array.Copy(body_bytes, 0, sendbuff, write_pos, write_length);

            write_pos += write_length;

            return sendbuff;

        }
        public void send(global::ProtoBuf.IExtensible base_msg)
        {
            String name = base_msg.ToString();
            byte[] byteArray = System.Text.Encoding.Default.GetBytes(name);
            UInt32 name_length = (UInt32)byteArray.Length;

            System.IO.MemoryStream mem = new System.IO.MemoryStream();
            ProtoBuf.Serializer.Serialize<global::ProtoBuf.IExtensible>(mem, base_msg);
            UInt32 length = (UInt32)mem.Length;
            byte[] bytes = new byte[length];
            Array.Copy(mem.GetBuffer(), bytes, length);
            UInt32 pb_flag = 0;
            UInt16 mask = 0;
            //mask = 1 << 2;
            UInt32 crc32 = 0;
            //crc32 = (UInt32)CRC32.GetCRC32(bytes);
            UInt32 source_base64_length = sizeof(UInt32) + sizeof(UInt32) + name_length + length;
            UInt32 head_length = sizeof(UInt16) + sizeof(UInt32);
            UInt32 max_length = 0;//= sizeof(UInt16) + sizeof(UInt32) + source_base64_length;
            UInt32 body_length = 0;
            byte[] base64_buff = new byte[source_base64_length];


            //////////
            UInt32 write_pos = 0;
            UInt32 write_length = sizeof(UInt32);


            Array.Copy(BitConverter.GetBytes(pb_flag), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = sizeof(UInt32);

            Array.Copy(BitConverter.GetBytes(name_length), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = name_length;
            Array.Copy(System.Text.Encoding.UTF8.GetBytes(name), 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            write_length = length;
            Array.Copy(bytes, 0, base64_buff, write_pos, write_length);
            write_pos += write_length;
            UInt32 char_base64_out_length = write_pos * 2;
            char[] base64_out = new char[char_base64_out_length];
            int base64_length = Convert.ToBase64CharArray(base64_buff, 0, (int)write_pos, base64_out, 0);
            string base64_str = new string(base64_out, 0, base64_length);


            byte[] body_bytes = base64_buff; // System.Text.Encoding.UTF8.GetBytes(base64_out, 0, base64_length);

            body_length = (UInt32)body_bytes.Length;
            max_length = head_length + body_length + sizeof(UInt32);
            byte[] sendbuff = new byte[max_length];

            write_pos = 0;
            write_length = sizeof(UInt32);
            Array.Copy(BitConverter.GetBytes(max_length), 0, sendbuff, write_pos, write_length);

            write_pos += write_length;
            write_length = sizeof(UInt16);
            Array.Copy(BitConverter.GetBytes(mask), 0, sendbuff, write_pos, write_length);
            write_pos += write_length;

            write_length = sizeof(UInt32);
            crc32 = (UInt32)CRC32.GetCRC32(body_bytes);
            Array.Copy(BitConverter.GetBytes(crc32), 0, sendbuff, write_pos, write_length);

            write_pos += write_length;
            write_length = body_length;
            Array.Copy(body_bytes, 0, sendbuff, write_pos, write_length);

            write_pos += write_length;

            _peer.Send(1, sendbuff, ENet.PacketFlags.Reliable);


            //ENet.Packet packet = new ENet.Packet();
            //packet.Initialize(sendbuff, 0, (int)write_pos, ENet.PacketFlags.Reliable);
            //packet.Freed += p =>
            //{
            //    //Console.WriteLine("Initial packet freed (channel {0}, square of channel {1})",
            //    //    p.GetUserData(),
            //    //    p.GetUserData("Test"));
            //};
            //_peer.Send(0, packet);
            //packet.Dispose();
            //socket_client.Send(sendbuff);
        }
        ENet.Host _host = new ENet.Host();
        ENet.Peer _peer;
        protected Queue _buffer_queue = new Queue();
        byte[] _read_buff = new byte[655300];
        int _read_pos = 0;
        private Queue messageQueue = new Queue();
        private object _lock_obj = new object();
        private object _lock_call_back = new object();
    }
}
