﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.Unity.IRegisteredNamesPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.ObjectBuilder2;
using System;
using System.Collections.Generic;

namespace Microsoft.Practices.Unity
{
  public interface IRegisteredNamesPolicy : IBuilderPolicy
  {
    IEnumerable<string> GetRegisteredNames(Type type);
  }
}
