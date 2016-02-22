// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: msgs2s.proto

#ifndef PROTOBUF_msgs2s_2eproto__INCLUDED
#define PROTOBUF_msgs2s_2eproto__INCLUDED

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
// @@protoc_insertion_point(includes)

namespace msgs2s {

// Internal implementation detail -- do not call these.
void  protobuf_AddDesc_msgs2s_2eproto();
void protobuf_AssignDesc_msgs2s_2eproto();
void protobuf_ShutdownFile_msgs2s_2eproto();

class ServerVersion;
class MsgServerRegister;
class MsgDB2GTChangeGS;

// ===================================================================

class ServerVersion : public ::google::protobuf::Message {
 public:
  ServerVersion();
  virtual ~ServerVersion();

  ServerVersion(const ServerVersion& from);

  inline ServerVersion& operator=(const ServerVersion& from) {
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
  static const ServerVersion& default_instance();

  void Swap(ServerVersion* other);

  // implements Message ----------------------------------------------

  ServerVersion* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const ServerVersion& from);
  void MergeFrom(const ServerVersion& from);
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

  // required uint32 Major = 1;
  inline bool has_major() const;
  inline void clear_major();
  static const int kMajorFieldNumber = 1;
  inline ::google::protobuf::uint32 major() const;
  inline void set_major(::google::protobuf::uint32 value);

  // required uint32 Minor = 2;
  inline bool has_minor() const;
  inline void clear_minor();
  static const int kMinorFieldNumber = 2;
  inline ::google::protobuf::uint32 minor() const;
  inline void set_minor(::google::protobuf::uint32 value);

  // required uint32 Build = 3;
  inline bool has_build() const;
  inline void clear_build();
  static const int kBuildFieldNumber = 3;
  inline ::google::protobuf::uint32 build() const;
  inline void set_build(::google::protobuf::uint32 value);

  // required uint32 AppSvn = 4;
  inline bool has_appsvn() const;
  inline void clear_appsvn();
  static const int kAppSvnFieldNumber = 4;
  inline ::google::protobuf::uint32 appsvn() const;
  inline void set_appsvn(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:msgs2s.ServerVersion)
 private:
  inline void set_has_major();
  inline void clear_has_major();
  inline void set_has_minor();
  inline void clear_has_minor();
  inline void set_has_build();
  inline void clear_has_build();
  inline void set_has_appsvn();
  inline void clear_has_appsvn();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 major_;
  ::google::protobuf::uint32 minor_;
  ::google::protobuf::uint32 build_;
  ::google::protobuf::uint32 appsvn_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(4 + 31) / 32];

  friend void  protobuf_AddDesc_msgs2s_2eproto();
  friend void protobuf_AssignDesc_msgs2s_2eproto();
  friend void protobuf_ShutdownFile_msgs2s_2eproto();

  void InitAsDefaultInstance();
  static ServerVersion* default_instance_;
};
// -------------------------------------------------------------------

class MsgServerRegister : public ::google::protobuf::Message {
 public:
  MsgServerRegister();
  virtual ~MsgServerRegister();

  MsgServerRegister(const MsgServerRegister& from);

  inline MsgServerRegister& operator=(const MsgServerRegister& from) {
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
  static const MsgServerRegister& default_instance();

  void Swap(MsgServerRegister* other);

  // implements Message ----------------------------------------------

  MsgServerRegister* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgServerRegister& from);
  void MergeFrom(const MsgServerRegister& from);
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

  // required uint32 ServerId = 1;
  inline bool has_serverid() const;
  inline void clear_serverid();
  static const int kServerIdFieldNumber = 1;
  inline ::google::protobuf::uint32 serverid() const;
  inline void set_serverid(::google::protobuf::uint32 value);

  // required .msgs2s.ServerVersion Version = 2;
  inline bool has_version() const;
  inline void clear_version();
  static const int kVersionFieldNumber = 2;
  inline const ::msgs2s::ServerVersion& version() const;
  inline ::msgs2s::ServerVersion* mutable_version();
  inline ::msgs2s::ServerVersion* release_version();
  inline void set_allocated_version(::msgs2s::ServerVersion* version);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgServerRegister)
 private:
  inline void set_has_serverid();
  inline void clear_has_serverid();
  inline void set_has_version();
  inline void clear_has_version();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::msgs2s::ServerVersion* version_;
  ::google::protobuf::uint32 serverid_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(2 + 31) / 32];

  friend void  protobuf_AddDesc_msgs2s_2eproto();
  friend void protobuf_AssignDesc_msgs2s_2eproto();
  friend void protobuf_ShutdownFile_msgs2s_2eproto();

  void InitAsDefaultInstance();
  static MsgServerRegister* default_instance_;
};
// -------------------------------------------------------------------

class MsgDB2GTChangeGS : public ::google::protobuf::Message {
 public:
  MsgDB2GTChangeGS();
  virtual ~MsgDB2GTChangeGS();

  MsgDB2GTChangeGS(const MsgDB2GTChangeGS& from);

  inline MsgDB2GTChangeGS& operator=(const MsgDB2GTChangeGS& from) {
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
  static const MsgDB2GTChangeGS& default_instance();

  void Swap(MsgDB2GTChangeGS* other);

  // implements Message ----------------------------------------------

  MsgDB2GTChangeGS* New() const;
  void CopyFrom(const ::google::protobuf::Message& from);
  void MergeFrom(const ::google::protobuf::Message& from);
  void CopyFrom(const MsgDB2GTChangeGS& from);
  void MergeFrom(const MsgDB2GTChangeGS& from);
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

  // required uint32 GameServerID = 1;
  inline bool has_gameserverid() const;
  inline void clear_gameserverid();
  static const int kGameServerIDFieldNumber = 1;
  inline ::google::protobuf::uint32 gameserverid() const;
  inline void set_gameserverid(::google::protobuf::uint32 value);

  // @@protoc_insertion_point(class_scope:msgs2s.MsgDB2GTChangeGS)
 private:
  inline void set_has_gameserverid();
  inline void clear_has_gameserverid();

  ::google::protobuf::UnknownFieldSet _unknown_fields_;

  ::google::protobuf::uint32 gameserverid_;

  mutable int _cached_size_;
  ::google::protobuf::uint32 _has_bits_[(1 + 31) / 32];

  friend void  protobuf_AddDesc_msgs2s_2eproto();
  friend void protobuf_AssignDesc_msgs2s_2eproto();
  friend void protobuf_ShutdownFile_msgs2s_2eproto();

  void InitAsDefaultInstance();
  static MsgDB2GTChangeGS* default_instance_;
};
// ===================================================================


// ===================================================================

// ServerVersion

// required uint32 Major = 1;
inline bool ServerVersion::has_major() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void ServerVersion::set_has_major() {
  _has_bits_[0] |= 0x00000001u;
}
inline void ServerVersion::clear_has_major() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void ServerVersion::clear_major() {
  major_ = 0u;
  clear_has_major();
}
inline ::google::protobuf::uint32 ServerVersion::major() const {
  return major_;
}
inline void ServerVersion::set_major(::google::protobuf::uint32 value) {
  set_has_major();
  major_ = value;
}

// required uint32 Minor = 2;
inline bool ServerVersion::has_minor() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void ServerVersion::set_has_minor() {
  _has_bits_[0] |= 0x00000002u;
}
inline void ServerVersion::clear_has_minor() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void ServerVersion::clear_minor() {
  minor_ = 0u;
  clear_has_minor();
}
inline ::google::protobuf::uint32 ServerVersion::minor() const {
  return minor_;
}
inline void ServerVersion::set_minor(::google::protobuf::uint32 value) {
  set_has_minor();
  minor_ = value;
}

// required uint32 Build = 3;
inline bool ServerVersion::has_build() const {
  return (_has_bits_[0] & 0x00000004u) != 0;
}
inline void ServerVersion::set_has_build() {
  _has_bits_[0] |= 0x00000004u;
}
inline void ServerVersion::clear_has_build() {
  _has_bits_[0] &= ~0x00000004u;
}
inline void ServerVersion::clear_build() {
  build_ = 0u;
  clear_has_build();
}
inline ::google::protobuf::uint32 ServerVersion::build() const {
  return build_;
}
inline void ServerVersion::set_build(::google::protobuf::uint32 value) {
  set_has_build();
  build_ = value;
}

// required uint32 AppSvn = 4;
inline bool ServerVersion::has_appsvn() const {
  return (_has_bits_[0] & 0x00000008u) != 0;
}
inline void ServerVersion::set_has_appsvn() {
  _has_bits_[0] |= 0x00000008u;
}
inline void ServerVersion::clear_has_appsvn() {
  _has_bits_[0] &= ~0x00000008u;
}
inline void ServerVersion::clear_appsvn() {
  appsvn_ = 0u;
  clear_has_appsvn();
}
inline ::google::protobuf::uint32 ServerVersion::appsvn() const {
  return appsvn_;
}
inline void ServerVersion::set_appsvn(::google::protobuf::uint32 value) {
  set_has_appsvn();
  appsvn_ = value;
}

// -------------------------------------------------------------------

// MsgServerRegister

// required uint32 ServerId = 1;
inline bool MsgServerRegister::has_serverid() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgServerRegister::set_has_serverid() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgServerRegister::clear_has_serverid() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgServerRegister::clear_serverid() {
  serverid_ = 0u;
  clear_has_serverid();
}
inline ::google::protobuf::uint32 MsgServerRegister::serverid() const {
  return serverid_;
}
inline void MsgServerRegister::set_serverid(::google::protobuf::uint32 value) {
  set_has_serverid();
  serverid_ = value;
}

// required .msgs2s.ServerVersion Version = 2;
inline bool MsgServerRegister::has_version() const {
  return (_has_bits_[0] & 0x00000002u) != 0;
}
inline void MsgServerRegister::set_has_version() {
  _has_bits_[0] |= 0x00000002u;
}
inline void MsgServerRegister::clear_has_version() {
  _has_bits_[0] &= ~0x00000002u;
}
inline void MsgServerRegister::clear_version() {
  if (version_ != NULL) version_->::msgs2s::ServerVersion::Clear();
  clear_has_version();
}
inline const ::msgs2s::ServerVersion& MsgServerRegister::version() const {
  return version_ != NULL ? *version_ : *default_instance_->version_;
}
inline ::msgs2s::ServerVersion* MsgServerRegister::mutable_version() {
  set_has_version();
  if (version_ == NULL) version_ = new ::msgs2s::ServerVersion;
  return version_;
}
inline ::msgs2s::ServerVersion* MsgServerRegister::release_version() {
  clear_has_version();
  ::msgs2s::ServerVersion* temp = version_;
  version_ = NULL;
  return temp;
}
inline void MsgServerRegister::set_allocated_version(::msgs2s::ServerVersion* version) {
  delete version_;
  version_ = version;
  if (version) {
    set_has_version();
  } else {
    clear_has_version();
  }
}

// -------------------------------------------------------------------

// MsgDB2GTChangeGS

// required uint32 GameServerID = 1;
inline bool MsgDB2GTChangeGS::has_gameserverid() const {
  return (_has_bits_[0] & 0x00000001u) != 0;
}
inline void MsgDB2GTChangeGS::set_has_gameserverid() {
  _has_bits_[0] |= 0x00000001u;
}
inline void MsgDB2GTChangeGS::clear_has_gameserverid() {
  _has_bits_[0] &= ~0x00000001u;
}
inline void MsgDB2GTChangeGS::clear_gameserverid() {
  gameserverid_ = 0u;
  clear_has_gameserverid();
}
inline ::google::protobuf::uint32 MsgDB2GTChangeGS::gameserverid() const {
  return gameserverid_;
}
inline void MsgDB2GTChangeGS::set_gameserverid(::google::protobuf::uint32 value) {
  set_has_gameserverid();
  gameserverid_ = value;
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

#endif  // PROTOBUF_msgs2s_2eproto__INCLUDED
