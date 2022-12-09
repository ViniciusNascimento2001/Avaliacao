var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "RealizarSoma",
    pattern: "RealizarSoma/{parameters}",
    defaults: new { controller = "Calculadora", action = "Somar" });

app.MapControllerRoute(
    name: "RealizarMultiplicacao",
    pattern: "RealizarMultiplicacao/{parameters}",
    defaults: new { controller = "Calculadora", action = "Multiplicar" });

app.MapControllerRoute(
    name: "RealizarDivisao",
    pattern: "RealizarDivisao/{parameter1}/{parameter2}",
    defaults: new { controller = "Calculadora", action = "Dividir" });

app.MapControllerRoute(
    name: "RealizarSubtracao",
    pattern: "RealizarSubtracao/{parameter1}/{parameter2}",
    defaults: new { controller = "Calculadora", action = "Subtrair" });

app.MapControllerRoute(
    name: "UploadFile",
    pattern: "UploadFile",
    defaults: new { controller = "Calculadora", action = "UploadFile" });


app.Run();
