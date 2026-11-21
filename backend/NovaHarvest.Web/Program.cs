
var builder = WebApplication.CreateBuilder(args);

// registra lo que necesita Razor Pages
builder.Services.AddRazorPages();

var app = builder.Build();

// Cultura, en este caso española
app.UseRequestLocalization("es-ES");

app.MapStaticAssets();

// una ruta por cada página de Pages/

app.MapRazorPages();

app.Run();
