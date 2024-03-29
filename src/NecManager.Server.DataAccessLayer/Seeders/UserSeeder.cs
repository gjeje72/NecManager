﻿namespace NecManager.Server.DataAccessLayer.Seeders;
using System;
using System.Linq;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

using NecManager.Common.DataEnum.Identity;
using NecManager.Server.DataAccessLayer.Model;


/// <summary>
///     Service to seed user data.
/// </summary>
public class UserSeeder
{
    private const string AdminLogin = "admin-nec";

    private readonly ILogger<UserSeeder> logger;

    private readonly UserManager<Student> userManager;

    private readonly RoleManager<IdentityRole> roleManager;

    /// <summary>
    ///     Initializes a new instance of the <see cref="UserSeeder" /> class.
    /// </summary>
    /// <param name="logger">the logger.</param>
    /// <param name="userManager">the user manager.</param>
    /// <param name="roleManager">the role manager.</param>
    public UserSeeder(ILogger<UserSeeder> logger, UserManager<Student> userManager, RoleManager<IdentityRole> roleManager)
    {
        this.logger = logger;
        this.userManager = userManager;
        this.roleManager = roleManager;
    }

    /// <summary>
    ///     Ensure that there is a root user in database context.
    /// </summary>
    /// <returns>
    ///     Returns the result of the asynchronous operation.
    /// </returns>
    public async Task EnsureSeedDataAsync()
    {
        await this.SeedRoles();
        await this.SeedRootUser();
    }

    /// <summary>
    ///     Seeds the root user if it not exists.
    /// </summary>
    /// <returns>
    ///     Returns the result of the asynchronous operation.
    /// </returns>
    private async Task SeedRootUser()
    {
        // Searching for root user
        var rootUser = await this.userManager.FindByNameAsync(AdminLogin).ConfigureAwait(false);

        if (rootUser == null)
        {
            // Create it
            var guidUser = Guid.NewGuid().ToString();
            rootUser = new()
            {
                Id = guidUser,
                UserName = AdminLogin,
                FirstName = "admin",
                Name = "nec",
                Email = "maitre@nec-escrime.fr",
            };

            var passwordHasher = new PasswordHasher<Student>();
            rootUser.PasswordHash = passwordHasher.HashPassword(rootUser, "@dminNec2023");

            var identityResult = await this.userManager.CreateAsync(rootUser).ConfigureAwait(false);
            if (!identityResult.Succeeded)
            {
                this.logger.LogError(string.Join("\n", identityResult.Errors));
                return;
            }

            var rootIdentityUser = await this.userManager.FindByNameAsync(AdminLogin);
            _ = await this.userManager.AddToRoleAsync(rootIdentityUser, RoleType.ADMIN.ToString());

            this.logger.LogInformation("Root user created");
        }
        else
        {
            _ = await this.userManager.UpdateAsync(rootUser);
        }
    }

    /// <summary>
    ///     Seeds roles if they do not exist.
    /// </summary>
    /// <returns>
    ///     Returns the result of the asynchronous operation.
    /// </returns>
    private async Task SeedRoles()
    {
        var existingRolesCollection = this.roleManager.Roles.Select(r => r.Name);
        var rolesToSeedCollection = Enum.GetNames(typeof(RoleType)).Except(existingRolesCollection);

        foreach (var role in rolesToSeedCollection)
        {
            await this.roleManager.CreateAsync(new()
            {
                Name = role
            }).ConfigureAwait(false);
            this.logger.LogInformation($"Role {role} created");
        }
    }
}
