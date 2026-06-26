using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using GymSystemMVC.DAL.Data.Contexts;
using GymSystemMVC.DAL.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GymSystemMVC.DAL.Data.DataSeeds
{
    public static class GymDataSeed
    {
        public static async Task SeedAsync(GymDbContext gymDbContext, string seedFilesPath, ILogger logger, CancellationToken ct = default)
        {
            try
            {
                if (!await gymDbContext.Plans.AnyAsync(ct))
                {
                    var plans = LoadDataFromJsonFile<Plan>("plans.json",seedFilesPath);

                    if(plans.Count > 0)
                    {
                        gymDbContext.Plans.AddRange(plans);

                        logger.LogInformation($"Seeded {plans.Count} Plans");
                    }
                }

                if (gymDbContext.ChangeTracker.HasChanges())
                    await gymDbContext.SaveChangesAsync(ct);
                
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Gym Data Seed Failed");
                throw;
            }

        }

            private static List<T> LoadDataFromJsonFile<T>(string fileName, string FolderPath)
            {
               var filePath = Path.Combine(FolderPath, fileName); // Combine => FolderPath/fileName

               if (!File.Exists(filePath))
               {
                 throw new FileNotFoundException($"Seed Data File Not Found : {filePath}");
               }

               var data = File.ReadAllText(filePath);

               var options = new JsonSerializerOptions
               {
                   PropertyNameCaseInsensitive = true,
               };

               options.Converters.Add(new JsonStringEnumConverter());

               return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
            }
        }
    }

