using LuceneNetEFCoreSearchTools.Web.Models;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LuceneNetEFCoreSearchTools.Web.Helpers
{
    public static class DbInitializer
    {


        public static void Initialize(AppDbContext context, IHostingEnvironment hostingEnvironment)
        {
            context.Database.EnsureCreated();

            var rootPath = hostingEnvironment.ContentRootPath;
            var usercsv = System.IO.Path.Combine(rootPath, "TestData", "MOCK_USERS.csv");
            var citycsv = System.IO.Path.Combine(rootPath, "TestData", "MOCK_CITIES.csv");

            if (context.Users.Any())
            {
                return; // DB has been seeded.
            }

            var allTestUsers = new List<User>();
            using (TextReader reader = new StreamReader(usercsv))
            {

                string data = reader.ReadLine();

                while ((data = reader.ReadLine()) != null)
                {
                    string[] line = data.Split(',');
                    allTestUsers.Add(new User()
                    {
                       // Id = int.Parse(line[0]),
                        FirstName = line[1],
                        Surname = line[2],
                        Email = line[3],
                        IndexId = new Guid(line[4]),
                        JobTitle = line[5]
                    });
                }


            }
            foreach (User u in allTestUsers)
            {
                context.Users.Add(u);
            }
            context.SaveChanges();

            var allTestCities = new List<City>();

            using (var reader = new StreamReader(citycsv))
            {


                string data = reader.ReadLine();
                while ((data = reader.ReadLine()) != null)
                {
                    string[] line = data.Split(',');
                    allTestCities.Add(new City()
                    {
                       // Id = int.Parse(line[0]),
                        Country = line[1],
                        Code = line[2],
                        Name = line[3],
                        IndexId = new Guid(line[4])
                    });
                }

                reader.Close();
            }

            foreach (City c in allTestCities)
            {
                context.Cities.Add(c);
            }
            context.SaveChanges();









        }
    }
}
