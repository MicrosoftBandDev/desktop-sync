﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.TypeBasedOverride`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

namespace Microsoft.Practices.Unity
{
  public class TypeBasedOverride<T> : TypeBasedOverride
  {
    public TypeBasedOverride(ResolverOverride innerOverride)
      : base(typeof (T), innerOverride)
    {
    }
  }
}
