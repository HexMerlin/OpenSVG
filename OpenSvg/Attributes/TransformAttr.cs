﻿using System.Text.RegularExpressions;

namespace OpenSvg.Attributes;

/// <summary>
/// Represents an attribute for transform data.
/// A transform is represented as a <see cref="Transform"/> class.
/// </summary>
public class TransformAttr : Attr<Transform>
{
    /// <summary>
    /// Constructs a new instance of <see cref="TransformAttr"/> with a default identity <see cref="Transform"/>.
    /// </summary>
    public TransformAttr() : base(SvgNames.Transform, Transform.Identity, false)
    {
    }

    /// <inheritdoc />
    protected override string Serialize(Transform value) => value.ToXmlString();

    /// <inheritdoc />
    protected override Transform Deserialize(string xmlString)
    {
        Transform result = Transform.Identity;
        MatchCollection matches = RegexStore.ValidTransformString().Matches(xmlString);

        foreach (Match match in matches.Cast<Match>())
        {
            string type = match.Groups[1].Value;
            string[] rawParams = match.Groups[2].Value.Split(new[] { ',', ' ' }, StringSplitOptions.RemoveEmptyEntries);
            int paramCount = rawParams.Length;

            result = type switch
            {
                "translate" => paramCount >= 2
                    ? result.ComposeWith(Transform.CreateTranslation(rawParams[0].ToFloat(), rawParams[1].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'translate': Expected 2, got {paramCount}"),
                "scale" => paramCount >= 2
                    ? result.ComposeWith(Transform.CreateScale(rawParams[0].ToFloat(), rawParams[1].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'scale': Expected 2, got {paramCount}"),
                "rotate" => paramCount >= 3
                    ? result.ComposeWith(Transform.CreateRotation(rawParams[0].ToFloat(), rawParams[1].ToFloat(),
                        rawParams[2].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'rotate': Expected 3, got {paramCount}"),
                "skewX" => paramCount >= 1
                    ? result.ComposeWith(Transform.CreateSkew(rawParams[0].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'skewX': Expected 1, got {paramCount}"),
                "skewY" => paramCount >= 1
                    ? result.ComposeWith(Transform.CreateSkew(0, rawParams[0].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'skewY': Expected 1, got {paramCount}"),
                "matrix" => paramCount >= 6
                    ? result.ComposeWith(Transform.CreateMatrix(rawParams[0].ToFloat(), rawParams[1].ToFloat(),
                        rawParams[2].ToFloat(), rawParams[3].ToFloat(), rawParams[4].ToFloat(),
                        rawParams[5].ToFloat()))
                    : throw new InvalidOperationException(
                        $"Invalid parameter count for 'matrix': Expected 6, got {paramCount}"),
                _ => throw new InvalidOperationException($"Invalid or unsupported transform type: {type}")
            };
        }

        return result;
    }

}