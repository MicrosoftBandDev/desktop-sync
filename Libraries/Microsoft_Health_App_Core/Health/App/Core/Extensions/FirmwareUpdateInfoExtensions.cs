﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Extensions.FirmwareUpdateInfoExtensions
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Band.Admin;
using Microsoft.Health.App.Core.Models;
using System;

namespace Microsoft.Health.App.Core.Extensions
{
  public static class FirmwareUpdateInfoExtensions
  {
    public static UpdateInfo ToUpdateInfo(
      this IFirmwareUpdateInfo firmwareUpdateInfo,
      DateTimeOffset now)
    {
      return new UpdateInfo()
      {
        FirmwareVersion = firmwareUpdateInfo.FirmwareVersion,
        IsFirmwareUpdateAvailable = firmwareUpdateInfo.IsFirmwareUpdateAvailable,
        LastUpdateTime = now
      };
    }
  }
}
