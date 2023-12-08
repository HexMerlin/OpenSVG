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

    public static SKColor[] GetBrightDistinctColors()
    {
        return new[]
        {
        new SKColor(255, 0, 0),       // Bright Red
        new SKColor(0, 255, 0),       // Lime Green
        new SKColor(0, 0, 255),       // Bright Blue
        new SKColor(255, 255, 0),     // Yellow
        new SKColor(255, 0, 255),     // Magenta
        new SKColor(0, 255, 255),     // Cyan
        new SKColor(255, 165, 0),     // Orange
        new SKColor(255, 105, 180),   // Hot Pink
        new SKColor(173, 216, 230),   // Light Blue
        new SKColor(144, 238, 144),   // Light Green
        new SKColor(255, 182, 193),   // Light Pink
        new SKColor(255, 215, 0),     // Gold
        new SKColor(0, 206, 209),     // Turquoise
        new SKColor(138, 43, 226),    // Blue Violet
        new SKColor(32, 178, 170),    // Light Sea Green
        new SKColor(218, 112, 214),   // Orchid
        new SKColor(255, 99, 71),     // Tomato
        new SKColor(30, 144, 255),    // Dodger Blue
        new SKColor(154, 205, 50),    // Yellow Green
        new SKColor(147, 112, 219)    // Medium Purple
    };
    }


    public static SKColor[] GetDarkDistinctColors()
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

