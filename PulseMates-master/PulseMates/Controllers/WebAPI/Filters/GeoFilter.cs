namespace PulseMates.Controllers.WebAPI.Filters
{
    public class GeoFilter
    {
        public GeoFilter()
        {
            Index = 0; Size = 10;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Radius { get; set; }

        public int Index { get; set; }
        public int Size { get; set; }

        public bool IsEmpty
        {
            get { return !HasLocation; }
        }

        public bool HasLocation
        {
            get { return Latitude != 0 && Longitude != 0 && Radius != 0; }
        }
    }
}