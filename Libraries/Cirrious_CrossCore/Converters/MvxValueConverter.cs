﻿// Decompiled with JetBrains decompiler
// Type: Cirrious.CrossCore.Converters.MvxValueConverter
// Assembly: Cirrious.CrossCore, Version=1.0.0.0, Culture=neutral, PublicKeyToken=e16445fd9b451819
// MVID: D5316BBF-25ED-4142-9846-D5815637A677
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Cirrious_CrossCore.dll

using System;
using System.Globalization;

namespace Cirrious.CrossCore.Converters
{
  public abstract class MvxValueConverter : IMvxValueConverter
  {
    public virtual object Convert(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) MvxBindingConstant.UnsetValue;
    }

    public virtual object ConvertBack(
      object value,
      Type targetType,
      object parameter,
      CultureInfo culture)
    {
      return (object) MvxBindingConstant.UnsetValue;
    }
  }
}
