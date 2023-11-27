namespace OpenSvg;


/// <summary>
/// Represents a bounding box defined by two points: the upper left and the lower right.
/// </summary>
/// <param name="UpperLeft">The upper left point of the bounding box.</param>
/// <param name="LowerRight">The lower right point of the bounding box.</param>
public readonly record struct BoundingBox(Point UpperLeft, Point LowerRight)
{

    /// <summary>
    /// Initializes a new instance of the <see cref="BoundingBox"/> struct with zero size, starting at the origin.
    /// </summary>
    public BoundingBox() : this(Point.Origin, Point.Origin) { }

    /// <summary>
    /// Initializes a new instance of the <see cref="BoundingBox"/> struct with the specified upper left point, width, and height.
    /// </summary>
    /// <param name="upperLeft">The upper left point of the bounding box.</param>
    /// <param name="width">The width of the bounding box.</param>
    /// <param name="height">The height of the bounding box.</param>
    public BoundingBox(Point upperLeft, double width, double height) : this(upperLeft, new Point(upperLeft.X + width, upperLeft.Y + height)) { }


    /// <summary>
    /// A static instance representing a bounding box with zero size, starting at the origin (0,0).
    /// </summary>
    public static readonly BoundingBox None = new(Point.Origin, Point.Origin);

    /// <summary>
    ///     Gets the width of the bounding box.
    /// </summary>
    public double Width => this.LowerRight.X - this.UpperLeft.X;

    /// <summary>
    ///     Gets the height of the bounding box.
    /// </summary>
    public double Height => this.LowerRight.Y - this.UpperLeft.Y;

    /// <summary>
    ///     Gets the size of the bounding box.
    /// </summary>
    public readonly Size Size => new(Width, Height);

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
    /// <param name="other">The other bounding box to include in the union.</param>
    /// <returns>The bounding box that represents the union of this and the other bounding box.</returns>

    public readonly BoundingBox UnionWith(BoundingBox other) => new(new Point(double.Min(MinX, other.MinX), double.Min(MinY, other.MinY)),
            new Point(double.Max(MaxX, other.MaxX), double.Max(MaxY, other.MaxY)));

    /// <summary>
    ///     Checks if this bounding box intersects with another bounding box.
    /// </summary>
    /// <param name="other">The other bounding box to check for intersection.</param>
    /// <returns><c>true</c> if the bounding boxes intersect, <c>false</c> otherwise.</returns>
    public readonly bool Intersects(BoundingBox other) => !(MaxX < other.MinX || MinX > other.MaxX || MaxY < other.MinY || MinY > other.MaxY);

    /// <summary>
    /// Returns a string that represents the current object.
    /// </summary>
    /// <returns>A string representation of the bounding box, showing the upper left and lower right points.</returns>
    public override string ToString() => $"Upper left: {this.UpperLeft}, Lower right: {this.LowerRight}";
}
