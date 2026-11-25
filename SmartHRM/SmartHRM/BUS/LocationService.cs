namespace SmartHRM.BUS
{
    public interface ILocationService
    {
        double CalculateDistanceMeters(double lat1, double lon1, double lat2, double lon2);
        bool IsWithinAllowedRadius(double currentLat, double currentLon, double officeLat, double officeLon, double maxRadiusMeters);
    }
}
