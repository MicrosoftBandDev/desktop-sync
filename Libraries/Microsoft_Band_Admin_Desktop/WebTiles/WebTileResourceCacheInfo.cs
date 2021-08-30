﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.WebTileResourceCacheInfo
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Collections.Generic;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin.WebTiles
{
  [DataContract]
  public class WebTileResourceCacheInfo : IWebTileResourceCacheInfo
  {
    [DataMember(Name = "ETag")]
    public string ETag { get; set; }

    [DataMember(Name = "LastModified")]
    public string LastModified { get; set; }

    [DataMember(EmitDefaultValue = false, Name = "FeedItemIds")]
    public List<string> FeedItemIds { get; set; }
  }
}
