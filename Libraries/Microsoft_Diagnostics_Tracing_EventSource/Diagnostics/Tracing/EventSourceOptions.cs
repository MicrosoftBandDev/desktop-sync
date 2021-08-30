﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Diagnostics.Tracing.EventSourceOptions
// Assembly: Microsoft.Diagnostics.Tracing.EventSource, Version=1.1.16.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
// MVID: 5BB68207-0B7F-412A-8836-4E370F261506
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Diagnostics_Tracing_EventSource.dll

namespace Microsoft.Diagnostics.Tracing
{
  public struct EventSourceOptions
  {
    internal const byte keywordsSet = 1;
    internal const byte tagsSet = 2;
    internal const byte levelSet = 4;
    internal const byte opcodeSet = 8;
    internal const byte activityOptionsSet = 16;
    internal EventKeywords keywords;
    internal EventTags tags;
    internal EventActivityOptions activityOptions;
    internal byte level;
    internal byte opcode;
    internal byte valuesSet;

    public EventLevel Level
    {
      get => (EventLevel) this.level;
      set
      {
        this.level = checked ((byte) (uint) value);
        this.valuesSet |= (byte) 4;
      }
    }

    public EventOpcode Opcode
    {
      get => (EventOpcode) this.opcode;
      set
      {
        this.opcode = checked ((byte) (uint) value);
        this.valuesSet |= (byte) 8;
      }
    }

    internal bool IsOpcodeSet => ((int) this.valuesSet & 8) != 0;

    public EventKeywords Keywords
    {
      get => this.keywords;
      set
      {
        this.keywords = value;
        this.valuesSet |= (byte) 1;
      }
    }

    public EventTags Tags
    {
      get => this.tags;
      set
      {
        this.tags = value;
        this.valuesSet |= (byte) 2;
      }
    }

    public EventActivityOptions ActivityOptions
    {
      get => this.activityOptions;
      set
      {
        this.activityOptions = value;
        this.valuesSet |= (byte) 16;
      }
    }
  }
}
