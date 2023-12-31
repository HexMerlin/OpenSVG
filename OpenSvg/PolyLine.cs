﻿using OpenSvg.SvgNodes;
using SkiaSharp;
using System.Collections;
using System.Collections.Immutable;
using System.Globalization;
using System.IO;


namespace OpenSvg;

/// <summary>
///     Represents a polyline of points.
///     Provides methods for generating bounding boxes and translating polylines using a specified transform.
/// </summary>
public class Polyline : PointList, IEquatable<Polyline>
{
    private readonly ConvexHull convexHull;

    /// <summary>
    ///     Initializes a new instance of the <see cref="Polyline" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">The collection of points.</param>
    public Polyline(IEnumerable<Point> points) : this(points.ToImmutableArray())
    {
        
    }

    /// <summary>
    ///     Initializes a new instance of the <see cref="Polyline" /> class with the specified collection of points.
    /// </summary>
    /// <param name="points">An immutable array of points.</param>
    public Polyline(ImmutableArray<Point> points) : base(points)
    {
        convexHull = new ConvexHull(this);
    }

    public static new Polyline FromXmlString(string xmlString) => new Polyline(PointList.FromXmlString(xmlString));

    public ConvexHull ConvexHull => convexHull;

    public BoundingBox BoundingBox => ConvexHull.BoundingBox;

    public SvgPolyline ToSvgPolyline()
    {
        SvgPolyline svgPolyline = new SvgPolyline();
        svgPolyline.Polyline = this;
        return svgPolyline;
    }



    /// <summary>
    /// Converts the <see cref="Polyline"/> to a <see cref="Path"/>.
    /// </summary>
    /// <returns>The <see cref="Polyline"/> represented as a <see cref="Path"/>.</returns>
    public Path ToPath()
    {
        SKPath skPath = new SKPath();
        skPath.AddPoly(this.Select(p => new SKPoint((float)p.X, (float)p.Y)).ToArray(), close: false);
 
        return new Path(skPath);       
    }

    /// <summary>
    /// Returns an empty polyLine.
    /// </summary>
    public static Polyline Empty => new(Enumerable.Empty<Point>());

    /// <summary>
    ///     Determines whether the specified object is equal to the current object.
    /// </summary>
    /// <param name="other">The object to compare with the current object.</param>
    /// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
    public bool Equals(Polyline? other) => base.Equals(other);


    ///<inheritdoc/>
    public override bool Equals(object? obj) => base.Equals(obj);

    ///<inheritdoc/>
    public override int GetHashCode() => base.GetHashCode();

}