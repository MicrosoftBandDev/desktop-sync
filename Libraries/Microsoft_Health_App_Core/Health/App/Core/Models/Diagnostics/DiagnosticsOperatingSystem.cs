﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.Diagnostics.DiagnosticsOperatingSystem
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using System.Runtime.Serialization;

namespace Microsoft.Health.App.Core.Models.Diagnostics
{
  [DataContract]
  public class DiagnosticsOperatingSystem
  {
    [DataMember(Name = "name")]
    public string Name { get; set; }

    [DataMember(Name = "version")]
    public string Version { get; set; }
  }
}
