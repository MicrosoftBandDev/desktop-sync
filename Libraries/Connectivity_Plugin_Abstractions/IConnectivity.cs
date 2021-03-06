// Decompiled with JetBrains decompiler
// Type: Connectivity.Plugin.Abstractions.IConnectivity
// Assembly: Connectivity.Plugin.Abstractions, Version=1.0.2.0, Culture=neutral, PublicKeyToken=null
// MVID: AB69076D-CAA0-4B7C-9B1E-3DD73914B51F
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Connectivity_Plugin_Abstractions.dll

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Connectivity.Plugin.Abstractions
{
  public interface IConnectivity : IDisposable
  {
    bool IsConnected { get; }

    Task<bool> IsReachable(string host, int msTimeout = 5000);

    Task<bool> IsRemoteReachable(string host, int port = 80, int msTimeout = 5000);

    IEnumerable<ConnectionType> ConnectionTypes { get; }

    IEnumerable<ulong> Bandwidths { get; }

    event ConnectivityChangedEventHandler ConnectivityChanged;
  }
}
