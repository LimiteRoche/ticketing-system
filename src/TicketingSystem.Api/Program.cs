using FastEndpoints;
using FastEndpoints.Swagger;
using TicketingSystem.Application.Extensions;
using TicketingSystem.Infrastructure.Extensions;

namespace TicketingSystem.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddFastEndpoints().SwaggerDocument(o =>
            {
                o.DocumentSettings = s =>
                {
                    s.Title = "Ticketing system assessment";
                    s.Version = "v1";
                };
            });
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("ClientPolicy", policy =>
                {
                    policy.WithOrigins(
                        "http://localhost:4200",    // Angular dev server
                        "https://localhost:4200"   // Angular dev server HTTPS
                    )
                    .AllowAnyMethod()
                    .AllowAnyHeader();
                });
            });

            builder.Services.AddInfrastructure();
            builder.Services.AddApplicationServices();

            var app = builder.Build();

            app.UseFastEndpoints(c => {
                c.Endpoints.RoutePrefix = "api";
                c.Versioning.Prefix = "v";
                c.Errors.UseProblemDetails();
            }).UseSwaggerGen();


            app.UseHttpsRedirection();
            app.UseCors("ClientPolicy");
            app.Run();
        }
    }
}
