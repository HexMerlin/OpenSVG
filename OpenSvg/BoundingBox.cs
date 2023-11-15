namespace OpenSvg;


/// <summary>
///     Represents a bounding box defined by two points.
/// </summary>
public readonly struct BoundingBox
{
    /// <summary>
    /// Initializes a new instance of the <see cref="BoundingBox"/> struct.
    /// </summary>
    public BoundingBox()
    {
        this.UpperLeft = Point.Origin;
        this.LowerRight = Point.Origin;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="BoundingBox"/> struct.
    /// </summary>
    /// <param name="upperLeft">The upper left point.</param>
    /// <param name="lowerRight">The lower right point.</param>
    public BoundingBox(Point upperLeft, Point lowerRight)
    {
        this.UpperLeft = upperLeft;
        this.LowerRight = lowerRight;
    }

    /// <summary>
    /// Gets the upper left point.
    /// </summary>
    public readonly Point UpperLeft;

    /// <summary>
    /// Gets the lower right point.
    /// </summary>
    public readonly Point LowerRight;

    /// <summary>
    ///     A bounding box starting at (0,0) thas has zero size.
    /// </summary>
    public static readonly BoundingBox None = new(Point.Origin, Point.Origin);

    /// <summary>
    ///     Gets the size of the bounding box.
    /// </summary>
    public readonly Size Size => new(this.LowerRight.X - this.UpperLeft.X, this.LowerRight.Y - this.UpperLeft.Y);

    /// <summary>
    ///     Gets the minimum x coordinate of the bounding box.
    /// </summary>
    public readonly double MinX => this.UpperLeft.X;

    /// <summary>
    ///     Gets the midpoint of the x coordinates of the bounding box.
    /// </summary>
    public readonly double MidX => (MinX + MaxX) / 2;

    /// <summary>
    ///     Gets the maximum x coordinate of the bounding box.
    /// </summary>
    public readonly double MaxX => this.LowerRight.X;

    /// <summary>
    ///     Gets the minimum y coordinate of the bounding box.
    /// </summary>
    public readonly double MinY => this.UpperLeft.Y;

    /// <summary>
    ///     Gets the midpoint of the y coordinates of the bounding box.
    /// </summary>
    public readonly double MidY => (MinY + MaxY) / 2;

    /// <summary>
    ///     Gets the maximum y coordinate of the bounding box.
    /// </summary>
    public readonly double MaxY => this.LowerRight.Y;

    /// <summary>
    ///     Calculates the minimum bounding box that contains both this bounding box and another bounding box.
    /// </summary>
    /// <param name="other">The other bounding box.</param>
    /// <returns>The union bounding box.</returns>
    public readonly BoundingBox UnionWith(BoundingBox other) => new(new Point(double.Min(MinX, other.MinX), double.Min(MinY, other.MinY)),
            new Point(double.Max(MaxX, other.MaxX), double.Max(MaxY, other.MaxY)));

    /// <summary>
    ///     Checks if this bounding box intersects with another bounding box.
    /// </summary>
    /// <param name="other">The other bounding box.</param>
    /// <returns>True if the bounding boxes intersect, false otherwise.</returns>
    public readonly bool Intersects(BoundingBox other) => !(MaxX < other.MinX || MinX > other.MaxX || MaxY < other.MinY || MinY > other.MaxY);

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string that represents the current object.</returns>
    public override string ToString() => $"Upper left: {this.UpperLeft}, Lower right: {this.LowerRight}";
}
