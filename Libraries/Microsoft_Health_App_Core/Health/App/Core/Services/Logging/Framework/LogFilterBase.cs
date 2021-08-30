﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Services.Logging.Framework.LogFilterBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

namespace Microsoft.Health.App.Core.Services.Logging.Framework
{
  public class LogFilterBase : ILogFilter
  {
    public ILogFilter NextFilter { get; set; }

    public string Name { get; set; }

    public ILogFilter Next() => this.NextFilter;

    public virtual bool IsFilteredOut(LoggingEvent loggingEvent) => false;
  }
}
