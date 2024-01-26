using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Country
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public static int GetOrCreateCountryId(string countryName)
        {
            using (var myDb = new MyDbContext())
            {
                var countrySearch = (from c in myDb.Countries
                                     where c.Name == countryName
                                     select c).SingleOrDefault();

                if (countrySearch == null)
                {
                    Country country = new Country() { Name = countryName };
                    myDb.Countries.Add(country);
                    myDb.SaveChanges();

                    var newCountryId = (from c in myDb.Countries
                                        where c.Name == countryName
                                        select c.Id).SingleOrDefault();
                    return newCountryId;
                }
                else
                {
                    return countrySearch.Id;
                }
            }
        }

        public static Country GetCountry(int countryId)
        {
            using (var myDb = new MyDbContext())
            {
                var country = (from c in myDb.Countries
                               where c.Id == countryId
                               select c).SingleOrDefault();
                return country;
            }
        }
    }
}
