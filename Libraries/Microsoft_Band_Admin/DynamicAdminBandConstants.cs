﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DynamicAdminBandConstants
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

namespace Microsoft.Band.Admin
{
  internal sealed class DynamicAdminBandConstants
  {
    internal static readonly DynamicAdminBandConstants Cargo = new DynamicAdminBandConstants(BandClass.Cargo);
    internal static readonly DynamicAdminBandConstants Envoy = new DynamicAdminBandConstants(BandClass.Envoy);

    private DynamicAdminBandConstants(BandClass bandClass) => this.BandClass = bandClass;

    public BandClass BandClass { get; private set; }

    public ushort BandProfileAppDataVersion
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 2;
          default:
            return 1;
        }
      }
    }

    public ushort BandProfileAppDataByteCount
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 397;
          default:
            return 128;
        }
      }
    }

    internal int RunMetricsDisplaySlotCount
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 7;
          default:
            return 5;
        }
      }
    }

    internal int BikeMetricsDisplaySlotCount
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 7;
          default:
            return 6;
        }
      }
    }

    internal ushort BandProfileDeviceReservedBytes
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 256;
          default:
            return 23;
        }
      }
    }

    internal ushort BandGoalsSerializedVersion
    {
      get
      {
        switch (this.BandClass)
        {
          case BandClass.Envoy:
            return 1;
          default:
            return 0;
        }
      }
    }
  }
}
