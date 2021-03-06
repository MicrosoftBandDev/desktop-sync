// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.ArrayTypeInfo`1
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  internal sealed class ArrayTypeInfo<ElementType> : TraceLoggingTypeInfo<ElementType[]>
  {
    private readonly TraceLoggingTypeInfo<ElementType> elementInfo;

    public ArrayTypeInfo(TraceLoggingTypeInfo<ElementType> elementInfo) => this.elementInfo = elementInfo;

    public override void WriteMetadata(
      TraceLoggingMetadataCollector collector,
      string name,
      EventFieldFormat format)
    {
      collector.BeginBufferedArray();
      this.elementInfo.WriteMetadata(collector, name, format);
      collector.EndBufferedArray();
    }

    public override void WriteData(TraceLoggingDataCollector collector, ref ElementType[] value)
    {
      int bookmark = collector.BeginBufferedArray();
      int count = 0;
      if (value != null)
      {
        count = value.Length;
        for (int index = 0; index < value.Length; ++index)
          this.elementInfo.WriteData(collector, ref value[index]);
      }
      collector.EndBufferedArray(bookmark, count);
    }

    public override object GetData(object value)
    {
      ElementType[] elementTypeArray = (ElementType[]) value;
      object[] objArray = new object[elementTypeArray.Length];
      for (int index = 0; index < elementTypeArray.Length; ++index)
        objArray[index] = this.elementInfo.GetData((object) elementTypeArray[index]);
      return (object) objArray;
    }
  }
}
