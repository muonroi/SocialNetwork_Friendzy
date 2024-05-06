namespace SearchPartners.Service.Helpers;

public static class DistanceCalculatorHelper
{
    public static double CalculateDistance(double userLat, double userLon, double storeLat, double storeLon)
    {
        double userLatRad = ToRadians(userLat);
        double userLonRad = ToRadians(userLon);
        double storeLatRad = ToRadians(storeLat);
        double storeLonRad = ToRadians(storeLon);

        double deltaLat = storeLatRad - userLatRad;
        double deltaLon = storeLonRad - userLonRad;

        double haversinePart = Math.Pow(Math.Sin(deltaLat / 2), 2) +
                   (Math.Cos(userLatRad) * Math.Cos(storeLatRad) *
                   Math.Pow(Math.Sin(deltaLon / 2), 2));

        double distanceInRadians = 2 * Math.Atan2(Math.Sqrt(haversinePart), Math.Sqrt(1 - haversinePart));

        return SearchPartnersConstants.EarthRadiusKm * distanceInRadians;
    }

    public static List<CoordinateDTO> SortCoordinatesByDistance(double clientLat, double clientLon, List<CoordinateDTO> coordinates)
    {
        coordinates.Sort((c1, c2) =>
        {
            double userDistance = CalculateDistance(clientLat, clientLon, c1.Latitude, c1.Longitude);
            double storeDistance = CalculateDistance(clientLat, clientLon, c2.Latitude, c2.Longitude);
            return userDistance.CompareTo(storeDistance);
        });

        return coordinates;
    }

    private static double ToRadians(double degrees)
    {
        return degrees * Math.PI / 180;
    }
}