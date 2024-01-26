using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConferenceRoomBookingApplication.Models
{
    internal class Company
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string Street { get; set; }
        public string PostalCode { get; set; }
        public int CityId { get; set; }
        public int CountryId { get; set; }

        public static Company GetCompany(string companyName)
        {
            using (var myDb = new MyDbContext())
            {
                var company = (from c in myDb.Companies
                               where c.Name.ToLower() == companyName.ToLower()
                               select c).SingleOrDefault();
                return company;
            }
        }

        public static Company GetCompany(int companyId)
        {
            using (var myDb = new MyDbContext())
            {
                var company = (from c in myDb.Companies
                               where c.Id == companyId
                               select c).SingleOrDefault();
                return company;
            }
        }

        public static Company CreateCompany(string companyName)
        {
            Console.Write("Enter your companys registration number: ");
            string registrationNumber = Console.ReadLine();

            Console.WriteLine("Enter your companys billing address");
            Console.Write("Street: ");
            string street = Console.ReadLine();

            Console.Write("Postal code: ");
            string postalCode = Console.ReadLine();

            Console.Write("City: ");
            string city = Console.ReadLine();
            int cityId = City.GetOrCreateCityId(city);

            Console.Write("Country: ");
            string country = Console.ReadLine();
            int countryId = Country.GetOrCreateCountryId(country);

            Company company = new Company()
            {
                Name = companyName,
                RegistrationNumber = registrationNumber,
                Street = street,
                PostalCode = postalCode,
                CityId = cityId,
                CountryId = countryId
            };

            using (var myDb = new MyDbContext())
            {
                myDb.Companies.Add(company);
                myDb.SaveChanges();
                return GetCompany(companyName);
            }
        }

        public static void ShowCompanies(User currentUser)
        {
            int companyCount = 0;
            List<int> companyIds = new List<int>();
            List<string> companyNames = new List<string>();
            using (var myDb = new MyDbContext())
            {
                foreach (var company in myDb.Companies)
                {
                    companyIds.Add(company.Id);
                    companyNames.Add(company.Name);
                    companyCount++;
                }
            }

            bool success = false;
            int inputId = -1;
            while (!success)
            {
                for (int i = 0; i < companyIds.Count; i++)
                {
                    Console.WriteLine("[" + companyIds[i] + "] " + companyNames[i]);
                }
                Console.WriteLine("\n[0] Return to menu");
                Console.Write("Enter id of the company to view: ");
                string input = Console.ReadLine();
                success = int.TryParse(input, out inputId);
                if (success && inputId == 0)
                {
                    Console.Clear();
                    Navigation.ToMenu(currentUser);
                }
                else if (success && inputId > 0 && inputId <= companyCount)
                {
                    Console.Clear();
                    ShowCompanyInfo(inputId);
                    Navigation.ShowReturnOption(currentUser);
                }
                else
                {
                    success = false;
                }
                Console.Clear();
            }
        }

        public static void ShowCompanyInfo(int companyId)
        {
            Company company = GetCompany(companyId);
            Console.WriteLine("Id:         " + company.Id);
            Console.WriteLine("Name:       " + company.Name);
            Console.WriteLine("Reg number: " + company.RegistrationNumber);
            Console.WriteLine("Address:");
            Console.WriteLine("\t" + company.Street);
            Console.WriteLine("\t" + company.PostalCode);
            Console.WriteLine("\t" + City.GetCity(company.CityId).Name);
            Console.WriteLine("\t" + Country.GetCountry(company.CountryId).Name + "\n");
        }
    }
}
