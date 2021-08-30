﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.IPlatformProvider
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;
using System.IO;

namespace Microsoft.Band.Admin
{
  internal interface IPlatformProvider
  {
    void Sleep(int milliseconds);

    string GetAssemblyVersion();

    Version GetHostOSVersion();

    string GetHostOS();

    string GetDefaultUserAgent(FirmwareVersions firmwareVersions);

    int MaxChunkRange { get; }

    byte[] ComputeHashMd5(byte[] data);

    byte[] ComputeHashMd5(Stream data);
  }
}
