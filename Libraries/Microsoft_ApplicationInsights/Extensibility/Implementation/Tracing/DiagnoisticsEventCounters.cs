// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing.DiagnoisticsEventCounters
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;

namespace Microsoft.ApplicationInsights.Extensibility.Implementation.Tracing
{
  internal class DiagnoisticsEventCounters
  {
    private readonly object syncRoot = new object();
    private volatile int execCount;

    internal DiagnoisticsEventCounters(int execCountInitial = 0) => this.execCount = execCountInitial;

    internal int ExecCount => this.execCount;

    internal int Increment()
    {
      this.syncRoot.ExecuteSpinWaitLock((Action) (() =>
      {
        if (int.MaxValue <= this.execCount)
          return;
        ++this.execCount;
      }));
      return this.execCount;
    }
  }
}
