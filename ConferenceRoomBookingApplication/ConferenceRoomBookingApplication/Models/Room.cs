using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Room
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public int Seats { get; set; }
        public string Description { get; set; }
        public int Price { get; set; }
        public ICollection<Facility> Facilities { get; set; }

        public static void ShowRooms(User currentUser)
        {
            using (var myDb = new MyDbContext())
            {
                foreach (var room in myDb.Rooms.Include(f => f.Facilities))
                {
                    Console.WriteLine("[" + room.Id + "] " + room.Name + ": " + room.Description);
                    Console.Write("Facilities: ");
                    for (int i = 0; i < room.Facilities.Count; i++)
                    {
                        Console.Write(room.Facilities.ToList()[i].Name);
                        if (i < room.Facilities.Count - 1)
                        {
                            Console.Write(", ");
                        }
                    }
                    Console.WriteLine("\nSize: " + Enum.GetName(typeof(MyEnums.Size), room.Size) + ", Seats: " + room.Seats + ", Price: " + room.Price + " SEK");
                    Console.WriteLine();
                }
            }
            Navigation.ShowReturnOption(currentUser);
        }

        public static void AddRoom(User currentUser)
        {
            Console.Write("Name: ");
            string name = Console.ReadLine();

            bool success = false;
            int size = 0;
            while (!success || size < 1 || size > Enum.GetNames(typeof(MyEnums.Size)).Length)
            {
                Console.Clear();
                Console.WriteLine("Size: ");

                foreach (int i in Enum.GetValues(typeof(MyEnums.Size)))
                {
                    Console.WriteLine("[" + i + "] " + Enum.GetName(typeof(MyEnums.Size), i));
                }
                success = int.TryParse(Console.ReadKey(true).KeyChar.ToString(), out size);
            }

            int seats = 0;
            success = false;
            while (!success || seats < 0)
            {
                Console.Clear();
                Console.Write("Seats: ");
                string seatsInput = Console.ReadLine();
                success = int.TryParse(seatsInput, out seats);
            }

            Console.Write("Description: ");
            string description = Console.ReadLine();

            int price = 0;
            success = false;
            while (!success || price < 0)
            {
                Console.Clear();
                Console.Write("Price: ");
                string priceInput = Console.ReadLine();
                success = int.TryParse(priceInput, out price);
            }

            //Add facilities
            List<Facility> roomFacilities = new List<Facility>();
            int facilityId = 0;
            success = false;
            bool done = false;
            while (!done)
            {
                Console.Clear();
                Console.WriteLine("Enter the number of the facility to add, when done enter 0");
                Facility.ShowFacilities();
                string facilityIdInput = Console.ReadLine();
                success = int.TryParse(facilityIdInput, out facilityId);

                if (success)
                {
                    if (facilityId == 0)
                    {
                        done = true;
                    }
                    else if (facilityId > 0 && facilityId <= Facility.GetFacilityCount())
                    {
                        Facility facility = Facility.GetFacility(facilityId);
                        roomFacilities.Add(facility);
                    }
                }
            }

            var room = new Room
            {
                Name = name,
                Size = size,
                Seats = seats,
                Description = description,
                Price = price
            };

            room.Facilities = new List<Facility>();
            Console.Clear();

            try
            {
                using (var myDb = new MyDbContext())
                {
                    for (int i = 0; i < roomFacilities.Count; i++)
                    {
                        room.Facilities.Add(myDb.Facilities.Where(x => x.Id == roomFacilities[i].Id).SingleOrDefault());
                    }

                    myDb.Rooms.Add(room);
                    myDb.SaveChanges();
                    Console.WriteLine("Room added!\n");
                }
            }
            catch (Exception error)
            {
                Console.WriteLine("Failed to add the new room to the database");
                Console.WriteLine("Press enter to go back\n");
                Console.WriteLine(error.ToString());
                Console.ReadLine();
            }
            Navigation.ToMenu(currentUser);
        }

        public static int CountRooms()
        {
            using (var myDb = new MyDbContext())
            {
                return myDb.Rooms.Count();
            }
        }
    }
}
