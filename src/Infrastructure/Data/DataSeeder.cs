//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using MySqlConnector;
//using Project.Domain.Common;
//using Project.Domain.Entities;
//using Project.Domain.Enums;

//namespace Project.Infrastructure.Data;

//public class DataSeeder(ApplicationDbContext dbContext, UserManager<User> userManager)
//{
//    public void Seed()
//    {
//        try
//        {
//            SeedRoles();
//            SeedUser();
//            SeedPermissions();
//            SeedEntityTypes();
//        }
//        catch (Exception ex)
//        {
//            Console.WriteLine($"Seeding failed: {ex.Message}");
//        }
//    }

//    private bool CheckTableExists(string tableName)
//    {
//        bool isTableExist = false;

//        if (!dbContext.Database.CanConnect())
//            return false;

//        var connection = dbContext.Database.GetDbConnection();
//        var dbName = new MySqlConnectionStringBuilder(connection.ConnectionString).Database;

//        var sqlCommand = $"SELECT COUNT(*) FROM information_schema.tables WHERE table_schema = '{dbName}' AND table_name = '{tableName}';";

//        using (var command = connection.CreateCommand())
//        {
//            command.CommandText = sqlCommand;
//            dbContext.Database.OpenConnection();

//            var result = command.ExecuteScalar();
//            isTableExist = Convert.ToInt32(result) > 0;

//            dbContext.Database.CloseConnection();
//        }

//        return isTableExist;
//    }

//    private void SeedRoles()
//    {
//        if (!CheckTableExists("Role"))
//        {
//            Console.WriteLine("Role table does not exist.");
//            return;
//        }

//        //// Step 1: Ensure departments exist
//        //var supportDept = dbContext.Department.FirstOrDefault(d => d.Id == 1);
//        //if (supportDept == null)
//        //{
//        //    supportDept = new Department { Id = 1, Name = "Support", Description = "Support team", CreatedBy = 1, LastModifiedBy = 1 };
//        //    dbContext.Department.Add(supportDept);
//        //}

//        //var generalUserDept = dbContext.Department.FirstOrDefault(d => d.Id == 2);
//        //if (generalUserDept == null)
//        //{
//        //    generalUserDept = new Department { Id = 2, Name = "General User", Description = "Default department for users", CreatedBy = 1, LastModifiedBy = 1 };
//        //    dbContext.Department.Add(generalUserDept);
//        //}

//        //dbContext.SaveChanges();

//        // Step 2: Ensure roles exist (by ID instead of name)
//        Domain.Entities.Role GetOrCreateRoleById(int roleId, string roleName, string description)
//        {
//            var role = dbContext.Role.FirstOrDefault(r => r.Id == roleId);
//            if (role == null)
//            {
//                role = new Domain.Entities.Role { Id = roleId, Name = roleName, Description = description, CreatedBy = 1, LastModifiedBy = 1 };
//                dbContext.Role.Add(role);
//                dbContext.SaveChanges();
//            }
//            return role;
//        }

//        var adminRole = GetOrCreateRoleById((int)Domain.Enums.Role.Admin, Domain.Enums.Role.Admin.GetDescription(), "System Administrator");
//        var supportStaffRole = GetOrCreateRoleById((int)Domain.Enums.Role.Support, Domain.Enums.Role.Support.GetDescription(), "Handles support tickets");
//        var userRole = GetOrCreateRoleById((int)Domain.Enums.Role.Employee, Domain.Enums.Role.Employee.GetDescription(), "General Employee");

//        dbContext.SaveChanges();

//        //// Step 3: Map roles to departments
//        //void MapDepartmentRole(int deptId, int roleId)
//        //{
//        //    var exists = dbContext.DepartmentRole.Any(dr => dr.DepartmentId == deptId && dr.RoleId == roleId);
//        //    if (!exists)
//        //    {
//        //        dbContext.DepartmentRole.Add(new DepartmentRole
//        //        {
//        //            DepartmentId = deptId,
//        //            RoleId = roleId,
//        //            CreatedBy = 1,
//        //            LastModifiedBy = 1
//        //        });
//        //    }
//        //}

//        //MapDepartmentRole(1, (int)UserType.Support); // Support -> Support Staff
//        //MapDepartmentRole(1, (int)UserType.Admin); // Support -> Admin
//        //MapDepartmentRole(2, (int)UserType.Employee); // General User -> User

//        //dbContext.SaveChanges();
//        Console.WriteLine("Roles seeded successfully.");
//    }

//    private void SeedUser()
//    {
//        var isUserTableExist = CheckTableExists("User");
//        if (!isUserTableExist)
//        {
//            Console.WriteLine("User table does not exist yet. Skipping seeding...");
//            return;
//        }

//        // Step 3: Method to create user
//        void CreateUser(int id, string firstName, string lastName, UserType userType, int roleId)
//        {
//            if (dbContext.Users.Any(u => u.Id == id))
//                return;

//            var user = new User
//            {
//                Id = id, // Hardcoded ID
//                FirstName = firstName,
//                LastName = lastName,
//                Email = $"{firstName.ToLower()}@mailinator.com",
//                UserName = $"{firstName.ToLower()}@mailinator.com",
//                IsActive = true,
//            };

//            // Temporarily enable identity insert
//            //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] ON");

//            var result = userManager.CreateAsync(user, "Asdf@1234").GetAwaiter().GetResult();

//            //dbContext.Database.ExecuteSqlRaw("SET IDENTITY_INSERT [User] OFF");

//            if (!result.Succeeded)
//            {
//                Console.WriteLine($"Failed to create user {user.Email}: {string.Join(", ", result.Errors.Select(e => e.Description))}");
//                return;
//            }

//            var userProfile = new UserProfile
//            {
//                UserId = user.Id,
//                FirstName = user.FirstName,
//                LastName = user.LastName,
//                Email = user.Email,
//                Address = "Default Address",
//                MobileNumber = "03001234567",
//                DateOfBirth = new DateOnly(1990, 1, 1),
//                Gender = GenderType.Male,
//                Language = LanguageCode.EN,
//                UserTypeId = userType,

//                Created = DateTimeOffset.UtcNow,
//                CreatedBy = 1,
//                LastModified = DateTimeOffset.UtcNow,
//                LastModifiedBy = 1
//            };

//            dbContext.UserProfile.Add(userProfile);

//            var userRoleEntry = new UserRole
//            {
//                UserId = user.Id,
//                RoleId = roleId,
//                Created = DateTimeOffset.UtcNow,
//                CreatedBy = 1,
//                LastModified = DateTimeOffset.UtcNow,
//                LastModifiedBy = 1
//            };

//            dbContext.UserRole.Add(userRoleEntry);
//            dbContext.SaveChanges();

//            Console.WriteLine($"User with ID {id} created successfully.");
//        }

//        // Step 4: Create the 3 users
//        CreateUser(1, "Admin", "TS", UserType.Admin, (int)Domain.Enums.Role.Admin);
//    }

//    private void SeedPermissions()
//    {
//        if (!CheckTableExists("RoleRight"))
//        {
//            Console.WriteLine("RoleRight table does not exist yet. Skipping seeding...");
//            return;
//        }

//        // Step 1: Seed Rights from enum
//        var rightsToAdd = new List<Right>();
//        var roleRightMappings = new List<(int RoleId, int RightId)>();

//        foreach (RightsEnum rightEnum in Enum.GetValues(typeof(RightsEnum)))
//        {
//            var field = typeof(RightsEnum).GetField(rightEnum.ToString());
//            var attr = field?.GetCustomAttributes(typeof(RightAttribute), false)
//                            .FirstOrDefault() as RightAttribute;

//            if (attr == null) continue;

//            // Check if Right already exists
//            var existingRight = dbContext.Right.FirstOrDefault(r => r.Id == (int)rightEnum);

//            if (existingRight == null)
//            {
//                rightsToAdd.Add(new Right
//                {
//                    Id = attr.Id,
//                    Name = rightEnum.ToReadableString(),
//                    NameAr = attr.ArabicDescription,
//                    Description = rightEnum.ToReadableString(),
//                    Created = DateTimeOffset.UtcNow,
//                    CreatedBy = 1,
//                    LastModified = DateTimeOffset.UtcNow,
//                    LastModifiedBy = 1,
//                });
//            }
//            else
//            {
//                // Update existing Right
//                existingRight.Name = rightEnum.ToReadableString();
//                existingRight.NameAr = attr.ArabicDescription;
//                existingRight.Description = rightEnum.ToReadableString();
//                existingRight.LastModified = DateTimeOffset.UtcNow;
//                existingRight.LastModifiedBy = 1;
//            }

//            // Store mapping of Right -> Role(s)
//            foreach (var roleId in attr.RoleIds)
//            {
//                roleRightMappings.Add((roleId, attr.Id));
//            }
//        }

//        // Insert any new Rights
//        if (rightsToAdd.Any())
//        {
//            dbContext.Right.AddRange(rightsToAdd);
//        }

//        // Commit inserts + updates
//        dbContext.SaveChanges();
//        Console.WriteLine("Rights seeded/updated.");

//        // Step 2: Assign rights to roles based on attributes
//        foreach (var group in roleRightMappings.GroupBy(r => r.RoleId))
//        {
//            var roleId = group.Key;
//            var rightIds = group.Select(g => g.RightId).Distinct();

//            var existingRights = dbContext.RoleRight
//                .Where(rr => rr.RoleId == roleId)
//                .Select(rr => rr.RightId)
//                .ToList();

//            var newRoleRights = rightIds
//                .Except(existingRights)
//                .Select(rightId => new RoleRight
//                {
//                    RoleId = roleId,
//                    RightId = rightId
//                })
//                .ToList();

//            if (newRoleRights.Any())
//            {
//                dbContext.RoleRight.AddRange(newRoleRights);
//                dbContext.SaveChanges();
//                Console.WriteLine($"Rights assigned to Role ID {roleId}.");
//            }
//        }
//    }

//    private void SeedEntityTypes()
//    {
//        if (!CheckTableExists("EntityType"))
//        {
//            Console.WriteLine("EntityType table does not exist. Skipping seeding...");
//            return;
//        }

//        // 1. Get current DB records
//        var dbEntityTypes = dbContext.EntityType.ToList();

//        // 2. Get all enum values
//        var enumEntityTypes = Enum.GetValues(typeof(Domain.Enums.EntityType))
//                                  .Cast<Domain.Enums.EntityType>()
//                                  .Select(e => new
//                                  {
//                                      Id = (int)e,
//                                      NameEn = e.GetDescription(),
//                                      NameAr = e.GetArabicDescription()
//                                  })
//                                  .ToList();

//        var toAdd = new List<Domain.Entities.EntityType>();
//        var toUpdate = new List<Domain.Entities.EntityType>();
//        var toRemove = new List<Domain.Entities.EntityType>();

//        // 3. Check for new or updated
//        foreach (var enumType in enumEntityTypes)
//        {
//            var existing = dbEntityTypes.FirstOrDefault(x => x.Id == enumType.Id);
//            if (existing == null)
//            {
//                // Add missing
//                toAdd.Add(new Domain.Entities.EntityType
//                {
//                    Id = enumType.Id,
//                    Name = enumType.NameEn,
//                    NameAr = enumType.NameAr,
//                    IsActive = true,
//                    Created = DateTimeOffset.UtcNow,
//                    CreatedBy = 1,
//                    LastModifiedBy = 1
//                });
//            }
//            else
//            {
//                // Update if names changed
//                if (existing.Name != enumType.NameEn || existing.NameAr != enumType.NameAr)
//                {
//                    existing.Name = enumType.NameEn;
//                    existing.NameAr = enumType.NameAr;
//                    existing.LastModified = DateTimeOffset.UtcNow;
//                    existing.LastModifiedBy = 1;
//                    toUpdate.Add(existing);
//                }
//            }
//        }

//        // 4. Remove ones no longer in enum
//        var enumIds = enumEntityTypes.Select(e => e.Id).ToList();
//        toRemove = dbEntityTypes.Where(x => !enumIds.Contains(x.Id)).ToList();

//        // 5. Apply changes
//        if (toAdd.Any())
//        {
//            dbContext.EntityType.AddRange(toAdd);
//            Console.WriteLine($"Added {toAdd.Count} EntityTypes.");
//        }

//        if (toUpdate.Any())
//        {
//            dbContext.EntityType.UpdateRange(toUpdate);
//            Console.WriteLine($"Updated {toUpdate.Count} EntityTypes.");
//        }

//        if (toRemove.Any())
//        {
//            dbContext.EntityType.RemoveRange(toRemove);
//            Console.WriteLine($"Removed {toRemove.Count} EntityTypes.");
//        }

//        if (toAdd.Any() || toUpdate.Any() || toRemove.Any())
//        {
//            dbContext.SaveChanges();
//            Console.WriteLine("EntityTypes synchronized successfully.");
//        }
//        else
//        {
//            Console.WriteLine("EntityTypes already up-to-date.");
//        }
//    }
//}
