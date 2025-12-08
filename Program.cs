using SpaAndBeautyWebsite.Components;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SpaAndBeautyWebsite.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using SpaAndBeautyWebsite.Interfaces;
using SpaAndBeautyWebsite.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContextFactory<SpaAndBeautyWebsiteContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SpaAndBeautyWebsiteContext") ?? throw new InvalidOperationException("Connection string 'SpaAndBeautyWebsiteContext' not found.")));

builder.Services.AddQuickGridEntityFrameworkAdapter();

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

// Register IHttpContextAccessor so components can SignIn/SignOut
builder.Services.AddHttpContextAccessor();

// Authentication: use cookie authentication (adjust LoginPath/AccessDeniedPath to your app routes)
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/"; // TODO: redirect to login page
        options.AccessDeniedPath = "/"; // Redirect to home page if access denied
        options.Cookie.Name = "SpaAndBeautyAuth";
    });

// Authorization: role-based policies
builder.Services.AddAuthorization(options =>
{
    // Policies for each role
    options.AddPolicy("RequireCustomer", policy => policy.RequireRole("Customer"));
    options.AddPolicy("RequireStaff", policy => policy.RequireRole("Staff"));
    options.AddPolicy("RequireManager", policy => policy.RequireRole("Manager"));
    options.AddPolicy("RequireAdmin", policy => policy.RequireRole("Admin"));

    // Convenience policies
    options.AddPolicy("StaffOrAbove", policy => policy.RequireRole("Staff", "Manager", "Admin"));
    options.AddPolicy("ManagerOrAbove", policy => policy.RequireRole("Manager", "Admin"));
});

// Register AnonymousOnly policy and its handler
builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AnonymousOnly", policy =>
        policy.Requirements.Add(new SpaAndBeautyWebsite.Authorization.AnonymousOnlyRequirement()));
});

builder.Services.AddSingleton<Microsoft.AspNetCore.Authorization.IAuthorizationHandler, SpaAndBeautyWebsite.Authorization.AnonymousOnlyHandler>();

// Note: "Guest" is an unauthenticated/anonymous user.
// Protect components/pages with [Authorize(Roles = "Customer")] or [Authorize(Policy = "RequireAdmin")] as needed.

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
try
{
    SeedData.Initialize(services);
}
catch (Exception ex)
{
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "An error occurred while seeding the database.");
}

// Add a minimal endpoint to perform the cookie sign-in on a normal HTTP request.
// Blazor Server components cannot set response cookies after the SignalR response has started,
// so we navigate the browser to this endpoint (forceLoad) after creating the account.
app.MapGet("/signin-local", async (HttpContext httpContext) =>
{
    var q = httpContext.Request.Query;
    var username = q["username"].FirstOrDefault();
    var id = q["id"].FirstOrDefault();
    var role = q["role"].FirstOrDefault() ?? "Customer";
    var returnUrl = q["returnUrl"].FirstOrDefault() ?? "/";

    if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(id))
    {
        return Results.BadRequest("username and id required");
    }

    var allowedRoles = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "Customer", "Staff", "Manager", "Admin" };
    if (!allowedRoles.Contains(role))
    {
        return Results.BadRequest("invalid role");
    }

    var claims = new List<Claim>
    {
        new Claim(ClaimTypes.Name, username),
        new Claim(ClaimTypes.NameIdentifier, id),
        new Claim(ClaimTypes.Role, role)
    };

    var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
    var principal = new ClaimsPrincipal(identity);

    // SignIn on a normal HTTP response (safe to set cookies here)
    await httpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, new AuthenticationProperties
    {
        IsPersistent = true
    });

    return Results.Redirect(returnUrl);
});

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
    app.UseMigrationsEndPoint();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

// Enable authentication + authorization middleware (order matters)
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();