﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Models.ActiveLocationPath
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.Cloud.Client;
using System.Collections.Generic;

namespace Microsoft.Health.App.Core.Models
{
  public class ActiveLocationPath
  {
    public ActiveLocationPath(ICollection<Location> path, int scaledPace)
    {
      this.Path = path;
      this.ScaledPace = scaledPace;
    }

    public ICollection<Location> Path { get; private set; }

    public int ScaledPace { get; private set; }
  }
}
