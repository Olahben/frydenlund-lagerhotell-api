namespace LagerhotellAPI.Models
{
    public class AddLocationRequest
    {
        public AddLocationRequest(Location location)
        {
            Location = location;
        }
        public Location Location { get; set; }
    }
}
