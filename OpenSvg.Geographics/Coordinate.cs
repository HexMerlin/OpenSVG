using DotSpatial.Projections;


namespace OpenSvg.Geographics;

/// <summary>
///     A world coordinate (in WGS84 datum) defined by longitude and latitude
/// </summary>
public readonly struct Coordinate
{
    public readonly double Long;
    
    public readonly double Lat;

    public Coordinate(double longitude, double latitude)
    {
        this.Long = NormalizeLong(longitude);
        this.Lat = NormalizeLat(latitude);
    }

    /// <summary>
    /// Determines whether the coordinate is within a specified bounding box.
    /// </summary>
    /// <param name="TopLeft">The top-left coordinate of the bounding box.</param>
    /// <param name="BottomRight">The bottom-right coordinate of the bounding box.</param>
    /// <returns>True if the coordinate is within the bounding box, false otherwise.</returns>
    public bool IsWithinBoundingBox(Coordinate TopLeft, Coordinate BottomRight)
    {
        return this.Long >= TopLeft.Long && this.Long <= BottomRight.Long
               && this.Lat <= TopLeft.Lat && this.Lat >= BottomRight.Lat;
    }

    /// <summary>
    /// Translates the coordinate by a specified distance in meters along the X and Y axes.
    /// </summary>
    /// <param name="dxMeters">East-west translation in meters. Positive values move east, negative values move west.</param>
    /// <param name="dyMeters">North-south translation in meters. Positive values move north, negative values move south.</param>
    /// <remarks>Note that the y-coordinates increase upwards (north), whereas SVG Y-coordinates increase downwards</remarks>
    /// <returns>A new Coordinate object representing the translated position.</returns>

    public Coordinate Translate(double dxMeters, double dyMeters)
    {
        // Define WGS84 ellipsoid
        ProjectionInfo sourceProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo targetProjection = KnownCoordinateSystems.Projected.World.WebMercator;

       
        // Transform the point to Mercator projection (meters)
        double[] z = { 0 };
        double[] xy = { Long, Lat };
        Reproject.ReprojectPoints(xy, z, sourceProjection, targetProjection, 0, 1);
        
        // Add the dxMeters and dyMeters
        xy[0] += dxMeters;
        xy[1] += dyMeters;

        // Transform the point back to WGS84
        Reproject.ReprojectPoints(xy, z, targetProjection, sourceProjection, 0, 1);

        // the xy array now holds the new coordinates
        var result = new Coordinate(xy[0], xy[1]);

        return result;
    }

    public (double x, double y) ToWebMercator()
    {
        ProjectionInfo sourceProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo targetProjection = KnownCoordinateSystems.Projected.World.WebMercator;
        double[] z = { 0 };
        double[] xy = { Long, Lat };
        Reproject.ReprojectPoints(xy, z, sourceProjection, targetProjection, 0, 1);
        return (xy[0], xy[1]);
    }


    /// <summary>
    /// Calculates the Cartesian offset to another coordinate in meters, as a tuple of X (dx) and Y (dy) distances.
    /// Positive dx indicates eastward distance, negative dx indicates westward distance.
    /// Positive dy indicates northward distance, negative dy indicates southward distance.
    /// This method assumes a flat Earth projection to compute the distances.
    /// </summary>
    /// <param name="coordinate">The coordinate to calculate the offset to.</param>
    /// <returns>
    /// A tuple containing the X (dx) and Y (dy) distances in meters between the current coordinate and the specified coordinate.
    /// </returns>
    public (double dx, double dy) CartesianOffset(Coordinate coordinate)
    {

        ProjectionInfo sourceProjection = KnownCoordinateSystems.Geographic.World.WGS1984;
        ProjectionInfo targetProjection = KnownCoordinateSystems.Projected.World.WebMercator;


        // Transform the points to Mercator projection (meters)
        double[] z = new double[2];
        double[] xy1 = [Long, Lat];
        double[] xy2 = [coordinate.Long, coordinate.Lat];
        Reproject.ReprojectPoints(xy1, z, sourceProjection, targetProjection, 0, 1);
        Reproject.ReprojectPoints(xy2, z, sourceProjection, targetProjection, 0, 1);

        // Calculate distance on a flat surface
        double dx = xy2[0] - xy1[0];
        double dy = xy2[1] - xy1[1];
        return (dx, dy);

    }


    /// <summary>
    /// Calculates the distance in meters to another coordinate.
    /// </summary>
    /// <remarks>
    /// This method uses a flat Earth projection to calculate the distance.
    /// It is fairly fast, but not as accurate as the <see cref="DistanceTo(Coordinate)"/> method.
    /// </remarks>
    /// <param name="coordinate">The coordinate to calculate the distance to.</param>
    /// <returns>The distance between the two coordinates in meters.</returns>

    public double DistanceToApprox(Coordinate coordinate)
    {
        var (dx, dy) = CartesianOffset(coordinate);
        double distance = Math.Sqrt(dx * dx + dy * dy);
        return distance;
    }


    /// <summary>
    /// Implements Vincenty formula (using the inverse method variant) for calculating geodetic distances between two points on the Earth's surface.
    /// Coordinates are assumed to be in the WGS84 datum.
    /// </summary>
    /// <remarks>
    /// This method is very accurate (down to 0.5 mm in distance) but also very slow. 
    /// However, WGS-84 datum is defined to be accurate to ±1m, which might overshadow the 0.5mm accuracy provided by Vincenty's formulae in several cases.
    /// The algorithm may fail to converge (and throw an exception) in rare cases,
    /// particularly for nearly antipodal points (when the two points are located on nearly opposite sides of the Earth), 
    /// where the iterative algorithm fails to converge to a solution within 50 iterations.
    /// </remarks>
    /// <param name="coordinate">The coordinate to calculate the distance to.</param>
    /// <returns>The distance between the two coordinates in meters.</returns>
    /// <exception cref="InvalidOperationException">Thrown when the method fails to converge to a solution.</exception>
    /// <seealso cref="https://en.wikipedia.org/wiki/Vincenty%27s_formulae"/>
    public double DistanceTo(Coordinate coordinate)
    {
        static double ToRadians(double degrees) => degrees * Math.PI / 180.0;

        if (this.Equals(coordinate))
        {
            return 0d;
        }

        double initialAzimuth = 0.0;
        double sphericalDistance = 0.0;
        double geodesicCorrection = 0.0;
        const double equatorialRadiusMeters = 6378137.0;
        const double polarRadiusMeters = 6356752.3142;
        double latRad1 = ToRadians(this.Lat);
        double longRad1 = ToRadians(this.Long);
        double latRad2 = ToRadians(coordinate.Lat);
        double longRad2 = ToRadians(coordinate.Long);
        if (Math.Abs(Math.PI / 2.0 - Math.Abs(latRad1)) < 1E-10)
        {
            latRad1 = Math.Sign(latRad1) * 1.5707963266948965;
        }

        if (Math.Abs(Math.PI / 2.0 - Math.Abs(latRad2)) < 1E-10)
        {
            latRad2 = Math.Sign(latRad2) * 1.5707963266948965;
        }
      
        const double flattening = 0.0033528106643315072;
        double reducedLat1 = Math.Atan((1.0 - flattening) * Math.Tan(latRad1));
        double reducedLat2 = Math.Atan((1.0 - flattening) * Math.Tan(latRad2));
        longRad1 %= Math.PI * 2.0;
        double longDiff = Math.Abs(longRad2 % (Math.PI * 2.0) - longRad1);
        if (longDiff > Math.PI)
        {
            longDiff = Math.PI * 2.0 - longDiff;
        }

        double lambda = longDiff;
        int iterCount = 0;
        bool flag = true;
        while (flag)
        {
            iterCount++;
            if (iterCount > 50)
            {
                throw new InvalidOperationException($"{nameof(DistanceTo)} method failed to converge.");
            }

            double lambdaPrev = lambda;
            double y = Math.Sqrt(Math.Pow(Math.Cos(reducedLat2) * Math.Sin(lambda), 2.0) + Math.Pow(Math.Cos(reducedLat1) * Math.Sin(reducedLat2) - Math.Sin(reducedLat1) * Math.Cos(reducedLat2) * Math.Cos(lambda), 2.0));
            double x = Math.Sin(reducedLat1) * Math.Sin(reducedLat2) + Math.Cos(reducedLat1) * Math.Cos(reducedLat2) * Math.Cos(lambda);
            double angularSeparation = Math.Atan2(y, x);
            double azimuthEquator = Math.Asin(Math.Cos(reducedLat1) * Math.Cos(reducedLat2) * Math.Sin(lambda) / Math.Sin(angularSeparation));
            double angularSeparationCorrection = Math.Cos(angularSeparation) - 2.0 * Math.Sin(reducedLat1) * Math.Sin(reducedLat2) / Math.Pow(Math.Cos(azimuthEquator), 2.0);
            double correctionFactor = flattening / 16.0 * Math.Pow(Math.Cos(azimuthEquator), 2.0) * (4.0 + flattening * (4.0 - 3.0 * Math.Pow(Math.Cos(azimuthEquator), 2.0)));
            lambda = longDiff + (1.0 - correctionFactor) * flattening * Math.Sin(azimuthEquator) * (angularSeparation + correctionFactor * Math.Sin(angularSeparation) * (angularSeparationCorrection + correctionFactor * Math.Cos(angularSeparation) * (-1.0 + 2.0 * Math.Pow(angularSeparationCorrection, 2.0))));
            if (lambda > Math.PI)
            {
                lambdaPrev = Math.PI;
                lambda = Math.PI;
            }

            flag = Math.Abs(lambda - lambdaPrev) > 1E-12;
            if (!double.IsNaN(azimuthEquator))
            {
                initialAzimuth = azimuthEquator;
                sphericalDistance = angularSeparation;
                geodesicCorrection = angularSeparationCorrection;
            }
        }

        double cosineSquaredCorrection = Math.Pow(Math.Cos(initialAzimuth), 2.0) * (Math.Pow(equatorialRadiusMeters, 2.0) - Math.Pow(polarRadiusMeters, 2.0)) / Math.Pow(polarRadiusMeters, 2.0);
        double factorA = 1.0 + cosineSquaredCorrection / 16384.0 * (4096.0 + cosineSquaredCorrection * (-768.0 + cosineSquaredCorrection * (320.0 - 175.0 * cosineSquaredCorrection)));
        double factorB = cosineSquaredCorrection / 1024.0 * (256.0 + cosineSquaredCorrection * (-128.0 + cosineSquaredCorrection * (74.0 - 47.0 * cosineSquaredCorrection)));
        double deltaSigma = factorB * Math.Sin(sphericalDistance) * (geodesicCorrection + factorB / 4.0 * (Math.Cos(sphericalDistance) * (-1.0 + 2.0 * Math.Pow(geodesicCorrection, 2.0)) - factorB / 6.0 * geodesicCorrection * (-3.0 + 4.0 * Math.Pow(Math.Sin(sphericalDistance), 2.0)) * (-3.0 + 4.0 * Math.Pow(geodesicCorrection, 2.0))));
        return polarRadiusMeters * factorA * (sphericalDistance - deltaSigma);
    }



    /// <summary>
    /// Normalizes a latitude value to the range [-90, 90].
    /// </summary>
    /// <remarks>
    /// This method adjusts any given latitude value to fall within the Earth's latitude bounds.
    /// Latitudes outside the range are wrapped around, preserving their equivalent position.
    /// </remarks>
    /// <param name="latitude">The latitude value in degrees to normalize.</param>
    /// <returns>The normalized latitude value within the range [-90, 90].</returns>
    /// <exception cref="ArgumentException">Thrown when the input is not a valid latitude value (e.g., Infinity or NaN).</exception>
    private static double NormalizeLat(double latitude)
    {
        if (double.IsInfinity(latitude) || double.IsNaN(latitude))
            throw new ArgumentException("Latitude must be a valid number.");

        int num = Convert.ToInt32(Math.Floor(latitude / 180.0));
        if (latitude < 0.0)
            num++;

        double num2 = latitude % 180.0;
        if (num2 > 90.0)
            num2 = 180.0 - num2;
        else if (num2 < -90.0)
            num2 = -180.0 - num2;

        return num % 2 == 0 ? num2 : -num2;
    }



    /// <summary>
    /// Normalizes a longitude value to the range ]-180, 180].
    /// </summary>
    /// <remarks>
    /// This method ensures that any given longitude value is adjusted to fall within the Earth's longitude bounds.
    /// It handles values outside the standard range by wrapping around, preserving the equivalent geographic position.
    /// </remarks>
    /// <param name="longitude">The longitude value in degrees to normalize.</param>
    /// <returns>The normalized longitude value within the range ]-180, 180].</returns>
    /// <exception cref="ArgumentException">Thrown when the input is not a valid longitude value (e.g., Infinity or NaN).</exception>
    private static double NormalizeLong(double longitude)
    {
        if (double.IsInfinity(longitude) || double.IsNaN(longitude))
        {
            throw new ArgumentException("Longitude must be a valid number.");
        }

        longitude = longitude % 360; // Normalize to be within -360 to 360 degrees

        if (longitude > 180)
        {
            longitude -= 360; // Adjust for values over 180
        }
        else if (longitude <= -180)
        {
            longitude += 360; // Adjust for values under -180
        }

        return longitude;
    }




    /// <summary>
    /// Calculates a Coordinate that is a certain fraction along the straight line from this Coordinate to another Coordinate.
    /// </summary>
    /// <param name="a">The starting coordinate.</param>
    /// <param name="b">The ending coordinate.</param>
    /// <param name="fraction">The fraction along the straight line between the two coordinates, ranging from 0.0 to 1.0.</param>
    /// <returns>A new Coordinate that represents the specified fraction along the line from 'a' to 'b'.</returns>
    public static Coordinate Interpolate(Coordinate a, Coordinate b, double fraction)

    {
        var (dx, dy) = a.CartesianOffset(b);

        // Calculate the translated distance as a percentage of the total distance
        double translatedDx = dx * fraction;
        double translatedDy = dy * fraction;

        // Translate the start coordinate by the calculated distances
        return a.Translate(translatedDx, translatedDy);
    }


    /// <summary>
    ///     A world coordinate as a string in a JSON friendly format
    /// </summary>
    /// <returns>The coordinate as a string in a JSON friendly format.</returns>
    public override string ToString() => $"{{ \"long\": {Long.ToXmlString()}, \"lat\": {Lat.ToXmlString()} }}";
}