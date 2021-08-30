﻿// Decompiled with JetBrains decompiler
// Type: Newtonsoft.Json.Converters.XmlDocumentWrapper
// Assembly: Newtonsoft.Json, Version=7.0.0.0, Culture=neutral, PublicKeyToken=30ad4fe6b2a6aeed
// MVID: 87D97053-987A-40AE-9D1A-A30FFAD1214B
// Assembly location: C:\Users\Pdawg\Downloads\Microsoft Band Sync Setup\Newtonsoft.Json.dll

using System.Xml;

namespace Newtonsoft.Json.Converters
{
  internal class XmlDocumentWrapper : XmlNodeWrapper, IXmlDocument, IXmlNode
  {
    private readonly XmlDocument _document;

    public XmlDocumentWrapper(XmlDocument document)
      : base((XmlNode) document)
    {
      this._document = document;
    }

    public IXmlNode CreateComment(string data) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateComment(data));

    public IXmlNode CreateTextNode(string text) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateTextNode(text));

    public IXmlNode CreateCDataSection(string data) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateCDataSection(data));

    public IXmlNode CreateWhitespace(string text) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateWhitespace(text));

    public IXmlNode CreateSignificantWhitespace(string text) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateSignificantWhitespace(text));

    public IXmlNode CreateXmlDeclaration(
      string version,
      string encoding,
      string standalone)
    {
      return (IXmlNode) new XmlDeclarationWrapper(this._document.CreateXmlDeclaration(version, encoding, standalone));
    }

    public IXmlNode CreateXmlDocumentType(
      string name,
      string publicId,
      string systemId,
      string internalSubset)
    {
      return (IXmlNode) new XmlDocumentTypeWrapper(this._document.CreateDocumentType(name, publicId, systemId, (string) null));
    }

    public IXmlNode CreateProcessingInstruction(string target, string data) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateProcessingInstruction(target, data));

    public IXmlElement CreateElement(string elementName) => (IXmlElement) new XmlElementWrapper(this._document.CreateElement(elementName));

    public IXmlElement CreateElement(string qualifiedName, string namespaceUri) => (IXmlElement) new XmlElementWrapper(this._document.CreateElement(qualifiedName, namespaceUri));

    public IXmlNode CreateAttribute(string name, string value) => (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateAttribute(name))
    {
      Value = value
    };

    public IXmlNode CreateAttribute(
      string qualifiedName,
      string namespaceUri,
      string value)
    {
      return (IXmlNode) new XmlNodeWrapper((XmlNode) this._document.CreateAttribute(qualifiedName, namespaceUri))
      {
        Value = value
      };
    }

    public IXmlElement DocumentElement => this._document.DocumentElement == null ? (IXmlElement) null : (IXmlElement) new XmlElementWrapper(this._document.DocumentElement);
  }
}
