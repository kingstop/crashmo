//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: crashmo.proto
// Note: requires additional types generated from: common.proto
namespace message
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgIntStringProto")]
  public partial class MsgIntStringProto : global::ProtoBuf.IExtensible
  {
    public MsgIntStringProto() {}
    
    private int _intger_temp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"intger_temp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int intger_temp
    {
      get { return _intger_temp; }
      set { _intger_temp = value; }
    }
    private string _string_temp;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"string_temp", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string string_temp
    {
      get { return _string_temp; }
      set { _string_temp = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CrashmoClientInit")]
  public partial class CrashmoClientInit : global::ProtoBuf.IExtensible
  {
    public CrashmoClientInit() {}
    
    private message.CrashPlayerInfo _info;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"info", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public message.CrashPlayerInfo info
    {
      get { return _info; }
      set { _info = value; }
    }
    private readonly global::System.Collections.Generic.List<message.MsgIntStringProto> _sections_names = new global::System.Collections.Generic.List<message.MsgIntStringProto>();
    [global::ProtoBuf.ProtoMember(2, Name=@"sections_names", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.MsgIntStringProto> sections_names
    {
      get { return _sections_names; }
    }
  
    private readonly global::System.Collections.Generic.List<message.intPair> _resources_config_max = new global::System.Collections.Generic.List<message.intPair>();
    [global::ProtoBuf.ProtoMember(3, Name=@"resources_config_max", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.intPair> resources_config_max
    {
      get { return _resources_config_max; }
    }
  
    private int _map_width_config_max;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"map_width_config_max", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_width_config_max
    {
      get { return _map_width_config_max; }
      set { _map_width_config_max = value; }
    }
    private int _map_height_config_max;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"map_height_config_max", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_height_config_max
    {
      get { return _map_height_config_max; }
      set { _map_height_config_max = value; }
    }
    private int _map_count_max;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"map_count_max", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_count_max
    {
      get { return _map_count_max; }
      set { _map_count_max = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgSaveMapReq")]
  public partial class MsgSaveMapReq : global::ProtoBuf.IExtensible
  {
    public MsgSaveMapReq() {}
    
    private message.CrashMapData _map;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"map", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public message.CrashMapData map
    {
      get { return _map; }
      set { _map = value; }
    }
    private message.MapType _save_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"save_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.MapType save_type
    {
      get { return _save_type; }
      set { _save_type = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgDelMapReq")]
  public partial class MsgDelMapReq : global::ProtoBuf.IExtensible
  {
    public MsgDelMapReq() {}
    
    private string _map_name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"map_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string map_name
    {
      get { return _map_name; }
      set { _map_name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgDelMapACK")]
  public partial class MsgDelMapACK : global::ProtoBuf.IExtensible
  {
    public MsgDelMapACK() {}
    
    private string _map_name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"map_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string map_name
    {
      get { return _map_name; }
      set { _map_name = value; }
    }
    private message.MapType _map_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"map_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.MapType map_type
    {
      get { return _map_type; }
      set { _map_type = value; }
    }
    private message.ServerError _error;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"error", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.ServerError error
    {
      get { return _error; }
      set { _error = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgSaveMapACK")]
  public partial class MsgSaveMapACK : global::ProtoBuf.IExtensible
  {
    public MsgSaveMapACK() {}
    
    private string _map_name;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"map_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string map_name
    {
      get { return _map_name; }
      set { _map_name = value; }
    }
    private message.MapType _save_type;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"save_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.MapType save_type
    {
      get { return _save_type; }
      set { _save_type = value; }
    }
    private message.CrashMapData _map;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"map", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public message.CrashMapData map
    {
      get { return _map; }
      set { _map = value; }
    }
    private message.ServerError _error;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"error", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.ServerError error
    {
      get { return _error; }
      set { _error = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgOfficilMapReq")]
  public partial class MsgOfficilMapReq : global::ProtoBuf.IExtensible
  {
    public MsgOfficilMapReq() {}
    
    private int _page;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"page", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int page
    {
      get { return _page; }
      set { _page = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgOfficilMapACK")]
  public partial class MsgOfficilMapACK : global::ProtoBuf.IExtensible
  {
    public MsgOfficilMapACK() {}
    
    private int _page;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"page", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int page
    {
      get { return _page; }
      set { _page = value; }
    }
    private readonly global::System.Collections.Generic.List<message.CrashMapData> _maps = new global::System.Collections.Generic.List<message.CrashMapData>();
    [global::ProtoBuf.ProtoMember(2, Name=@"maps", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.CrashMapData> maps
    {
      get { return _maps; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgModifySectionNameReq")]
  public partial class MsgModifySectionNameReq : global::ProtoBuf.IExtensible
  {
    public MsgModifySectionNameReq() {}
    
    private int _section;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"section", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int section
    {
      get { return _section; }
      set { _section = value; }
    }
    private string _section_name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"section_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string section_name
    {
      get { return _section_name; }
      set { _section_name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgModifySectionNameACK")]
  public partial class MsgModifySectionNameACK : global::ProtoBuf.IExtensible
  {
    public MsgModifySectionNameACK() {}
    
    private int _section;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"section", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int section
    {
      get { return _section; }
      set { _section = value; }
    }
    private string _section_name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"section_name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string section_name
    {
      get { return _section_name; }
      set { _section_name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgSectionNameReq")]
  public partial class MsgSectionNameReq : global::ProtoBuf.IExtensible
  {
    public MsgSectionNameReq() {}
    
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgSectionNameACK")]
  public partial class MsgSectionNameACK : global::ProtoBuf.IExtensible
  {
    public MsgSectionNameACK() {}
    
    private readonly global::System.Collections.Generic.List<message.MsgIntStringProto> _sections = new global::System.Collections.Generic.List<message.MsgIntStringProto>();
    [global::ProtoBuf.ProtoMember(1, Name=@"sections", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.MsgIntStringProto> sections
    {
      get { return _sections; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"MsgS2CNotifyPing")]
  public partial class MsgS2CNotifyPing : global::ProtoBuf.IExtensible
  {
    public MsgS2CNotifyPing() {}
    
    private long _time_stamp;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"time_stamp", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public long time_stamp
    {
      get { return _time_stamp; }
      set { _time_stamp = value; }
    }
    private int _ping_count;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"ping_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int ping_count
    {
      get { return _ping_count; }
      set { _ping_count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
    [global::ProtoBuf.ProtoContract(Name=@"ServerError")]
    public enum ServerError
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ServerError_NO", Value=0)]
      ServerError_NO = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ServerError_Unknow", Value=1)]
      ServerError_Unknow = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ServerError_HaveSameName", Value=2)]
      ServerError_HaveSameName = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ServerError_NotFoundMapNameWhenDel", Value=3)]
      ServerError_NotFoundMapNameWhenDel = 3
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"MapType")]
    public enum MapType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"OfficeMap", Value=1)]
      OfficeMap = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ImcompleteMap", Value=2)]
      ImcompleteMap = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"CompleteMap", Value=3)]
      CompleteMap = 3
    }
  
}