﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Utilities.StringUtils
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Newtonsoft.Json.Utilities
{
  internal static class StringUtils
  {
    public const string CarriageReturnLineFeed = "\r\n";
    public const string Empty = "";
    public const char CarriageReturn = '\r';
    public const char LineFeed = '\n';
    public const char Tab = '\t';

    public static string FormatWith(this string format, IFormatProvider provider, object arg0) => StringUtils.FormatWith(format, provider, new object[1]
    {
      arg0
    });

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object arg0,
      object arg1)
    {
      return StringUtils.FormatWith(format, provider, new object[2]
      {
        arg0,
        arg1
      });
    }

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object arg0,
      object arg1,
      object arg2)
    {
      return StringUtils.FormatWith(format, provider, new object[3]
      {
        arg0,
        arg1,
        arg2
      });
    }

    public static string FormatWith(
      this string format,
      IFormatProvider provider,
      object arg0,
      object arg1,
      object arg2,
      object arg3)
    {
      return StringUtils.FormatWith(format, provider, new object[4]
      {
        arg0,
        arg1,
        arg2,
        arg3
      });
    }

    private static string FormatWith(
      this string format,
      IFormatProvider provider,
      params object[] args)
    {
      ValidationUtils.ArgumentNotNull((object) format, nameof (format));
      return string.Format(provider, format, args);
    }

    public static bool IsWhiteSpace(string s)
    {
      switch (s)
      {
        case "":
          return false;
        case null:
          throw new ArgumentNullException(nameof (s));
        default:
          for (int index = 0; index < s.Length; ++index)
          {
            if (!char.IsWhiteSpace(s[index]))
              return false;
          }
          return true;
      }
    }

    public static string NullEmptyString(string s) => !string.IsNullOrEmpty(s) ? s : (string) null;

    public static StringWriter CreateStringWriter(int capacity) => new StringWriter(new StringBuilder(capacity), (IFormatProvider) CultureInfo.InvariantCulture);

    public static int? GetLength(string value) => value?.Length;

    public static void ToCharAsUnicode(char c, char[] buffer)
    {
      buffer[0] = '\\';
      buffer[1] = 'u';
      buffer[2] = MathUtils.IntToHex((int) c >> 12 & 15);
      buffer[3] = MathUtils.IntToHex((int) c >> 8 & 15);
      buffer[4] = MathUtils.IntToHex((int) c >> 4 & 15);
      buffer[5] = MathUtils.IntToHex((int) c & 15);
    }

    public static TSource ForgivingCaseSensitiveFind<TSource>(
      this IEnumerable<TSource> source,
      Func<TSource, string> valueSelector,
      string testValue)
    {
      if (source == null)
        throw new ArgumentNullException(nameof (source));
      if (valueSelector == null)
        throw new ArgumentNullException(nameof (valueSelector));
      IEnumerable<TSource> source1 = source.Where<TSource>((Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.OrdinalIgnoreCase)));
      return source1.Count<TSource>() <= 1 ? source1.SingleOrDefault<TSource>() : source.Where<TSource>((Func<TSource, bool>) (s => string.Equals(valueSelector(s), testValue, StringComparison.Ordinal))).SingleOrDefault<TSource>();
    }

    public static string ToCamelCase(string s)
    {
      if (string.IsNullOrEmpty(s) || !char.IsUpper(s[0]))
        return s;
      char[] charArray = s.ToCharArray();
      for (int index = 0; index < charArray.Length; ++index)
      {
        bool flag = index + 1 < charArray.Length;
        if (index <= 0 || !flag || char.IsUpper(charArray[index + 1]))
          charArray[index] = char.ToLower(charArray[index], CultureInfo.InvariantCulture);
        else
          break;
      }
      return new string(charArray);
    }

    public static bool IsHighSurrogate(char c) => char.IsHighSurrogate(c);

    public static bool IsLowSurrogate(char c) => char.IsLowSurrogate(c);

    public static bool StartsWith(this string source, char value) => source.Length > 0 && (int) source[0] == (int) value;

    public static bool EndsWith(this string source, char value) => source.Length > 0 && (int) source[source.Length - 1] == (int) value;
  }
}
