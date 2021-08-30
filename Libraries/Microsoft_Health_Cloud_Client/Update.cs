﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.Update
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Json;
using System.Runtime.Serialization;

namespace Microsoft.Health.Cloud.Client
{
  [DataContract]
  public class Update : DeserializableObjectBase
  {
    [DataMember(Name = "id")]
    public string Id { get; set; }

    [DataMember(Name = "type")]
    public UpdateType TypeId { get; set; }

    [DataMember(Name = "info")]
    public string Info { get; set; }

    protected override void Validate()
    {
      base.Validate();
      JsonAssert.PropertyIsNotNullOrWhiteSpace("id", this.Id);
    }
  }
}
