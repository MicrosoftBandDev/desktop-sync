// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.TraceLoggingTypeInfo`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

using System;
using System.Collections.Generic;
using System.Threading;

namespace Microsoft.Diagnostics.Tracing
{
  internal abstract class TraceLoggingTypeInfo<DataType> : TraceLoggingTypeInfo
  {
    private static TraceLoggingTypeInfo<DataType> instance;

    protected TraceLoggingTypeInfo()
      : base(typeof (DataType))
    {
    }

    protected TraceLoggingTypeInfo(
      string name,
      EventLevel level,
      EventOpcode opcode,
      EventKeywords keywords,
      EventTags tags)
      : base(typeof (DataType), name, level, opcode, keywords, tags)
    {
    }

    public static TraceLoggingTypeInfo<DataType> Instance => TraceLoggingTypeInfo<DataType>.instance ?? TraceLoggingTypeInfo<DataType>.InitInstance();

    public abstract void WriteData(TraceLoggingDataCollector collector, ref DataType value);

    public override void WriteObjectData(TraceLoggingDataCollector collector, object value)
    {
      DataType dataType = value == null ? default (DataType) : (DataType) value;
      this.WriteData(collector, ref dataType);
    }

    internal static TraceLoggingTypeInfo<DataType> GetInstance(
      List<Type> recursionCheck)
    {
      if (TraceLoggingTypeInfo<DataType>.instance == null)
      {
        int count = recursionCheck.Count;
        TraceLoggingTypeInfo<DataType> defaultTypeInfo = Statics.CreateDefaultTypeInfo<DataType>(recursionCheck);
        Interlocked.CompareExchange<TraceLoggingTypeInfo<DataType>>(ref TraceLoggingTypeInfo<DataType>.instance, defaultTypeInfo, (TraceLoggingTypeInfo<DataType>) null);
        recursionCheck.RemoveRange(count, recursionCheck.Count - count);
      }
      return TraceLoggingTypeInfo<DataType>.instance;
    }

    private static TraceLoggingTypeInfo<DataType> InitInstance() => TraceLoggingTypeInfo<DataType>.GetInstance(new List<Type>());
  }
}
