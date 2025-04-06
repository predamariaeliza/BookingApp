using BookingApp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace BookingApp.Application.Common.Utility
{
    public static class StaticDetails
    {
        public const string Role_Admin = "Admin";
        public const string Role_Customer = "Customer";
        public const string Role_PropertyOwner = "PropertyOwner";

        public const string StatusPending = "Pending";
        public const string StatusApproved = "Approved";
        public const string StatusCheckedIn = "CheckedIn";
        public const string StatusCompleted = "Completed";
        public const string StatusCancelled = "Cancelled";
        public const string StatusRefunded = "Refunded";

        public static int PropertyRoomsAvailable_Count(int propertyId, List<PropertyNumber> propertyNumberList, DateOnly checkInDate, int nights, List<Booking> bookings)
        {
            //how many bookings collide with our date
            List<int> bookingInDate = new();

            var roomsInProperty = propertyNumberList.Where(x => x.PropertyId == propertyId).Count();
            for(int i = 0; i < nights; i++)
            {
                //if property is booked for the selected criterias
                var propertyBooked = bookings.Where(u => u.CheckInDate <= checkInDate.AddDays(i)
                && u.CheckOutDate > checkInDate.AddDays(i) 
                && u.PropertyId == propertyId);

                foreach(var booking in propertyBooked)
                {
                    if(!bookingInDate.Contains(booking.Id))
                    {
                        bookingInDate.Add(booking.Id);
                    }
                }

                //calculate on that particular night how many rooms are available
                var totalAvailableRooms = roomsInProperty - bookingInDate.Count;
                if(totalAvailableRooms == 0)
                {
                    return 0;
                }
            }
        }
    }
}
