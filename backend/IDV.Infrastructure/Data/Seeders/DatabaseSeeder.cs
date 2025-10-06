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
        
        if (!await context.Products.AnyAsync())
        {
            await SeedProducts(context);
        }

        if (!await context.IDSourceClients.AnyAsync())
        {
            await SeedIDSourceClients(context);
        }

        // Clear and reseed Products properly (handle foreign key constraints)

        // Always reseed IDSourceClients with correct formats
        
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
            // REAL PRUDENTIAL ZAMBIA PRODUCTS (Verified from official website)
            
            // EDUCATION & SAVINGS PRODUCTS
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SCEP",
                ProductName = "Pru Smart Child Education Plan",
                Category = "Education",
                Description = "Plan for children's future school fees with protection - Educational lumpsum, maturity bonus, and waiver benefits",
                PremiumAmount = 150.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "FFP",
                ProductName = "Pru Flexi Farewell",
                Category = "Funeral Insurance",
                Description = "Funeral insurance with no medical exams, no lapse provision, hospital cash benefits and loyalty bonuses",
                PremiumAmount = 85.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "STP",
                ProductName = "Pru Smart Treasure Plan",
                Category = "Life Insurance",
                Description = "Life protection with investment growth, spouse benefit (10% of sum assured) and accidental death coverage",
                PremiumAmount = 450.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "SSIP",
                ProductName = "Pru Smart Saver Investment Plan",
                Category = "Savings & Investment",
                Description = "Long-term savings with investment returns, flexible access to funds and embedded life protection",
                PremiumAmount = 250.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // PREMIUM INVESTMENT PRODUCTS
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PLATINUM",
                ProductName = "Pru Platinum Smart Saver",
                Category = "Premium Savings",
                Description = "Lump sum investment with K25,000 embedded life cover, 10% annual withdrawals and maturity bonus",
                PremiumAmount = 5000.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PRUDOLLAR",
                ProductName = "Pru Dollar Platinum Saver",
                Category = "USD Investment",
                Description = "USD-denominated savings with flexible withdrawals (up to 10% annually) and currency protection",
                PremiumAmount = 1000.00m,
                Currency = "USD",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // TRAVEL & PROTECTION PRODUCTS
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "OTI",
                ProductName = "Overseas Travel Insurance",
                Category = "Travel Insurance",
                Description = "Comprehensive travel protection with up to USD $250,000 medical coverage, repatriation and baggage cover",
                PremiumAmount = 75.00m,
                Currency = "USD",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // MEDICAL INSURANCE PRODUCTS (Tiered Coverage)
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "MED-BRONZE",
                ProductName = "Medical Insurance - Bronze",
                Category = "Health Insurance",
                Description = "Basic medical cover - Out-patient and In-patient services within Zambia",
                PremiumAmount = 180.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "MED-SILVER",
                ProductName = "Medical Insurance - Silver",
                Category = "Health Insurance",
                Description = "Enhanced medical cover with maternity benefits, optical and dental services",
                PremiumAmount = 350.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "MED-GOLD",
                ProductName = "Medical Insurance - Gold",
                Category = "Health Insurance",
                Description = "Comprehensive medical with evacuation to pre-determined countries and enhanced limits",
                PremiumAmount = 650.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "MED-PLATINUM",
                ProductName = "Medical Insurance - Platinum",
                Category = "Health Insurance",
                Description = "Premium medical with international coverage, highest limits and comprehensive benefits",
                PremiumAmount = 1200.00m,
                Currency = "ZMW",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            },

            // DIGITAL HEALTH SERVICES
            new Product
            {
                ProductId = Guid.NewGuid(),
                ProductCode = "PC24",
                ProductName = "PruCare24 Telemedicine",
                Category = "Digital Health",
                Description = "24/7 teleconsultation services via iVitals platform - Connect with medical professionals anytime",
                PremiumAmount = 25.00m,
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

        var idSources = new[] { "INRIS", "ZRA", "MNO_AIRTEL", "MNO_MTN", "MNO_ZAMTEL", "BANK_ZANACO", "BANK_FNB", "BANK_STANCHART", "GOVT_PAYROLL", "NAPSA", "RTSA" };
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