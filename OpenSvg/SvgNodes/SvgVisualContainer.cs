namespace OpenSvg.SvgNodes;

public abstract class SvgVisualContainer : SvgVisual, ISvgElementContainer
{
    public List<SvgElement> ChildElements { get; set; } = new();

    public void Add(SvgElement svgElement)
    {
        svgElement.Parent = this;
        ChildElements.Add(svgElement);
    }


    /// <returns>
    ///     Returns an <see cref="IEnumerable" /> of <see cref="SvgElement" /> containing the descendants (children
    ///     and their children down to the leaf level) of the specified type
    /// </returns>
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

    public IEnumerable<SvgElement> Children() => ChildElements;

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

    protected override ConvexHull ComputeConvexHull() => new(ChildElements.OfType<SvgVisual>().Select(c => c.ConvexHull));

    public override (bool Equal, string Message) CompareSelfAndDescendants(SvgElement other,
        double doublePrecision = Constants.DoublePrecision)
    {
        if (ReferenceEquals(this, other)) return (true, "Same reference");
        (bool equal, string message) = base.CompareSelfAndDescendants(other);
        if (!equal)
            return (equal, message);
        var sameType = (SvgVisualContainer)other;
        if (ChildElements.Count != sameType.ChildElements.Count)
            return (false, $"ChildElements count: {ChildElements.Count} != {sameType.ChildElements.Count}");
        for (int i = 0; i < ChildElements.Count; i++)
        {
            (equal, message) = ChildElements[i].CompareSelfAndDescendants(sameType.ChildElements[i]);
            if (!equal)
                return (false, $"ChildElements[{i}]: {message}");
        }

        return (true, "Equal");
    }
}