// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.Int16ArrayTypeInfo
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class Int16ArrayTypeInfo : TraceLoggingTypeInfo<short[]>
  {
    public override void WriteMetadata(
      TraceLoggingMetadataCollector collector,
      string name,
      EventFieldFormat format)
    {
      collector.AddArray(name, Statics.Format16(format, TraceLoggingDataType.Int16));
    }

    public override void WriteData(TraceLoggingDataCollector collector, ref short[] value) => collector.AddArray(value);
  }
}
