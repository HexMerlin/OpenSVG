namespace OpenSvg.SvgNodes;

/// <summary>
///     Represents a visual SVG element that can contain other element as children.
/// </summary>
public abstract class SvgVisualContainer : SvgVisual, ISvgElementContainer
{
    /// <summary>
    /// Gets or sets the child elements of this <see cref="SvgVisualContainer"/>.
    /// </summary>
    /// <remarks>
    /// This property holds a list of child <see cref="SvgElement"/> instances.
    /// </remarks>
    public List<SvgElement> ChildElements { get; set; } = new();


    /// <summary>
    /// Retrieves the children of this <see cref="SvgVisualContainer"/>.
    /// </summary>
    /// <returns>An enumerable of <see cref="SvgElement"/> representing the direct children.</returns>
    public IEnumerable<SvgElement> Children() => ChildElements;

    /// <summary>
    /// Retrieves all descendant elements of this <see cref="SvgVisualContainer"/>.
    /// </summary>
    /// <returns>An enumerable of <see cref="SvgElement"/> representing the descendants.</returns>
    public IEnumerable<SvgElement> Descendants()
    {
        foreach (SvgElement element in ChildElements)
        {
            yield return element;

            if (element is not ISvgElementContainer container)
                continue;

            foreach (SvgElement child in container.Descendants())
                yield return child;
        }
    }

    /// <summary>
    /// Adds a <see cref="SvgElement"/> as a child of this <see cref="SvgVisualContainer"/>.
    /// </summary>
    /// <param name="svgElement"></param>
    public void Add(SvgElement svgElement)
    {
        svgElement.Parent = this;
        ChildElements.Add(svgElement);
    }

    /// <summary>
    /// Adds a collection of <see cref="SvgElement"/> as children of this <see cref="SvgVisualContainer"/>.
    /// </summary>
    /// <param name="svgElements">The collection of <see cref="SvgElement"/> to add.</param>

    public void AddAll(IEnumerable<SvgElement> svgElements)
    {
        foreach (SvgElement svgElement in svgElements)
            Add(svgElement);
    }

    /// <summary>
    ///     Adds an <see cref="SvgVisual" /> child inside this <see cref="SvgVisualContainer" /> with specified horizontal and
    ///     vertical alignment.
    /// </summary>
    /// <remarks>
    ///     Alignment is calculated relative to the bounding boxes of parent and child, when the add is made.
    ///     Later changes to the bounding boxes of the parent or the child will not trigger a recalculation of the alignment.
    /// </remarks>
    /// <param name="child">The <see cref="SvgVisual" /> child to add.</param>
    /// <param name="horizontalAlignment">The horizontal alignment for the child, in relation to its new parent.</param>
    /// <param name="verticalAlignment">The vertical alignment for the child in relation, in relation to its new parent.</param>
    public void AddChildWithAlignment(SvgVisual child, HorizontalAlignment horizontalAlignment,
        VerticalAlignment verticalAlignment)
    {
        child.AlignRelativeTo(this, horizontalAlignment, verticalAlignment);
        Add(child);
    }

    ///<inheritdoc/>
    protected override ConvexHull ComputeConvexHull() 
        => new(ChildElements.OfType<SvgVisual>().Select(c => c.ConvexHull));

}