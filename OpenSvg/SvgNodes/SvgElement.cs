﻿using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using OpenSvg.Attributes;

namespace OpenSvg.SvgNodes;

public abstract class SvgElement : IXmlSerializable
{
    public abstract string SvgName { get; }

    public StringAttr ID { get; set; } = new(SvgNames.ID, "", false);

    public SvgDocument RootDocument => Root as SvgDocument ??
                                       throw new InvalidOperationException(
                                           "Element does not have a SvgDocument as root element");

    public SvgElement Root => Parent?.Root ?? this;

    public SvgElement? Parent { get; set; } = null;


    /// <summary>
    ///     Gets the current viewport for the element.
    /// </summary>
    /// <remarks>
    ///     <list type="bullet">
    ///         <description>
    ///             If the element is an <see cref="SvgDocument" /> , the viewport will be defined by its explicit size:
    ///             <list type="bullet">
    ///                 <item>If the <see cref="SvgDocument" />'s value is absolute, that value is returned</item>
    ///                 <item>
    ///                     If the <see cref="SvgDocument" />'s value is a ratio (a percentage value), query ancestors until
    ///                     an absolute value can be resolved
    ///                 </item>
    ///             </list>
    ///         </description>
    ///         <item>If the element is not an SvgDocument, the viewport will be equal to its closest SvgDocument ancestor.</item>
    ///         <item>
    ///             If the closest SvgDocument ancestor has a relative defined size (e.g., <c>40%</c>), the viewport will be
    ///             calculated by traversing upwards, accumulating ratios until an ancestor with an absolute size is found.
    ///         </item>
    ///         <item>If an SvgDocument has no size set, it is equivalent to having a ratio of 1 (<c>100%</c>).</item>
    ///         <item>If the topmost SvgDocument has a ratio or no size set, a default container size is assumed</item>
    ///     </list>
    /// </remarks>
    /// <returns>The absolute value for the viewport size, width and height.</returns>
    /// <seealso cref="ViewPortWidth" />
    /// <seealso cref="ViewPortHeight" />
    /// <seealso cref="Constants.DefaultContainerWidth" />
    /// <seealso cref="Constants.DefaultContainerHeight" />

    public Size ViewPort => new(ViewPortWidth, ViewPortHeight);


    public virtual double ViewPortWidth => Parent?.ViewPortWidth ?? Constants.DefaultContainerWidth;

    public virtual double ViewPortHeight => Parent?.ViewPortHeight ?? Constants.DefaultContainerHeight;

    public XmlSchema? GetSchema() => null;


    public void WriteXml(XmlWriter writer)
    {
        foreach (var attribute in Attributes().Where(attribute => !attribute.HasDefaultValue)
                     .OrderBy(attr => attr.Name.Length + attr.ToXmlString().Length).ThenBy(attr => attr.Name))
            writer.WriteAttributeString(attribute.Name, attribute.ToXmlString());

        if (this is IHasElementContent hasElementContent) writer.WriteString(hasElementContent.Content);

        if (this is ISvgElementContainer svgElementContainer)
            foreach (var childElement in svgElementContainer.Children())
            {
                writer.WriteStartElement(childElement.SvgName);
                childElement.WriteXml(writer);
                writer.WriteEndElement();
            }
    }


    public void ReadXml(XmlReader reader)
    {
        if (reader.MoveToContent() != XmlNodeType.Element)
            return;

        // Read attributes
        foreach (var attribute in Attributes())
        {
            var xmlString = reader.GetAttribute(attribute.Name);

            if (!attribute.IsConstant && xmlString is not null)
                attribute.Set(xmlString);
        }

        if (!reader.IsEmptyElement)
        {
            reader.ReadStartElement();

            // Read content if the element has content
            if (this is IHasElementContent hasElementContent && reader.NodeType != XmlNodeType.Element)
                hasElementContent.Content = reader.ReadContentAsString();

            // Read child elements if the element is a container
            if (this is ISvgElementContainer svgElementContainer)
                while (reader.NodeType != XmlNodeType.EndElement)
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        var childElement = CreateSvgElement(reader.Name);
                        childElement.ReadXml(reader);
                        svgElementContainer.Add(childElement);
                    }
                    else
                    {
                        reader.Read();
                    }

            reader.ReadEndElement();
        }
        else
        {
            reader.Read();
        }
    }


    /// <summary>
    ///     Encapsulates the current <see cref="SvgElement" /> in an <see cref="SvgGroup" />.
    /// </summary>
    /// <returns>
    ///     An instance of <see cref="SvgGroup" /> if the current object is an <see cref="SvgGroup" /> or a new
    ///     <see cref="SvgGroup" /> object created from the current object.
    /// </returns>
    public SvgGroup ToSvgGroup()
    {
        if (this is SvgGroup svgGroup) return svgGroup;
        var newSvgGroup = new SvgGroup();
        newSvgGroup.Add(this);
        return newSvgGroup;
    }

    public SvgDocument ToSvgDocument()
    {
        if (this is SvgDocument svgDocument) return svgDocument;
        var newSvgSvgDocument = new SvgDocument();
        newSvgSvgDocument.Add(this);
        return newSvgSvgDocument;
    }

    public virtual (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
        double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other))
            return (true, "Same reference");
        if (GetType() != other.GetType())
            return (false, $"Type: {GetType()} != {other.GetType()}");
        if (ID != other.ID)
            return (false, $"ID: {ID} != {other.ID}");

        return (true, "Equal");
    }


    public IEnumerable<IAttr> Attributes()
    {
        return GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Select(field => field.GetValue(this)).OfType<IAttr>();
    }

    private SvgElement CreateSvgElement(string elementName)
    {
        return elementName switch
        {
            SvgNames.Svg => new SvgDocument(),
            SvgNames.Line => new SvgLine(),
            SvgNames.Group => new SvgGroup(),
            SvgNames.Text => new SvgText(),
            SvgNames.Path => new SvgPath(),
            SvgNames.Polygon => new SvgPolygon(),
            SvgNames.Defs => new SvgDefs(),
            SvgNames.Style => new SvgCssStyle(),
            _ => throw new InvalidOperationException($"Unsupported SVG element: {elementName}")
        };
    }
}