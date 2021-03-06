// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Linq.JsonPath.ArrayMultipleIndexFilter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Collections.Generic;

namespace Newtonsoft.Json.Linq.JsonPath
{
  internal class ArrayMultipleIndexFilter : PathFilter
  {
    public List<int> Indexes { get; set; }

    public override IEnumerable<JToken> ExecuteFilter(
      IEnumerable<JToken> current,
      bool errorWhenNoMatch)
    {
      foreach (JToken t in current)
      {
        foreach (int i in this.Indexes)
        {
          JToken v = PathFilter.GetTokenIndex(t, errorWhenNoMatch, i);
          if (v != null)
            yield return v;
        }
      }
    }
  }
}
