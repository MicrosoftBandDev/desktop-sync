﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.ViewModels.Themes.ApplicationThemeViewModelBase
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Mvvm;
using Microsoft.Health.App.Core.Themes;

namespace Microsoft.Health.App.Core.ViewModels.Themes
{
  public abstract class ApplicationThemeViewModelBase : HealthViewModelBase
  {
    public abstract ApplicationTheme Default { get; protected set; }

    public abstract ApplicationTheme Current { get; protected set; }
  }
}
