// Decompiled with JetBrains decompiler
// Type: Microsoft.Practices.ObjectBuilder2.PropertySelectorPolicy`1
// Assembly: Microsoft.Practices.Unity, Version=3.5.1.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35
// MVID: 084A87B0-7628-41EC-95DE-FCD38CE75A19
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Practices_Unity.dll

using Microsoft.Practices.Unity.Utility;
using System;
using System.Reflection;

namespace Microsoft.Practices.ObjectBuilder2
{
  public class PropertySelectorPolicy<TResolutionAttribute> : 
    PropertySelectorBase<TResolutionAttribute>
    where TResolutionAttribute : Attribute
  {
    protected override IDependencyResolverPolicy CreateResolver(
      PropertyInfo property)
    {
      Guard.ArgumentNotNull((object) property, nameof (property));
      return (IDependencyResolverPolicy) new FixedTypeResolverPolicy(property.PropertyType);
    }
  }
}
