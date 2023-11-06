using System.Xml.Serialization;

namespace OpenSvg.SvgNodes;
public class SvgNode
{
    [XmlIgnore]
    public SvgElement? Parent { get; set; }
}
