using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication
{
    internal class Info
    {
        public static int GetCurrentWeek()
        {
            DateTime dateTime = DateTime.Now;
            Calendar calendar = new CultureInfo("en-US").Calendar;
            return calendar.GetWeekOfYear(dateTime, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

        public static DateTime GetMondayDate(int week, int year)
        {
            DateTime firstOfJanuari = new DateTime(year, 1, 1);
            int daysOffset = DayOfWeek.Thursday - firstOfJanuari.DayOfWeek;
            DateTime firstThursday = firstOfJanuari.AddDays(daysOffset);
            var cal = CultureInfo.CurrentCulture.Calendar;
            int firstWeek = cal.GetWeekOfYear(firstThursday, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            if (firstWeek == 1)
            {
                week--;
            }
            return firstThursday.AddDays((week * 7) - 3);
        }

        public static void ShowCalendar(int week, int year)
        {
            DateTime choosenDate = GetMondayDate(week, year);
            List<string> weekDays = new List<string>()
            {
                "Monday   ",
                "Tuesday  ",
                "Wednesday",
                "Thursday ",
                "Friday   "
            };

            using (var myDb = new Models.MyDbContext())
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine("Selected week: " + week);
                Console.WriteLine("-----------------------");

                Console.Write("\t");
                foreach (Models.Room room in myDb.Rooms)
                {
                    Console.Write("\t" + "Rum " + room.Id);
                }
                Console.WriteLine();

                for (int i = 0; i < weekDays.Count; i++)
                {
                    Console.Write("\n" + weekDays[i] + "\t");
                    foreach (Models.Room room in myDb.Rooms)
                    {
                        Models.Booking occupied = Models.Booking.GetOccupied(choosenDate, room.Id);

                        if (occupied != null)
                        {
                            Console.Write("(" + occupied.UserId + ")\t");
                        }
                        else
                        {
                            Console.Write("Free\t");
                        }
                    }
                    Console.WriteLine();
                    choosenDate = choosenDate.AddDays(1);
                }
            }
        }

        public static void ShowMyCosts(Models.User currentUser)
        {
            int lunchPrice = Models.Lunch.LUNCH_PRICE;
            int totalCost = 0;

            using (var myDb = new Models.MyDbContext())
            {
                var costsPerBooking = from b in myDb.Bookings
                                      join r in myDb.Rooms on b.RoomId equals r.Id
                                      where b.UserId == currentUser.Id
                                      select new
                                      {
                                          BookingId = b.Id,
                                          RoomId = r.Id,
                                          RoomCost = r.Price,
                                          Lunches = b.Lunches
                                      };

                foreach (var booking in costsPerBooking)
                {
                    Console.Write("Booking id: " + booking.BookingId + ", Room id: " + booking.RoomId + ", Room cost: " + booking.RoomCost + " SEK");
                    if (booking.Lunches > 0)
                    {
                        Console.Write(" + " + booking.Lunches + " lunches á " + lunchPrice + " SEK");
                    }
                    totalCost += booking.RoomCost + (booking.Lunches * lunchPrice);
                    Console.WriteLine(" | Booking total: " + (booking.RoomCost + (booking.Lunches * lunchPrice)) + "\n");
                }
            }
            Console.WriteLine("Total cost all bookings: " + totalCost + " SEK\n");

            Navigation.ShowReturnOption(currentUser);
        }

        public static void ShowEconomy()
        {
            using (var myDb = new Models.MyDbContext())
            {
                Console.WriteLine("-----------------------");
                Console.WriteLine("Total income");
                Console.WriteLine("-----------------------\n");

                //Total income; past and future projections
                int totalAndProjectedRooms = 0;
                int totalAndProjectedLunches = 0;
                foreach (var b in myDb.Bookings.ToList())
                {
                    totalAndProjectedRooms += myDb.Rooms.Where(x => x.Id == b.RoomId).FirstOrDefault().Price;
                    totalAndProjectedLunches += b.Lunches * Models.Lunch.LUNCH_PRICE;
                }
                Console.WriteLine("Past dates and preliminary bookings: " + (totalAndProjectedRooms + totalAndProjectedLunches) + " SEK");
                Console.WriteLine("From booking fees: " + totalAndProjectedRooms + " SEK, from lunches: " + totalAndProjectedLunches + " SEK");
                Console.WriteLine("\n");


                //Income for dates that has passed
                int totalRooms = 0;
                int totalLunches = 0;
                foreach (var b in myDb.Bookings.Where(x => x.Date < DateTime.Now).ToList())
                {
                    totalRooms += myDb.Rooms.Where(x => x.Id == b.RoomId).FirstOrDefault().Price;
                    totalLunches += b.Lunches * Models.Lunch.LUNCH_PRICE;
                }
                Console.WriteLine("Completed bookings: " + (totalRooms + totalLunches) + " SEK");
                Console.WriteLine("From booking fees: " + totalRooms + " SEK, from lunches: " + totalLunches + " SEK");
                Console.WriteLine("\n");


                //Projected income for future dates
                int projectedRooms = 0;
                int projectedLunches = 0;
                foreach (var b in myDb.Bookings.Where(x => x.Date >= DateTime.Now).ToList())
                {
                    projectedRooms += myDb.Rooms.Where(x => x.Id == b.RoomId).FirstOrDefault().Price;
                    projectedLunches += b.Lunches * Models.Lunch.LUNCH_PRICE;
                }
                Console.WriteLine("Projections, preliminary bookings: " + (projectedRooms + projectedLunches) + " SEK");
                Console.WriteLine("From booking fees: " + projectedRooms + " SEK, from lunches: " + projectedLunches + " SEK");
                Console.WriteLine("\n");


                //Income per week
                int weekRooms = 0;
                int weekLunches = 0;
                int numWeeks = 5;
                int weekDays = 5;

                for (int i = 0; i < numWeeks; i++)
                {
                    int week = GetCurrentWeek();
                    week += i - 1;
                    DateTime mondayDate = GetMondayDate(week, DateTime.Now.Year);

                    for (int j = 0; j < weekDays; j++)
                    {
                        foreach (var b in myDb.Bookings.Where(x => x.Date == mondayDate).ToList())
                        {
                            weekRooms += myDb.Rooms.Where(x => x.Id == b.RoomId).FirstOrDefault().Price;
                            weekLunches += b.Lunches * Models.Lunch.LUNCH_PRICE;
                        }
                        mondayDate = mondayDate.AddDays(1);
                    }
                    Console.WriteLine("Week " + week + " total: " + (weekRooms + weekLunches) + " SEK");
                    Console.WriteLine("\tFrom booking fees: " + weekRooms + " SEK");
                    Console.WriteLine("\tFrom lunches: " + weekLunches + " SEK");
                    Console.WriteLine();
                    weekRooms = 0;
                    weekLunches = 0;
                }
                Console.WriteLine("\n");


                //More query ideas:
                //Income per person
                //Income per company
            }
        }

        public static void ShowStatistics()
        {
            using (var myDb = new Models.MyDbContext())
            {
                //Most popular rooms
                var bookingGroups = (from b in myDb.Bookings
                                     group b by b.RoomId into br
                                     orderby br.Count() descending
                                     select br.ToList()).ToList();

                Console.WriteLine("Rooms sorted by popularity\n");

                int place = 1;
                foreach (var group in bookingGroups)
                {
                    int bookings = group.Count();
                    int id = group.FirstOrDefault().RoomId;
                    Models.Room room = myDb.Rooms.Where(x => x.Id == id).FirstOrDefault();
                    Console.WriteLine(place + ". Id: (" + id + "), Name: " + room.Name + ", Bookings: " + bookings);
                    place++;
                }
                Console.WriteLine("\n");


                //Persons with most bookings
                var userGroups = (from b in myDb.Bookings
                                  group b by b.UserId into ub
                                  orderby ub.Count() descending
                                  select ub.ToList()).ToList();

                Console.WriteLine("People sorted by number of bookings\n");

                place = 1;
                foreach (var group in userGroups)
                {
                    int bookings = group.Count();
                    int id = group.FirstOrDefault().UserId;
                    Models.User user = myDb.Users.Where(x => x.Id == id).FirstOrDefault();
                    Console.WriteLine(place + ". Id: (" + id + "), Name: " + user.Name + ", number of bookings: " + bookings);
                    place++;
                }
                Console.WriteLine("\n");


                //Company with most bookings
                var companyBookings = (from b in myDb.Bookings
                                       join u in myDb.Users on b.UserId equals u.Id
                                       join c in myDb.Companies on u.CompanyId equals c.Id
                                       group c by c.Id into ci
                                       orderby ci.Count() descending
                                       select ci.ToList()).ToList();

                Console.WriteLine("Companies sorted by number of bookings\n");

                place = 1;
                foreach (var group in companyBookings)
                {
                    int bookings = group.Count();
                    int id = group.FirstOrDefault().Id;
                    Models.Company company = myDb.Companies.Where(x => x.Id == id).FirstOrDefault();
                    Console.WriteLine(place + ". Id: (" + id + "), Name: " + company.Name + ", number of bookings: " + bookings);
                    place++;
                }
                Console.WriteLine("\n");


                //Percentages of free/booked rooms in a given week
                Console.WriteLine("Current week: " + GetCurrentWeek());
                Console.WriteLine("Booked Rooms percentage");

                int numWeeks = 5;
                int weekDays = 5;
                for (int i = 0; i < numWeeks; i++)
                {
                    int week = GetCurrentWeek();
                    week += i - 1;
                    DateTime mondayDate = GetMondayDate(week, DateTime.Now.Year);
                    int bookedRooms = 0;

                    for (int j = 0; j < weekDays; j++)
                    {
                        bookedRooms += myDb.Bookings.Where(x => x.Date == mondayDate).Count();
                        mondayDate = mondayDate.AddDays(1);
                    }

                    double bookedPercentage = ((double)bookedRooms / (double)(myDb.Rooms.Count() * weekDays) * 100);
                    Console.WriteLine("\tweek " + week + ": " + bookedPercentage + "%");
                }
                Console.WriteLine("\n");


                //More query ideas:
                //Most popular weekDay to book
                //Percentages of free/booked room in a given month
                //Percentages of free/booked room in a given year
                //Most popular facilities
            }
        }
    }
}
