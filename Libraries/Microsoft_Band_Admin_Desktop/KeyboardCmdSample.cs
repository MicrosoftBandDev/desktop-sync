﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.KeyboardCmdSample
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System.Runtime.InteropServices;

namespace Microsoft.Band.Admin
{
  [StructLayout(LayoutKind.Sequential, Pack = 1)]
  internal struct KeyboardCmdSample
  {
    internal const int MAX_NUM_OF_CANDIDATES = 4;
    internal const int MAX_KBDCMD_DATA_LEN = 400;
    internal KeyboardMessageType KeyboardMsgType;
    internal byte NumOfCandidates;
    internal byte WordIndex;
    internal uint DataLength;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 400)]
    internal byte[] Datafield;
  }
}
