using FileUploadService.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpClient<IFileProcessingService, FileProcessingService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5001"); // Замість "http://data-service-url" вкажіть фактичну адресу DataService
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend",
        builder =>
        {
            builder.WithOrigins("https://localhost:5003") // Додаємо адресу вашого FrontendService
                   .AllowAnyHeader()
                   .AllowAnyMethod();
        });
});
builder.Services.AddScoped<IFileProcessingService, FileProcessingService>();

builder.Services.AddControllers();

var app = builder.Build();

// Додаємо CORS у pipeline
app.UseCors("AllowFrontend");

app.MapControllers();

app.Run();