﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.OverriddenBuildPlanMarkerPolicy
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Properties;
using System;

namespace Microsoft.Practices.ObjectBuilder2
{
  internal class OverriddenBuildPlanMarkerPolicy : IBuildPlanPolicy, IBuilderPolicy
  {
    public void BuildUp(IBuilderContext context) => throw new InvalidOperationException(Resources.MarkerBuildPlanInvoked);
  }
}
