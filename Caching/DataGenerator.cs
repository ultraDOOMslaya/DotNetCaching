using Caching.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Caching
{
    public class DataGenerator
    {
        
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ApiContext(
                serviceProvider.GetRequiredService<DbContextOptions<ApiContext>>()))
            {
                // Look for any drugs.
                if (context.Drugs.Any())
                {
                    return;   // Data was already seeded
                }

                context.Drugs.AddRange(

                    new Models.Drug
                    {
                        Id = 1,
                        drugNdc = "000918370",
                        drugName = "Lipitor",
                        drugPrice = Decimal.Parse("87.99", System.Globalization.NumberStyles.Currency),
                        packSize = 30
                    },
                    new Models.Drug
                    {
                        Id = 2,
                        drugNdc = "000914570",
                        drugName = "Viagra",
                        drugPrice = Decimal.Parse("34.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 3,
                        drugNdc = "000914533",
                        drugName = "Lisinopril",
                        drugPrice = Decimal.Parse("64.69", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 4,
                        drugNdc = "000914571",
                        drugName = "Omeprazole",
                        drugPrice = Decimal.Parse("54.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 5,
                        drugNdc = "000624570",
                        drugName = "Metformin",
                        drugPrice = Decimal.Parse("39.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 6,
                        drugNdc = "033414570",
                        drugName = "Simvastatin",
                        drugPrice = Decimal.Parse("55.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 7,
                        drugNdc = "001114570",
                        drugName = "Hydrocodone",
                        drugPrice = Decimal.Parse("79.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 8,
                        drugNdc = "222714570",
                        drugName = "Metoprolol",
                        drugPrice = Decimal.Parse("319.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 9,
                        drugNdc = "008675309",
                        drugName = "Omnitech-RX",
                        drugPrice = Decimal.Parse("111.11", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    },
                    new Models.Drug
                    {
                        Id = 10,
                        drugNdc = "100914570",
                        drugName = "Levothroxine",
                        drugPrice = Decimal.Parse("14.99", System.Globalization.NumberStyles.Currency),
                        packSize = 10
                    });

                context.SaveChanges();
            }
        }
        
    }
}
