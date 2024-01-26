using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Booking
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int RoomId { get; set; }
        public DateTime Date { get; set; }
        public int Lunches { get; set; }

        public static void ShowMyBookings(User currentUser)
        {
            using (var myDb = new MyDbContext())
            {
                var myBookings = from b in myDb.Bookings
                                 where b.UserId == currentUser.Id
                                 select b;

                foreach (Booking booking in myBookings)
                {
                    Console.Write("Booking-Id: " + booking.Id + ", Room number: " + booking.RoomId + ", Date: " + booking.Date.ToShortDateString());
                    if (booking.Lunches > 0)
                    {
                        Console.Write(", Lunches ordered: " + booking.Lunches);
                    }
                    Console.WriteLine("\n");
                }
            }
            Console.WriteLine("[1] Return to menu");
            Console.WriteLine("[2] Cancel booking");
            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    Navigation.ToMenu(currentUser);
                    break;
                case '2':
                    CancelBooking(currentUser);
                    break;
                default:
                    Console.Clear();
                    ShowMyBookings(currentUser);
                    break;
            }
        }

        public static Booking GetOccupied(DateTime date, int roomId)
        {
            using (var myDb = new MyDbContext())
            {
                var occupied = (from b in myDb.Bookings
                                where b.Date == date
                                && b.RoomId == roomId
                                select b).FirstOrDefault();

                return occupied;
            }
        }

        public static void MakeBooking(User currentUser, int week)
        {
            bool success = false;
            int room = 0;
            while (!success || room < 1 || room > Room.CountRooms())
            {
                Console.Clear();
                Info.ShowCalendar(week, DateTime.Now.Year);
                Console.Write("\nWhich room would you like to book: ");
                string input = Console.ReadLine();
                success = int.TryParse(input, out room);
            }

            success = false;
            int weekDay = 0;
            while (!success || weekDay < 1 || weekDay > 5)
            {
                Console.Clear();
                Info.ShowCalendar(week, DateTime.Now.Year);
                Console.WriteLine("\nSelected room: " + room);
                Console.WriteLine("\nMonday    = 1");
                Console.WriteLine("Tuesday   = 2");
                Console.WriteLine("Wednesday = 3");
                Console.WriteLine("Thursday  = 4");
                Console.WriteLine("Friday    = 5");
                Console.Write("\nWhich day of the week: ");
                string input = Console.ReadLine();
                success = int.TryParse(input, out weekDay);
            }

            DateTime mondayDate = Info.GetMondayDate(week, DateTime.Now.Year);
            DateTime choosenDate = mondayDate.AddDays(weekDay - 1);
            Booking occupied = GetOccupied(choosenDate, room);

            if (occupied == null)
            {
                int lunches = 0;
                Console.WriteLine("\nThe room is available at that date!");
                Console.Write("Would you like to order lunch as well? (" + Lunch.LUNCH_PRICE + " SEK per person) ");
                string lunchAnswer = Console.ReadLine();
                switch (lunchAnswer.ToLower())
                {
                    case "yes":
                    case "y":
                    case "sure":
                    case "yeah":
                    case "okay":
                    case "okey":
                    case "ok":
                        success = false;
                        while (!success || lunches < 0)
                        {
                            Console.Clear();
                            Console.Write("How many lunches? ");
                            string lunchInput = Console.ReadLine();
                            success = int.TryParse(lunchInput, out lunches);
                        }
                        break;
                }
                Booking newBooking = new Booking { Date = choosenDate, UserId = currentUser.Id, RoomId = room, Lunches = lunches };

                using (var myDb = new MyDbContext())
                {
                    myDb.Bookings.Add(newBooking);
                    myDb.SaveChanges();
                }
                Console.Clear();
                Console.WriteLine("Room successfully booked!\n");
            }
            else
            {
                Console.Clear();
                Console.WriteLine("The room is occupied at that date!\n");

            }
            Navigation.ToMenu(currentUser);
        }

        public static void CancelBooking(User currentUser)
        {
            int bookingId = 0;
            bool success = false;
            while (!success)
            {
                Console.Clear();
                Console.Write("Enter id for the booking you'd like to cancel: ");
                string input = Console.ReadLine();
                success = int.TryParse(input, out bookingId);
            }

            using (var myDb = new MyDbContext())
            {
                var booking = (from b in myDb.Bookings
                               where b.Id == bookingId
                               && b.UserId == currentUser.Id
                               select b).FirstOrDefault();

                if (booking == null)
                {
                    Console.Clear();
                    Console.WriteLine("Could not find your booking with that id\n");
                }
                else
                {
                    myDb.Bookings.Remove(booking);
                    myDb.SaveChanges();
                    Console.Clear();
                    Console.WriteLine("Booking removed!\n");
                }
            }
            Navigation.ToMenu(currentUser);
        }
    }
}
