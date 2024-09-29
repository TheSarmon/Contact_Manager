var builder = WebApplication.CreateBuilder(args);

// Додаємо сервіси для MVC
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Налаштовуємо маршрутизацію
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();  // Для статичних файлів (CSS, JS)

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
