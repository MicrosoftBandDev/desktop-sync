// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.JsonWriter
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using Newtonsoft.Json.Utilities;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Numerics;

namespace Newtonsoft.Json
{
  public abstract class JsonWriter : IDisposable
  {
    private static readonly JsonWriter.State[][] StateArray;
    internal static readonly JsonWriter.State[][] StateArrayTempate = new JsonWriter.State[8][]
    {
      new JsonWriter.State[10]
      {
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.ConstructorStart,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Property,
        JsonWriter.State.Property,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Property,
        JsonWriter.State.ObjectStart,
        JsonWriter.State.Object,
        JsonWriter.State.ArrayStart,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      },
      new JsonWriter.State[10]
      {
        JsonWriter.State.Start,
        JsonWriter.State.Object,
        JsonWriter.State.Error,
        JsonWriter.State.Error,
        JsonWriter.State.Array,
        JsonWriter.State.Array,
        JsonWriter.State.Constructor,
        JsonWriter.State.Constructor,
        JsonWriter.State.Error,
        JsonWriter.State.Error
      }
    };
    private readonly List<JsonPosition> _stack;
    private JsonPosition _currentPosition;
    private JsonWriter.State _currentState;
    private Formatting _formatting;
    private DateFormatHandling _dateFormatHandling;
    private DateTimeZoneHandling _dateTimeZoneHandling;
    private StringEscapeHandling _stringEscapeHandling;
    private FloatFormatHandling _floatFormatHandling;
    private string _dateFormatString;
    private CultureInfo _culture;

    internal static JsonWriter.State[][] BuildStateArray()
    {
      List<JsonWriter.State[]> list = ((IEnumerable<JsonWriter.State[]>) JsonWriter.StateArrayTempate).ToList<JsonWriter.State[]>();
      JsonWriter.State[] stateArray1 = JsonWriter.StateArrayTempate[0];
      JsonWriter.State[] stateArray2 = JsonWriter.StateArrayTempate[7];
      foreach (JsonToken jsonToken in (IEnumerable<object>) EnumUtils.GetValues(typeof (JsonToken)))
      {
        if ((JsonToken) list.Count <= jsonToken)
        {
          switch (jsonToken)
          {
            case JsonToken.Integer:
            case JsonToken.Float:
            case JsonToken.String:
            case JsonToken.Boolean:
            case JsonToken.Null:
            case JsonToken.Undefined:
            case JsonToken.Date:
            case JsonToken.Bytes:
              list.Add(stateArray2);
              continue;
            default:
              list.Add(stateArray1);
              continue;
          }
        }
      }
      return list.ToArray();
    }

    static JsonWriter() => JsonWriter.StateArray = JsonWriter.BuildStateArray();

    public bool CloseOutput { get; set; }

    protected internal int Top
    {
      get
      {
        int count = this._stack.Count;
        if (this.Peek() != JsonContainerType.None)
          ++count;
        return count;
      }
    }

    public WriteState WriteState
    {
      get
      {
        switch (this._currentState)
        {
          case JsonWriter.State.Start:
            return WriteState.Start;
          case JsonWriter.State.Property:
            return WriteState.Property;
          case JsonWriter.State.ObjectStart:
          case JsonWriter.State.Object:
            return WriteState.Object;
          case JsonWriter.State.ArrayStart:
          case JsonWriter.State.Array:
            return WriteState.Array;
          case JsonWriter.State.ConstructorStart:
          case JsonWriter.State.Constructor:
            return WriteState.Constructor;
          case JsonWriter.State.Closed:
            return WriteState.Closed;
          case JsonWriter.State.Error:
            return WriteState.Error;
          default:
            throw JsonWriterException.Create(this, "Invalid state: " + (object) this._currentState, (Exception) null);
        }
      }
    }

    internal string ContainerPath => this._currentPosition.Type == JsonContainerType.None ? string.Empty : JsonPosition.BuildPath((IEnumerable<JsonPosition>) this._stack);

    public string Path
    {
      get
      {
        if (this._currentPosition.Type == JsonContainerType.None)
          return string.Empty;
        IEnumerable<JsonPosition> positions;
        if (this._currentState != JsonWriter.State.ArrayStart && this._currentState != JsonWriter.State.ConstructorStart && this._currentState != JsonWriter.State.ObjectStart)
          positions = this._stack.Concat<JsonPosition>((IEnumerable<JsonPosition>) new JsonPosition[1]
          {
            this._currentPosition
          });
        else
          positions = (IEnumerable<JsonPosition>) this._stack;
        return JsonPosition.BuildPath(positions);
      }
    }

    public Formatting Formatting
    {
      get => this._formatting;
      set => this._formatting = value;
    }

    public DateFormatHandling DateFormatHandling
    {
      get => this._dateFormatHandling;
      set => this._dateFormatHandling = value;
    }

    public DateTimeZoneHandling DateTimeZoneHandling
    {
      get => this._dateTimeZoneHandling;
      set => this._dateTimeZoneHandling = value;
    }

    public StringEscapeHandling StringEscapeHandling
    {
      get => this._stringEscapeHandling;
      set
      {
        this._stringEscapeHandling = value;
        this.OnStringEscapeHandlingChanged();
      }
    }

    internal virtual void OnStringEscapeHandlingChanged()
    {
    }

    public FloatFormatHandling FloatFormatHandling
    {
      get => this._floatFormatHandling;
      set => this._floatFormatHandling = value;
    }

    public string DateFormatString
    {
      get => this._dateFormatString;
      set => this._dateFormatString = value;
    }

    public CultureInfo Culture
    {
      get => this._culture ?? CultureInfo.InvariantCulture;
      set => this._culture = value;
    }

    protected JsonWriter()
    {
      this._stack = new List<JsonPosition>(4);
      this._currentState = JsonWriter.State.Start;
      this._formatting = Formatting.None;
      this._dateTimeZoneHandling = DateTimeZoneHandling.RoundtripKind;
      this.CloseOutput = true;
    }

    internal void UpdateScopeWithFinishedValue()
    {
      if (!this._currentPosition.HasIndex)
        return;
      ++this._currentPosition.Position;
    }

    private void Push(JsonContainerType value)
    {
      if (this._currentPosition.Type != JsonContainerType.None)
        this._stack.Add(this._currentPosition);
      this._currentPosition = new JsonPosition(value);
    }

    private JsonContainerType Pop()
    {
      JsonPosition currentPosition = this._currentPosition;
      if (this._stack.Count > 0)
      {
        this._currentPosition = this._stack[this._stack.Count - 1];
        this._stack.RemoveAt(this._stack.Count - 1);
      }
      else
        this._currentPosition = new JsonPosition();
      return currentPosition.Type;
    }

    private JsonContainerType Peek() => this._currentPosition.Type;

    public abstract void Flush();

    public virtual void Close() => this.AutoCompleteAll();

    public virtual void WriteStartObject() => this.InternalWriteStart(JsonToken.StartObject, JsonContainerType.Object);

    public virtual void WriteEndObject() => this.InternalWriteEnd(JsonContainerType.Object);

    public virtual void WriteStartArray() => this.InternalWriteStart(JsonToken.StartArray, JsonContainerType.Array);

    public virtual void WriteEndArray() => this.InternalWriteEnd(JsonContainerType.Array);

    public virtual void WriteStartConstructor(string name) => this.InternalWriteStart(JsonToken.StartConstructor, JsonContainerType.Constructor);

    public virtual void WriteEndConstructor() => this.InternalWriteEnd(JsonContainerType.Constructor);

    public virtual void WritePropertyName(string name) => this.InternalWritePropertyName(name);

    public virtual void WritePropertyName(string name, bool escape) => this.WritePropertyName(name);

    public virtual void WriteEnd() => this.WriteEnd(this.Peek());

    public void WriteToken(JsonReader reader) => this.WriteToken(reader, true, true);

    public void WriteToken(JsonReader reader, bool writeChildren)
    {
      ValidationUtils.ArgumentNotNull((object) reader, nameof (reader));
      this.WriteToken(reader, writeChildren, true);
    }

    public void WriteToken(JsonToken token, object value) => this.WriteTokenInternal(token, value);

    public void WriteToken(JsonToken token) => this.WriteTokenInternal(token, (object) null);

    internal void WriteToken(
      JsonReader reader,
      bool writeChildren,
      bool writeDateConstructorAsDate)
    {
      int initialDepth = reader.TokenType != JsonToken.None ? (JsonTokenUtils.IsStartToken(reader.TokenType) ? reader.Depth : reader.Depth + 1) : -1;
      this.WriteToken(reader, initialDepth, writeChildren, writeDateConstructorAsDate);
    }

    internal void WriteToken(
      JsonReader reader,
      int initialDepth,
      bool writeChildren,
      bool writeDateConstructorAsDate)
    {
      do
      {
        if (writeDateConstructorAsDate && reader.TokenType == JsonToken.StartConstructor && string.Equals(reader.Value.ToString(), "Date", StringComparison.Ordinal))
          this.WriteConstructorDate(reader);
        else
          this.WriteTokenInternal(reader.TokenType, reader.Value);
      }
      while (initialDepth - 1 < reader.Depth - (JsonTokenUtils.IsEndToken(reader.TokenType) ? 1 : 0) && writeChildren && reader.Read());
    }

    private void WriteTokenInternal(JsonToken tokenType, object value)
    {
      switch (tokenType)
      {
        case JsonToken.None:
          break;
        case JsonToken.StartObject:
          this.WriteStartObject();
          break;
        case JsonToken.StartArray:
          this.WriteStartArray();
          break;
        case JsonToken.StartConstructor:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          this.WriteStartConstructor(value.ToString());
          break;
        case JsonToken.PropertyName:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          this.WritePropertyName(value.ToString());
          break;
        case JsonToken.Comment:
          this.WriteComment(value?.ToString());
          break;
        case JsonToken.Raw:
          this.WriteRawValue(value?.ToString());
          break;
        case JsonToken.Integer:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          if (value is BigInteger bigInteger2)
          {
            this.WriteValue((object) bigInteger2);
            break;
          }
          this.WriteValue(Convert.ToInt64(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JsonToken.Float:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          switch (value)
          {
            case Decimal num4:
              this.WriteValue(num4);
              return;
            case double num5:
              this.WriteValue(num5);
              return;
            case float num6:
              this.WriteValue(num6);
              return;
            default:
              this.WriteValue(Convert.ToDouble(value, (IFormatProvider) CultureInfo.InvariantCulture));
              return;
          }
        case JsonToken.String:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          this.WriteValue(value.ToString());
          break;
        case JsonToken.Boolean:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          this.WriteValue(Convert.ToBoolean(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JsonToken.Null:
          this.WriteNull();
          break;
        case JsonToken.Undefined:
          this.WriteUndefined();
          break;
        case JsonToken.EndObject:
          this.WriteEndObject();
          break;
        case JsonToken.EndArray:
          this.WriteEndArray();
          break;
        case JsonToken.EndConstructor:
          this.WriteEndConstructor();
          break;
        case JsonToken.Date:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          if (value is DateTimeOffset dateTimeOffset2)
          {
            this.WriteValue(dateTimeOffset2);
            break;
          }
          this.WriteValue(Convert.ToDateTime(value, (IFormatProvider) CultureInfo.InvariantCulture));
          break;
        case JsonToken.Bytes:
          ValidationUtils.ArgumentNotNull(value, nameof (value));
          if (value is Guid guid2)
          {
            this.WriteValue(guid2);
            break;
          }
          this.WriteValue((byte[]) value);
          break;
        default:
          throw MiscellaneousUtils.CreateArgumentOutOfRangeException("TokenType", (object) tokenType, "Unexpected token type.");
      }
    }

    private void WriteConstructorDate(JsonReader reader)
    {
      if (!reader.Read())
        throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", (Exception) null);
      DateTime dateTime = reader.TokenType == JsonToken.Integer ? DateTimeUtils.ConvertJavaScriptTicksToDateTime((long) reader.Value) : throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected Integer, got " + (object) reader.TokenType, (Exception) null);
      if (!reader.Read())
        throw JsonWriterException.Create(this, "Unexpected end when reading date constructor.", (Exception) null);
      if (reader.TokenType != JsonToken.EndConstructor)
        throw JsonWriterException.Create(this, "Unexpected token when reading date constructor. Expected EndConstructor, got " + (object) reader.TokenType, (Exception) null);
      this.WriteValue(dateTime);
    }

    private void WriteEnd(JsonContainerType type)
    {
      switch (type)
      {
        case JsonContainerType.Object:
          this.WriteEndObject();
          break;
        case JsonContainerType.Array:
          this.WriteEndArray();
          break;
        case JsonContainerType.Constructor:
          this.WriteEndConstructor();
          break;
        default:
          throw JsonWriterException.Create(this, "Unexpected type when writing end: " + (object) type, (Exception) null);
      }
    }

    private void AutoCompleteAll()
    {
      while (this.Top > 0)
        this.WriteEnd();
    }

    private JsonToken GetCloseTokenForType(JsonContainerType type)
    {
      switch (type)
      {
        case JsonContainerType.Object:
          return JsonToken.EndObject;
        case JsonContainerType.Array:
          return JsonToken.EndArray;
        case JsonContainerType.Constructor:
          return JsonToken.EndConstructor;
        default:
          throw JsonWriterException.Create(this, "No close token for type: " + (object) type, (Exception) null);
      }
    }

    private void AutoCompleteClose(JsonContainerType type)
    {
      int num1 = 0;
      if (this._currentPosition.Type == type)
      {
        num1 = 1;
      }
      else
      {
        int num2 = this.Top - 2;
        for (int index = num2; index >= 0; --index)
        {
          if (this._stack[num2 - index].Type == type)
          {
            num1 = index + 2;
            break;
          }
        }
      }
      if (num1 == 0)
        throw JsonWriterException.Create(this, "No token to close.", (Exception) null);
      for (int index = 0; index < num1; ++index)
      {
        JsonToken closeTokenForType = this.GetCloseTokenForType(this.Pop());
        if (this._currentState == JsonWriter.State.Property)
          this.WriteNull();
        if (this._formatting == Formatting.Indented && this._currentState != JsonWriter.State.ObjectStart && this._currentState != JsonWriter.State.ArrayStart)
          this.WriteIndent();
        this.WriteEnd(closeTokenForType);
        JsonContainerType jsonContainerType = this.Peek();
        switch (jsonContainerType)
        {
          case JsonContainerType.None:
            this._currentState = JsonWriter.State.Start;
            break;
          case JsonContainerType.Object:
            this._currentState = JsonWriter.State.Object;
            break;
          case JsonContainerType.Array:
            this._currentState = JsonWriter.State.Array;
            break;
          case JsonContainerType.Constructor:
            this._currentState = JsonWriter.State.Array;
            break;
          default:
            throw JsonWriterException.Create(this, "Unknown JsonType: " + (object) jsonContainerType, (Exception) null);
        }
      }
    }

    protected virtual void WriteEnd(JsonToken token)
    {
    }

    protected virtual void WriteIndent()
    {
    }

    protected virtual void WriteValueDelimiter()
    {
    }

    protected virtual void WriteIndentSpace()
    {
    }

    internal void AutoComplete(JsonToken tokenBeingWritten)
    {
      JsonWriter.State state = JsonWriter.StateArray[(int) tokenBeingWritten][(int) this._currentState];
      if (state == JsonWriter.State.Error)
        throw JsonWriterException.Create(this, "Token {0} in state {1} would result in an invalid JSON object.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) tokenBeingWritten.ToString(), (object) this._currentState.ToString()), (Exception) null);
      if ((this._currentState == JsonWriter.State.Object || this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.Constructor) && tokenBeingWritten != JsonToken.Comment)
        this.WriteValueDelimiter();
      if (this._formatting == Formatting.Indented)
      {
        if (this._currentState == JsonWriter.State.Property)
          this.WriteIndentSpace();
        if (this._currentState == JsonWriter.State.Array || this._currentState == JsonWriter.State.ArrayStart || this._currentState == JsonWriter.State.Constructor || this._currentState == JsonWriter.State.ConstructorStart || tokenBeingWritten == JsonToken.PropertyName && this._currentState != JsonWriter.State.Start)
          this.WriteIndent();
      }
      this._currentState = state;
    }

    public virtual void WriteNull() => this.InternalWriteValue(JsonToken.Null);

    public virtual void WriteUndefined() => this.InternalWriteValue(JsonToken.Undefined);

    public virtual void WriteRaw(string json) => this.InternalWriteRaw();

    public virtual void WriteRawValue(string json)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(JsonToken.Undefined);
      this.WriteRaw(json);
    }

    public virtual void WriteValue(string value) => this.InternalWriteValue(JsonToken.String);

    public virtual void WriteValue(int value) => this.InternalWriteValue(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(uint value) => this.InternalWriteValue(JsonToken.Integer);

    public virtual void WriteValue(long value) => this.InternalWriteValue(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong value) => this.InternalWriteValue(JsonToken.Integer);

    public virtual void WriteValue(float value) => this.InternalWriteValue(JsonToken.Float);

    public virtual void WriteValue(double value) => this.InternalWriteValue(JsonToken.Float);

    public virtual void WriteValue(bool value) => this.InternalWriteValue(JsonToken.Boolean);

    public virtual void WriteValue(short value) => this.InternalWriteValue(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort value) => this.InternalWriteValue(JsonToken.Integer);

    public virtual void WriteValue(char value) => this.InternalWriteValue(JsonToken.String);

    public virtual void WriteValue(byte value) => this.InternalWriteValue(JsonToken.Integer);

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte value) => this.InternalWriteValue(JsonToken.Integer);

    public virtual void WriteValue(Decimal value) => this.InternalWriteValue(JsonToken.Float);

    public virtual void WriteValue(DateTime value) => this.InternalWriteValue(JsonToken.Date);

    public virtual void WriteValue(DateTimeOffset value) => this.InternalWriteValue(JsonToken.Date);

    public virtual void WriteValue(Guid value) => this.InternalWriteValue(JsonToken.String);

    public virtual void WriteValue(TimeSpan value) => this.InternalWriteValue(JsonToken.String);

    public virtual void WriteValue(int? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(uint? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(long? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ulong? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(float? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(double? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(bool? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(short? value)
    {
      short? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(ushort? value)
    {
      ushort? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(char? value)
    {
      char? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte? value)
    {
      byte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    [CLSCompliant(false)]
    public virtual void WriteValue(sbyte? value)
    {
      sbyte? nullable = value;
      if (!(nullable.HasValue ? new int?((int) nullable.GetValueOrDefault()) : new int?()).HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Decimal? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(DateTime? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(DateTimeOffset? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(Guid? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(TimeSpan? value)
    {
      if (!value.HasValue)
        this.WriteNull();
      else
        this.WriteValue(value.Value);
    }

    public virtual void WriteValue(byte[] value)
    {
      if (value == null)
        this.WriteNull();
      else
        this.InternalWriteValue(JsonToken.Bytes);
    }

    public virtual void WriteValue(Uri value)
    {
      if (value == (Uri) null)
        this.WriteNull();
      else
        this.InternalWriteValue(JsonToken.String);
    }

    public virtual void WriteValue(object value)
    {
      if (value == null)
      {
        this.WriteNull();
      }
      else
      {
        if (value is BigInteger)
          throw JsonWriter.CreateUnsupportedTypeException(this, value);
        JsonWriter.WriteValue(this, ConvertUtils.GetTypeCode(value.GetType()), value);
      }
    }

    public virtual void WriteComment(string text) => this.InternalWriteComment();

    public virtual void WriteWhitespace(string ws) => this.InternalWriteWhitespace(ws);

    void IDisposable.Dispose() => this.Dispose(true);

    private void Dispose(bool disposing)
    {
      if (this._currentState == JsonWriter.State.Closed)
        return;
      this.Close();
    }

    internal static void WriteValue(JsonWriter writer, PrimitiveTypeCode typeCode, object value)
    {
      switch (typeCode)
      {
        case PrimitiveTypeCode.Char:
          writer.WriteValue((char) value);
          break;
        case PrimitiveTypeCode.CharNullable:
          writer.WriteValue(value == null ? new char?() : new char?((char) value));
          break;
        case PrimitiveTypeCode.Boolean:
          writer.WriteValue((bool) value);
          break;
        case PrimitiveTypeCode.BooleanNullable:
          writer.WriteValue(value == null ? new bool?() : new bool?((bool) value));
          break;
        case PrimitiveTypeCode.SByte:
          writer.WriteValue((sbyte) value);
          break;
        case PrimitiveTypeCode.SByteNullable:
          writer.WriteValue(value == null ? new sbyte?() : new sbyte?((sbyte) value));
          break;
        case PrimitiveTypeCode.Int16:
          writer.WriteValue((short) value);
          break;
        case PrimitiveTypeCode.Int16Nullable:
          writer.WriteValue(value == null ? new short?() : new short?((short) value));
          break;
        case PrimitiveTypeCode.UInt16:
          writer.WriteValue((ushort) value);
          break;
        case PrimitiveTypeCode.UInt16Nullable:
          writer.WriteValue(value == null ? new ushort?() : new ushort?((ushort) value));
          break;
        case PrimitiveTypeCode.Int32:
          writer.WriteValue((int) value);
          break;
        case PrimitiveTypeCode.Int32Nullable:
          writer.WriteValue(value == null ? new int?() : new int?((int) value));
          break;
        case PrimitiveTypeCode.Byte:
          writer.WriteValue((byte) value);
          break;
        case PrimitiveTypeCode.ByteNullable:
          writer.WriteValue(value == null ? new byte?() : new byte?((byte) value));
          break;
        case PrimitiveTypeCode.UInt32:
          writer.WriteValue((uint) value);
          break;
        case PrimitiveTypeCode.UInt32Nullable:
          writer.WriteValue(value == null ? new uint?() : new uint?((uint) value));
          break;
        case PrimitiveTypeCode.Int64:
          writer.WriteValue((long) value);
          break;
        case PrimitiveTypeCode.Int64Nullable:
          writer.WriteValue(value == null ? new long?() : new long?((long) value));
          break;
        case PrimitiveTypeCode.UInt64:
          writer.WriteValue((ulong) value);
          break;
        case PrimitiveTypeCode.UInt64Nullable:
          writer.WriteValue(value == null ? new ulong?() : new ulong?((ulong) value));
          break;
        case PrimitiveTypeCode.Single:
          writer.WriteValue((float) value);
          break;
        case PrimitiveTypeCode.SingleNullable:
          writer.WriteValue(value == null ? new float?() : new float?((float) value));
          break;
        case PrimitiveTypeCode.Double:
          writer.WriteValue((double) value);
          break;
        case PrimitiveTypeCode.DoubleNullable:
          writer.WriteValue(value == null ? new double?() : new double?((double) value));
          break;
        case PrimitiveTypeCode.DateTime:
          writer.WriteValue((DateTime) value);
          break;
        case PrimitiveTypeCode.DateTimeNullable:
          writer.WriteValue(value == null ? new DateTime?() : new DateTime?((DateTime) value));
          break;
        case PrimitiveTypeCode.DateTimeOffset:
          writer.WriteValue((DateTimeOffset) value);
          break;
        case PrimitiveTypeCode.DateTimeOffsetNullable:
          writer.WriteValue(value == null ? new DateTimeOffset?() : new DateTimeOffset?((DateTimeOffset) value));
          break;
        case PrimitiveTypeCode.Decimal:
          writer.WriteValue((Decimal) value);
          break;
        case PrimitiveTypeCode.DecimalNullable:
          writer.WriteValue(value == null ? new Decimal?() : new Decimal?((Decimal) value));
          break;
        case PrimitiveTypeCode.Guid:
          writer.WriteValue((Guid) value);
          break;
        case PrimitiveTypeCode.GuidNullable:
          writer.WriteValue(value == null ? new Guid?() : new Guid?((Guid) value));
          break;
        case PrimitiveTypeCode.TimeSpan:
          writer.WriteValue((TimeSpan) value);
          break;
        case PrimitiveTypeCode.TimeSpanNullable:
          writer.WriteValue(value == null ? new TimeSpan?() : new TimeSpan?((TimeSpan) value));
          break;
        case PrimitiveTypeCode.BigInteger:
          writer.WriteValue((object) (BigInteger) value);
          break;
        case PrimitiveTypeCode.BigIntegerNullable:
          writer.WriteValue((object) (value == null ? new BigInteger?() : new BigInteger?((BigInteger) value)));
          break;
        case PrimitiveTypeCode.Uri:
          writer.WriteValue((Uri) value);
          break;
        case PrimitiveTypeCode.String:
          writer.WriteValue((string) value);
          break;
        case PrimitiveTypeCode.Bytes:
          writer.WriteValue((byte[]) value);
          break;
        case PrimitiveTypeCode.DBNull:
          writer.WriteNull();
          break;
        default:
          IConvertible convertable = value is IConvertible ? (IConvertible) value : throw JsonWriter.CreateUnsupportedTypeException(writer, value);
          TypeInformation typeInformation = ConvertUtils.GetTypeInformation(convertable);
          PrimitiveTypeCode typeCode1 = typeInformation.TypeCode == PrimitiveTypeCode.Object ? PrimitiveTypeCode.String : typeInformation.TypeCode;
          Type conversionType = typeInformation.TypeCode == PrimitiveTypeCode.Object ? typeof (string) : typeInformation.Type;
          object type = convertable.ToType(conversionType, (IFormatProvider) CultureInfo.InvariantCulture);
          JsonWriter.WriteValue(writer, typeCode1, type);
          break;
      }
    }

    private static JsonWriterException CreateUnsupportedTypeException(
      JsonWriter writer,
      object value)
    {
      return JsonWriterException.Create(writer, "Unsupported type: {0}. Use the JsonSerializer class to get the object's JSON representation.".FormatWith((IFormatProvider) CultureInfo.InvariantCulture, (object) value.GetType()), (Exception) null);
    }

    protected void SetWriteState(JsonToken token, object value)
    {
      switch (token)
      {
        case JsonToken.StartObject:
          this.InternalWriteStart(token, JsonContainerType.Object);
          break;
        case JsonToken.StartArray:
          this.InternalWriteStart(token, JsonContainerType.Array);
          break;
        case JsonToken.StartConstructor:
          this.InternalWriteStart(token, JsonContainerType.Constructor);
          break;
        case JsonToken.PropertyName:
          if (!(value is string))
            throw new ArgumentException("A name is required when setting property name state.", nameof (value));
          this.InternalWritePropertyName((string) value);
          break;
        case JsonToken.Comment:
          this.InternalWriteComment();
          break;
        case JsonToken.Raw:
          this.InternalWriteRaw();
          break;
        case JsonToken.Integer:
        case JsonToken.Float:
        case JsonToken.String:
        case JsonToken.Boolean:
        case JsonToken.Null:
        case JsonToken.Undefined:
        case JsonToken.Date:
        case JsonToken.Bytes:
          this.InternalWriteValue(token);
          break;
        case JsonToken.EndObject:
          this.InternalWriteEnd(JsonContainerType.Object);
          break;
        case JsonToken.EndArray:
          this.InternalWriteEnd(JsonContainerType.Array);
          break;
        case JsonToken.EndConstructor:
          this.InternalWriteEnd(JsonContainerType.Constructor);
          break;
        default:
          throw new ArgumentOutOfRangeException(nameof (token));
      }
    }

    internal void InternalWriteEnd(JsonContainerType container) => this.AutoCompleteClose(container);

    internal void InternalWritePropertyName(string name)
    {
      this._currentPosition.PropertyName = name;
      this.AutoComplete(JsonToken.PropertyName);
    }

    internal void InternalWriteRaw()
    {
    }

    internal void InternalWriteStart(JsonToken token, JsonContainerType container)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(token);
      this.Push(container);
    }

    internal void InternalWriteValue(JsonToken token)
    {
      this.UpdateScopeWithFinishedValue();
      this.AutoComplete(token);
    }

    internal void InternalWriteWhitespace(string ws)
    {
      if (ws != null && !StringUtils.IsWhiteSpace(ws))
        throw JsonWriterException.Create(this, "Only white space characters should be used.", (Exception) null);
    }

    internal void InternalWriteComment() => this.AutoComplete(JsonToken.Comment);

    internal enum State
    {
      Start,
      Property,
      ObjectStart,
      Object,
      ArrayStart,
      Array,
      ConstructorStart,
      Constructor,
      Closed,
      Error,
    }
  }
}
