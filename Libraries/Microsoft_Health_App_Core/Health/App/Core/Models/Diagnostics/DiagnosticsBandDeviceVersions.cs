﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsBandDeviceVersions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsBandDeviceVersions
  {
    [DataMember(Name = "pcbId")]
    public int PcbId { get; set; }

    [DataMember(Name = "application")]
    public string Application { get; set; }

    [DataMember(Name = "bootloader")]
    public string Bootloader { get; set; }

    [DataMember(Name = "updater")]
    public string Updater { get; set; }
  }
}