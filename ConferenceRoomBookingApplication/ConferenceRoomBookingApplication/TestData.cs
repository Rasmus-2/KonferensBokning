using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication
{
    internal class TestData
    {
        public static void AddToDatabase()
        {
            using (var myDb = new Models.MyDbContext())
            {
                Models.Facility whiteboard = new Models.Facility { Name = "Whiteboard" };
                Models.Facility projector = new Models.Facility { Name = "Projector" };
                Models.Facility smartboard = new Models.Facility { Name = "Smartboard" };
                Models.Facility overheadProjector = new Models.Facility { Name = "Overhead projector" };
                Models.Facility surroundSpeakers = new Models.Facility { Name = "Surround speakers" };
                Models.Facility projectorScreen = new Models.Facility { Name = "Large Projector screen" };
                Models.Facility microphones = new Models.Facility { Name = "Microphones" };
                Models.Facility discoLightning = new Models.Facility { Name = "Disco lightning" };

                myDb.AddRange(whiteboard, projector, smartboard, overheadProjector, surroundSpeakers, projectorScreen, microphones, discoLightning);

                Models.Room room1 = new Models.Room { Name = "Lagerlöf", Size = 3, Seats = 40, Description = "Jungle-themed room with lots of plants and bamboo furniture", Price = 1400, Facilities = new List<Models.Facility> { whiteboard, projector, surroundSpeakers } };
                Models.Room room2 = new Models.Room { Name = "Strindberg", Size = 2, Seats = 30, Description = "Red-themed room with an industrial design", Price = 1100, Facilities = new List<Models.Facility> { whiteboard, projector, overheadProjector } };
                Models.Room room3 = new Models.Room { Name = "Martinsson", Size = 4, Seats = 70, Description = "Blue-themed room with a circular table placement", Price = 1900, Facilities = new List<Models.Facility> { projector, projectorScreen, microphones } };
                Models.Room room4 = new Models.Room { Name = "Lindgren", Size = 5, Seats = 120, Description = "Yellow-themed open room with lots of natural light on sunny days", Price = 2700, Facilities = new List<Models.Facility> { projector, projectorScreen, microphones, surroundSpeakers } };

                myDb.AddRange(room1, room2, room3, room4);

                Models.Country sweden = new Models.Country { Name = "Sweden" };
                Models.Country denmark = new Models.Country { Name = "Denmark" };
                Models.City stockholm = new Models.City { Name = "Stockholm" };
                Models.City gothenburg = new Models.City { Name = "Gothenburg" };

                myDb.AddRange(sweden, denmark, stockholm, gothenburg);

                Models.Company company1 = new Models.Company { Name = "Cozy City Conferences", RegistrationNumber = "558043-3296", Street = "Storgatan 5", PostalCode = "12345", CityId = 1, CountryId = 1 };
                Models.Company company2 = new Models.Company { Name = "IKEA", RegistrationNumber = "556074-7569", Street = "Ringvägen 23", PostalCode = "67890", CityId = 1, CountryId = 1 };
                Models.Company company3 = new Models.Company { Name = "Online_Solutions AB", RegistrationNumber = "558712-2813", Street = "Kungsvägen 11", PostalCode = "13579", CityId = 2, CountryId = 1 };

                myDb.AddRange(company1, company2, company3);

                Models.User alice = new Models.User { Name = "Adam Andersson", UserName = "admin", Password = "qwerty", Email = "adam_admin@gmail.com", Phone = "+46731234567", CompanyId = 1, Department = "It", IsAdmin = true };
                Models.User bob = new Models.User { Name = "Bob Bengtsson", UserName = "bob", Password = "1234", Email = "bobby1@gmail.com", Phone = "+46701438517", CompanyId = 2, Department = "Human Resources", IsAdmin = false };
                Models.User chloe = new Models.User { Name = "Chloe Carlsson", UserName = "chloe", Password = "asdf", Email = "chloe_carlsson@hotmail.se", Phone = "+46721335565", CompanyId = 3, Department = "Sales", IsAdmin = false };
                Models.User david = new Models.User { Name = "David Dahl", UserName = "david", Password = "password", Email = "ddahl@yahoo.com", Phone = "+46769896150", CompanyId = 2, Department = "Intra store Coordination", IsAdmin = false };

                myDb.AddRange(alice, bob, chloe, david);

                //Vecka 3
                Models.Booking booking1 = new Models.Booking() { Date = new DateTime(2024, 01, 15), RoomId = 1, Lunches = 35, UserId = 2 };
                Models.Booking booking2 = new Models.Booking() { Date = new DateTime(2024, 01, 16), RoomId = 1, Lunches = 30, UserId = 3 };
                Models.Booking booking3 = new Models.Booking() { Date = new DateTime(2024, 01, 19), RoomId = 2, Lunches = 25, UserId = 3 };
                Models.Booking booking4 = new Models.Booking() { Date = new DateTime(2024, 01, 19), RoomId = 3, Lunches = 60, UserId = 3 };
                Models.Booking booking5 = new Models.Booking() { Date = new DateTime(2024, 01, 15), RoomId = 4, Lunches = 110, UserId = 4 };
                Models.Booking booking6 = new Models.Booking() { Date = new DateTime(2024, 01, 16), RoomId = 4, Lunches = 100, UserId = 4 };

                //Vecka 4
                Models.Booking booking7 = new Models.Booking() { Date = new DateTime(2024, 01, 23), RoomId = 1, Lunches = 32, UserId = 2 };
                Models.Booking booking8 = new Models.Booking() { Date = new DateTime(2024, 01, 24), RoomId = 1, Lunches = 35, UserId = 3 };
                Models.Booking booking9 = new Models.Booking() { Date = new DateTime(2024, 01, 22), RoomId = 2, Lunches = 20, UserId = 3 };
                Models.Booking booking10 = new Models.Booking() { Date = new DateTime(2024, 01, 25), RoomId = 3, Lunches = 65, UserId = 3 };
                Models.Booking booking11 = new Models.Booking() { Date = new DateTime(2024, 01, 26), RoomId = 4, Lunches = 115, UserId = 4 };
                Models.Booking booking12 = new Models.Booking() { Date = new DateTime(2024, 01, 24), RoomId = 2, Lunches = 24, UserId = 4 };

                //Vecka 5
                Models.Booking booking13 = new Models.Booking() { Date = new DateTime(2024, 01, 30), RoomId = 1, Lunches = 30, UserId = 2 };
                Models.Booking booking14 = new Models.Booking() { Date = new DateTime(2024, 01, 31), RoomId = 1, Lunches = 38, UserId = 2 };
                Models.Booking booking15 = new Models.Booking() { Date = new DateTime(2024, 02, 02), RoomId = 3, Lunches = 18, UserId = 3 };

                //Vecka 6
                Models.Booking booking16 = new Models.Booking() { Date = new DateTime(2024, 02, 05), RoomId = 3, Lunches = 60, UserId = 3 };
                Models.Booking booking17 = new Models.Booking() { Date = new DateTime(2024, 02, 07), RoomId = 4, Lunches = 112, UserId = 4 };
                Models.Booking booking18 = new Models.Booking() { Date = new DateTime(2024, 02, 09), RoomId = 4, Lunches = 23, UserId = 4 };

                myDb.AddRange(booking1, booking2, booking3, booking4, booking5, booking6, booking7, booking8, booking9, booking10, booking11, booking12, booking13, booking14, booking15, booking16, booking17, booking18);

                myDb.SaveChanges();
            }
        }
    }
}
