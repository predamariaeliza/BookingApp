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

        /// <summary>
        /// This is a function that is capable of finding the property rooms that are available, based on the parameter that we pass in our method
        /// This logic might break, you need to focus more on this !!!
        /// </summary>
        /// <param name="propertyId"></param>
        /// <param name="propertyNumberList"></param>
        /// <param name="checkInDate"></param>
        /// <param name="nights"></param>
        /// <param name="bookings"></param>
        /// <returns></returns>
        public static int PropertyRoomsAvailable_Count(int propertyId, List<PropertyNumber> propertyNumberList, DateOnly checkInDate, int nights, List<Booking> bookings)
        {
            //how many bookings collide with our date
            List<int> bookingInDate = new();
            int finalAvailableRoomForAllNights = int.MaxValue;
            var roomsInProperty = propertyNumberList.Where(x => x.PropertyId == propertyId).Count();

            //this is the total number of rooms in the property available for each night
            for (int i = 0; i < nights; i++)
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
                if (totalAvailableRooms == 0)
                {
                    return 0;
                }
                else
                {
                    //how we get the lowest value of the available rooms for every night
                    if (finalAvailableRoomForAllNights > totalAvailableRooms)
                    {
                        finalAvailableRoomForAllNights = totalAvailableRooms;
                    }
                }
            }

            return finalAvailableRoomForAllNights;
        }
    }
}
