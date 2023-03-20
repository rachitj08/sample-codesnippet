namespace ParkingProvider.Portal.Model
{
    public class DropDownMaster
    {
        public string Id { get; set; }
        public string Name { get; set; }
    }
    public class ParkingProvider
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class SubLocation
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class SubLocationType
    {
        public string Name { get; set; }
    }
    public class ActivityCode
    {
        public string Name { get; set; }
    }
    public class ParkingSpot
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
