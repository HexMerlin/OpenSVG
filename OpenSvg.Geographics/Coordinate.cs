

using CoordinateSharp;

namespace OpenSvg.Geographics;

/// <summary>
///     A world coordinate (in WGS84 datum) defined by longitude and latitude
/// </summary>
public readonly struct Coordinate
{
    public readonly double Long;
    
    public readonly double Lat;

    /// <summary>
    /// Needed by CoordinateSharp to speed up things
    /// </summary>
    /// <seealso cref="https://coordinatesharp.com/Performance"/>

    private readonly static EagerLoad eagerLoad = EagerLoad.Create(EagerLoadType.WebMercator); //

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

    public (double x, double y) ToWebMercator()
    {
        CoordinateSharp.Coordinate c = new(this.Lat, this.Long, eagerLoad);

        WebMercator webMercator = c.WebMercator;
        return (webMercator.Easting, webMercator.Northing);
    }

    public static Coordinate ToCoordinate(double xWebMercator, double yWebMercator)
    {
        CoordinateSharp.Coordinate c = WebMercator.ConvertWebMercatortoLatLong(xWebMercator, yWebMercator, eagerLoad);
        return new Coordinate(c.Longitude.DecimalDegree, c.Latitude.DecimalDegree);
    }



    /// <summary>
    ///     Implements Vincenty's formula (using the inverse method variant) for calculating geodetic distances between two points on the Earth's surface.
    ///     Coordinates are assumed to be in the WGS84 datum.
    /// </summary>
    /// <remarks>
    /// <list type="bullet">
    /// <item>This method is very accurate (down to 0.5 mm in distance) but also very slow.</item>
    /// <item>However, WGS-84 datum is defined to be accurate to ±1m, which might overshadow the 0.5mm accuracy provided by Vincenty's formulae in several cases.</item>
    /// <item>The algorithm may fail to converge (and throw an exception) in rare cases,
    ///     particularly for nearly antipodal points (when the two points are located on nearly opposite sides of the Earth),</item>
    /// <item>where the iterative algorithm fails to converge to a solution within 50 iterations.</item>
    /// <item>The method should produce near identical results to this reference <see href="https://geodesyapps.ga.gov.au/vincenty-inverse"></see></item>
    /// </list>
    /// </remarks>
    /// <param name="coordinate">The coordinate pair (in WGS84 datum) to calculate the distance to.</param>
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

        const double flattening = 1d / 298.257223563;
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
    /// Translates the coordinate by given east-west and north-south distances.
    /// </summary>
    /// <param name="eastWestMeters">The eastward (positive) or westward (negative) distance in meters.</param>
    /// <param name="northSouthMeters">The northward (positive) or southward (negative) distance in meters.</param>
    /// <returns>The translated coordinate.</returns>
    public Coordinate TranslateByOffsets(double eastWestMeters, double northSouthMeters)
    {
        const double EarthRadius = 6378137.0; // Radius of the Earth in meters (WGS84)
        const double DegreesToRadians = Math.PI / 180.0;
        const double RadiansToDegrees = 180.0 / Math.PI;

        // Convert latitude and longitude to radians
        double latitudeRad = this.Lat * DegreesToRadians;
        double longitudeRad = this.Long * DegreesToRadians;

        // Calculate new latitude
        double newLatitudeRad = latitudeRad + (northSouthMeters / EarthRadius);
        double newLatitude = newLatitudeRad * RadiansToDegrees;

        // Correct for reaching beyond the poles
        if (newLatitude > 90)
        {
            newLatitude = 180 - newLatitude;
        }
        else if (newLatitude < -90)
        {
            newLatitude = -180 - newLatitude;
        }

        // Calculate the new longitude
        double latitudeCircumference = 2 * Math.PI * EarthRadius * Math.Cos(latitudeRad);
        double newLongitudeRad = longitudeRad + (eastWestMeters / latitudeCircumference);
        double newLongitude = newLongitudeRad * RadiansToDegrees;

        // Normalize the longitude to be within [-180, 180]
        newLongitude = NormalizeLong(newLongitude);

        return new Coordinate(newLongitude, newLatitude);
    }

    /// <summary>
    /// Translates the coordinate by a given bearing and distance.
    /// </summary>
    /// <param name="bearingInDegrees">The bearing in degrees.</param>
    /// <param name="lenInMeters">The distance in meters.</param>
    /// <returns>The translated coordinate.</returns>
    public Coordinate TranslateByBearingAndDistance(double bearingInDegrees, double lenInMeters)
    {

        const double flattening = 1d / 298.257223563; 
        const double radiusLong = 6378137.0d;
        const double radiusShort = radiusLong * (1 - flattening);
        static double Sqr(double x) => x * x;
        static double ToRadians(double deg) => deg * Math.PI / 180;
        static double ToDegrees(double rad) => rad * 180 / Math.PI;
        double bearingInRad = ToRadians(bearingInDegrees);
        double longRad = ToRadians(this.Long);
        double latRad = ToRadians(this.Lat);
        
        var U1 = Math.Atan((1 - flattening) * Math.Tan(latRad));
        var sigma1 = Math.Atan(Math.Tan(U1) / Math.Cos(bearingInRad));
        var alpha = Math.Asin(Math.Cos(U1) * Math.Sin(bearingInRad));
        var u2 = Sqr(Math.Cos(alpha)) * (Sqr(radiusLong) - Sqr(radiusShort)) / Sqr(radiusShort);
        var A = 1 + (u2 / 16384) * (4096 + u2 * (-768 + u2 * (320 - 175 * u2)));
        var B = (u2 / 1024) * (256 + u2 * (-128 + u2 * (74 - 47 * u2)));
        var sigma = lenInMeters / radiusShort / A;

        double sigma0;
        double dm2;

        do
        {
            sigma0 = sigma;
            dm2 = 2d * sigma1 + sigma;
            var tempX = Math.Cos(sigma) * (-1d + 2d * Sqr(Math.Cos(dm2))) - B / 6d * Math.Cos(dm2) * (-3d + 4d * Sqr(Math.Sin(dm2))) * (-3d + 4d * Sqr(Math.Cos(dm2)));
            var dSigma = B * Math.Sin(sigma) * (Math.Cos(dm2) + B / 4 * tempX);
            sigma = lenInMeters / radiusShort / A + dSigma;
        } while (Math.Abs(sigma0 - sigma) > 1e-9);

        var x = Math.Sin(U1) * Math.Cos(sigma) + Math.Cos(U1) * Math.Sin(sigma) * Math.Cos(bearingInRad);
        var hxy = Math.Sqrt(Sqr(Math.Sin(alpha)) + Sqr(Math.Sin(U1) * Math.Sin(sigma) - Math.Cos(U1) * Math.Cos(sigma) * Math.Cos(bearingInRad)));
        var tempF = (1.0 - flattening);
        var y = tempF * hxy;
        var lamda = Math.Sin(sigma) * Math.Sin(bearingInRad) / (Math.Cos(U1) * Math.Cos(sigma) - Math.Sin(U1) * Math.Sin(sigma) * Math.Cos(bearingInRad));
        lamda = Math.Atan(lamda);
        var C = (flattening / 16) * Sqr(Math.Cos(alpha)) * (4 + flattening * (4 - 3 * Sqr(Math.Cos(alpha))));
        var z = Math.Cos(dm2) + C * Math.Cos(sigma) * (-1 + 2 * Sqr(Math.Cos(dm2)));
        var omega = lamda - (1 - C) * flattening * Math.Sin(alpha) * (sigma + C * Math.Sin(sigma) * z);

        return new Coordinate(longitude: ToDegrees(longRad + omega), latitude: ToDegrees(Math.Atan(x / y)));
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
    //public static Coordinate Interpolate(Coordinate a, Coordinate b, double fraction)

    //{
    //    var (dx, dy) = a.CartesianOffset(b);

    //    // Calculate the translated distance as a percentage of the total distance
    //    double translatedDx = dx * fraction;
    //    double translatedDy = dy * fraction;

    //    // Translate the start coordinate by the calculated distances
    //    return a.Translate(translatedDx, translatedDy);
    //}


    /// <summary>
    ///     A world coordinate as a string in a JSON friendly format
    /// </summary>
    /// <returns>The coordinate as a string in a JSON friendly format.</returns>
    public override string ToString() => $"{{ \"long\": {Long.ToXmlString()}, \"lat\": {Lat.ToXmlString()} }}";
}