﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Health.App.Core.Utilities.EnumUtilities
// Assembly: Microsoft.Health.App.Core, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: 647AFE6E-8F28-4A0E-818D-2655ABCF9984
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Health_App_Core.dll

using Microsoft.Health.App.Core.Resources;

namespace Microsoft.Health.App.Core.Utilities
{
  public static class EnumUtilities
  {
    public static string GetResource<T>(T enumValue) => EnumResources.ResourceManager.GetString(enumValue.GetType().Name + "_" + (object) enumValue) ?? EnumResources.Unconfigured;
  }
}
