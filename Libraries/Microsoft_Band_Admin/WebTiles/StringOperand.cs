﻿// Decompiled with JetBrains decompiler
// Type: Microsoft.Band.Admin.WebTiles.StringOperand
// Assembly: Microsoft.Band.Admin, Version=1.3.20517.1, Culture=neutral, PublicKeyToken=null
// MVID: FA971F26-9473-45C8-99C9-634D5B7E7758
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Microsoft_Band_Admin.dll

using System.Collections.Generic;

namespace Microsoft.Band.Admin.WebTiles
{
  public class StringOperand : Operand
  {
    private StringOperand(string tokenValue, int position)
      : base(tokenValue, position)
    {
    }

    public static StringOperand Create(string tokenValue, int position) => new StringOperand(tokenValue, position);

    public override object GetValue(Dictionary<string, string> variableValues, bool stringRequired) => (object) StringTokenizer.RemoveEscapes(this.MatchedString.Substring(1, this.MatchedString.Length - 2));
  }
}
