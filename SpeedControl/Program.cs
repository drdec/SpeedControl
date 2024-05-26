
using Microsoft.Extensions.Caching.Memory;
using SpeedControl.Data.Implementation;
using SpeedControl.Data.Interfaces;
using SpeedControl.Domain.Implementation;
using SpeedControl.Domain.Interfaces;

namespace SpeedControl
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();

            var filePath = builder.Configuration["FileStorage:FilePath"];

            var timeAccessStartTime = DateTime.Parse(builder.Configuration["TimeAccess:StartTime"]);
            var timeAccessEndTime = DateTime.Parse(builder.Configuration["TimeAccess:EndTime"]);

            AddRepositories(builder.Services, filePath);
            AddManagers(builder.Services, timeAccessStartTime, timeAccessEndTime);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }

        public static void AddManagers(IServiceCollection serviceCollection, DateTime startTime, DateTime endTime)
        {
            serviceCollection.AddScoped<IVehicleRecordManager>(sp =>
                new VehicleRecordManager(
                    sp.GetRequiredService<IVehicleRecordRepository>(),
                    startTime,
                    endTime));
        }

        public static void AddRepositories(IServiceCollection serviceCollection, string filePath)
        {
            var cashe = serviceCollection.AddMemoryCache();
            serviceCollection.AddSingleton<IVehicleRecordRepository>(sp =>
                new VehicleRecordRepository(filePath, sp.GetRequiredService<IMemoryCache>()));
        }
    }
}