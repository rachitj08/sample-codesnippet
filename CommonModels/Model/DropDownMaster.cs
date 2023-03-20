namespace Common.Model
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
        public long SubLocationTypeId { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
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
    public class CountryVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    } 
    public class StateVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    } 
    public class CityVM
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
