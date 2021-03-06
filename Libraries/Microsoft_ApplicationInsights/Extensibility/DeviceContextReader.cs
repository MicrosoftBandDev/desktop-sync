// Decompiled with JetBrains decompiler
// Type: Microsoft.ApplicationInsights.Extensibility.DeviceContextReader
// Assembly: Microsoft.ApplicationInsights, Version=0.16.1.418, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 0F3F1F13-BE28-490B-A9F6-61E26D29AE67
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_ApplicationInsights.dll

using System;
using System.Globalization;
using System.Management;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.ApplicationInsights.Extensibility
{
  internal class DeviceContextReader : IDeviceContextReader
  {
    internal const string ContextPersistentStorageFileName = "ApplicationInsights.DeviceContext.xml";
    private string deviceId;
    private string operatingSystem;
    private string deviceManufacturer;
    private string deviceName;
    private int? networkType;
    private FallbackDeviceContext cachedContext;
    private static IDeviceContextReader instance;
    private readonly object syncRoot = new object();

    public virtual FallbackDeviceContext FallbackContext => this.ReadSerializedContext();

    public virtual void Initialize()
    {
    }

    public virtual string GetDeviceType() => "PC";

    public virtual string GetDeviceUniqueId()
    {
      if (this.deviceId != null)
        return this.deviceId;
      string domainName = IPGlobalProperties.GetIPGlobalProperties().DomainName;
      string str = Dns.GetHostName();
      if (!str.EndsWith(domainName, StringComparison.OrdinalIgnoreCase))
        str = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "{0}.{1}", (object) str, (object) domainName);
      return this.deviceId = str;
    }

    public virtual Task<string> GetOperatingSystemAsync()
    {
      if (this.operatingSystem == null)
        this.operatingSystem = string.Format((IFormatProvider) CultureInfo.InvariantCulture, "Windows NT {0}", (object) Environment.OSVersion.Version.ToString(4));
      TaskCompletionSource<string> completionSource = new TaskCompletionSource<string>();
      completionSource.SetResult(this.operatingSystem);
      return completionSource.Task;
    }

    public virtual string GetOemName() => this.deviceManufacturer != null ? this.deviceManufacturer : (this.deviceManufacturer = this.RunWmiQuery("Win32_ComputerSystem", "Manufacturer", string.Empty));

    public virtual string GetDeviceModel() => this.deviceName != null ? this.deviceName : (this.deviceName = this.RunWmiQuery("Win32_ComputerSystem", "Model", string.Empty));

    public int GetNetworkType()
    {
      if (this.networkType.HasValue)
        return this.networkType.Value;
      if (NetworkInterface.GetIsNetworkAvailable())
      {
        foreach (NetworkInterface networkInterface in NetworkInterface.GetAllNetworkInterfaces())
        {
          if (networkInterface.OperationalStatus == OperationalStatus.Up)
          {
            this.networkType = new int?((int) networkInterface.NetworkInterfaceType);
            return this.networkType.Value;
          }
        }
      }
      this.networkType = new int?(0);
      return this.networkType.Value;
    }

    public Task<string> GetScreenResolutionAsync()
    {
      TaskCompletionSource<string> completionSource = new TaskCompletionSource<string>();
      completionSource.SetResult(string.Empty);
      return completionSource.Task;
    }

    private FallbackDeviceContext ReadSerializedContext()
    {
      if (this.cachedContext != null)
        return this.cachedContext;
      lock (this.syncRoot)
      {
        if (this.cachedContext != null)
          return this.cachedContext;
        FallbackDeviceContext fallbackDeviceContext = Utils.ReadSerializedContext<FallbackDeviceContext>("ApplicationInsights.DeviceContext.xml");
        Thread.MemoryBarrier();
        this.cachedContext = fallbackDeviceContext;
      }
      return this.cachedContext;
    }

    private string RunWmiQuery(string table, string property, string defaultValue)
    {
      try
      {
        using (ManagementObjectSearcher managementObjectSearcher = new ManagementObjectSearcher(string.Format((IFormatProvider) CultureInfo.InvariantCulture, "SELECT {0} FROM {1}", (object) property, (object) table)))
        {
          foreach (ManagementBaseObject managementBaseObject in managementObjectSearcher.Get())
          {
            object obj = managementBaseObject[property];
            if (obj != null)
              return obj.ToString();
          }
        }
      }
      catch (Exception ex)
      {
      }
      return defaultValue;
    }

    internal DeviceContextReader()
    {
    }

    public static IDeviceContextReader Instance
    {
      get
      {
        if (DeviceContextReader.instance != null)
          return DeviceContextReader.instance;
        Interlocked.CompareExchange<IDeviceContextReader>(ref DeviceContextReader.instance, (IDeviceContextReader) new DeviceContextReader(), (IDeviceContextReader) null);
        DeviceContextReader.instance.Initialize();
        return DeviceContextReader.instance;
      }
      internal set => DeviceContextReader.instance = value;
    }

    public virtual string GetHostSystemLocale() => CultureInfo.CurrentCulture.Name;
  }
}
