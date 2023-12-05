using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace OpenSvg.Netex;
public class ColorHelper
{

    public static SKColor[] GetDistinctColors()
    {
        return new[]
        {
                new SKColor(139, 0, 0),       // Deep Red
                new SKColor(34, 139, 34),     // Forest Green
                new SKColor(0, 0, 128),       // Navy Blue
                new SKColor(255, 140, 0),     // Dark Orange
                new SKColor(128, 0, 128),     // Purple
                new SKColor(128, 128, 0),     // Olive Green
                new SKColor(0, 128, 128),     // Teal
                new SKColor(128, 0, 0),       // Maroon
                new SKColor(75, 0, 130),      // Indigo
                new SKColor(165, 42, 42),     // Brown
                new SKColor(0, 139, 139),     // Dark Cyan
                new SKColor(160, 82, 45),     // Sienna
                new SKColor(148, 0, 211),     // Dark Violet
                new SKColor(112, 128, 144),   // Slate Gray
                new SKColor(184, 134, 11),    // Dark Goldenrod
                new SKColor(25, 25, 112)      // Midnight Blue
            };
    }
}

