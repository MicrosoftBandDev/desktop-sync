﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.CargoSystemTime
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System;

namespace Microsoft.Band.Admin
{
  public class CargoSystemTime
  {
    private const int serializedByteCount = 16;

    public ushort Year { get; set; }

    public ushort Month { get; set; }

    public ushort DayOfWeek { get; set; }

    public ushort Day { get; set; }

    public ushort Hour { get; set; }

    public ushort Minute { get; set; }

    public ushort Second { get; set; }

    public ushort Milliseconds { get; set; }

    internal static int GetSerializedByteCount() => 16;

    internal static CargoSystemTime DeserializeFromBand(ICargoReader reader) => new CargoSystemTime()
    {
      Year = reader.ReadUInt16(),
      Month = reader.ReadUInt16(),
      DayOfWeek = reader.ReadUInt16(),
      Day = reader.ReadUInt16(),
      Hour = reader.ReadUInt16(),
      Minute = reader.ReadUInt16(),
      Second = reader.ReadUInt16(),
      Milliseconds = reader.ReadUInt16()
    };

    internal static DateTime DeserializeFromBandAsDateTime(
      ICargoReader reader,
      DateTimeKind kind)
    {
      return new DateTime((int) reader.ReadUInt16(), (int) reader.ReadUInt16(), (int) CargoSystemTime.DiscardUInt16ReturnNext(reader), (int) reader.ReadUInt16(), (int) reader.ReadUInt16(), (int) reader.ReadUInt16(), (int) reader.ReadUInt16(), kind);
    }

    private static ushort DiscardUInt16ReturnNext(ICargoReader reader)
    {
      reader.ReadExactAndDiscard(2);
      return reader.ReadUInt16();
    }

    internal void SerializeToBand(ICargoWriter writer)
    {
      writer.WriteUInt16(this.Year);
      writer.WriteUInt16(this.Month);
      writer.WriteUInt16(this.DayOfWeek);
      writer.WriteUInt16(this.Day);
      writer.WriteUInt16(this.Hour);
      writer.WriteUInt16(this.Minute);
      writer.WriteUInt16(this.Second);
      writer.WriteUInt16(this.Milliseconds);
    }
  }
}
