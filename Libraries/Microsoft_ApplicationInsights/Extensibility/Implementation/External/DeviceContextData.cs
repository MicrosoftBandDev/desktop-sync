// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.External.DeviceContextData
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System.Collections.Generic;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.External
{
  internal sealed class DeviceContextData
  {
    private readonly IDictionary<string, string> tags;

    internal DeviceContextData(IDictionary<string, string> tags) => this.tags = tags;

    public string Type
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceType);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceType, value);
    }

    public string Id
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceId);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceId, value);
    }

    public string OperatingSystem
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceOSVersion);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceOSVersion, value);
    }

    public string OemName
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceOEMName);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceOEMName, value);
    }

    public string Model
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceModel);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceModel, value);
    }

    public int? NetworkType
    {
      get => this.tags.GetTagIntValueOrNull(ContextTagKeys.Keys.DeviceNetwork);
      set => this.tags.SetTagValueOrRemove<int?>(ContextTagKeys.Keys.DeviceNetwork, value);
    }

    public string ScreenResolution
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceScreenResolution);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceScreenResolution, value);
    }

    public string Language
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceLanguage);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceLanguage, value);
    }

    public string RoleName
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceRoleName);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceRoleName, value);
    }

    public string RoleInstance
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceRoleInstance);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceRoleInstance, value);
    }

    public string Ip
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceIp);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceIp, value);
    }

    public string MachineName
    {
      get => this.tags.GetTagValueOrNull(ContextTagKeys.Keys.DeviceMachineName);
      set => this.tags.SetStringValueOrRemove(ContextTagKeys.Keys.DeviceMachineName, value);
    }

    internal void SetDefaults(DeviceContextData source)
    {
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceType, source.Type);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceId, source.Id);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceOSVersion, source.OperatingSystem);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceOEMName, source.OemName);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceModel, source.Model);
      this.tags.InitializeTagValue<int?>(ContextTagKeys.Keys.DeviceNetwork, source.NetworkType);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceScreenResolution, source.ScreenResolution);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceLanguage, source.Language);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceIp, source.Ip);
      this.tags.InitializeTagValue<string>(ContextTagKeys.Keys.DeviceMachineName, source.MachineName);
    }
  }
}
