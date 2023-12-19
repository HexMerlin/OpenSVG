using OpenSvg.Geographics;

namespace OpenSvg.Tests.Geographics;

/// <summary>
/// Testing the DistanceTo method
/// Reference data is generated from here <see href="https://geodesyapps.ga.gov.au/vincenty-inverse">Austrailian Government Geoscience Australia</see>
/// </summary>
public class DistanceToTests
{
    

    [Fact]
    public void DistanceTo_KnownDistance1_ReturnsCorrect()
    {
        Coordinate c1 = new(18.07013179188922, 59.32814079754625);
        Coordinate c2 = new(18.070139030819174, 59.328136049754185);

        double distance = c1.DistanceTo(c2);

        Assert.Equal(0.67, distance, 3);
    }


    [Fact]
    public void DistanceTo_KnownDistance2_ReturnsCorrect()
    {
        Coordinate c1 = new(18.05870021077238, 59.34039778289869);
        Coordinate c2 = new(18.059096351138987, 59.340513214964545);

        double distance = c1.DistanceTo(c2);

        Assert.Equal(25.953, distance, 3);
    }

    [Fact]
    public void DistanceTo_KnownDistance3_ReturnsCorrect()
    {
        Coordinate c1 = new(18.07171594393971, 59.32699728123926);
        Coordinate c2 = new(18.072050199154425, 59.33831542247987);

        double distance = c1.DistanceTo(c2);
     
        Assert.Equal(1260.994, distance, 3);
    }

    [Fact]
    public void DistanceTo_KnownDistance4_ReturnsCorrect()
    {
        Coordinate c1 = new(17.41782476750391, 64.99286597393623);
        Coordinate c2 = new(-0.14143351554215036, 51.501565818297294);

        double distance = c1.DistanceTo(c2);

        Assert.Equal(1808807.877, distance, 3);
    }



}