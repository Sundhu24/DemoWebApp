namespace DemoWebApp.Model
{
    public class City
    {
        public int Id { get; set; }
        public string Cityname { get; set; }
        public int? CountryId { get; set; }
        public int? Population { get; set; }
        public int? Rating { get; set; }
        public int? Points { get; set; }
    }
}
