using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Facility
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Room> Rooms { get; set; }

        public static int GetFacilityCount()
        {
            using (var myDb = new MyDbContext())
            {
                return myDb.Facilities.Count();
            }
        }

        public static void ShowFacilities()
        {
            using (var myDb = new MyDbContext())
            {
                foreach (Facility facility in myDb.Facilities)
                {
                    Console.WriteLine("[" + facility.Id + "] " + facility.Name);
                }
            }
        }

        public static Facility GetFacility(int facilityId)
        {
            using (var myDb = new MyDbContext())
            {
                var facility = (from f in myDb.Facilities
                                where f.Id == facilityId
                                select f).FirstOrDefault();
                return facility;
            }
        }
    }
}
