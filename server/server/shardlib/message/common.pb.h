// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: common.proto

#ifndef PROTOBUF_common_2eproto__INCLUDED
#define PROTOBUF_common_2eproto__INCLUDED

#include <string>

#include <google/protobuf/stubs/common.h>

#if GOOGLE_PROTOBUF_VERSION < 2005000
#error This file was generated by a newer version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please update
#error your headers.
#endif
#if 2005000 < GOOGLE_PROTOBUF_MIN_PROTOC_VERSION
#error This file was generated by an older version of protoc which is
#error incompatible with your Protocol Buffer headers.  Please
#error regenerate this file with a newer version of protoc.
#endif

#include <google/protobuf/generated_message_util.h>
#include <google/protobuf/message.h>
#include <google/protobuf/repeated_field.h>
#include <google/protobuf/extension_set.h>
#include <google/protobuf/generated_enum_reflection.h>
#include <google/protobuf/unknown_field_set.h>
// @@protoc_insertion_point(includes)

// Internal implementation detail -- do not call these.
void  protobuf_AddDesc_common_2eproto();
void protobuf_AssignDesc_common_2eproto();
void protobuf_ShutdownFile_common_2eproto();

class CharInfo;
class MapCharInfo;

enum enCharState {
  CharState_Move = 0,
  CharState_Stop = 1
};
bool enCharState_IsValid(int value);
const enCharState enCharState_MIN = CharState_Move;
const enCharState enCharState_MAX = CharState_Stop;
const int enCharState_ARRAYSIZE = enCharState_MAX + 1;

const ::google::protobuf::EnumDescriptor* enCharState_descriptor();
inline const ::std::string& enCharState_Name(enCharState value) {
  return ::google::protobuf::internal::NameOfEnum(
    enCharState_descriptor(), value);
}
inline bool enCharState_Parse(
    const ::std::string& name, enCharState* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enCharState>(
    enCharState_descriptor(), name, value);
}
enum enumGetCharResult {
  Response_Success = 0,
  Response_SystemError = 1,
  Response_NewChar = 2
};
bool enumGetCharResult_IsValid(int value);
const enumGetCharResult enumGetCharResult_MIN = Response_Success;
const enumGetCharResult enumGetCharResult_MAX = Response_NewChar;
const int enumGetCharResult_ARRAYSIZE = enumGetCharResult_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumGetCharResult_descriptor();
inline const ::std::string& enumGetCharResult_Name(enumGetCharResult value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumGetCharResult_descriptor(), value);
}
inline bool enumGetCharResult_Parse(
    const ::std::string& name, enumGetCharResult* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumGetCharResult>(
    enumGetCharResult_descriptor(), name, value);
}
enum CommonConst {
  MAX_NAMESIZE = 32
};
bool CommonConst_IsValid(int value);
const CommonConst CommonConst_MIN = MAX_NAMESIZE;
const CommonConst CommonConst_MAX = MAX_NAMESIZE;
const int CommonConst_ARRAYSIZE = CommonConst_MAX + 1;

const ::google::protobuf::EnumDescriptor* CommonConst_descriptor();
inline const ::std::string& CommonConst_Name(CommonConst value) {
  return ::google::protobuf::internal::NameOfEnum(
    CommonConst_descriptor(), value);
}
inline bool CommonConst_Parse(
    const ::std::string& name, CommonConst* value) {
  return ::google::protobuf::internal::ParseNamedEnum<CommonConst>(
    CommonConst_descriptor(), name, value);
}
enum SCREEN_CONST {
  SCREEN_WIDTH = 13,
  SCREEN_HEIGHT = 19
};
bool SCREEN_CONST_IsValid(int value);
const SCREEN_CONST SCREEN_CONST_MIN = SCREEN_WIDTH;
const SCREEN_CONST SCREEN_CONST_MAX = SCREEN_HEIGHT;
const int SCREEN_CONST_ARRAYSIZE = SCREEN_CONST_MAX + 1;

const ::google::protobuf::EnumDescriptor* SCREEN_CONST_descriptor();
inline const ::std::string& SCREEN_CONST_Name(SCREEN_CONST value) {
  return ::google::protobuf::internal::NameOfEnum(
    SCREEN_CONST_descriptor(), value);
}
inline bool SCREEN_CONST_Parse(
    const ::std::string& name, SCREEN_CONST* value) {
  return ::google::protobuf::internal::ParseNamedEnum<SCREEN_CONST>(
    SCREEN_CONST_descriptor(), name, value);
}
enum DIR {
  DIR_UP = 0,
  DIR_UPRIGHT = 1,
  DIR_RIGHT = 2,
  DIR_DOWNRIGHT = 3,
  DIR_DOWN = 4,
  DIR_DOWNLEFT = 5,
  DIR_LEFT = 6,
  DIR_UPLEFT = 7,
  DIR_ERROR = 8
};
bool DIR_IsValid(int value);
const DIR DIR_MIN = DIR_UP;
const DIR DIR_MAX = DIR_ERROR;
const int DIR_ARRAYSIZE = DIR_MAX + 1;

const ::google::protobuf::EnumDescriptor* DIR_descriptor();
inline const ::std::string& DIR_Name(DIR value) {
  return ::google::protobuf::internal::NameOfEnum(
    DIR_descriptor(), value);
}
inline bool DIR_Parse(
    const ::std::string& name, DIR* value) {
  return ::google::protobuf::internal::ParseNamedEnum<DIR>(
    DIR_descriptor(), name, value);
}
enum enumCharType {
  enumChar_Person = 1,
  enumChar_Company = 2
};
bool enumCharType_IsValid(int value);
const enumCharType enumCharType_MIN = enumChar_Person;
const enumCharType enumCharType_MAX = enumChar_Company;
const int enumCharType_ARRAYSIZE = enumCharType_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumCharType_descriptor();
inline const ::std::string& enumCharType_Name(enumCharType value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumCharType_descriptor(), value);
}
inline bool enumCharType_Parse(
    const ::std::string& name, enumCharType* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumCharType>(
    enumCharType_descriptor(), name, value);
}
enum enumGroupType {
  enumGroup_Friend = 1,
  enumGroup_Shop = 2,
  enumGroup_Recruit = 3,
  enumGroup_Blacklist = 4,
  enumGroup_ToBlacklist = 5
};
bool enumGroupType_IsValid(int value);
const enumGroupType enumGroupType_MIN = enumGroup_Friend;
const enumGroupType enumGroupType_MAX = enumGroup_ToBlacklist;
const int enumGroupType_ARRAYSIZE = enumGroupType_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumGroupType_descriptor();
inline const ::std::string& enumGroupType_Name(enumGroupType value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumGroupType_descriptor(), value);
}
inline bool enumGroupType_Parse(
    const ::std::string& name, enumGroupType* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumGroupType>(
    enumGroupType_descriptor(), name, value);
}
enum enumWordmsgType {
  enumWordmsg_ComanyNeed = 1,
  enumWordmsg_Sys = 2
};
bool enumWordmsgType_IsValid(int value);
const enumWordmsgType enumWordmsgType_MIN = enumWordmsg_ComanyNeed;
const enumWordmsgType enumWordmsgType_MAX = enumWordmsg_Sys;
const int enumWordmsgType_ARRAYSIZE = enumWordmsgType_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumWordmsgType_descriptor();
inline const ::std::string& enumWordmsgType_Name(enumWordmsgType value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumWordmsgType_descriptor(), value);
}
inline bool enumWordmsgType_Parse(
    const ::std::string& name, enumWordmsgType* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumWordmsgType>(
    enumWordmsgType_descriptor(), name, value);
}
enum enumAssessType {
  enumAssess_Person_Resumeinfo = 1,
  enumAssess_Person_Friendinfo = 2,
  enumAssess_Person_Shoppinginfo = 3,
  enumAssess_Company_Companyinfo = 4,
  enumAssess_Company_Proresume = 5
};
bool enumAssessType_IsValid(int value);
const enumAssessType enumAssessType_MIN = enumAssess_Person_Resumeinfo;
const enumAssessType enumAssessType_MAX = enumAssess_Company_Proresume;
const int enumAssessType_ARRAYSIZE = enumAssessType_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumAssessType_descriptor();
inline const ::std::string& enumAssessType_Name(enumAssessType value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumAssessType_descriptor(), value);
}
inline bool enumAssessType_Parse(
    const ::std::string& name, enumAssessType* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumAssessType>(
    enumAssessType_descriptor(), name, value);
}
enum enumCompanyTalentType {
  enumCompanyTalent_Recycle = 3,
  enumCompanyTalent_Noprocess = 1,
  enumCompanyTalent_Talentpool = 2
};
bool enumCompanyTalentType_IsValid(int value);
const enumCompanyTalentType enumCompanyTalentType_MIN = enumCompanyTalent_Noprocess;
const enumCompanyTalentType enumCompanyTalentType_MAX = enumCompanyTalent_Recycle;
const int enumCompanyTalentType_ARRAYSIZE = enumCompanyTalentType_MAX + 1;

const ::google::protobuf::EnumDescriptor* enumCompanyTalentType_descriptor();
inline const ::std::string& enumCompanyTalentType_Name(enumCompanyTalentType value) {
  return ::google::protobuf::internal::NameOfEnum(
    enumCompanyTalentType_descriptor(), value);
}
inline bool enumCompanyTalentType_Parse(
    const ::std::string& name, enumCompanyTalentType* value) {
  return ::google::protobuf::internal::ParseNamedEnum<enumCompanyTalentType>(
    enumCompanyTalentType_descriptor(), name, value);
}
// ===================================================================

class CharInfo : public ::google::protobuf::Message {
 public:
  CharInfo();
  virtual ~CharInfo();

  CharInfo(const CharInfo& from);

  inline CharInfo& operator=(const CharInfo& from) {
    CopyFrom(from);
    return *this;
  }

  inline const ::google::protobuf::UnknownFieldSet& unknown_fields() const {
    return _unknown_fields_;
  }

  inline ::google::protobuf::UnknownFieldSet* mutable_unknown_fields() {
    return &_unknown_fields_;
  }

  static const ::google::protobuf::Descriptor* descriptor();
  static const CharInfo& default_instance();

  void Swap(CharInfo* other);

  // implements Message ----------------------------------------------

  CharInfo* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const CharInfo& from);
  void MergeFrom(const CharInfo& from);
  void Clear();
  bool IsInitialized() const;

  int ByteSize() const;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input);
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const;
  ::google::protobuf::uint8* SerializeWithCachedSizesToArray(::google::protobuf::uint8* output) const;
  int GetCachedSize() const { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const;
  public:

  ::google::protobuf::Metadata GetMetadata() const;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // optional uint32 charid = 1;
  inline bool has_charid() const;
  inline void clear_charid();
  static const int kCharidFieldNumber = 1;
  inline ::google::protobuf::uint32 charid() const;
  inline void set_charid(::google::protobuf::uint32 value);

  // optional uint32 user_account = 2;
  inline bool has_user_account() const;
  inline void clear_user_account();
  static const int kUserAccountFieldNumber = 2;
  inline ::google::protobuf::uint32 user_account() const;
  inline void set_user_account(::google::protobuf::uint32 value);

  // optional string name = 3;
  inline bool has_name() const;
  inline void clear_name();
  static const int kNameFieldNumber = 3;
  inline const ::std::string& name() const;
  inline void set_name(const ::std::string& value);
  inline void set_name(const char* value);
  inline void set_name(const char* value, size_t size);
  inline ::std::string* mutable_name();
  inline ::std::string* release_name();
  inline void set_allocated_name(::std::string* name);

  // optional uint32 head = 4;
  inline bool has_head() const;
  inline void clear_head();
  static const int kHeadFieldNumber = 4;
  inline ::google::protobuf::uint32 head() const;
  inline void set_head(::google::protobuf::uint32 value);

  // optional uint32 sex = 5;
  inline bool has_sex() const;
  inline void clear_sex();
  static const int kSexFieldNumber = 5;
  inline ::google::protobuf::uint32 sex() const;
  inline void set_sex(::google::protobuf::uint32 value);

  // optional uint32 colthid = 6;
  inline bool has_colthid() const;
  inline void clear_colthid();
  static const int kColthidFieldNumber = 6;
  inline ::google::protobuf::uint32 colthid() const;
  inline void set_colthid(::google::protobuf::uint32 value);

  // optional .enumCharType chartype = 7;
  inline bool has_chartype() const;
  inline void clear_chartype();
  static const int kChartypeFieldNumber = 7;
  inline ::enumCharType chartype() const;
  inline void set_chartype(::enumCharType value);

  // @@protoc_insertion_point(class_scope:CharInfo)
 private:
  inline void set_has_charid();
  inline void clear_has_charid();
  inline void set_has_user_account();
  inline void clear_has_user_account();
  inline void set_has_name();
  inline void clear_has_name();
  inline void set_has_head();
  inline void clear_has_head();
  inline void set_has_sex();
  inline void clear_has_sex();
  inline void set_has_colthid();
  inline void clear_has_colthid();
  inline void set_has_chartype();
  inline void clear_has_chartype();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 charid_;
  ::google::protobuf::uint32 user_account_;
  ::std::string* name_;
  ::google::protobuf::uint32 head_;
  ::google::protobuf::uint32 sex_;
  ::google::protobuf::uint32 colthid_;
  int chartype_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(7 + 31) / 32];

  friend void  protobuf_AddDesc_common_2eproto();
  friend void protobuf_AssignDesc_common_2eproto();
  friend void protobuf_ShutdownFile_common_2eproto();

  void InitAsDefaultInstance();
  static CharInfo* default_instance_;
};
// -------------------------------------------------------------------

class MapCharInfo : public ::google::protobuf::Message {
 public:
  MapCharInfo();
  virtual ~MapCharInfo();

  MapCharInfo(const MapCharInfo& from);

  inline MapCharInfo& operator=(const MapCharInfo& from) {
    CopyFrom(from);
    return *this;
  }

  inline const ::google::protobuf::UnknownFieldSet& unknown_fields() const {
    return _unknown_fields_;
  }

  inline ::google::protobuf::UnknownFieldSet* mutable_unknown_fields() {
    return &_unknown_fields_;
  }

  static const ::google::protobuf::Descriptor* descriptor();
  static const MapCharInfo& default_instance();

  void Swap(MapCharInfo* other);

  // implements Message ----------------------------------------------

  MapCharInfo* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MapCharInfo& from);
  void MergeFrom(const MapCharInfo& from);
  void Clear();
  bool IsInitialized() const;

  int ByteSize() const;
  bool MergePartialFromCodedStream(
      ::google::protobuf::io::CodedInputStream* input);
  void SerializeWithCachedSizes(
      ::google::protobuf::io::CodedOutputStream* output) const;
  ::google::protobuf::uint8* SerializeWithCachedSizesToArray(::google::protobuf::uint8* output) const;
  int GetCachedSize() const { return _cached_size_; }
  private:
  void SharedCtor();
  void SharedDtor();
  void SetCachedSize(int size) const;
  public:

  ::google::protobuf::Metadata GetMetadata() const;

  // nested types ----------------------------------------------------

  // accessors -------------------------------------------------------

  // optional .CharInfo charinfo = 1;
  inline bool has_charinfo() const;
  inline void clear_charinfo();
  static const int kCharinfoFieldNumber = 1;
  inline const ::CharInfo& charinfo() const;
  inline ::CharInfo* mutable_charinfo();
  inline ::CharInfo* release_charinfo();
  inline void set_allocated_charinfo(::CharInfo* charinfo);

  // optional float x = 2;
  inline bool has_x() const;
  inline void clear_x();
  static const int kXFieldNumber = 2;
  inline float x() const;
  inline void set_x(float value);

  // optional float y = 3;
  inline bool has_y() const;
  inline void clear_y();
  static const int kYFieldNumber = 3;
  inline float y() const;
  inline void set_y(float value);

  // optional float dir = 4;
  inline bool has_dir() const;
  inline void clear_dir();
  static const int kDirFieldNumber = 4;
  inline float dir() const;
  inline void set_dir(float value);

  // optional .enCharState state = 5;
  inline bool has_state() const;
  inline void clear_state();
  static const int kStateFieldNumber = 5;
  inline ::enCharState state() const;
  inline void set_state(::enCharState value);

  // @@protoc_insertion_point(class_scope:MapCharInfo)
 private:
  inline void set_has_charinfo();
  inline void clear_has_charinfo();
  inline void set_has_x();
  inline void clear_has_x();
  inline void set_has_y();
  inline void clear_has_y();
  inline void set_has_dir();
  inline void clear_has_dir();
  inline void set_has_state();
  inline void clear_has_state();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::CharInfo* charinfo_;
  float x_;
  float y_;
  float dir_;
  int state_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(5 + 31) / 32];

  friend void  protobuf_AddDesc_common_2eproto();
  friend void protobuf_AssignDesc_common_2eproto();
  friend void protobuf_ShutdownFile_common_2eproto();

  void InitAsDefaultInstance();
  static MapCharInfo* default_instance_;
};
// ===================================================================


// ===================================================================

// CharInfo

// optional uint32 charid = 1;
inline bool CharInfo::has_charid() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void CharInfo::set_has_charid() {
  _has_bits_[0] |= 0x00000001u;
}
inline void CharInfo::clear_has_charid() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void CharInfo::clear_charid() {
  charid_ = 0u;
  clear_has_charid();
}
inline ::google::protobuf::uint32 CharInfo::charid() const {
  return charid_;
}
inline void CharInfo::set_charid(::google::protobuf::uint32 value) {
  set_has_charid();
  charid_ = value;
}

// optional uint32 user_account = 2;
inline bool CharInfo::has_user_account() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void CharInfo::set_has_user_account() {
  _has_bits_[0] |= 0x00000002u;
}
inline void CharInfo::clear_has_user_account() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void CharInfo::clear_user_account() {
  user_account_ = 0u;
  clear_has_user_account();
}
inline ::google::protobuf::uint32 CharInfo::user_account() const {
  return user_account_;
}
inline void CharInfo::set_user_account(::google::protobuf::uint32 value) {
  set_has_user_account();
  user_account_ = value;
}

// optional string name = 3;
inline bool CharInfo::has_name() const {
  return (_has_bits_[0] & 0x00000004u) != 0;
}
inline void CharInfo::set_has_name() {
  _has_bits_[0] |= 0x00000004u;
}
inline void CharInfo::clear_has_name() {
  _has_bits_[0] &= ~0x00000004u;
}
inline void CharInfo::clear_name() {
  if (name_ != &::google::protobuf::internal::kEmptyString) {
    name_->clear();
  }
  clear_has_name();
}
inline const ::std::string& CharInfo::name() const {
  return *name_;
}
inline void CharInfo::set_name(const ::std::string& value) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(value);
}
inline void CharInfo::set_name(const char* value) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(value);
}
inline void CharInfo::set_name(const char* value, size_t size) {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  name_->assign(reinterpret_cast<const char*>(value), size);
}
inline ::std::string* CharInfo::mutable_name() {
  set_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    name_ = new ::std::string;
  }
  return name_;
}
inline ::std::string* CharInfo::release_name() {
  clear_has_name();
  if (name_ == &::google::protobuf::internal::kEmptyString) {
    return NULL;
  } else {
    ::std::string* temp = name_;
    name_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
    return temp;
  }
}
inline void CharInfo::set_allocated_name(::std::string* name) {
  if (name_ != &::google::protobuf::internal::kEmptyString) {
    delete name_;
  }
  if (name) {
    set_has_name();
    name_ = name;
  } else {
    clear_has_name();
    name_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  }
}

// optional uint32 head = 4;
inline bool CharInfo::has_head() const {
  return (_has_bits_[0] & 0x00000008u) != 0;
}
inline void CharInfo::set_has_head() {
  _has_bits_[0] |= 0x00000008u;
}
inline void CharInfo::clear_has_head() {
  _has_bits_[0] &= ~0x00000008u;
}
inline void CharInfo::clear_head() {
  head_ = 0u;
  clear_has_head();
}
inline ::google::protobuf::uint32 CharInfo::head() const {
  return head_;
}
inline void CharInfo::set_head(::google::protobuf::uint32 value) {
  set_has_head();
  head_ = value;
}

// optional uint32 sex = 5;
inline bool CharInfo::has_sex() const {
  return (_has_bits_[0] & 0x00000010u) != 0;
}
inline void CharInfo::set_has_sex() {
  _has_bits_[0] |= 0x00000010u;
}
inline void CharInfo::clear_has_sex() {
  _has_bits_[0] &= ~0x00000010u;
}
inline void CharInfo::clear_sex() {
  sex_ = 0u;
  clear_has_sex();
}
inline ::google::protobuf::uint32 CharInfo::sex() const {
  return sex_;
}
inline void CharInfo::set_sex(::google::protobuf::uint32 value) {
  set_has_sex();
  sex_ = value;
}

// optional uint32 colthid = 6;
inline bool CharInfo::has_colthid() const {
  return (_has_bits_[0] & 0x00000020u) != 0;
}
inline void CharInfo::set_has_colthid() {
  _has_bits_[0] |= 0x00000020u;
}
inline void CharInfo::clear_has_colthid() {
  _has_bits_[0] &= ~0x00000020u;
}
inline void CharInfo::clear_colthid() {
  colthid_ = 0u;
  clear_has_colthid();
}
inline ::google::protobuf::uint32 CharInfo::colthid() const {
  return colthid_;
}
inline void CharInfo::set_colthid(::google::protobuf::uint32 value) {
  set_has_colthid();
  colthid_ = value;
}

// optional .enumCharType chartype = 7;
inline bool CharInfo::has_chartype() const {
  return (_has_bits_[0] & 0x00000040u) != 0;
}
inline void CharInfo::set_has_chartype() {
  _has_bits_[0] |= 0x00000040u;
}
inline void CharInfo::clear_has_chartype() {
  _has_bits_[0] &= ~0x00000040u;
}
inline void CharInfo::clear_chartype() {
  chartype_ = 1;
  clear_has_chartype();
}
inline ::enumCharType CharInfo::chartype() const {
  return static_cast< ::enumCharType >(chartype_);
}
inline void CharInfo::set_chartype(::enumCharType value) {
  assert(::enumCharType_IsValid(value));
  set_has_chartype();
  chartype_ = value;
}

// -------------------------------------------------------------------

// MapCharInfo

// optional .CharInfo charinfo = 1;
inline bool MapCharInfo::has_charinfo() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MapCharInfo::set_has_charinfo() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MapCharInfo::clear_has_charinfo() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MapCharInfo::clear_charinfo() {
  if (charinfo_ != NULL) charinfo_->::CharInfo::Clear();
  clear_has_charinfo();
}
inline const ::CharInfo& MapCharInfo::charinfo() const {
  return charinfo_ != NULL ? *charinfo_ : *default_instance_->charinfo_;
}
inline ::CharInfo* MapCharInfo::mutable_charinfo() {
  set_has_charinfo();
  if (charinfo_ == NULL) charinfo_ = new ::CharInfo;
  return charinfo_;
}
inline ::CharInfo* MapCharInfo::release_charinfo() {
  clear_has_charinfo();
  ::CharInfo* temp = charinfo_;
  charinfo_ = NULL;
  return temp;
}
inline void MapCharInfo::set_allocated_charinfo(::CharInfo* charinfo) {
  delete charinfo_;
  charinfo_ = charinfo;
  if (charinfo) {
    set_has_charinfo();
  } else {
    clear_has_charinfo();
  }
}

// optional float x = 2;
inline bool MapCharInfo::has_x() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void MapCharInfo::set_has_x() {
  _has_bits_[0] |= 0x00000002u;
}
inline void MapCharInfo::clear_has_x() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void MapCharInfo::clear_x() {
  x_ = 0;
  clear_has_x();
}
inline float MapCharInfo::x() const {
  return x_;
}
inline void MapCharInfo::set_x(float value) {
  set_has_x();
  x_ = value;
}

// optional float y = 3;
inline bool MapCharInfo::has_y() const {
  return (_has_bits_[0] & 0x00000004u) != 0;
}
inline void MapCharInfo::set_has_y() {
  _has_bits_[0] |= 0x00000004u;
}
inline void MapCharInfo::clear_has_y() {
  _has_bits_[0] &= ~0x00000004u;
}
inline void MapCharInfo::clear_y() {
  y_ = 0;
  clear_has_y();
}
inline float MapCharInfo::y() const {
  return y_;
}
inline void MapCharInfo::set_y(float value) {
  set_has_y();
  y_ = value;
}

// optional float dir = 4;
inline bool MapCharInfo::has_dir() const {
  return (_has_bits_[0] & 0x00000008u) != 0;
}
inline void MapCharInfo::set_has_dir() {
  _has_bits_[0] |= 0x00000008u;
}
inline void MapCharInfo::clear_has_dir() {
  _has_bits_[0] &= ~0x00000008u;
}
inline void MapCharInfo::clear_dir() {
  dir_ = 0;
  clear_has_dir();
}
inline float MapCharInfo::dir() const {
  return dir_;
}
inline void MapCharInfo::set_dir(float value) {
  set_has_dir();
  dir_ = value;
}

// optional .enCharState state = 5;
inline bool MapCharInfo::has_state() const {
  return (_has_bits_[0] & 0x00000010u) != 0;
}
inline void MapCharInfo::set_has_state() {
  _has_bits_[0] |= 0x00000010u;
}
inline void MapCharInfo::clear_has_state() {
  _has_bits_[0] &= ~0x00000010u;
}
inline void MapCharInfo::clear_state() {
  state_ = 0;
  clear_has_state();
}
inline ::enCharState MapCharInfo::state() const {
  return static_cast< ::enCharState >(state_);
}
inline void MapCharInfo::set_state(::enCharState value) {
  assert(::enCharState_IsValid(value));
  set_has_state();
  state_ = value;
}


// @@protoc_insertion_point(namespace_scope)

#ifndef SWIG
namespace google {
namespace protobuf {

template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enCharState>() {
  return ::enCharState_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumGetCharResult>() {
  return ::enumGetCharResult_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::CommonConst>() {
  return ::CommonConst_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::SCREEN_CONST>() {
  return ::SCREEN_CONST_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::DIR>() {
  return ::DIR_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumCharType>() {
  return ::enumCharType_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumGroupType>() {
  return ::enumGroupType_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumWordmsgType>() {
  return ::enumWordmsgType_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumAssessType>() {
  return ::enumAssessType_descriptor();
}
template <>
inline const EnumDescriptor* GetEnumDescriptor< ::enumCompanyTalentType>() {
  return ::enumCompanyTalentType_descriptor();
}

}  // namespace google
}  // namespace protobuf
#endif  // SWIG

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_common_2eproto__INCLUDED
