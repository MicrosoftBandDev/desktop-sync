﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CloudProfileDeviceLink
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class CloudProfileDeviceLink
  {
    [DataMember(EmitDefaultValue = true)]
    internal string LastKDKSyncUpdateOn;
    [DataMember]
    internal DeviceLinkIds ApplicationSettings;
    [DataMember]
    internal DeviceLinkIds DeviceSettings;
  }
}
