using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public int CompanyId { get; set; }
        public string Department { get; set; }
        public bool IsAdmin { get; set; }

        public static async Task<List<User>> GetListOfUsers()
        {
            using (var myDb = new MyDbContext())
            {
                var users = await myDb.Users.ToListAsync();
                return users;
            }
        }

        public static User GetUser(string userName)
        {
            using (var myDb = new MyDbContext())
            {
                var user = (from u in myDb.Users
                            where u.UserName == userName
                            select u).FirstOrDefault();
                return user;
            }
        }

        public static User GetUser(int userId)
        {
            using (var myDb = new MyDbContext())
            {
                var user = (from u in myDb.Users
                            where u.Id == userId
                            select u).FirstOrDefault();
                return user;
            }
        }

        public static User CreateAccount()
        {
            string name = "";
            while (!name.Contains(' ') || !name.Any(n => char.IsLetter(n)))
            {
                Console.Clear();
                Console.Write("Enter full name: ");
                name = Console.ReadLine();
            }

            string userName = "";
            do
            {
                Console.Clear();
                Console.Write("Choose a unique username: ");
                userName = Console.ReadLine();
            }
            while (GetUser(userName) != null);

            string password = "";
            while (password.Length < 8 || !password.Any(p => char.IsLetter(p)) || !password.Any(p => char.IsDigit(p)))
            {
                Console.Clear();
                Console.Write("Choose a password (minimum 8 characters, both letters and digits): ");
                password = Console.ReadLine();
            }

            string email = "";
            while (!email.Contains('@') || !email.Contains('.'))
            {
                Console.Clear();
                Console.Write("Enter a valid email: ");
                email = Console.ReadLine();
            }

            Console.Write("Enter phone number: ");
            string phoneNumber = Console.ReadLine();

            Console.Write("Enter company name: ");
            string companyName = Console.ReadLine();
            Company company = Company.GetCompany(companyName);
            if (company == null)
            {
                company = Company.CreateCompany(companyName);
            }

            Console.Write("Enter your department at your company: ");
            string department = Console.ReadLine();

            User newUser = new User()
            {
                Name = name,
                UserName = userName,
                Password = password,
                Email = email,
                Phone = phoneNumber,
                CompanyId = company.Id,
                Department = department,
                IsAdmin = false
            };

            User user = null;

            try
            {
                using (var myDb = new MyDbContext())
                {
                    myDb.Users.Add(newUser);
                    myDb.SaveChanges();
                    user = User.GetUser(userName);
                }
            }
            catch (Exception error)
            {
                Console.Clear();
                Console.WriteLine("Failed to add the new account to the database");
                Console.WriteLine("Press enter to go back\n");
                Console.WriteLine(error.ToString());
                Console.ReadLine();
            }
            return user;
        }

        public static void ShowUserInfo(int userId)
        {
            User user = User.GetUser(userId);
            if (user.IsAdmin)
            {
                Console.WriteLine("Id:           " + user.Id + " (Admin)");
            }
            else
            {
                Console.WriteLine("Id:           " + user.Id);
            }
            Console.WriteLine("Name:         " + user.Name);
            Console.WriteLine("Username:     " + user.UserName);
            Console.WriteLine("Email:        " + user.Email);
            Console.WriteLine("Phone number: " + user.Phone);
            Console.WriteLine("Company:      " + Company.GetCompany(user.CompanyId).Name);
            Console.WriteLine("Department:   " + user.Department + "\n");
        }

        public static void ShowUsers(User currentUser)
        {
            int userCount = 0;
            List<int> userIds = new List<int>();
            List<string> userNames = new List<string>();
            using (var myDb = new MyDbContext())
            {
                foreach (var user in myDb.Users)
                {
                    userIds.Add(user.Id);
                    userNames.Add(user.Name);
                    userCount++;
                }
            }

            bool success = false;
            int inputId = -1;
            while (!success)
            {
                for (int i = 0; i < userIds.Count; i++)
                {
                    Console.WriteLine("[" + userIds[i] + "] " + userNames[i]);
                }
                Console.WriteLine("\n[0] Return to menu");
                Console.Write("Enter id of the user to view: ");
                string input = Console.ReadLine();
                success = int.TryParse(input, out inputId);
                if (success && inputId == 0)
                {
                    Console.Clear();
                    Navigation.ToMenu(currentUser);
                }
                else if (success && inputId > 0 && inputId <= userCount)
                {
                    Console.Clear();
                    ShowUserInfo(inputId);
                    Navigation.ShowReturnOption(currentUser);
                }
                else
                {
                    success = false;
                }
                Console.Clear();
            }
        }
    }
}