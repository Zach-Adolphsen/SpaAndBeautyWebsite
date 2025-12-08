using Microsoft.EntityFrameworkCore;
using SpaAndBeautyWebsite.Data;
using SpaAndBeautyWebsite.Interfaces;
using SpaAndBeautyWebsite.Models;

namespace SpaAndBeautyWebsite.Services
{
    public class UserService : IUserService
    {
        private readonly SpaAndBeautyWebsiteContext _context;

        public UserService(SpaAndBeautyWebsiteContext context)
        {
            _context = context;
        }

        // LOOKUP: Finds user by Email in either Employee or Customer table
        // LOOKUP: Finds user by Email OR Username in either Employee or Customer table
        public async Task<User?> GetProfileAsync(string emailOrUsername)
        {
            // 1. Check if the user is an Employee
            var employee = await _context.Employee
                .AsNoTracking()
                // FIX: Check BOTH Email AND Username columns
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

            // 2. If not an Employee, check if the user is a Customer
            var customer = await _context.Customer
                .AsNoTracking()
                // FIX: Check BOTH Email AND Username columns here too
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

            // 3. User not found
            return null;
        }

        // UPDATE: Saves changes back to the correct table
        public async Task UpdateProfileAsync(User profile)
        {
            if (profile.UserType == "Employee")
            {
                var employee = await _context.Employee.FindAsync(profile.Id);
                if (employee != null)
                {
                    // Only update fields that exist on the Employee model and are safe to change
                    employee.FirstName = profile.FirstName;
                    employee.LastName = profile.LastName;
                    employee.PhoneNumber = profile.Phone;
                    employee.Username = profile.Username;

                    // Note: We do NOT update Salary, Permission, or Address here 
                    // because the simple Account form doesn't include those inputs.

                    await _context.SaveChangesAsync();
                }
            }
            else if (profile.UserType == "Customer")
            {
                var customer = await _context.Customer.FindAsync(profile.Id);
                if (customer != null)
                {
                    // Only update fields that exist on the Customer model
                    customer.FirstName = profile.FirstName;
                    customer.LastName = profile.LastName;
                    customer.PhoneNumber = profile.Phone;
                    customer.Username = profile.Username;

                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}