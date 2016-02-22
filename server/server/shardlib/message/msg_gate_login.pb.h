// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: msg_gate_login.proto

#ifndef PROTOBUF_msg_5fgate_5flogin_2eproto__INCLUDED
#define PROTOBUF_msg_5fgate_5flogin_2eproto__INCLUDED

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
#include <google/protobuf/unknown_field_set.h>
#include "msgs2s.pb.h"
// @@protoc_insertion_point(includes)

namespace msgs2s {

// Internal implementation detail -- do not call these.
void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

class GTLGData;
class MsgLN2GTUserLogin;
class MsgLN2GTKickUser;
class MsgGT2LNOnlines;
class MsgGT2LNPrepar;
class MsgGTRegisterLG;

// ===================================================================

class GTLGData : public ::google::protobuf::Message {
 public:
  GTLGData();
  virtual ~GTLGData();

  GTLGData(const GTLGData& from);

  inline GTLGData& operator=(const GTLGData& from) {
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
  static const GTLGData& default_instance();

  void Swap(GTLGData* other);

  // implements Message ----------------------------------------------

  GTLGData* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const GTLGData& from);
  void MergeFrom(const GTLGData& from);
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

  // required uint32 account = 1;
  inline bool has_account() const;
  inline void clear_account();
  static const int kAccountFieldNumber = 1;
  inline ::google::protobuf::uint32 account() const;
  inline void set_account(::google::protobuf::uint32 value);

  // required uint32 transid = 2;
  inline bool has_transid() const;
  inline void clear_transid();
  static const int kTransidFieldNumber = 2;
  inline ::google::protobuf::uint32 transid() const;
  inline void set_transid(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:msgs2s.GTLGData)
 private:
  inline void set_has_account();
  inline void clear_has_account();
  inline void set_has_transid();
  inline void clear_has_transid();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 account_;
  ::google::protobuf::uint32 transid_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(2 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static GTLGData* default_instance_;
};
// -------------------------------------------------------------------

class MsgLN2GTUserLogin : public ::google::protobuf::Message {
 public:
  MsgLN2GTUserLogin();
  virtual ~MsgLN2GTUserLogin();

  MsgLN2GTUserLogin(const MsgLN2GTUserLogin& from);

  inline MsgLN2GTUserLogin& operator=(const MsgLN2GTUserLogin& from) {
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
  static const MsgLN2GTUserLogin& default_instance();

  void Swap(MsgLN2GTUserLogin* other);

  // implements Message ----------------------------------------------

  MsgLN2GTUserLogin* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgLN2GTUserLogin& from);
  void MergeFrom(const MsgLN2GTUserLogin& from);
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

  // required .msgs2s.GTLGData data = 1;
  inline bool has_data() const;
  inline void clear_data();
  static const int kDataFieldNumber = 1;
  inline const ::msgs2s::GTLGData& data() const;
  inline ::msgs2s::GTLGData* mutable_data();
  inline ::msgs2s::GTLGData* release_data();
  inline void set_allocated_data(::msgs2s::GTLGData* data);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgLN2GTUserLogin)
 private:
  inline void set_has_data();
  inline void clear_has_data();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::msgs2s::GTLGData* data_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(1 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static MsgLN2GTUserLogin* default_instance_;
};
// -------------------------------------------------------------------

class MsgLN2GTKickUser : public ::google::protobuf::Message {
 public:
  MsgLN2GTKickUser();
  virtual ~MsgLN2GTKickUser();

  MsgLN2GTKickUser(const MsgLN2GTKickUser& from);

  inline MsgLN2GTKickUser& operator=(const MsgLN2GTKickUser& from) {
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
  static const MsgLN2GTKickUser& default_instance();

  void Swap(MsgLN2GTKickUser* other);

  // implements Message ----------------------------------------------

  MsgLN2GTKickUser* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgLN2GTKickUser& from);
  void MergeFrom(const MsgLN2GTKickUser& from);
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

  // required .msgs2s.GTLGData data = 1;
  inline bool has_data() const;
  inline void clear_data();
  static const int kDataFieldNumber = 1;
  inline const ::msgs2s::GTLGData& data() const;
  inline ::msgs2s::GTLGData* mutable_data();
  inline ::msgs2s::GTLGData* release_data();
  inline void set_allocated_data(::msgs2s::GTLGData* data);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgLN2GTKickUser)
 private:
  inline void set_has_data();
  inline void clear_has_data();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::msgs2s::GTLGData* data_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(1 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static MsgLN2GTKickUser* default_instance_;
};
// -------------------------------------------------------------------

class MsgGT2LNOnlines : public ::google::protobuf::Message {
 public:
  MsgGT2LNOnlines();
  virtual ~MsgGT2LNOnlines();

  MsgGT2LNOnlines(const MsgGT2LNOnlines& from);

  inline MsgGT2LNOnlines& operator=(const MsgGT2LNOnlines& from) {
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
  static const MsgGT2LNOnlines& default_instance();

  void Swap(MsgGT2LNOnlines* other);

  // implements Message ----------------------------------------------

  MsgGT2LNOnlines* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgGT2LNOnlines& from);
  void MergeFrom(const MsgGT2LNOnlines& from);
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

  // required uint32 onlines = 1;
  inline bool has_onlines() const;
  inline void clear_onlines();
  static const int kOnlinesFieldNumber = 1;
  inline ::google::protobuf::uint32 onlines() const;
  inline void set_onlines(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgGT2LNOnlines)
 private:
  inline void set_has_onlines();
  inline void clear_has_onlines();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 onlines_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(1 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static MsgGT2LNOnlines* default_instance_;
};
// -------------------------------------------------------------------

class MsgGT2LNPrepar : public ::google::protobuf::Message {
 public:
  MsgGT2LNPrepar();
  virtual ~MsgGT2LNPrepar();

  MsgGT2LNPrepar(const MsgGT2LNPrepar& from);

  inline MsgGT2LNPrepar& operator=(const MsgGT2LNPrepar& from) {
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
  static const MsgGT2LNPrepar& default_instance();

  void Swap(MsgGT2LNPrepar* other);

  // implements Message ----------------------------------------------

  MsgGT2LNPrepar* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgGT2LNPrepar& from);
  void MergeFrom(const MsgGT2LNPrepar& from);
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

  // required uint32 account = 1;
  inline bool has_account() const;
  inline void clear_account();
  static const int kAccountFieldNumber = 1;
  inline ::google::protobuf::uint32 account() const;
  inline void set_account(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgGT2LNPrepar)
 private:
  inline void set_has_account();
  inline void clear_has_account();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 account_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(1 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static MsgGT2LNPrepar* default_instance_;
};
// -------------------------------------------------------------------

class MsgGTRegisterLG : public ::google::protobuf::Message {
 public:
  MsgGTRegisterLG();
  virtual ~MsgGTRegisterLG();

  MsgGTRegisterLG(const MsgGTRegisterLG& from);

  inline MsgGTRegisterLG& operator=(const MsgGTRegisterLG& from) {
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
  static const MsgGTRegisterLG& default_instance();

  void Swap(MsgGTRegisterLG* other);

  // implements Message ----------------------------------------------

  MsgGTRegisterLG* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgGTRegisterLG& from);
  void MergeFrom(const MsgGTRegisterLG& from);
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

  // required .msgs2s.MsgServerRegister GateInfo = 1;
  inline bool has_gateinfo() const;
  inline void clear_gateinfo();
  static const int kGateInfoFieldNumber = 1;
  inline const ::msgs2s::MsgServerRegister& gateinfo() const;
  inline ::msgs2s::MsgServerRegister* mutable_gateinfo();
  inline ::msgs2s::MsgServerRegister* release_gateinfo();
  inline void set_allocated_gateinfo(::msgs2s::MsgServerRegister* gateinfo);

  // required string ip = 2;
  inline bool has_ip() const;
  inline void clear_ip();
  static const int kIpFieldNumber = 2;
  inline const ::std::string& ip() const;
  inline void set_ip(const ::std::string& value);
  inline void set_ip(const char* value);
  inline void set_ip(const char* value, size_t size);
  inline ::std::string* mutable_ip();
  inline ::std::string* release_ip();
  inline void set_allocated_ip(::std::string* ip);

  // required uint32 port = 3;
  inline bool has_port() const;
  inline void clear_port();
  static const int kPortFieldNumber = 3;
  inline ::google::protobuf::uint32 port() const;
  inline void set_port(::google::protobuf::uint32 value);

  // repeated .msgs2s.GTLGData Accounts = 4;
  inline int accounts_size() const;
  inline void clear_accounts();
  static const int kAccountsFieldNumber = 4;
  inline const ::msgs2s::GTLGData& accounts(int index) const;
  inline ::msgs2s::GTLGData* mutable_accounts(int index);
  inline ::msgs2s::GTLGData* add_accounts();
  inline const ::google::protobuf::RepeatedPtrField< ::msgs2s::GTLGData >&
      accounts() const;
  inline ::google::protobuf::RepeatedPtrField< ::msgs2s::GTLGData >*
      mutable_accounts();

  // @@protoc_insertion_point(class_scope:msgs2s.MsgGTRegisterLG)
 private:
  inline void set_has_gateinfo();
  inline void clear_has_gateinfo();
  inline void set_has_ip();
  inline void clear_has_ip();
  inline void set_has_port();
  inline void clear_has_port();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::msgs2s::MsgServerRegister* gateinfo_;
  ::std::string* ip_;
  ::google::protobuf::RepeatedPtrField< ::msgs2s::GTLGData > accounts_;
  ::google::protobuf::uint32 port_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(4 + 31) / 32];

  friend void  protobuf_AddDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_AssignDesc_msg_5fgate_5flogin_2eproto();
  friend void protobuf_ShutdownFile_msg_5fgate_5flogin_2eproto();

  void InitAsDefaultInstance();
  static MsgGTRegisterLG* default_instance_;
};
// ===================================================================


// ===================================================================

// GTLGData

// required uint32 account = 1;
inline bool GTLGData::has_account() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void GTLGData::set_has_account() {
  _has_bits_[0] |= 0x00000001u;
}
inline void GTLGData::clear_has_account() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void GTLGData::clear_account() {
  account_ = 0u;
  clear_has_account();
}
inline ::google::protobuf::uint32 GTLGData::account() const {
  return account_;
}
inline void GTLGData::set_account(::google::protobuf::uint32 value) {
  set_has_account();
  account_ = value;
}

// required uint32 transid = 2;
inline bool GTLGData::has_transid() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void GTLGData::set_has_transid() {
  _has_bits_[0] |= 0x00000002u;
}
inline void GTLGData::clear_has_transid() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void GTLGData::clear_transid() {
  transid_ = 0u;
  clear_has_transid();
}
inline ::google::protobuf::uint32 GTLGData::transid() const {
  return transid_;
}
inline void GTLGData::set_transid(::google::protobuf::uint32 value) {
  set_has_transid();
  transid_ = value;
}

// -------------------------------------------------------------------

// MsgLN2GTUserLogin

// required .msgs2s.GTLGData data = 1;
inline bool MsgLN2GTUserLogin::has_data() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgLN2GTUserLogin::set_has_data() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgLN2GTUserLogin::clear_has_data() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgLN2GTUserLogin::clear_data() {
  if (data_ != NULL) data_->::msgs2s::GTLGData::Clear();
  clear_has_data();
}
inline const ::msgs2s::GTLGData& MsgLN2GTUserLogin::data() const {
  return data_ != NULL ? *data_ : *default_instance_->data_;
}
inline ::msgs2s::GTLGData* MsgLN2GTUserLogin::mutable_data() {
  set_has_data();
  if (data_ == NULL) data_ = new ::msgs2s::GTLGData;
  return data_;
}
inline ::msgs2s::GTLGData* MsgLN2GTUserLogin::release_data() {
  clear_has_data();
  ::msgs2s::GTLGData* temp = data_;
  data_ = NULL;
  return temp;
}
inline void MsgLN2GTUserLogin::set_allocated_data(::msgs2s::GTLGData* data) {
  delete data_;
  data_ = data;
  if (data) {
    set_has_data();
  } else {
    clear_has_data();
  }
}

// -------------------------------------------------------------------

// MsgLN2GTKickUser

// required .msgs2s.GTLGData data = 1;
inline bool MsgLN2GTKickUser::has_data() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgLN2GTKickUser::set_has_data() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgLN2GTKickUser::clear_has_data() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgLN2GTKickUser::clear_data() {
  if (data_ != NULL) data_->::msgs2s::GTLGData::Clear();
  clear_has_data();
}
inline const ::msgs2s::GTLGData& MsgLN2GTKickUser::data() const {
  return data_ != NULL ? *data_ : *default_instance_->data_;
}
inline ::msgs2s::GTLGData* MsgLN2GTKickUser::mutable_data() {
  set_has_data();
  if (data_ == NULL) data_ = new ::msgs2s::GTLGData;
  return data_;
}
inline ::msgs2s::GTLGData* MsgLN2GTKickUser::release_data() {
  clear_has_data();
  ::msgs2s::GTLGData* temp = data_;
  data_ = NULL;
  return temp;
}
inline void MsgLN2GTKickUser::set_allocated_data(::msgs2s::GTLGData* data) {
  delete data_;
  data_ = data;
  if (data) {
    set_has_data();
  } else {
    clear_has_data();
  }
}

// -------------------------------------------------------------------

// MsgGT2LNOnlines

// required uint32 onlines = 1;
inline bool MsgGT2LNOnlines::has_onlines() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgGT2LNOnlines::set_has_onlines() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgGT2LNOnlines::clear_has_onlines() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgGT2LNOnlines::clear_onlines() {
  onlines_ = 0u;
  clear_has_onlines();
}
inline ::google::protobuf::uint32 MsgGT2LNOnlines::onlines() const {
  return onlines_;
}
inline void MsgGT2LNOnlines::set_onlines(::google::protobuf::uint32 value) {
  set_has_onlines();
  onlines_ = value;
}

// -------------------------------------------------------------------

// MsgGT2LNPrepar

// required uint32 account = 1;
inline bool MsgGT2LNPrepar::has_account() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgGT2LNPrepar::set_has_account() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgGT2LNPrepar::clear_has_account() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgGT2LNPrepar::clear_account() {
  account_ = 0u;
  clear_has_account();
}
inline ::google::protobuf::uint32 MsgGT2LNPrepar::account() const {
  return account_;
}
inline void MsgGT2LNPrepar::set_account(::google::protobuf::uint32 value) {
  set_has_account();
  account_ = value;
}

// -------------------------------------------------------------------

// MsgGTRegisterLG

// required .msgs2s.MsgServerRegister GateInfo = 1;
inline bool MsgGTRegisterLG::has_gateinfo() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgGTRegisterLG::set_has_gateinfo() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgGTRegisterLG::clear_has_gateinfo() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgGTRegisterLG::clear_gateinfo() {
  if (gateinfo_ != NULL) gateinfo_->::msgs2s::MsgServerRegister::Clear();
  clear_has_gateinfo();
}
inline const ::msgs2s::MsgServerRegister& MsgGTRegisterLG::gateinfo() const {
  return gateinfo_ != NULL ? *gateinfo_ : *default_instance_->gateinfo_;
}
inline ::msgs2s::MsgServerRegister* MsgGTRegisterLG::mutable_gateinfo() {
  set_has_gateinfo();
  if (gateinfo_ == NULL) gateinfo_ = new ::msgs2s::MsgServerRegister;
  return gateinfo_;
}
inline ::msgs2s::MsgServerRegister* MsgGTRegisterLG::release_gateinfo() {
  clear_has_gateinfo();
  ::msgs2s::MsgServerRegister* temp = gateinfo_;
  gateinfo_ = NULL;
  return temp;
}
inline void MsgGTRegisterLG::set_allocated_gateinfo(::msgs2s::MsgServerRegister* gateinfo) {
  delete gateinfo_;
  gateinfo_ = gateinfo;
  if (gateinfo) {
    set_has_gateinfo();
  } else {
    clear_has_gateinfo();
  }
}

// required string ip = 2;
inline bool MsgGTRegisterLG::has_ip() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void MsgGTRegisterLG::set_has_ip() {
  _has_bits_[0] |= 0x00000002u;
}
inline void MsgGTRegisterLG::clear_has_ip() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void MsgGTRegisterLG::clear_ip() {
  if (ip_ != &::google::protobuf::internal::kEmptyString) {
    ip_->clear();
  }
  clear_has_ip();
}
inline const ::std::string& MsgGTRegisterLG::ip() const {
  return *ip_;
}
inline void MsgGTRegisterLG::set_ip(const ::std::string& value) {
  set_has_ip();
  if (ip_ == &::google::protobuf::internal::kEmptyString) {
    ip_ = new ::std::string;
  }
  ip_->assign(value);
}
inline void MsgGTRegisterLG::set_ip(const char* value) {
  set_has_ip();
  if (ip_ == &::google::protobuf::internal::kEmptyString) {
    ip_ = new ::std::string;
  }
  ip_->assign(value);
}
inline void MsgGTRegisterLG::set_ip(const char* value, size_t size) {
  set_has_ip();
  if (ip_ == &::google::protobuf::internal::kEmptyString) {
    ip_ = new ::std::string;
  }
  ip_->assign(reinterpret_cast<const char*>(value), size);
}
inline ::std::string* MsgGTRegisterLG::mutable_ip() {
  set_has_ip();
  if (ip_ == &::google::protobuf::internal::kEmptyString) {
    ip_ = new ::std::string;
  }
  return ip_;
}
inline ::std::string* MsgGTRegisterLG::release_ip() {
  clear_has_ip();
  if (ip_ == &::google::protobuf::internal::kEmptyString) {
    return NULL;
  } else {
    ::std::string* temp = ip_;
    ip_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
    return temp;
  }
}
inline void MsgGTRegisterLG::set_allocated_ip(::std::string* ip) {
  if (ip_ != &::google::protobuf::internal::kEmptyString) {
    delete ip_;
  }
  if (ip) {
    set_has_ip();
    ip_ = ip;
  } else {
    clear_has_ip();
    ip_ = const_cast< ::std::string*>(&::google::protobuf::internal::kEmptyString);
  }
}

// required uint32 port = 3;
inline bool MsgGTRegisterLG::has_port() const {
  return (_has_bits_[0] & 0x00000004u) != 0;
}
inline void MsgGTRegisterLG::set_has_port() {
  _has_bits_[0] |= 0x00000004u;
}
inline void MsgGTRegisterLG::clear_has_port() {
  _has_bits_[0] &= ~0x00000004u;
}
inline void MsgGTRegisterLG::clear_port() {
  port_ = 0u;
  clear_has_port();
}
inline ::google::protobuf::uint32 MsgGTRegisterLG::port() const {
  return port_;
}
inline void MsgGTRegisterLG::set_port(::google::protobuf::uint32 value) {
  set_has_port();
  port_ = value;
}

// repeated .msgs2s.GTLGData Accounts = 4;
inline int MsgGTRegisterLG::accounts_size() const {
  return accounts_.size();
}
inline void MsgGTRegisterLG::clear_accounts() {
  accounts_.Clear();
}
inline const ::msgs2s::GTLGData& MsgGTRegisterLG::accounts(int index) const {
  return accounts_.Get(index);
}
inline ::msgs2s::GTLGData* MsgGTRegisterLG::mutable_accounts(int index) {
  return accounts_.Mutable(index);
}
inline ::msgs2s::GTLGData* MsgGTRegisterLG::add_accounts() {
  return accounts_.Add();
}
inline const ::google::protobuf::RepeatedPtrField< ::msgs2s::GTLGData >&
MsgGTRegisterLG::accounts() const {
  return accounts_;
}
inline ::google::protobuf::RepeatedPtrField< ::msgs2s::GTLGData >*
MsgGTRegisterLG::mutable_accounts() {
  return &accounts_;
}


// @@protoc_insertion_point(namespace_scope)

}  // namespace msgs2s

#ifndef SWIG
namespace google {
namespace protobuf {


}  // namespace google
}  // namespace protobuf
#endif  // SWIG

// @@protoc_insertion_point(global_scope)

#endif  // PROTOBUF_msg_5fgate_5flogin_2eproto__INCLUDED
