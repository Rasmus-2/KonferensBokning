using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class City
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static int GetOrCreateCityId(string cityName)
        {
            using (var myDb = new MyDbContext())
            {
                var citySearch = (from c in myDb.Cities
                                  where c.Name == cityName
                                  select c).SingleOrDefault();

                if (citySearch == null)
                {
                    City city = new City() { Name = cityName };
                    myDb.Cities.Add(city);
                    myDb.SaveChanges();

                    var newCityId = (from c in myDb.Cities
                                     where c.Name == cityName
                                     select c.Id).SingleOrDefault();
                    return newCityId;
                }
                else
                {
                    return citySearch.Id;
                }
            }
        }

        public static City GetCity(int cityId)
        {
            using (var myDb = new MyDbContext())
            {
                var city = (from c in myDb.Cities
                            where c.Id == cityId
                            select c).SingleOrDefault();
                return city;
            }
        }

    }
}
