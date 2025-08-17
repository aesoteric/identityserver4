using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityModel;
using IdentityServer.Constants;
using IdentityServer.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Serilog;

namespace IdentityServer.Data
{
    public class ApplicationDbContextInitialiser
    {
        private const string DefaultPassword = "Pass123$";
        
        private readonly ILogger<ApplicationDbContextInitialiser> _logger;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ApplicationDbContextInitialiser(
            ILogger<ApplicationDbContextInitialiser> logger, 
            ApplicationDbContext context, 
            UserManager<ApplicationUser> userManager, 
            RoleManager<IdentityRole> roleManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public async Task InitialiseAsync()
        {
            try
            {
                await _context.Database.MigrateAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while initialising the database.");
                throw;
            }
        }

        public async Task SeedAsync()
        {
            try
            {
                await TrySeedAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while seeding the database.");
                throw;
            }
        }
        
        public async Task TrySeedAsync()
        {
            await SeedDefaultRoles();

            await SeedDefaultUsers();
        }

        private async Task SeedDefaultUsers()
        {
            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            var administratorRole = new IdentityRole(Roles.Administrator);
            
            if (_userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await _userManager.CreateAsync(administrator, "Administrator1!");
                if (!string.IsNullOrWhiteSpace(administratorRole.Name))
                {
                    await _userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
                }
            }
            
            var buyerRole = new IdentityRole(Roles.Buyer);
            
            var alice = _userManager.FindByEmailAsync("alice.smith@lula.co.za").Result;
            if (alice == null)
            {
                alice = new ApplicationUser
                {
                    FirstName = "Alice",
                    LastName = "Smith",
                    UserName = "alice.smith@lula.co.za",
                    Email = "alice.smith@lula.co.za",
                    EmailConfirmed = true,
                    PhoneNumber = "+27820000000",
                    PhoneNumberConfirmed = true
                };
                var result = _userManager.CreateAsync(alice, DefaultPassword).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                await _userManager.AddToRolesAsync(alice, new [] { buyerRole.Name });

                result = _userManager.AddClaimsAsync(alice, new[]
                {
                    new Claim(JwtClaimTypes.Name, "Alice Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Alice"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "https://lula.co.za/")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                Log.Debug("alice created");
            }
            else
            {
                Log.Debug("alice already exists");
            }
            
            var sellerRole = new IdentityRole(Roles.Seller);
            
            var bob = _userManager.FindByEmailAsync("bob.smith@lula.co.za").Result;
            if (bob == null)
            {
                bob = new ApplicationUser
                {
                    FirstName = "Bob",
                    LastName = "Smith",
                    UserName = "bob.smith@lula.co.za",
                    Email = "bob.smith@lula.co.za",
                    EmailConfirmed = true,
                    PhoneNumber = "+27830000000",
                    PhoneNumberConfirmed = true
                };
                var result = _userManager.CreateAsync(bob, DefaultPassword).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                await _userManager.AddToRolesAsync(bob, new [] { sellerRole.Name });

                result = _userManager.AddClaimsAsync(bob, new[]
                {
                    new Claim(JwtClaimTypes.Name, "Bob Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Bob"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "https://lula.co.za/")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                Log.Debug("bob created");
            }
            else
            {
                Log.Debug("bob already exists");
            }
            
            var james = _userManager.FindByEmailAsync("james.smith@lula.co.za").Result;
            if (james == null)
            {
                james = new ApplicationUser
                {
                    FirstName = "James",
                    LastName = "Smith",
                    UserName = "james.smith@lula.co.za",
                    Email = "james.smith@lula.co.za",
                    EmailConfirmed = true,
                    PhoneNumber = "+27630000000",
                    PhoneNumberConfirmed = true
                };
                var result = _userManager.CreateAsync(james, DefaultPassword).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                await _userManager.AddToRolesAsync(james, new [] { buyerRole.Name, sellerRole.Name });

                result = _userManager.AddClaimsAsync(james, new[]
                {
                    new Claim(JwtClaimTypes.Name, "James Smith"),
                    new Claim(JwtClaimTypes.GivenName, "James"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "https://lula.co.za/")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                Log.Debug("james created");
            }
            else
            {
                Log.Debug("james already exists");
            }
            
            var onboardingRole = new IdentityRole(Roles.Onboarding);
            
            var mike = _userManager.FindByEmailAsync("mike.smith@lula.co.za").Result;
            if (mike == null)
            {
                mike = new ApplicationUser
                {
                    FirstName = "Mike",
                    LastName = "Smith",
                    UserName = "mike.smith@lula.co.za",
                    Email = "mike.smith@lula.co.za",
                    EmailConfirmed = true,
                    PhoneNumber = "+27730000000",
                    PhoneNumberConfirmed = true
                };
                var result = _userManager.CreateAsync(mike, DefaultPassword).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                
                await _userManager.AddToRolesAsync(mike, new [] { onboardingRole.Name });

                result = _userManager.AddClaimsAsync(mike, new[]
                {
                    new Claim(JwtClaimTypes.Name, "Mike Smith"),
                    new Claim(JwtClaimTypes.GivenName, "Mike"),
                    new Claim(JwtClaimTypes.FamilyName, "Smith"),
                    new Claim(JwtClaimTypes.WebSite, "https://lula.co.za/")
                }).Result;
                if (!result.Succeeded) throw new Exception(result.Errors.First().Description);
                Log.Debug("mike created");
            }
            else
            {
                Log.Debug("mike already exists");
            }
        }

        private async Task SeedDefaultRoles()
        {
            var administratorRole = new IdentityRole(Roles.Administrator);

            if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await _roleManager.CreateAsync(administratorRole);
            }
            
            var buyerRole = new IdentityRole(Roles.Buyer);

            if (_roleManager.Roles.All(r => r.Name != buyerRole.Name))
            {
                await _roleManager.CreateAsync(buyerRole);
            }
            
            var sellerRole = new IdentityRole(Roles.Seller);

            if (_roleManager.Roles.All(r => r.Name != sellerRole.Name))
            {
                await _roleManager.CreateAsync(sellerRole);
            }
            
            var onboardingRole = new IdentityRole(Roles.Onboarding);

            if (_roleManager.Roles.All(r => r.Name != onboardingRole.Name))
            {
                await _roleManager.CreateAsync(onboardingRole);
            }
        }
    }
}