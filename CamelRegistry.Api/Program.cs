
using CamelRegistry.Entities;
using CamelRegistry.Logic;
using CamelRegistry.Logic.Helpers;
using CamelRegistry.Logic.Validators;
using CamelRegistry.Repository;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace CamelRegistry.Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddAuthorization();

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            //SQL
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? "Data Source=CamelRegistry.db";
            builder.Services.AddDbContext<CamelDbContext>(opt =>
            {
                opt.UseSqlite(connectionString);
            });

            //DI
            builder.Services.AddScoped<ICamelRepository, CamelRepository>();
            builder.Services.AddScoped<ICamelLogic, CamelLogic>();
            builder.Services.AddAutoMapper(cfg =>
            {
                cfg.AddProfile<CamelMappingProfile>();
            });
            builder.Services.AddScoped<IValidator<CamelCreateModel>, CamelCreateModelValidator>();
            builder.Services.AddScoped<IValidator<CamelUpdateModel>, CamelUpdateModelValidator>();


            var app = builder.Build();

            // Configure the HTTP request pipeline.
           
            
            app.UseSwagger();
            app.UseSwaggerUI();
            


            //exception handling
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (ValidationException ex)
                {
                    context.Response.StatusCode = 400;
                    await context.Response.WriteAsJsonAsync(new { error = "Validation Error", details = ex.Message });
                }
                catch (KeyNotFoundException ex)
                {
                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsJsonAsync(new { error = "Not Found", details = ex.Message });
                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = 500;
                    await context.Response.WriteAsJsonAsync(new { error = "Internal Server Error", details = ex.Message });
                }
            });



            var camels = app.MapGroup("/camels").WithTags("Camels");

            camels.MapGet("/", async (ICamelLogic logic) =>
            {
                var allCamels = await logic.GetAllAsync();
                return Results.Ok(allCamels);
            })
                .WithName("GetAllCamels")
                .WithSummary("Retrieves all camels");

            camels.MapGet("/{id:guid}", async (Guid id, ICamelLogic logic) =>
            {
                var eliteCamel= await logic.GetByIdAsync(id);
                return Results.Ok(eliteCamel);
            })
                .WithName("GetCamelById")
                .WithSummary("Retrieves a camel by its unique identifier");

            camels.MapPost("/", async (CamelCreateModel createModel, ICamelLogic logic) =>
            {
                var result = await logic.AddAsync(createModel);
                return Results.Created($"/camels/{result.Id}", result);  
            })
                .WithName("CreateCamel")
                .WithSummary("Creates a new camel with the provided details");

            camels.MapPut("/{id:guid}", async (Guid id, CamelUpdateModel updateModel, ICamelLogic logic) =>
            {
                var updatedModel= await logic.UpdateAsync(id, updateModel);
                return Results.Ok(updatedModel);
            })
                .WithName("UpdateCamel")
                .WithSummary("Updates an existing camel's details by its unique identifier");
            camels.MapDelete("/{id:guid}", async (Guid id, ICamelLogic logic) =>
            {
                await logic.DeleteAsync(id);
                return Results.NoContent();
            })
                .WithName("DeleteCamel")
                .WithSummary("Deletes a camel by its unique identifier");


            using (var scope= app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<CamelDbContext>();
                db.Database.Migrate();
            }

            app.UseAuthorization();

            

            app.Run();
        }
    }
}
