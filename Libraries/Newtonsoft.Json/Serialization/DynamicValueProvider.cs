﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Serialization.DynamicValueProvider
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Globalization;
using System.Reflection;

namespace Newtonsoft.Json.Serialization
{
  public class DynamicValueProvider : IValueProvider
  {
    private readonly MemberInfo _memberInfo;
    private Func<object, object> _getter;
    private Action<object, object> _setter;

    public DynamicValueProvider(MemberInfo memberInfo)
    {
      ValidationUtils.ArgumentNotNull((object) memberInfo, nameof (memberInfo));
      this._memberInfo = memberInfo;
    }

    public void SetValue(object target, object value)
    {
      try
      {
        if (this._setter == null)
          this._setter = DynamicReflectionDelegateFactory.Instance.CreateSet<object>(this._memberInfo);
        this._setter(target, value);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error setting value to '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }

    public object GetValue(object target)
    {
      try
      {
        if (this._getter == null)
          this._getter = DynamicReflectionDelegateFactory.Instance.CreateGet<object>(this._memberInfo);
        return this._getter(target);
      }
      catch (Exception ex)
      {
        throw new JsonSerializationException("Error getting value from '{0}' on '{1}'.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) this._memberInfo.Name, (object) target.GetType()), ex);
      }
    }
  }
}
