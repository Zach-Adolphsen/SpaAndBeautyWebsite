using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging; // Required for Logging
using SpaAndBeautyWebsite.Data;
using SpaAndBeautyWebsite.Interfaces;
using SpaAndBeautyWebsite.Models;
using System;
using System.Threading.Tasks;

namespace SpaAndBeautyWebsite.Services
{
    public class UserService : IUserService
    {
        private readonly IDbContextFactory<SpaAndBeautyWebsiteContext> _contextFactory;
        private readonly ILogger<UserService> _logger;

        // We inject ILogger to print logs to the Visual Studio Output window
        public UserService(IDbContextFactory<SpaAndBeautyWebsiteContext> contextFactory, ILogger<UserService> logger)
        {
            _contextFactory = contextFactory;
            _logger = logger;
        }

        public async Task<User?> GetProfileAsync(string emailOrUsername)
        {
            using var context = _contextFactory.CreateDbContext();

            // 1. Check Employee
            var employee = await context.Employee
                .AsNoTracking()
                .FirstOrDefaultAsync(e => e.Email == emailOrUsername || e.Username == emailOrUsername);

            if (employee != null)
            {
                return new User
                {
                    Id = employee.EmployeeId,
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.PhoneNumber,
                    JobTitle = employee.JobTitle,
                    Location = $"{employee.City}, {employee.State}",
                    UserType = "Employee"
                };
            }

            // 2. Check Customer
            var customer = await context.Customer
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Email == emailOrUsername || c.Username == emailOrUsername);

            if (customer != null)
            {
                return new User
                {
                    Id = customer.CustomerId,
                    FirstName = customer.FirstName,
                    LastName = customer.LastName,
                    Username = customer.Username,
                    Email = customer.Email,
                    Phone = customer.PhoneNumber,
                    JobTitle = "Valued Customer",
                    Location = "Online",
                    UserType = "Customer"
                };
            }

            return null;
        }

        public async Task UpdateProfileAsync(User profile)
        {
            _logger.LogInformation("--- STARTING SAVE ---");
            _logger.LogInformation("Attempting to save User: {Name} (ID: {Id}, Type: {Type})", profile.FirstName, profile.Id, profile.UserType);

            using var context = _contextFactory.CreateDbContext();

            if (profile.UserType == "Employee")
            {
                var employee = await context.Employee.FindAsync(profile.Id);
                if (employee == null)
                {
                    _logger.LogError("ERROR: Employee with ID {Id} was not found in the database.", profile.Id);
                    return;
                }

                // Update basic fields
                employee.FirstName = profile.FirstName;
                employee.LastName = profile.LastName;
                employee.PhoneNumber = profile.Phone;
                // employee.Username = profile.Username; // Uncomment if username changes are allowed

                // Update Location (Split "City, State")
                if (!string.IsNullOrWhiteSpace(profile.Location) && profile.Location.Contains(","))
                {
                    var parts = profile.Location.Split(',');
                    if (parts.Length >= 2)
                    {
                        employee.City = parts[0].Trim();
                        employee.State = parts[1].Trim();
                        _logger.LogInformation("Updated Location to: {City}, {State}", employee.City, employee.State);
                    }
                }

                await context.SaveChangesAsync();
                _logger.LogInformation("SUCCESS: Employee record saved.");
            }
            else if (profile.UserType == "Customer")
            {
                var customer = await context.Customer.FindAsync(profile.Id);
                if (customer == null)
                {
                    _logger.LogError("ERROR: Customer with ID {Id} was not found.", profile.Id);
                    return;
                }

                customer.FirstName = profile.FirstName;
                customer.LastName = profile.LastName;
                customer.PhoneNumber = profile.Phone;
                // customer.Username = profile.Username; 

                await context.SaveChangesAsync();
                _logger.LogInformation("SUCCESS: Customer record saved.");
            }
            else
            {
                _logger.LogError("ERROR: Invalid UserType '{UserType}'. Cannot save.", profile.UserType);
            }
        }
    }
}
