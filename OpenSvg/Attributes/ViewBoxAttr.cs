﻿using System.Globalization;

namespace OpenSvg.Attributes;


/// <summary>
/// Represents an attribute for setting the viewbox for an SVG document.
/// </summary>
public class ViewBoxAttr : Attr<BoundingBox>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="ViewBoxAttr"/> class.
    /// </summary>
    public ViewBoxAttr() : base(SvgNames.ViewBox, BoundingBox.None, false)
    {
    }

    /// <inheritdoc/>
    protected override string Serialize(BoundingBox value) 
        => value.Equals(BoundingBox.None)
            ? string.Empty
            : $"{value.MinX.ToXmlString()} {value.MinY.ToXmlString()} {value.Width.ToXmlString()} {value.Height.ToXmlString()}";

    /// <inheritdoc/>
    protected override BoundingBox Deserialize(string xmlString)
    {
        var values = xmlString.Split(' ', StringSplitOptions.RemoveEmptyEntries);
        if (values.Length != 4)
        {
            return BoundingBox.None;
        }
        Point upperLeft = new(values[0].ToFloat(), values[1].ToFloat());
    
        return new BoundingBox(upperLeft, values[2].ToFloat(), values[3].ToFloat());
    }
}
