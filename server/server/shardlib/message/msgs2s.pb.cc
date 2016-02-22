// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: msgs2s.proto

#define INTERNAL_SUPPRESS_PROTOBUF_FIELD_DEPRECATION
#include "msgs2s.pb.h"

#include <algorithm>

#include <google/protobuf/stubs/common.h>
#include <google/protobuf/stubs/once.h>
#include <google/protobuf/io/coded_stream.h>
#include <google/protobuf/wire_format_lite_inl.h>
#include <google/protobuf/descriptor.h>
#include <google/protobuf/generated_message_reflection.h>
#include <google/protobuf/reflection_ops.h>
#include <google/protobuf/wire_format.h>
// @@protoc_insertion_point(includes)

namespace msgs2s {

namespace {

const ::google::protobuf::Descriptor* ServerVersion_descriptor_ = NULL;
const ::google::protobuf::internal::GeneratedMessageReflection*
  ServerVersion_reflection_ = NULL;
const ::google::protobuf::Descriptor* MsgServerRegister_descriptor_ = NULL;
const ::google::protobuf::internal::GeneratedMessageReflection*
  MsgServerRegister_reflection_ = NULL;
const ::google::protobuf::Descriptor* MsgDB2GTChangeGS_descriptor_ = NULL;
const ::google::protobuf::internal::GeneratedMessageReflection*
  MsgDB2GTChangeGS_reflection_ = NULL;

}  // namespace


void protobuf_AssignDesc_msgs2s_2eproto() {
  protobuf_AddDesc_msgs2s_2eproto();
  const ::google::protobuf::FileDescriptor* file =
    ::google::protobuf::DescriptorPool::generated_pool()->FindFileByName(
      "msgs2s.proto");
  GOOGLE_CHECK(file != NULL);
  ServerVersion_descriptor_ = file->message_type(0);
  static const int ServerVersion_offsets_[4] = {
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, major_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, minor_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, build_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, appsvn_),
  };
  ServerVersion_reflection_ =
    new ::google::protobuf::internal::GeneratedMessageReflection(
      ServerVersion_descriptor_,
      ServerVersion::default_instance_,
      ServerVersion_offsets_,
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, _has_bits_[0]),
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(ServerVersion, _unknown_fields_),
      -1,
      ::google::protobuf::DescriptorPool::generated_pool(),
      ::google::protobuf::MessageFactory::generated_factory(),
      sizeof(ServerVersion));
  MsgServerRegister_descriptor_ = file->message_type(1);
  static const int MsgServerRegister_offsets_[2] = {
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgServerRegister, serverid_),
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgServerRegister, version_),
  };
  MsgServerRegister_reflection_ =
    new ::google::protobuf::internal::GeneratedMessageReflection(
      MsgServerRegister_descriptor_,
      MsgServerRegister::default_instance_,
      MsgServerRegister_offsets_,
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgServerRegister, _has_bits_[0]),
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgServerRegister, _unknown_fields_),
      -1,
      ::google::protobuf::DescriptorPool::generated_pool(),
      ::google::protobuf::MessageFactory::generated_factory(),
      sizeof(MsgServerRegister));
  MsgDB2GTChangeGS_descriptor_ = file->message_type(2);
  static const int MsgDB2GTChangeGS_offsets_[1] = {
    GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgDB2GTChangeGS, gameserverid_),
  };
  MsgDB2GTChangeGS_reflection_ =
    new ::google::protobuf::internal::GeneratedMessageReflection(
      MsgDB2GTChangeGS_descriptor_,
      MsgDB2GTChangeGS::default_instance_,
      MsgDB2GTChangeGS_offsets_,
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgDB2GTChangeGS, _has_bits_[0]),
      GOOGLE_PROTOBUF_GENERATED_MESSAGE_FIELD_OFFSET(MsgDB2GTChangeGS, _unknown_fields_),
      -1,
      ::google::protobuf::DescriptorPool::generated_pool(),
      ::google::protobuf::MessageFactory::generated_factory(),
      sizeof(MsgDB2GTChangeGS));
}

namespace {

GOOGLE_PROTOBUF_DECLARE_ONCE(protobuf_AssignDescriptors_once_);
inline void protobuf_AssignDescriptorsOnce() {
  ::google::protobuf::GoogleOnceInit(&protobuf_AssignDescriptors_once_,
                 &protobuf_AssignDesc_msgs2s_2eproto);
}

void protobuf_RegisterTypes(const ::std::string&) {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedMessage(
    ServerVersion_descriptor_, &ServerVersion::default_instance());
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedMessage(
    MsgServerRegister_descriptor_, &MsgServerRegister::default_instance());
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedMessage(
    MsgDB2GTChangeGS_descriptor_, &MsgDB2GTChangeGS::default_instance());
}

}  // namespace

void protobuf_ShutdownFile_msgs2s_2eproto() {
  delete ServerVersion::default_instance_;
  delete ServerVersion_reflection_;
  delete MsgServerRegister::default_instance_;
  delete MsgServerRegister_reflection_;
  delete MsgDB2GTChangeGS::default_instance_;
  delete MsgDB2GTChangeGS_reflection_;
}

void protobuf_AddDesc_msgs2s_2eproto() {
  static bool already_here = false;
  if (already_here) return;
  already_here = true;
  GOOGLE_PROTOBUF_VERIFY_VERSION;

  ::google::protobuf::DescriptorPool::InternalAddGeneratedFile(
    "\n\014msgs2s.proto\022\006msgs2s\"L\n\rServerVersion\022"
    "\r\n\005Major\030\001 \002(\r\022\r\n\005Minor\030\002 \002(\r\022\r\n\005Build\030\003"
    " \002(\r\022\016\n\006AppSvn\030\004 \002(\r\"M\n\021MsgServerRegiste"
    "r\022\020\n\010ServerId\030\001 \002(\r\022&\n\007Version\030\002 \002(\0132\025.m"
    "sgs2s.ServerVersion\"(\n\020MsgDB2GTChangeGS\022"
    "\024\n\014GameServerID\030\001 \002(\r", 221);
  ::google::protobuf::MessageFactory::InternalRegisterGeneratedFile(
    "msgs2s.proto", &protobuf_RegisterTypes);
  ServerVersion::default_instance_ = new ServerVersion();
  MsgServerRegister::default_instance_ = new MsgServerRegister();
  MsgDB2GTChangeGS::default_instance_ = new MsgDB2GTChangeGS();
  ServerVersion::default_instance_->InitAsDefaultInstance();
  MsgServerRegister::default_instance_->InitAsDefaultInstance();
  MsgDB2GTChangeGS::default_instance_->InitAsDefaultInstance();
  ::google::protobuf::internal::OnShutdown(&protobuf_ShutdownFile_msgs2s_2eproto);
}

// Force AddDescriptors() to be called at static initialization time.
struct StaticDescriptorInitializer_msgs2s_2eproto {
  StaticDescriptorInitializer_msgs2s_2eproto() {
    protobuf_AddDesc_msgs2s_2eproto();
  }
} static_descriptor_initializer_msgs2s_2eproto_;

// ===================================================================

#ifndef _MSC_VER
const int ServerVersion::kMajorFieldNumber;
const int ServerVersion::kMinorFieldNumber;
const int ServerVersion::kBuildFieldNumber;
const int ServerVersion::kAppSvnFieldNumber;
#endif  // !_MSC_VER

ServerVersion::ServerVersion()
  : ::google::protobuf::Message() {
  SharedCtor();
}

void ServerVersion::InitAsDefaultInstance() {
}

ServerVersion::ServerVersion(const ServerVersion& from)
  : ::google::protobuf::Message() {
  SharedCtor();
  MergeFrom(from);
}

void ServerVersion::SharedCtor() {
  _cached_size_ = 0;
  major_ = 0u;
  minor_ = 0u;
  build_ = 0u;
  appsvn_ = 0u;
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
}

ServerVersion::~ServerVersion() {
  SharedDtor();
}

void ServerVersion::SharedDtor() {
  if (this != default_instance_) {
  }
}

void ServerVersion::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* ServerVersion::descriptor() {
  protobuf_AssignDescriptorsOnce();
  return ServerVersion_descriptor_;
}

const ServerVersion& ServerVersion::default_instance() {
  if (default_instance_ == NULL) protobuf_AddDesc_msgs2s_2eproto();
  return *default_instance_;
}

ServerVersion* ServerVersion::default_instance_ = NULL;

ServerVersion* ServerVersion::New() const {
  return new ServerVersion;
}

void ServerVersion::Clear() {
  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    major_ = 0u;
    minor_ = 0u;
    build_ = 0u;
    appsvn_ = 0u;
  }
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
  mutable_unknown_fields()->Clear();
}

bool ServerVersion::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!(EXPRESSION)) return false
  ::google::protobuf::uint32 tag;
  while ((tag = input->ReadTag()) != 0) {
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // required uint32 Major = 1;
      case 1: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &major_)));
          set_has_major();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(16)) goto parse_Minor;
        break;
      }

      // required uint32 Minor = 2;
      case 2: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
         parse_Minor:
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &minor_)));
          set_has_minor();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(24)) goto parse_Build;
        break;
      }

      // required uint32 Build = 3;
      case 3: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
         parse_Build:
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &build_)));
          set_has_build();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(32)) goto parse_AppSvn;
        break;
      }

      // required uint32 AppSvn = 4;
      case 4: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
         parse_AppSvn:
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &appsvn_)));
          set_has_appsvn();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectAtEnd()) return true;
        break;
      }

      default: {
      handle_uninterpreted:
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_END_GROUP) {
          return true;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, mutable_unknown_fields()));
        break;
      }
    }
  }
  return true;
#undef DO_
}

void ServerVersion::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // required uint32 Major = 1;
  if (has_major()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(1, this->major(), output);
  }

  // required uint32 Minor = 2;
  if (has_minor()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(2, this->minor(), output);
  }

  // required uint32 Build = 3;
  if (has_build()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(3, this->build(), output);
  }

  // required uint32 AppSvn = 4;
  if (has_appsvn()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(4, this->appsvn(), output);
  }

  if (!unknown_fields().empty()) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        unknown_fields(), output);
  }
}

::google::protobuf::uint8* ServerVersion::SerializeWithCachedSizesToArray(
    ::google::protobuf::uint8* target) const {
  // required uint32 Major = 1;
  if (has_major()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(1, this->major(), target);
  }

  // required uint32 Minor = 2;
  if (has_minor()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(2, this->minor(), target);
  }

  // required uint32 Build = 3;
  if (has_build()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(3, this->build(), target);
  }

  // required uint32 AppSvn = 4;
  if (has_appsvn()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(4, this->appsvn(), target);
  }

  if (!unknown_fields().empty()) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        unknown_fields(), target);
  }
  return target;
}

int ServerVersion::ByteSize() const {
  int total_size = 0;

  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    // required uint32 Major = 1;
    if (has_major()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->major());
    }

    // required uint32 Minor = 2;
    if (has_minor()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->minor());
    }

    // required uint32 Build = 3;
    if (has_build()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->build());
    }

    // required uint32 AppSvn = 4;
    if (has_appsvn()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->appsvn());
    }

  }
  if (!unknown_fields().empty()) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        unknown_fields());
  }
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = total_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void ServerVersion::MergeFrom(const ::google::protobuf::Message& from) {
  GOOGLE_CHECK_NE(&from, this);
  const ServerVersion* source =
    ::google::protobuf::internal::dynamic_cast_if_available<const ServerVersion*>(
      &from);
  if (source == NULL) {
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
    MergeFrom(*source);
  }
}

void ServerVersion::MergeFrom(const ServerVersion& from) {
  GOOGLE_CHECK_NE(&from, this);
  if (from._has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (from.has_major()) {
      set_major(from.major());
    }
    if (from.has_minor()) {
      set_minor(from.minor());
    }
    if (from.has_build()) {
      set_build(from.build());
    }
    if (from.has_appsvn()) {
      set_appsvn(from.appsvn());
    }
  }
  mutable_unknown_fields()->MergeFrom(from.unknown_fields());
}

void ServerVersion::CopyFrom(const ::google::protobuf::Message& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void ServerVersion::CopyFrom(const ServerVersion& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool ServerVersion::IsInitialized() const {
  if ((_has_bits_[0] & 0x0000000f) != 0x0000000f) return false;

  return true;
}

void ServerVersion::Swap(ServerVersion* other) {
  if (other != this) {
    std::swap(major_, other->major_);
    std::swap(minor_, other->minor_);
    std::swap(build_, other->build_);
    std::swap(appsvn_, other->appsvn_);
    std::swap(_has_bits_[0], other->_has_bits_[0]);
    _unknown_fields_.Swap(&other->_unknown_fields_);
    std::swap(_cached_size_, other->_cached_size_);
  }
}

::google::protobuf::Metadata ServerVersion::GetMetadata() const {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::Metadata metadata;
  metadata.descriptor = ServerVersion_descriptor_;
  metadata.reflection = ServerVersion_reflection_;
  return metadata;
}


// ===================================================================

#ifndef _MSC_VER
const int MsgServerRegister::kServerIdFieldNumber;
const int MsgServerRegister::kVersionFieldNumber;
#endif  // !_MSC_VER

MsgServerRegister::MsgServerRegister()
  : ::google::protobuf::Message() {
  SharedCtor();
}

void MsgServerRegister::InitAsDefaultInstance() {
  version_ = const_cast< ::msgs2s::ServerVersion*>(&::msgs2s::ServerVersion::default_instance());
}

MsgServerRegister::MsgServerRegister(const MsgServerRegister& from)
  : ::google::protobuf::Message() {
  SharedCtor();
  MergeFrom(from);
}

void MsgServerRegister::SharedCtor() {
  _cached_size_ = 0;
  serverid_ = 0u;
  version_ = NULL;
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
}

MsgServerRegister::~MsgServerRegister() {
  SharedDtor();
}

void MsgServerRegister::SharedDtor() {
  if (this != default_instance_) {
    delete version_;
  }
}

void MsgServerRegister::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* MsgServerRegister::descriptor() {
  protobuf_AssignDescriptorsOnce();
  return MsgServerRegister_descriptor_;
}

const MsgServerRegister& MsgServerRegister::default_instance() {
  if (default_instance_ == NULL) protobuf_AddDesc_msgs2s_2eproto();
  return *default_instance_;
}

MsgServerRegister* MsgServerRegister::default_instance_ = NULL;

MsgServerRegister* MsgServerRegister::New() const {
  return new MsgServerRegister;
}

void MsgServerRegister::Clear() {
  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    serverid_ = 0u;
    if (has_version()) {
      if (version_ != NULL) version_->::msgs2s::ServerVersion::Clear();
    }
  }
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
  mutable_unknown_fields()->Clear();
}

bool MsgServerRegister::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!(EXPRESSION)) return false
  ::google::protobuf::uint32 tag;
  while ((tag = input->ReadTag()) != 0) {
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // required uint32 ServerId = 1;
      case 1: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &serverid_)));
          set_has_serverid();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectTag(18)) goto parse_Version;
        break;
      }

      // required .msgs2s.ServerVersion Version = 2;
      case 2: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_LENGTH_DELIMITED) {
         parse_Version:
          DO_(::google::protobuf::internal::WireFormatLite::ReadMessageNoVirtual(
               input, mutable_version()));
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectAtEnd()) return true;
        break;
      }

      default: {
      handle_uninterpreted:
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_END_GROUP) {
          return true;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, mutable_unknown_fields()));
        break;
      }
    }
  }
  return true;
#undef DO_
}

void MsgServerRegister::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // required uint32 ServerId = 1;
  if (has_serverid()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(1, this->serverid(), output);
  }

  // required .msgs2s.ServerVersion Version = 2;
  if (has_version()) {
    ::google::protobuf::internal::WireFormatLite::WriteMessageMaybeToArray(
      2, this->version(), output);
  }

  if (!unknown_fields().empty()) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        unknown_fields(), output);
  }
}

::google::protobuf::uint8* MsgServerRegister::SerializeWithCachedSizesToArray(
    ::google::protobuf::uint8* target) const {
  // required uint32 ServerId = 1;
  if (has_serverid()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(1, this->serverid(), target);
  }

  // required .msgs2s.ServerVersion Version = 2;
  if (has_version()) {
    target = ::google::protobuf::internal::WireFormatLite::
      WriteMessageNoVirtualToArray(
        2, this->version(), target);
  }

  if (!unknown_fields().empty()) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        unknown_fields(), target);
  }
  return target;
}

int MsgServerRegister::ByteSize() const {
  int total_size = 0;

  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    // required uint32 ServerId = 1;
    if (has_serverid()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->serverid());
    }

    // required .msgs2s.ServerVersion Version = 2;
    if (has_version()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::MessageSizeNoVirtual(
          this->version());
    }

  }
  if (!unknown_fields().empty()) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        unknown_fields());
  }
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = total_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void MsgServerRegister::MergeFrom(const ::google::protobuf::Message& from) {
  GOOGLE_CHECK_NE(&from, this);
  const MsgServerRegister* source =
    ::google::protobuf::internal::dynamic_cast_if_available<const MsgServerRegister*>(
      &from);
  if (source == NULL) {
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
    MergeFrom(*source);
  }
}

void MsgServerRegister::MergeFrom(const MsgServerRegister& from) {
  GOOGLE_CHECK_NE(&from, this);
  if (from._has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (from.has_serverid()) {
      set_serverid(from.serverid());
    }
    if (from.has_version()) {
      mutable_version()->::msgs2s::ServerVersion::MergeFrom(from.version());
    }
  }
  mutable_unknown_fields()->MergeFrom(from.unknown_fields());
}

void MsgServerRegister::CopyFrom(const ::google::protobuf::Message& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void MsgServerRegister::CopyFrom(const MsgServerRegister& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool MsgServerRegister::IsInitialized() const {
  if ((_has_bits_[0] & 0x00000003) != 0x00000003) return false;

  if (has_version()) {
    if (!this->version().IsInitialized()) return false;
  }
  return true;
}

void MsgServerRegister::Swap(MsgServerRegister* other) {
  if (other != this) {
    std::swap(serverid_, other->serverid_);
    std::swap(version_, other->version_);
    std::swap(_has_bits_[0], other->_has_bits_[0]);
    _unknown_fields_.Swap(&other->_unknown_fields_);
    std::swap(_cached_size_, other->_cached_size_);
  }
}

::google::protobuf::Metadata MsgServerRegister::GetMetadata() const {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::Metadata metadata;
  metadata.descriptor = MsgServerRegister_descriptor_;
  metadata.reflection = MsgServerRegister_reflection_;
  return metadata;
}


// ===================================================================

#ifndef _MSC_VER
const int MsgDB2GTChangeGS::kGameServerIDFieldNumber;
#endif  // !_MSC_VER

MsgDB2GTChangeGS::MsgDB2GTChangeGS()
  : ::google::protobuf::Message() {
  SharedCtor();
}

void MsgDB2GTChangeGS::InitAsDefaultInstance() {
}

MsgDB2GTChangeGS::MsgDB2GTChangeGS(const MsgDB2GTChangeGS& from)
  : ::google::protobuf::Message() {
  SharedCtor();
  MergeFrom(from);
}

void MsgDB2GTChangeGS::SharedCtor() {
  _cached_size_ = 0;
  gameserverid_ = 0u;
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
}

MsgDB2GTChangeGS::~MsgDB2GTChangeGS() {
  SharedDtor();
}

void MsgDB2GTChangeGS::SharedDtor() {
  if (this != default_instance_) {
  }
}

void MsgDB2GTChangeGS::SetCachedSize(int size) const {
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
}
const ::google::protobuf::Descriptor* MsgDB2GTChangeGS::descriptor() {
  protobuf_AssignDescriptorsOnce();
  return MsgDB2GTChangeGS_descriptor_;
}

const MsgDB2GTChangeGS& MsgDB2GTChangeGS::default_instance() {
  if (default_instance_ == NULL) protobuf_AddDesc_msgs2s_2eproto();
  return *default_instance_;
}

MsgDB2GTChangeGS* MsgDB2GTChangeGS::default_instance_ = NULL;

MsgDB2GTChangeGS* MsgDB2GTChangeGS::New() const {
  return new MsgDB2GTChangeGS;
}

void MsgDB2GTChangeGS::Clear() {
  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    gameserverid_ = 0u;
  }
  ::memset(_has_bits_, 0, sizeof(_has_bits_));
  mutable_unknown_fields()->Clear();
}

bool MsgDB2GTChangeGS::MergePartialFromCodedStream(
    ::google::protobuf::io::CodedInputStream* input) {
#define DO_(EXPRESSION) if (!(EXPRESSION)) return false
  ::google::protobuf::uint32 tag;
  while ((tag = input->ReadTag()) != 0) {
    switch (::google::protobuf::internal::WireFormatLite::GetTagFieldNumber(tag)) {
      // required uint32 GameServerID = 1;
      case 1: {
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_VARINT) {
          DO_((::google::protobuf::internal::WireFormatLite::ReadPrimitive<
                   ::google::protobuf::uint32, ::google::protobuf::internal::WireFormatLite::TYPE_UINT32>(
                 input, &gameserverid_)));
          set_has_gameserverid();
        } else {
          goto handle_uninterpreted;
        }
        if (input->ExpectAtEnd()) return true;
        break;
      }

      default: {
      handle_uninterpreted:
        if (::google::protobuf::internal::WireFormatLite::GetTagWireType(tag) ==
            ::google::protobuf::internal::WireFormatLite::WIRETYPE_END_GROUP) {
          return true;
        }
        DO_(::google::protobuf::internal::WireFormat::SkipField(
              input, tag, mutable_unknown_fields()));
        break;
      }
    }
  }
  return true;
#undef DO_
}

void MsgDB2GTChangeGS::SerializeWithCachedSizes(
    ::google::protobuf::io::CodedOutputStream* output) const {
  // required uint32 GameServerID = 1;
  if (has_gameserverid()) {
    ::google::protobuf::internal::WireFormatLite::WriteUInt32(1, this->gameserverid(), output);
  }

  if (!unknown_fields().empty()) {
    ::google::protobuf::internal::WireFormat::SerializeUnknownFields(
        unknown_fields(), output);
  }
}

::google::protobuf::uint8* MsgDB2GTChangeGS::SerializeWithCachedSizesToArray(
    ::google::protobuf::uint8* target) const {
  // required uint32 GameServerID = 1;
  if (has_gameserverid()) {
    target = ::google::protobuf::internal::WireFormatLite::WriteUInt32ToArray(1, this->gameserverid(), target);
  }

  if (!unknown_fields().empty()) {
    target = ::google::protobuf::internal::WireFormat::SerializeUnknownFieldsToArray(
        unknown_fields(), target);
  }
  return target;
}

int MsgDB2GTChangeGS::ByteSize() const {
  int total_size = 0;

  if (_has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    // required uint32 GameServerID = 1;
    if (has_gameserverid()) {
      total_size += 1 +
        ::google::protobuf::internal::WireFormatLite::UInt32Size(
          this->gameserverid());
    }

  }
  if (!unknown_fields().empty()) {
    total_size +=
      ::google::protobuf::internal::WireFormat::ComputeUnknownFieldsSize(
        unknown_fields());
  }
  GOOGLE_SAFE_CONCURRENT_WRITES_BEGIN();
  _cached_size_ = total_size;
  GOOGLE_SAFE_CONCURRENT_WRITES_END();
  return total_size;
}

void MsgDB2GTChangeGS::MergeFrom(const ::google::protobuf::Message& from) {
  GOOGLE_CHECK_NE(&from, this);
  const MsgDB2GTChangeGS* source =
    ::google::protobuf::internal::dynamic_cast_if_available<const MsgDB2GTChangeGS*>(
      &from);
  if (source == NULL) {
    ::google::protobuf::internal::ReflectionOps::Merge(from, this);
  } else {
    MergeFrom(*source);
  }
}

void MsgDB2GTChangeGS::MergeFrom(const MsgDB2GTChangeGS& from) {
  GOOGLE_CHECK_NE(&from, this);
  if (from._has_bits_[0 / 32] & (0xffu << (0 % 32))) {
    if (from.has_gameserverid()) {
      set_gameserverid(from.gameserverid());
    }
  }
  mutable_unknown_fields()->MergeFrom(from.unknown_fields());
}

void MsgDB2GTChangeGS::CopyFrom(const ::google::protobuf::Message& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

void MsgDB2GTChangeGS::CopyFrom(const MsgDB2GTChangeGS& from) {
  if (&from == this) return;
  Clear();
  MergeFrom(from);
}

bool MsgDB2GTChangeGS::IsInitialized() const {
  if ((_has_bits_[0] & 0x00000001) != 0x00000001) return false;

  return true;
}

void MsgDB2GTChangeGS::Swap(MsgDB2GTChangeGS* other) {
  if (other != this) {
    std::swap(gameserverid_, other->gameserverid_);
    std::swap(_has_bits_[0], other->_has_bits_[0]);
    _unknown_fields_.Swap(&other->_unknown_fields_);
    std::swap(_cached_size_, other->_cached_size_);
  }
}

::google::protobuf::Metadata MsgDB2GTChangeGS::GetMetadata() const {
  protobuf_AssignDescriptorsOnce();
  ::google::protobuf::Metadata metadata;
  metadata.descriptor = MsgDB2GTChangeGS_descriptor_;
  metadata.reflection = MsgDB2GTChangeGS_reflection_;
  return metadata;
}


// @@protoc_insertion_point(namespace_scope)

}  // namespace msgs2s

// @@protoc_insertion_point(global_scope)
