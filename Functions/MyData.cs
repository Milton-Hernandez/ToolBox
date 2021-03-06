﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using pb = global::Google.Protobuf;
using pbc = global::Google.Protobuf.Collections;
using pbr = global::Google.Protobuf.Reflection;
using scg = global::System.Collections.Generic;


namespace Functions
{
// Generated by the protocol buffer compiler.  DO NOT EDIT!
// source: sample.proto
#pragma warning disable 1591, 0612, 3021
#region Designer generated code


/// <summary>Holder for reflection information generated from sample.proto</summary>
[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
public static partial class SampleReflection {

  #region Descriptor
  /// <summary>File descriptor for sample.proto</summary>
  public static pbr::FileDescriptor Descriptor {
    get { return descriptor; }
  }
  private static pbr::FileDescriptor descriptor;

  static SampleReflection() {
    byte[] descriptorData = global::System.Convert.FromBase64String(
        string.Concat(
          "CgxzYW1wbGUucHJvdG8iRwoITXlTYW1wbGUSDQoFcXVlcnkYASABKAkSEwoL",
          "cGFnZV9udW1iZXIYAiABKAUSFwoPcmVzdWx0X3Blcl9wYWdlGAMgAygFYgZw",
          "cm90bzM="));
    descriptor = pbr::FileDescriptor.FromGeneratedCode(descriptorData,
        new pbr::FileDescriptor[] { },
        new pbr::GeneratedCodeInfo(null, new pbr::GeneratedCodeInfo[] {
          new pbr::GeneratedCodeInfo(typeof(global::MySample), global::MySample.Parser, new[]{ "Query", "PageNumber", "ResultPerPage" }, null, null, null)
        }));
  }
  #endregion

}
#region Messages
[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
public sealed partial class MySample : pb::IMessage<MySample> {
  private static readonly pb::MessageParser<MySample> _parser = new pb::MessageParser<MySample>(() => new MySample());
  public static pb::MessageParser<MySample> Parser { get { return _parser; } }

  public static pbr::MessageDescriptor Descriptor {
    get { return global::SampleReflection.Descriptor.MessageTypes[0]; }
  }

  pbr::MessageDescriptor pb::IMessage.Descriptor {
    get { return Descriptor; }
  }

  public MySample() {
    OnConstruction();
  }

  partial void OnConstruction();

  public MySample(MySample other) : this() {
    query_ = other.query_;
    pageNumber_ = other.pageNumber_;
    resultPerPage_ = other.resultPerPage_.Clone();
  }

  public MySample Clone() {
    return new MySample(this);
  }

  /// <summary>Field number for the "query" field.</summary>
  public const int QueryFieldNumber = 1;
  private string query_ = "";
  public string Query {
    get { return query_; }
    set {
      query_ = pb::Preconditions.CheckNotNull(value, "value");
    }
  }

  /// <summary>Field number for the "page_number" field.</summary>
  public const int PageNumberFieldNumber = 2;
  private int pageNumber_;
  public int PageNumber {
    get { return pageNumber_; }
    set {
      pageNumber_ = value;
    }
  }

  /// <summary>Field number for the "result_per_page" field.</summary>
  public const int ResultPerPageFieldNumber = 3;
  private static readonly pb::FieldCodec<int> _repeated_resultPerPage_codec
      = pb::FieldCodec.ForInt32(26);
  private readonly pbc::RepeatedField<int> resultPerPage_ = new pbc::RepeatedField<int>();
  public pbc::RepeatedField<int> ResultPerPage {
    get { return resultPerPage_; }
  }

  public override bool Equals(object other) {
    return Equals(other as MySample);
  }

  public bool Equals(MySample other) {
    if (ReferenceEquals(other, null)) {
      return false;
    }
    if (ReferenceEquals(other, this)) {
      return true;
    }
    if (Query != other.Query) return false;
    if (PageNumber != other.PageNumber) return false;
    if(!resultPerPage_.Equals(other.resultPerPage_)) return false;
    return true;
  }

  public override int GetHashCode() {
    int hash = 1;
    if (Query.Length != 0) hash ^= Query.GetHashCode();
    if (PageNumber != 0) hash ^= PageNumber.GetHashCode();
    hash ^= resultPerPage_.GetHashCode();
    return hash;
  }

  public override string ToString() {
    return pb::JsonFormatter.ToDiagnosticString(this);
  }

  public void WriteTo(pb::CodedOutputStream output) {
    if (Query.Length != 0) {
      output.WriteRawTag(10);
      output.WriteString(Query);
    }
    if (PageNumber != 0) {
      output.WriteRawTag(16);
      output.WriteInt32(PageNumber);
    }
    resultPerPage_.WriteTo(output, _repeated_resultPerPage_codec);
  }

  public int CalculateSize() {
    int size = 0;
    if (Query.Length != 0) {
      size += 1 + pb::CodedOutputStream.ComputeStringSize(Query);
    }
    if (PageNumber != 0) {
      size += 1 + pb::CodedOutputStream.ComputeInt32Size(PageNumber);
    }
    size += resultPerPage_.CalculateSize(_repeated_resultPerPage_codec);
    return size;
  }

  public void MergeFrom(MySample other) {
    if (other == null) {
      return;
    }
    if (other.Query.Length != 0) {
      Query = other.Query;
    }
    if (other.PageNumber != 0) {
      PageNumber = other.PageNumber;
    }
    resultPerPage_.Add(other.resultPerPage_);
  }

  public void MergeFrom(pb::CodedInputStream input) {
    uint tag;
    while ((tag = input.ReadTag()) != 0) {
      switch(tag) {
        default:
          input.SkipLastField();
          break;
        case 10: {
          Query = input.ReadString();
          break;
        }
        case 16: {
          PageNumber = input.ReadInt32();
          break;
        }
        case 26:
        case 24: {
          resultPerPage_.AddEntriesFrom(input, _repeated_resultPerPage_codec);
          break;
        }
      }
    }
  }

}

#endregion


#endregion Designer generated code

}
