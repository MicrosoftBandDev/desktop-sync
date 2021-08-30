﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.Cloud.Client.IDeviceClient
// Assembly: Microsoft.Health.Cloud.Client, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: A3B3A7E2-B593-422B-B9F9-2AFA12370654
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_Cloud_Client.dll

using Microsoft.Health.Cloud.Client.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.Health.Cloud.Client
{
  public interface IDeviceClient
  {
    Task AddDevicesAsync(
      IEnumerable<RegisteredDeviceSettings> devices,
      CancellationToken cancellationToken);

    Task<IEnumerable<RegisteredDeviceSettings>> GetDevicesAsync(
      CancellationToken cancellationToken);

    Task RemoveDeviceAsync(string deviceId, CancellationToken cancellationToken);
  }
}
