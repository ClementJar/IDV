using IDV.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace IDV.Infrastructure.Data.Seeders;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(IDVDbContext context)
    {
        await context.Database.EnsureCreatedAsync();

        // Clear and reseed IDSourceClients to ensure correct formats
        if (await context.IDSourceClients.AnyAsync())
        {
            context.IDSourceClients.RemoveRange(await context.IDSourceClients.ToListAsync());
            await context.SaveChangesAsync();
        }

        // Seed Users only if they don't exist
        if (!await context.Users.AnyAsync())
        {
            await SeedUsers(context);
        }

        // Seed Products only if they don't exist
        if (!await context.Products.AnyAsync())
        {
            await SeedProducts(context);
        }

        // Always reseed IDSourceClients with correct formats
        await SeedIDSourceClients(context);
    }

    private static async Task SeedUsers(IDVDbContext context)
    {
        var users = new List<User>
        {
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "admin",
                Email = "admin@ekwantu.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123"),
                FullName = "System Administrator",
                Role = "Admin",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "agent",
                Email = "agent@ekwantu.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Agent@123"),
                FullName = "Insurance Agent",
                Role = "Agent",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new User
            {
                UserId = Guid.NewGuid(),
                Username = "viewer",
                Email = "viewer@ekwantu.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Viewer@123"),
                FullName = "Report Viewer",
                Role = "Viewer",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Users.AddRangeAsync(users);
    }

    private static async Task SeedProducts(IDVDbContext context)
    {
        var products = new List<Product>
        {
            // Life Insurance (6 products)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE001",
                ProductName = "Term Life Cover",
                Category = "Life",
                Description = "Basic death benefit coverage for a specified term period.",
                PremiumAmount = 50.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE002",
                ProductName = "Whole Life Assurance",
                Category = "Life",
                Description = "Lifetime coverage with cash value accumulation.",
                PremiumAmount = 120.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE003",
                ProductName = "Endowment Policy",
                Category = "Life",
                Description = "Savings plus insurance combination product.",
                PremiumAmount = 200.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE004",
                ProductName = "Unit-Linked Life Plan",
                Category = "Life",
                Description = "Investment-linked life insurance with market returns.",
                PremiumAmount = 300.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE005",
                ProductName = "Family Protection Plan",
                Category = "Life",
                Description = "Comprehensive family coverage with multiple beneficiaries.",
                PremiumAmount = 180.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "LIFE006",
                ProductName = "Funeral Cover",
                Category = "Life",
                Description = "Immediate payout for funeral expenses.",
                PremiumAmount = 25.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // Health & Protection (5 products)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "HEALTH001",
                ProductName = "Critical Illness Cover",
                Category = "Health",
                Description = "Coverage for cancer, heart attack, stroke and other critical illnesses.",
                PremiumAmount = 80.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "HEALTH002",
                ProductName = "Personal Accident Plan",
                Category = "Health",
                Description = "Protection against disability or death from accidents.",
                PremiumAmount = 60.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "HEALTH003",
                ProductName = "Income Protection",
                Category = "Health",
                Description = "Salary replacement during illness or disability.",
                PremiumAmount = 150.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "HEALTH004",
                ProductName = "Hospital Cash Plan",
                Category = "Health",
                Description = "Daily hospital allowance for medical treatment.",
                PremiumAmount = 40.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "HEALTH005",
                ProductName = "Comprehensive Medical Cover",
                Category = "Health",
                Description = "Full hospitalization and medical expenses coverage.",
                PremiumAmount = 250.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // Savings & Investment (5 products)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SAVE001",
                ProductName = "Smart Save Plan",
                Category = "Savings",
                Description = "Guaranteed returns with annual bonus potential.",
                PremiumAmount = 100.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SAVE002",
                ProductName = "Education Endowment",
                Category = "Savings",
                Description = "Child education fund with maturity benefits.",
                PremiumAmount = 75.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SAVE003",
                ProductName = "Balanced Investment Fund",
                Category = "Savings",
                Description = "Mixed portfolio of stocks and bonds.",
                PremiumAmount = 200.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SAVE004",
                ProductName = "Equity Growth Fund",
                Category = "Savings",
                Description = "Stock market exposure for long-term growth.",
                PremiumAmount = 300.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SAVE005",
                ProductName = "Fixed Income Plan",
                Category = "Savings",
                Description = "Stable returns through government and corporate bonds.",
                PremiumAmount = 150.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // Pensions/Retirement (4 products)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PENSION001",
                ProductName = "Individual Retirement Account",
                Category = "Pension",
                Description = "Personal pension savings with tax benefits.",
                PremiumAmount = 250.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PENSION002",
                ProductName = "Group Pension Scheme",
                Category = "Pension",
                Description = "Employer-sponsored retirement benefits.",
                PremiumAmount = 400.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PENSION003",
                ProductName = "Immediate Annuity",
                Category = "Pension",
                Description = "Convert lump sum to guaranteed monthly income.",
                PremiumAmount = 500.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PENSION004",
                ProductName = "Deferred Annuity",
                Category = "Pension",
                Description = "Future retirement income starting at specified age.",
                PremiumAmount = 180.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // Asset Management (3 products)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "ASSET001",
                ProductName = "Property Investment Fund",
                Category = "Asset",
                Description = "Real estate portfolio for capital appreciation.",
                PremiumAmount = 600.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "ASSET002",
                ProductName = "Money Market Fund",
                Category = "Asset",
                Description = "Short-term investment instruments for liquidity.",
                PremiumAmount = 100.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "ASSET003",
                ProductName = "Aggressive Growth Portfolio",
                Category = "Asset",
                Description = "High-risk, high-return investment strategy.",
                PremiumAmount = 800.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            }
        };

        await context.Products.AddRangeAsync(products);
    }

    private static async Task SeedIDSourceClients(IDVDbContext context)
    {
        var random = new Random();
        var zambiianFirstNames = new[]
        {
            "Mwamba", "Chanda", "Musonda", "Temba", "Bwalya", "Mutale", "Kunda", "Nkandu",
            "Chisomo", "Thandiwe", "Chimwemwe", "Mutinta", "Patrick", "Joseph", "Mary", "Grace",
            "Emmanuel", "Ruth", "David", "Sarah", "Moses", "Esther", "Daniel", "Rebecca",
            "Samuel", "Mercy", "Peter", "Faith", "John", "Joyce", "Michael", "Janet"
        };

        var zambiianLastNames = new[]
        {
            "Banda", "Phiri", "Mwanza", "Zulu", "Sakala", "Lungu", "Tembo", "Sichone",
            "Mulenga", "Mulonda", "Kambole", "Chileshe", "Nyirenda", "Lubinda", "Mulundu",
            "Kashimba", "Chilufya", "Mukuka", "Mutendi", "Kabwe", "Zimba", "Chulu", "Katongo",
            "Mubanga", "Simukonda", "Lubasi", "Mumba", "Siame", "Kabimba", "Sinyangwe"
        };

        var zambianProvinces = new[]
        {
            "Lusaka", "Copperbelt", "Eastern", "Western", "Southern", "Northern",
            "North-Western", "Central", "Luapula", "Muchinga"
        };

        var districts = new Dictionary<string, string[]>
        {
            ["Lusaka"] = new[] { "Lusaka", "Kafue", "Chongwe", "Luangwa" },
            ["Copperbelt"] = new[] { "Ndola", "Kitwe", "Chingola", "Mufulira", "Kalulushi" },
            ["Eastern"] = new[] { "Chipata", "Petauke", "Katete", "Lundazi" },
            ["Western"] = new[] { "Mongu", "Senanga", "Kalabo", "Shangombo" },
            ["Southern"] = new[] { "Livingstone", "Choma", "Mazabuka", "Monze" },
            ["Northern"] = new[] { "Kasama", "Mbala", "Mpika", "Luwingu" },
            ["North-Western"] = new[] { "Solwezi", "Kasempa", "Mufumbwe", "Kabompo" },
            ["Central"] = new[] { "Kabwe", "Kapiri Mposhi", "Mkushi", "Serenje" },
            ["Luapula"] = new[] { "Mansa", "Kawambwa", "Nchelenge", "Samfya" },
            ["Muchinga"] = new[] { "Chinsali", "Isoka", "Nakonde", "Mpika" }
        };

        var idSources = new[] { "INRIS", "ZRA", "MNO_AIRTEL", "MNO_MTN", "MNO_ZAMTEL", "BANK_ZANACO", "BANK_FNB", "BANK_STANCHART", "GOVT_PAYROLL", "NAPSA" };
        var idTypes = new[] { "NationalID", "Passport", "DriversLicense" };
        var genders = new[] { "Male", "Female" };

        var clients = new List<IDSourceClient>();

        for (int i = 0; i < 75; i++) // Create 75 mock clients
        {
            var firstName = zambiianFirstNames[random.Next(zambiianFirstNames.Length)];
            var lastName = zambiianLastNames[random.Next(zambiianLastNames.Length)];
            var province = zambianProvinces[random.Next(zambianProvinces.Length)];
            var district = districts[province][random.Next(districts[province].Length)];
            var gender = genders[random.Next(genders.Length)];
            var birthYear = random.Next(1960, 2005);
            var birthMonth = random.Next(1, 13);
            var birthDay = random.Next(1, 28);

            // Generate ID number based on type
            var idTypeIndex = random.Next(100);
            var idType = idTypeIndex < 80 ? "NationalID" : (idTypeIndex < 95 ? "Passport" : "DriversLicense");
            var idNumber = GenerateIDNumber(idType, birthYear, random);

            clients.Add(new IDSourceClient
            {
                ClientId = Guid.NewGuid(),
                IDType = idType,
                IDNumber = idNumber,
                FullName = $"{firstName} {lastName}",
                DateOfBirth = DateTime.SpecifyKind(new DateTime(birthYear, birthMonth, birthDay), DateTimeKind.Utc),
                Gender = gender,
                MobileNumber = $"+260{random.Next(95, 98)}{random.Next(1000000, 9999999)}",
                Province = province,
                District = district,
                PostalCode = $"{random.Next(10000, 99999)}",
                Source = idSources[random.Next(idSources.Length)],
                IsVerified = true,
                CreatedAt = DateTime.UtcNow.AddDays(-random.Next(30, 365))
            });
        }

        // Add specific test cases for demo - distributed across different sources for realistic IDV experience
        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "NationalID",
            IDNumber = "19850615/10/1", // Found in INRIS (YYYYMMDD/DD/C format)
            FullName = "John Mwanza",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 6, 15), DateTimeKind.Utc),
            Gender = "Male",
            MobileNumber = "+260977123456",
            Province = "Lusaka",
            District = "Lusaka",
            PostalCode = "10101",
            Source = "INRIS",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "NationalID",
            IDNumber = "19750418/08/1", // Found in ZRA (Tax records) (YYYYMMDD/DD/C format)
            FullName = "Peter Phiri",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1975, 4, 18), DateTimeKind.Utc),
            Gender = "Male",
            MobileNumber = "+260955654321",
            Province = "Eastern",
            District = "Chipata",
            PostalCode = "30100",
            Source = "ZRA",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "NationalID",
            IDNumber = "19910725/07/1", // Found in MNO Airtel (YYYYMMDD/DD/C format)
            FullName = "David Mulenga",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1991, 7, 25), DateTimeKind.Utc),
            Gender = "Male",
            MobileNumber = "+260966234567",
            Province = "Northern",
            District = "Kasama",
            PostalCode = "31100",
            Source = "MNO_AIRTEL",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "NationalID",
            IDNumber = "19930612/05/1", // Found in Banking records (YYYYMMDD/DD/C format)
            FullName = "Grace Tembo",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1993, 6, 12), DateTimeKind.Utc),
            Gender = "Female",
            MobileNumber = "+260977567890",
            Province = "Lusaka",
            District = "Chongwe",
            PostalCode = "10102",
            Source = "BANK_ZANACO",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "NationalID",
            IDNumber = "19900822/10/7", // Zambian NRC format (YYYYMMDD/DD/C format)
            FullName = "Mary Phiri",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 8, 22), DateTimeKind.Utc),
            Gender = "Female",
            MobileNumber = "+260966987654",
            Province = "Copperbelt",
            District = "Ndola",
            PostalCode = "20001",
            Source = "ZRA",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        // Add Passport entries (Zambian format: ZN + 7 digits)
        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "Passport",
            IDNumber = "ZN1234567",
            FullName = "James Banda",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1988, 3, 15), DateTimeKind.Utc),
            Gender = "Male",
            MobileNumber = "+260977345678",
            Province = "Lusaka",
            District = "Lusaka", 
            PostalCode = "10101",
            Source = "PASSPORT_OFFICE",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "Passport",
            IDNumber = "ZN9876543",
            FullName = "Jennifer Mulenga",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1992, 11, 8), DateTimeKind.Utc),
            Gender = "Female",
            MobileNumber = "+260955876543",
            Province = "Western",
            District = "Mongu",
            PostalCode = "90100",
            Source = "PASSPORT_OFFICE",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        // Add Driving License entries (Zambian format: ZM + 6 digits)
        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "Driving License",
            IDNumber = "ZM123456",
            FullName = "Michael Tembo",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1985, 7, 20), DateTimeKind.Utc),
            Gender = "Male",
            MobileNumber = "+260966234789",
            Province = "Copperbelt",
            District = "Kitwe",
            PostalCode = "20200",
            Source = "RTSA",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        clients.Add(new IDSourceClient
        {
            ClientId = Guid.NewGuid(),
            IDType = "Driving License", 
            IDNumber = "ZM987654",
            FullName = "Susan Kabwe",
            DateOfBirth = DateTime.SpecifyKind(new DateTime(1990, 12, 5), DateTimeKind.Utc),
            Gender = "Female",
            MobileNumber = "+260977654321",
            Province = "Southern",
            District = "Livingstone",
            PostalCode = "60100",
            Source = "RTSA",
            IsVerified = true,
            CreatedAt = DateTime.UtcNow
        });

        // Log detailed seeding information
        Console.WriteLine("=== PREPARING TO SEED ===");
        Console.WriteLine($"Total clients to seed: {clients.Count}");
        
        var nrcCount = clients.Count(c => c.IDType == "NationalID");
        var passportCount = clients.Count(c => c.IDType == "Passport");
        var dlCount = clients.Count(c => c.IDType == "DriversLicense");
        
        Console.WriteLine($"NationalID count: {nrcCount}");
        Console.WriteLine($"Passport count: {passportCount}");
        Console.WriteLine($"DriversLicense count: {dlCount}");
        
        Console.WriteLine("=== MANUAL ENTRIES ===");
        var manualEntries = clients.Where(c => c.IDType == "Passport" || c.IDType == "DriversLicense").ToList();
        foreach (var entry in manualEntries)
        {
            Console.WriteLine($"Manual Entry: {entry.IDType} - {entry.IDNumber} - {entry.FullName}");
        }
        Console.WriteLine("====================");

        await context.IDSourceClients.AddRangeAsync(clients);
        Console.WriteLine("Clients added to context. Saving changes...");
        
        // Save and verify
        await context.SaveChangesAsync();
        Console.WriteLine("Changes saved. Verifying database contents...");
        
        // Verify what was actually saved
        var savedNrcCount = await context.IDSourceClients.CountAsync(c => c.IDType == "NationalID");
        var savedPassportCount = await context.IDSourceClients.CountAsync(c => c.IDType == "Passport");
        var savedDlCount = await context.IDSourceClients.CountAsync(c => c.IDType == "DriversLicense");
        
        Console.WriteLine("=== DATABASE VERIFICATION ===");
        Console.WriteLine($"Saved NationalID count: {savedNrcCount}");
        Console.WriteLine($"Saved Passport count: {savedPassportCount}");
        Console.WriteLine($"Saved DriversLicense count: {savedDlCount}");
        
        // Log some sample IDs for verification
        Console.WriteLine("=== SEEDED ID SAMPLES ===");
        var sampleClients = await context.IDSourceClients.Take(15).ToListAsync();
        foreach (var client in sampleClients)
        {
            Console.WriteLine($"{client.IDType}: {client.IDNumber} - {client.FullName} ({client.Source})");
        }
        Console.WriteLine("========================");
    }

    private static string GenerateIDNumber(string idType, int birthYear, Random random)
    {
        return idType switch
        {
            "NationalID" => GenerateZambianNRC(birthYear, random),
            "Passport" => $"ZN{random.Next(1000000, 9999999)}", // ZN + 7 digits
            "DriversLicense" => $"ZM{random.Next(100000, 999999)}", // ZM + 6 digits
            _ => $"{random.Next(100000000, 999999999)}"
        };
    }

    private static string GenerateZambianNRC(int birthYear, Random random)
    {
        // Generate a random birth date for the given year
        var startDate = new DateTime(birthYear, 1, 1);
        var endDate = new DateTime(birthYear, 12, 31);
        var randomDate = startDate.AddDays(random.Next((endDate - startDate).Days));
        
        // Format: YYYYMMDD/DD/C (8 digits for birth date + district + check digit)
        var dateStr = randomDate.ToString("yyyyMMdd");
        var districtCode = random.Next(10, 99); // District codes 10-99
        var checkDigit = random.Next(1, 9); // Check digit 1-9
        
        return $"{dateStr}/{districtCode:D2}/{checkDigit}";
    }
}