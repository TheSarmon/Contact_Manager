using Microsoft.EntityFrameworkCore;
using MediatR;
using DataService.Repositories;
using DataService.Services;
using DataService.DataAccess;
using DataService.Mapping;
using DataService.Handlers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IContactService, ContactService>();
builder.Services.AddScoped<IContactRepository, ContactRepository>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:5003")
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddMediatR(typeof(AddContactCommandHandler).Assembly);

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();