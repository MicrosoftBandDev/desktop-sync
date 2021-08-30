﻿// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Core.MvxObjectExtensions
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;

namespace Cirrious.CrossCore.Core
{
  public static class MvxObjectExtensions
  {
    public static void DisposeIfDisposable(this object thing)
    {
      if (!(thing is IDisposable disposable))
        return;
      disposable.Dispose();
    }
  }
}
