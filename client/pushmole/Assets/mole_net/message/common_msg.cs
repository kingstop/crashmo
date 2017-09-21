//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

// Generated from: common.proto
namespace message
{
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"int32array")]
  public partial class int32array : global::ProtoBuf.IExtensible
  {
    public int32array() {}
    
    private readonly global::System.Collections.Generic.List<int> _data = new global::System.Collections.Generic.List<int>();
    [global::ProtoBuf.ProtoMember(1, Name=@"data", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<int> data
    {
      get { return _data; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CrashmoMapBaseData")]
  public partial class CrashmoMapBaseData : global::ProtoBuf.IExtensible
  {
    public CrashmoMapBaseData() {}
    
    private int _width;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"width", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int width
    {
      get { return _width; }
      set { _width = value; }
    }
    private int _height;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"height", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int height
    {
      get { return _height; }
      set { _height = value; }
    }
    private readonly global::System.Collections.Generic.List<message.int32array> _map_data = new global::System.Collections.Generic.List<message.int32array>();
    [global::ProtoBuf.ProtoMember(3, Name=@"map_data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.int32array> map_data
    {
      get { return _map_data; }
    }
  
    private ulong _map_index;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"map_index", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong map_index
    {
      get { return _map_index; }
      set { _map_index = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CrashMapData")]
  public partial class CrashMapData : global::ProtoBuf.IExtensible
  {
    public CrashMapData() {}
    
    private message.CrashmoMapBaseData _Data;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"Data", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public message.CrashmoMapBaseData Data
    {
      get { return _Data; }
      set { _Data = value; }
    }
    private string _MapName;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"MapName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string MapName
    {
      get { return _MapName; }
      set { _MapName = value; }
    }
    private string _CreaterName;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"CreaterName", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string CreaterName
    {
      get { return _CreaterName; }
      set { _CreaterName = value; }
    }
    private int _Section;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"Section", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Section
    {
      get { return _Section; }
      set { _Section = value; }
    }
    private int _Chapter;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"Chapter", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int Chapter
    {
      get { return _Chapter; }
      set { _Chapter = value; }
    }
    private ulong _create_time;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"create_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong create_time
    {
      get { return _create_time; }
      set { _create_time = value; }
    }
    private int _gold;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"gold", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int gold
    {
      get { return _gold; }
      set { _gold = value; }
    }
    private readonly global::System.Collections.Generic.List<message.RankMapBlogEntry> _map_blog = new global::System.Collections.Generic.List<message.RankMapBlogEntry>();
    [global::ProtoBuf.ProtoMember(8, Name=@"map_blog", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.RankMapBlogEntry> map_blog
    {
      get { return _map_blog; }
    }
  
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TaskInfo")]
  public partial class TaskInfo : global::ProtoBuf.IExtensible
  {
    public TaskInfo() {}
    
    private int _task_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"task_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int task_id
    {
      get { return _task_id; }
      set { _task_id = value; }
    }
    private int _argu_1;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"argu_1", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int argu_1
    {
      get { return _argu_1; }
      set { _argu_1 = value; }
    }
    private int _argu_2;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"argu_2", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int argu_2
    {
      get { return _argu_2; }
      set { _argu_2 = value; }
    }
    private int _argu_3;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"argu_3", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int argu_3
    {
      get { return _argu_3; }
      set { _argu_3 = value; }
    }
    private string _describe;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"describe", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string describe
    {
      get { return _describe; }
      set { _describe = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"RankMapBlogEntry")]
  public partial class RankMapBlogEntry : global::ProtoBuf.IExtensible
  {
    public RankMapBlogEntry() {}
    
    private ulong _acc;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"acc", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong acc
    {
      get { return _acc; }
      set { _acc = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _sugges_;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"sugges_", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string sugges_
    {
      get { return _sugges_; }
      set { _sugges_ = value; }
    }
    private int _time;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int time
    {
      get { return _time; }
      set { _time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CrashPlayerPublishMap")]
  public partial class CrashPlayerPublishMap : global::ProtoBuf.IExtensible
  {
    public CrashPlayerPublishMap() {}
    
    private message.CrashMapData _crashmap;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"crashmap", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public message.CrashMapData crashmap
    {
      get { return _crashmap; }
      set { _crashmap = value; }
    }
    private ulong _publish_time;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"publish_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong publish_time
    {
      get { return _publish_time; }
      set { _publish_time = value; }
    }
    private int _challenge_times;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"challenge_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int challenge_times
    {
      get { return _challenge_times; }
      set { _challenge_times = value; }
    }
    private int _failed_of_challenge_times;
    [global::ProtoBuf.ProtoMember(4, IsRequired = true, Name=@"failed_of_challenge_times", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int failed_of_challenge_times
    {
      get { return _failed_of_challenge_times; }
      set { _failed_of_challenge_times = value; }
    }
    private int _map_rank;
    [global::ProtoBuf.ProtoMember(5, IsRequired = true, Name=@"map_rank", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_rank
    {
      get { return _map_rank; }
      set { _map_rank = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"intPair")]
  public partial class intPair : global::ProtoBuf.IExtensible
  {
    public intPair() {}
    
    private int _number_1;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"number_1", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int number_1
    {
      get { return _number_1; }
      set { _number_1 = value; }
    }
    private int _number_2;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"number_2", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int number_2
    {
      get { return _number_2; }
      set { _number_2 = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"CrashPlayerInfo")]
  public partial class CrashPlayerInfo : global::ProtoBuf.IExtensible
  {
    public CrashPlayerInfo() {}
    
    private ulong _account;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"account", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong account
    {
      get { return _account; }
      set { _account = value; }
    }
    private readonly global::System.Collections.Generic.List<message.intPair> _passed_record = new global::System.Collections.Generic.List<message.intPair>();
    [global::ProtoBuf.ProtoMember(2, Name=@"passed_record", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.intPair> passed_record
    {
      get { return _passed_record; }
    }
  
    private string _name;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private readonly global::System.Collections.Generic.List<ulong> _IncompleteMap = new global::System.Collections.Generic.List<ulong>();
    [global::ProtoBuf.ProtoMember(4, Name=@"IncompleteMap", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<ulong> IncompleteMap
    {
      get { return _IncompleteMap; }
    }
  
    private readonly global::System.Collections.Generic.List<ulong> _CompleteMap = new global::System.Collections.Generic.List<ulong>();
    [global::ProtoBuf.ProtoMember(5, Name=@"CompleteMap", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public global::System.Collections.Generic.List<ulong> CompleteMap
    {
      get { return _CompleteMap; }
    }
  
    private bool _isadmin;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"isadmin", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public bool isadmin
    {
      get { return _isadmin; }
      set { _isadmin = value; }
    }
    private readonly global::System.Collections.Generic.List<message.intPair> _resources = new global::System.Collections.Generic.List<message.intPair>();
    [global::ProtoBuf.ProtoMember(7, Name=@"resources", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.intPair> resources
    {
      get { return _resources; }
    }
  
    private int _map_width;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"map_width", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_width
    {
      get { return _map_width; }
      set { _map_width = value; }
    }
    private int _map_height;
    [global::ProtoBuf.ProtoMember(9, IsRequired = true, Name=@"map_height", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_height
    {
      get { return _map_height; }
      set { _map_height = value; }
    }
    private int _map_count;
    [global::ProtoBuf.ProtoMember(10, IsRequired = true, Name=@"map_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int map_count
    {
      get { return _map_count; }
      set { _map_count = value; }
    }
    private int _gold;
    [global::ProtoBuf.ProtoMember(11, IsRequired = true, Name=@"gold", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int gold
    {
      get { return _gold; }
      set { _gold = value; }
    }
    private readonly global::System.Collections.Generic.List<message.TaskInfo> _current_task = new global::System.Collections.Generic.List<message.TaskInfo>();
    [global::ProtoBuf.ProtoMember(12, Name=@"current_task", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.TaskInfo> current_task
    {
      get { return _current_task; }
    }
  
    private int _complete_task_count;
    [global::ProtoBuf.ProtoMember(13, IsRequired = true, Name=@"complete_task_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int complete_task_count
    {
      get { return _complete_task_count; }
      set { _complete_task_count = value; }
    }
    private int _jewel;
    [global::ProtoBuf.ProtoMember(14, IsRequired = true, Name=@"jewel", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int jewel
    {
      get { return _jewel; }
      set { _jewel = value; }
    }
    private ulong _last_accept_task_time;
    [global::ProtoBuf.ProtoMember(15, IsRequired = true, Name=@"last_accept_task_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong last_accept_task_time
    {
      get { return _last_accept_task_time; }
      set { _last_accept_task_time = value; }
    }
    private ulong _last_publish_map_time;
    [global::ProtoBuf.ProtoMember(16, IsRequired = true, Name=@"last_publish_map_time", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public ulong last_publish_map_time
    {
      get { return _last_publish_map_time; }
      set { _last_publish_map_time = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TaskConditionTypeConfig")]
  public partial class TaskConditionTypeConfig : global::ProtoBuf.IExtensible
  {
    public TaskConditionTypeConfig() {}
    
    private message.ConditionType _condition;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"condition", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.ConditionType condition
    {
      get { return _condition; }
      set { _condition = value; }
    }
    private int _argu_1;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"argu_1", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int argu_1
    {
      get { return _argu_1; }
      set { _argu_1 = value; }
    }
    private int _argu_2;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"argu_2", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int argu_2
    {
      get { return _argu_2; }
      set { _argu_2 = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TaskRewardConfig")]
  public partial class TaskRewardConfig : global::ProtoBuf.IExtensible
  {
    public TaskRewardConfig() {}
    
    private message.ResourceType _resource_type;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"resource_type", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public message.ResourceType resource_type
    {
      get { return _resource_type; }
      set { _resource_type = value; }
    }
    private int _count;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int count
    {
      get { return _count; }
      set { _count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
  }
  
  [global::System.Serializable, global::ProtoBuf.ProtoContract(Name=@"TaskInfoConfig")]
  public partial class TaskInfoConfig : global::ProtoBuf.IExtensible
  {
    public TaskInfoConfig() {}
    
    private int _task_id;
    [global::ProtoBuf.ProtoMember(1, IsRequired = true, Name=@"task_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int task_id
    {
      get { return _task_id; }
      set { _task_id = value; }
    }
    private string _name;
    [global::ProtoBuf.ProtoMember(2, IsRequired = true, Name=@"name", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string name
    {
      get { return _name; }
      set { _name = value; }
    }
    private string _describe;
    [global::ProtoBuf.ProtoMember(3, IsRequired = true, Name=@"describe", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public string describe
    {
      get { return _describe; }
      set { _describe = value; }
    }
    private readonly global::System.Collections.Generic.List<message.TaskConditionTypeConfig> _conditions = new global::System.Collections.Generic.List<message.TaskConditionTypeConfig>();
    [global::ProtoBuf.ProtoMember(4, Name=@"conditions", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.TaskConditionTypeConfig> conditions
    {
      get { return _conditions; }
    }
  
    private readonly global::System.Collections.Generic.List<message.TaskRewardConfig> _rewards = new global::System.Collections.Generic.List<message.TaskRewardConfig>();
    [global::ProtoBuf.ProtoMember(5, Name=@"rewards", DataFormat = global::ProtoBuf.DataFormat.Default)]
    public global::System.Collections.Generic.List<message.TaskRewardConfig> rewards
    {
      get { return _rewards; }
    }
  
    private int _required_pass_chapter_id;
    [global::ProtoBuf.ProtoMember(6, IsRequired = true, Name=@"required_pass_chapter_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int required_pass_chapter_id
    {
      get { return _required_pass_chapter_id; }
      set { _required_pass_chapter_id = value; }
    }
    private int _required_pass_section_id;
    [global::ProtoBuf.ProtoMember(7, IsRequired = true, Name=@"required_pass_section_id", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int required_pass_section_id
    {
      get { return _required_pass_section_id; }
      set { _required_pass_section_id = value; }
    }
    private int _required_complete_task_count;
    [global::ProtoBuf.ProtoMember(8, IsRequired = true, Name=@"required_complete_task_count", DataFormat = global::ProtoBuf.DataFormat.TwosComplement)]
    public int required_complete_task_count
    {
      get { return _required_complete_task_count; }
      set { _required_complete_task_count = value; }
    }
    private global::ProtoBuf.IExtension extensionObject;
    global::ProtoBuf.IExtension global::ProtoBuf.IExtensible.GetExtensionObject(bool createIfMissing)
      { return global::ProtoBuf.Extensible.GetExtensionObject(ref extensionObject, createIfMissing); }
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
  
    [global::ProtoBuf.ProtoContract(Name=@"ConditionType")]
    public enum ConditionType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_NULL", Value=0)]
      ConditionType_NULL = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_PassOfficilGame", Value=1)]
      ConditionType_PassOfficilGame = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_LimitedTime", Value=2)]
      ConditionType_LimitedTime = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_LimitedStep", Value=3)]
      ConditionType_LimitedStep = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_PassUserGame", Value=4)]
      ConditionType_PassUserGame = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ConditionType_Max", Value=5)]
      ConditionType_Max = 5
    }
  
    [global::ProtoBuf.ProtoContract(Name=@"ResourceType")]
    public enum ResourceType
    {
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_NULL", Value=0)]
      ResourceType_NULL = 0,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_0", Value=1)]
      ResourceType_0 = 1,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_1", Value=2)]
      ResourceType_1 = 2,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_2", Value=3)]
      ResourceType_2 = 3,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_3", Value=4)]
      ResourceType_3 = 4,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_4", Value=5)]
      ResourceType_4 = 5,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_5", Value=6)]
      ResourceType_5 = 6,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_6", Value=7)]
      ResourceType_6 = 7,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_7", Value=8)]
      ResourceType_7 = 8,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_gold", Value=9)]
      ResourceType_gold = 9,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_jewel", Value=10)]
      ResourceType_jewel = 10,
            
      [global::ProtoBuf.ProtoEnum(Name=@"ResourceType_Max", Value=11)]
      ResourceType_Max = 11
    }
  
}