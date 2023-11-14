using OpenSvg.Attributes;
using System.Reflection;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;

/// <summary>
/// Represents the base class for all SVG elements.
/// </summary>
public abstract class SvgElement : IXmlSerializable, IEquatable<SvgElement>
{
    public abstract string SvgName { get; }
    /// <summary>
    /// Gets or sets the ID of the element.
    /// </summary>
    public readonly StringAttr ID = new(SvgNames.ID, "", false);

    /// <summary>
    /// Gets the root document of the element.
    /// </summary>
    public SvgDocument RootDocument => Root as SvgDocument ??
                                       throw new InvalidOperationException(
                                           "Element does not have a SvgDocument as root element");

    /// <summary>
    /// Gets the root element of the element.
    /// </summary>
    public SvgElement Root => Parent?.Root ?? this;

    /// <summary>
    /// Gets or sets the parent element of the element.
    /// </summary>
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
    ///         <item>If no topmost SvgDocument exists, or the topmost SvgDocument has a ratio or no size set, a default container size is assumed</item>
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
        foreach (IAttr? attribute in Attributes().Where(attribute => !attribute.HasDefaultValue)
                     .OrderBy(attr => attr.Name.Length + attr.ToXmlString().Length).ThenBy(attr => attr.Name))
            writer.WriteAttributeString(attribute.Name, attribute.ToXmlString());

        if (this is IHasElementContent hasElementContent) writer.WriteString(hasElementContent.Content);

        if (this is ISvgElementContainer svgElementContainer)
            foreach (SvgElement childElement in svgElementContainer.Children())
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
        foreach (IAttr attribute in Attributes())
        {
            string? xmlString = reader.GetAttribute(attribute.Name);

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
                        SvgElement childElement = CreateSvgElement(reader.Name);
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




    public IEnumerable<IAttr> Attributes() => GetType().GetFields(BindingFlags.Instance | BindingFlags.Public)
            .Select(field => field.GetValue(this)).OfType<IAttr>();

    private static SvgElement CreateSvgElement(string elementName) => elementName switch
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
    public bool Equals(SvgElement? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        if (GetType() != other.GetType()) return false;
        if (! Attributes().SequenceEqual(other.Attributes()) ) return false;
        if (this is IHasElementContent hasElementContent)
            if (hasElementContent.Content != ((IHasElementContent) other).Content) return false;
        if (this is ISvgElementContainer svgElementContainer)
            if (!svgElementContainer.Children().SequenceEqual(((ISvgElementContainer) other).Children())) return false;
        return true;
    }

    public override bool Equals(object? obj) => Equals(obj as SvgElement);

    public (bool equal, string diffMessage) InformedEquals(SvgElement? other)
    {
        string Descr = $"Element {SvgName}:";

        if (other is null) return (false, $"{Descr} other is null");
        if (ReferenceEquals(this, other)) return (true, "same reference");
        if (GetType() != other.GetType()) return (false, $"{Descr} {GetType()} != {other.GetType()}");
        IAttr[] attributes1 = Attributes().ToArray();
        IAttr[] attributes2 = other.Attributes().ToArray();
        if (attributes1.Length != attributes2.Length) return (false, $"{Descr} Attribute count: {attributes1.Length} != {attributes2.Length}");
        for (int i = 0; i < attributes1.Length; i++)
        {
            IAttr a1 = attributes1[i];
            IAttr a2 = attributes2[i];
           
            if (a1.Name != a2.Name) return (false, $"{Descr} Attribute {i+1}: {a1.Name} != {a2.Name}");
            if (!a1.Equals(a2)) 
                return (false, $"{Descr} Attribute {a1.Name}:\n{a1.ToXmlString()} !=\n{a2.ToXmlString()}"); 
        }

        if (this is IHasElementContent hasElementContent && hasElementContent.Content != ((IHasElementContent)other).Content)
           return (false, $"{Descr} Content: {hasElementContent.Content} != {((IHasElementContent)other).Content}");
        if (this is ISvgElementContainer svgElementContainer)
        {
            SvgElement[] c1 = svgElementContainer.Children().ToArray();
            SvgElement[] c2 = ((ISvgElementContainer)other).Children().ToArray();
            if (c1.Length != c2.Length) return (false, $"{Descr} Child count: {c1.Length} != {c2.Length}");
            for (int i = 0; i < c1.Length; i++)
            {
                (bool equal, string diffMessage) = c1[i].InformedEquals(c2[i]);
                if (!equal) return (false, diffMessage);
            }

        }
        return (true, "equal");
    }

    
    public static bool operator ==(SvgElement left, SvgElement right) => left.Equals(right);

    public static bool operator !=(SvgElement? left, SvgElement? right) => !left.Equals(right);
    public override int GetHashCode() => HashCode.Combine(SvgName, ID.Get());

   

}