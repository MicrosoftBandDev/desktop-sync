﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.DeviceFileSyncTimeInfo
// Assembly: Microsoft.Band.Admin.Desktop, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 14F573E4-478A-4BD1-B169-7232F63F8A40
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin_Desktop.dll

using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace Microsoft.Band.Admin
{
  [DataContract]
  internal sealed class DeviceFileSyncTimeInfo
  {
    private static readonly string[] DateTimeFormats = new string[9]
    {
      "o",
      "yyyy-MM-ddTHH:mm:sszzz",
      "yyyy-MM-ddTHH:mm:ss.fzzz",
      "yyyy-MM-ddTHH:mm:ss.ffzzz",
      "yyyy-MM-ddTHH:mm:ss.fffzzz",
      "yyyy-MM-ddTHH:mm:ssZ",
      "yyyy-MM-ddTHH:mm:ss.fZ",
      "yyyy-MM-ddTHH:mm:ss.ffZ",
      "yyyy-MM-ddTHH:mm:ss.fffZ"
    };
    private DateTime? lastDeviceFileDownloadAttemptTime;

    [DataMember(Name = "LastDeviceFileDownloadAttemptTime")]
    private string LastDeviceFileDownloadAttemptTimeSerialized
    {
      get => this.GetSerializedDateTimeValue(this.lastDeviceFileDownloadAttemptTime);
      set => this.SetDeserializedDateTimeValue(value, out this.lastDeviceFileDownloadAttemptTime);
    }

    public DateTime? LastDeviceFileDownloadAttemptTime
    {
      get => this.lastDeviceFileDownloadAttemptTime;
      set => this.lastDeviceFileDownloadAttemptTime = value;
    }

    private string GetSerializedDateTimeValue(DateTime? deserialized) => !deserialized.HasValue ? (string) null : deserialized.Value.ToString(DeviceFileSyncTimeInfo.DateTimeFormats[0]);

    private void SetDeserializedDateTimeValue(string serialized, out DateTime? deserialized)
    {
      DateTime result;
      if (DateTime.TryParseExact(serialized, DeviceFileSyncTimeInfo.DateTimeFormats, (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.AdjustToUniversal, out result))
        deserialized = new DateTime?(result);
      else
        deserialized = new DateTime?();
    }
  }
}
