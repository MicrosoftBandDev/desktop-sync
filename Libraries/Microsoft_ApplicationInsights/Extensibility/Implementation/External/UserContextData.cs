// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.UserContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class UserContextData
  {
    private readonly IDictionary<string, string> tags;

    internal UserContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string Id
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserId, value);
    }

    public string AccountId
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserAccountId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserAccountId, value);
    }

    public string UserAgent
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserAgent);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserAgent, value);
    }

    public string StoreRegion
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.UserStoreRegion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.UserStoreRegion, value);
    }

    public DateTimeOffset? AcquisitionDate
    {
      get => this.tags.GetTagDateTimeOffsetValueOrNull(ContextTagKeys.Keys.UserAccountAcquisitionDate);
      set => this.tags.SetDateTimeOffsetValueOrRemove(ContextTagKeys.Keys.UserAccountAcquisitionDate, value);
    }

    internal void SetDefaults(UserContextData source)
    {
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.UserId, source.Id);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.UserAgent, source.UserAgent);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.UserAccountId, source.AccountId);
      this.tags.InitializeTagDateTimeOffsetValue(ContextTagKeys.Keys.UserAccountAcquisitionDate, source.AcquisitionDate);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.UserStoreRegion, source.StoreRegion);
    }
  }
}
