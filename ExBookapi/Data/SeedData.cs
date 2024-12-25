using ExBookapi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExBookapi.Data
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider, IWebHostEnvironment env)
        {
            var context = serviceProvider.GetRequiredService<ComicSystemContext>();
            
            if (context.Customers.Any())
            {
                return; 
            }
            context.Customers.AddRange(
                new Customer
                {
                    FullName = "John Doe",
                    PhoneNumber = "123-456-7890",
                    Registration = DateTime.Now.AddYears(-1)
                },
                new Customer
                {
                    FullName = "Jane Smith",
                    PhoneNumber = "987-654-3210",
                    Registration = DateTime.Now.AddMonths(-6)
                }
            );
            context.SaveChanges();
        }
    }
}