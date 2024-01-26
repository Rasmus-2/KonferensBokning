using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication
{
    internal class Navigation
    {
        public static Models.User ShowStartPage()
        {
            Models.User currentUser = new Models.User();

            bool success = false;
            while (!success)
            {
                Console.WriteLine("Welcome to the booking app");
                Console.WriteLine("[1] Login");
                Console.WriteLine("[2] Create account");

                ConsoleKeyInfo key = Console.ReadKey();

                if (key.KeyChar == '1')
                {
                    currentUser = Login().Result;
                    success = true;
                }
                else if (key.KeyChar == '2')
                {
                    currentUser = Models.User.CreateAccount();
                    if (currentUser != null)
                    {
                        success = true;
                    }
                }
                Console.Clear();
            }
            string firstName = currentUser.Name.Split(' ').First();
            Console.WriteLine("Welcome " + firstName);
            return currentUser;
        }

        public static async Task<Models.User> Login()
        {
            Models.User user = new Models.User();
            Console.Clear();
            Console.Write("Enter username: ");

            Task<List<Models.User>> listOfUsers = Models.User.GetListOfUsers();

            string userName = Console.ReadLine();
            List<Models.User> users = await listOfUsers;
            user = users.Where(x => x.UserName == userName).FirstOrDefault();

            while (user == null)
            {
                Console.Clear();
                Console.WriteLine("We can't find your account, please try again");
                Console.Write("Enter username: ");
                userName = Console.ReadLine();
                user = users.Where(x => x.UserName == userName).FirstOrDefault();
            }

            Console.Write("Enter password: ");
            string password = Console.ReadLine();
            while (user.Password != password)
            {
                Console.Clear();
                Console.WriteLine("Wrong password, please try again");
                Console.Write("Enter password: ");
                password = Console.ReadLine();
            }
            return user;
        }

        public static void ToMenu(Models.User currentUser)
        {
            if (currentUser.IsAdmin)
            {
                Navigation.ShowAdminOptions(currentUser);
            }
            else
            {
                Navigation.ShowUserOptions(currentUser);
            }
        }

        public static void ShowReturnOption(Models.User currentUser)
        {
            Console.WriteLine("Press Enter to return");
            Console.ReadLine();
            Console.Clear();
            Navigation.ToMenu(currentUser);
        }

        public static void ShowUserOptions(Models.User currentUser)
        {
            Console.WriteLine("User options");
            Console.WriteLine("-------------------------");
            Console.WriteLine("[1] Show room info");
            Console.WriteLine("[2] Show booking calendar");
            Console.WriteLine("[3] View my bookings");
            Console.WriteLine("[4] Show costs");
            Console.WriteLine("[5] My account");
            Console.WriteLine("[6] Log out");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    Models.Room.ShowRooms(currentUser);
                    break;
                case '2':
                    Console.Clear();
                    Info.ShowCalendar(Info.GetCurrentWeek(), DateTime.Now.Year);
                    CalenderMenu(currentUser, Info.GetCurrentWeek());
                    break;
                case '3':
                    Console.Clear();
                    Models.Booking.ShowMyBookings(currentUser);
                    break;
                case '4':
                    Console.Clear();
                    Info.ShowMyCosts(currentUser);
                    break;
                case '5':
                    Console.Clear();
                    Models.User.ShowUserInfo(currentUser.Id);
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Models.Company.ShowCompanyInfo(currentUser.CompanyId);
                    ShowReturnOption(currentUser);
                    break;
                case '6':
                    Console.Clear();
                    currentUser = ShowStartPage();
                    ToMenu(currentUser);
                    break;
                default:
                    Console.Clear();
                    ShowUserOptions(currentUser);
                    break;
            }
        }

        public static void ShowAdminOptions(Models.User currentUser)
        {
            Console.WriteLine("Admin options");
            Console.WriteLine("-------------------------");
            Console.WriteLine("[1] Show room info");
            Console.WriteLine("[2] Show booking calendar");
            Console.WriteLine("[3] View my bookings");
            Console.WriteLine("[4] Add room");
            Console.WriteLine("[5] Show users");
            Console.WriteLine("[6] Show companies");
            Console.WriteLine("[7] Economic overview");
            Console.WriteLine("[8] Statistics");
            Console.WriteLine("[9] My Account");
            Console.WriteLine("[0] Log out");

            ConsoleKeyInfo key = Console.ReadKey();
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    Models.Room.ShowRooms(currentUser);
                    break;
                case '2':
                    Console.Clear();
                    Info.ShowCalendar(Info.GetCurrentWeek(), DateTime.Now.Year);
                    CalenderMenu(currentUser, 4);
                    break;
                case '3':
                    Console.Clear();
                    Models.Booking.ShowMyBookings(currentUser);
                    break;
                case '4':
                    Console.Clear();
                    Models.Room.AddRoom(currentUser);
                    break;
                case '5':
                    Console.Clear();
                    Models.User.ShowUsers(currentUser);
                    break;
                case '6':
                    Console.Clear();
                    Models.Company.ShowCompanies(currentUser);
                    break;
                case '7':
                    Console.Clear();
                    Info.ShowEconomy();
                    ShowReturnOption(currentUser);
                    break;
                case '8':
                    Console.Clear();
                    Info.ShowStatistics();
                    ShowReturnOption(currentUser);
                    break;
                case '9':
                    Console.Clear();
                    Models.User.ShowUserInfo(currentUser.Id);
                    Console.WriteLine("Press enter to continue");
                    Console.ReadLine();
                    Console.Clear();
                    Models.Company.ShowCompanyInfo(currentUser.CompanyId);
                    ShowReturnOption(currentUser);
                    break;
                case '0':
                    Console.Clear();
                    currentUser = ShowStartPage();
                    ToMenu(currentUser);
                    break;
                default:
                    Console.Clear();
                    ShowAdminOptions(currentUser);
                    break;
            }
        }

        public static void CalenderMenu(Models.User currentUser, int week)
        {
            Console.WriteLine("\n-----------------------");
            Console.WriteLine("Your id: (" + currentUser.Id + ")");
            Console.WriteLine("-----------------------");

            Console.WriteLine("\n[1] Return to menu");
            Console.WriteLine("[2] Change week");
            Console.WriteLine("[3] Make a booking");
            Console.WriteLine("[4] See my bookings");

            ConsoleKeyInfo key = Console.ReadKey(true);
            switch (key.KeyChar)
            {
                case '1':
                    Console.Clear();
                    ToMenu(currentUser);
                    break;
                case '2':
                    bool success = false;
                    while (!success)
                    {
                        Console.Clear();
                        Console.Write("Choose a week: ");
                        string inputWeek = Console.ReadLine();
                        success = int.TryParse(inputWeek, out week);
                        if (success && (week < 1 || week > 53))
                        {
                            success = false;
                        }
                    }
                    Console.Clear();
                    Info.ShowCalendar(week, DateTime.Now.Year);
                    CalenderMenu(currentUser, week);
                    break;
                case '3':
                    Models.Booking.MakeBooking(currentUser, week);
                    break;
                case '4':
                    Console.Clear();
                    Models.Booking.ShowMyBookings(currentUser);
                    break;
                default:
                    Console.Clear();
                    Info.ShowCalendar(week, DateTime.Now.Year);
                    CalenderMenu(currentUser, week);
                    break;
            }
        }
    }
}
