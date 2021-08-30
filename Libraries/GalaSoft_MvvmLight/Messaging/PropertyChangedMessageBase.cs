﻿// Decompiled with JetBrains decompiler
// Type: GalaSoft.MvvmLight.Messaging.PropertyChangedMessageBase
// Assembly: GalaSoft.MvvmLight, Version=5.0.2.32240, Culture=neutral, PublicKeyToken=e7570ab207bcb616
// MVID: 672AD33A-61F0-448A-AE1B-56983EAB4C33
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\GalaSoft_MvvmLight.dll

namespace GalaSoft.MvvmLight.Messaging
{
  public abstract class PropertyChangedMessageBase : MessageBase
  {
    protected PropertyChangedMessageBase(object sender, string propertyName)
      : base(sender)
    {
      this.PropertyName = propertyName;
    }

    protected PropertyChangedMessageBase(object sender, object target, string propertyName)
      : base(sender, target)
    {
      this.PropertyName = propertyName;
    }

    protected PropertyChangedMessageBase(string propertyName) => this.PropertyName = propertyName;

    public string PropertyName { get; protected set; }
  }
}
