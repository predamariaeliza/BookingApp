using BookingApp.Domain.Entities;

namespace BookingAppWeb.ViewModels
{
    public class HomeVM
    {
        public IEnumerable<Property>? PropertiesList { get; set; }
        public DateOnly CheckInDate  { get; set; }
        public DateOnly? CheckOutDate  { get; set; }
        public int Nights  { get; set; }
    }
}
