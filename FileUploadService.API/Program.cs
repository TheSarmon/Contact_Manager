using FileUploadService.Services;
using DataService.Mapping;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddAutoMapper(typeof(MappingProfile));

builder.Services.AddHttpClient<IFileProcessingService, FileProcessingService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001");
});

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
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

builder.Services.AddControllers();

var app = builder.Build();

app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();