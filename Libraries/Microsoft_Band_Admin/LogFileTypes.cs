﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.LogFileTypes
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  public enum LogFileTypes
  {
    [EnumMember] Unknown = 0,
    [EnumMember] Sensor = 5,
    CrashDump = 6,
    [EnumMember] KAppLogs = 7,
    Telemetry = 8,
  }
}
