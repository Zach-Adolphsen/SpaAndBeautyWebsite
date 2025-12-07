using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaAndBeautyWebsite.Models;

namespace SpaAndBeautyWebsite.Data;

public class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new SpaAndBeautyWebsiteContext(
            serviceProvider.GetRequiredService<DbContextOptions<SpaAndBeautyWebsiteContext>>());

        if (context == null || context.Service == null || context.Customer == null)
        {
            throw new NullReferenceException("Null SpaAndBeautyWebsiteContext or required DbSet(s)");
        }

        // Seed employees if missing
        if (!context.Employee.Any())
        {
            context.Employee.AddRange(
                new Employee
                {
                    FirstName = "Sofia",
                    LastName = "Andersson",
                    Username = "sofia.andersson",
                    Password = "sand123", // Keep only one Password assignment
                    PhoneNumber = "+1-111-222-3333",
                    Email = "sofia.andersson@example.com",
                    Street = "123 Main St",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "627-012-3456",
                    JobTitle = "Massage Therapist",
                    Salary = 55000m,
                    Permission = "Staff"
                },
                new Employee
                {
                    FirstName = "Lucas",
                    LastName = "Martinez",
                    Username = "lucas.martinez",
                    Password = "lmartinez!", // Keep only one Password assignment
                    PhoneNumber = "+1-222-333-4444",
                    Email = "lucas.martinez@example.com",
                    Street = "456 Oak Ave",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "627-111-2222",
                    JobTitle = "Esthetician",
                    Salary = 50000m,
                    Permission = "Manager"
                },
                new Employee
                {
                    FirstName = "Emma",
                    LastName = "Nguyen",
                    Username = "emma.nguyen",
                    Password = "emmanailtech", // Keep only one Password assignment
                    PhoneNumber = "+1-333-444-5555",
                    Email = "emma.nguyen@example.com",
                    Street = "789 Pine Rd",
                    City = "Springfield",
                    State = "IL",
                    ZipCode = "627-222-3333",
                    JobTitle = "Nail Technician",
                    Salary = 48000m,
                    Permission = "Admin"
                }
            );

            context.SaveChanges();
        }

        // Seed services if missing
        if (!context.Service.Any())
        {
            context.Service.AddRange(
                new Service
                {
                    Name = "Swedish Massage",
                    Description = "A gentle, relaxing full-body massage using long strokes to relieve tension.",
                    DurationMinutes = 60,
                    DurationPrice = 90.00m
                },
                new Service
                {
                    Name = "Deep Tissue Massage",
                    Description = "Focused massage to release chronic muscle tension and knots.",
                    DurationMinutes = 75,
                    DurationPrice = 120.00m
                },
                new Service
                {
                    Name = "Signature Facial",
                    Description = "Custom facial treatment to cleanse, exfoliate and hydrate skin.",
                    DurationMinutes = 50,
                    DurationPrice = 70.00m
                },
                new Service
                {
                    Name = "Manicure & Pedicure",
                    Description = "Complete hand and foot care with polish and massage.",
                    DurationMinutes = 90,
                    DurationPrice = 85.00m
                }
            );

            context.SaveChanges();
        }

        // Seed customers if missing
        if (!context.Customer.Any())
        {
            context.Customer.AddRange(
                new Customer
                {
                    FirstName = "Ava",
                    LastName = "Johnson",
                    Username = "ava.j",
                    Password = "Passw0rd!",
                    PhoneNumber = "123-456-7890",
                    Email = "ava.johnson@example.com"
                },
                new Customer
                {
                    FirstName = "Liam",
                    LastName = "Garcia",
                    Username = "liamg",
                    Password = "Secure123",
                    PhoneNumber = "+1-234-567-8901",
                    Email = "liam.garcia@example.com"
                },
                new Customer
                {
                    FirstName = "Olivia",
                    LastName = "Nguyen",
                    Username = "olivia.ng",
                    Password = "MySecret",
                    PhoneNumber = "321-654-0987",
                    Email = "olivia.nguyen@example.com"
                },
                new Customer
                {
                    FirstName = "Noah",
                    LastName = "Smith",
                    Username = "noah.s",
                    Password = "NoahPass",
                    PhoneNumber = "555-123-4567",
                    Email = "noah.smith@example.com"
                }
            );

            context.SaveChanges();
        }

        // load saved entities to obtain generated IDs
        var savedEmployees = context.Employee.ToList();
        var savedServices = context.Service.ToList();
        var savedCustomers = context.Customer.ToList();

        // Seed appointments (use saved employee IDs)
        if (context.Appointment != null && !context.Appointment.Any()
            && savedEmployees.Any() && savedServices.Any() && savedCustomers.Any())
        {
            context.Appointment.AddRange(
                new Appointment
                {
                    EmployeeId = 1,
                    CustomerId = savedCustomers[0].CustomerId,
                    ServiceId = savedServices.First(s => s.Name == "Swedish Massage").ServiceId,
                    ScheduledDateTime = DateTime.Now.AddDays(3).AddHours(10),
                    DurationMinutes = 60,
                    Status = "Scheduled"
                },
                new Appointment
                {
                    EmployeeId = 2,
                    CustomerId = savedCustomers[1].CustomerId,
                    ServiceId = savedServices.First(s => s.Name == "Deep Tissue Massage").ServiceId,
                    ScheduledDateTime = DateTime.Now.AddDays(1).AddHours(14),
                    DurationMinutes = 75,
                    Status = "Scheduled"
                },
                new Appointment
                {
                    EmployeeId = 1,
                    CustomerId = savedCustomers[2].CustomerId,
                    ServiceId = savedServices.First(s => s.Name == "Signature Facial").ServiceId,
                    ScheduledDateTime = DateTime.Now.AddDays(-2).AddHours(9),
                    DurationMinutes = 50,
                    Status = "Completed"
                }
            );
        }

       

        context.SaveChanges();
    }
}
