
var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

// Configure static files from wwwroot
app.MapStaticAssets();

// Home page
app.MapGet("/", () =>
{
    var year = DateTime.Now.Year;

    return Results.Content($"""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>NovaHarvest - Inicio</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <img src="/img/logo.png" class="logo" alt="Logo NovaHarvest">
            <p>NovaHarvest</p>
            <nav>
                <a href="/">Inicio</a>
                <a href="/about">Acerca de</a>
                <a href="/status">Estado</a>
            </nav>
        </header>

        <main>
            <h1>Inicio</h1>
            <p>En el Valle del Jerte, los productores agrícolas generan excedentes de fruta y verdura que a veces no consiguen vender a tiempo.</p>
            <p>NovaHarvest quiere conectar a estos productores con compradores que puedan aprovechar sus productos.</p>
            <p>Entre ellos se encuentran residencias, comedores sociales, restaurantes y particulares.</p>
            <p>El objetivo es reducir el desperdicio alimentario y facilitar el aprovechamiento de los excedentes agrícolas.</p>
        </main>

        <footer>
            <p>NovaHarvest · IES Valle del Jerte · {year}</p>
        </footer>
    </body>
    </html>
    """, "text/html; charset=utf-8");
});

// About page
app.MapGet("/about", () =>
{
    var year = DateTime.Now.Year;

    return Results.Content($"""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>NovaHarvest - Acerca de</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <img src="/img/logo.png" class="logo" alt="Logo NovaHarvest">
            <p>NovaHarvest</p>
            <nav>
                <a href="/">Inicio</a>
                <a href="/about">Acerca de</a>
                <a href="/status">Estado</a>
            </nav>
        </header>

        <main>
            <h1>Acerca de</h1>
            <p>NovaHarvest es un proyecto inspirado en el aprovechamiento de excedentes agrícolas del Valle del Jerte.</p>
            <p>Centro educativo: IES Valle del Jerte.</p>
            <p>Autor: Asier</p>
            <p>Versión: 0.1.0</p>
        </main>

        <footer>
            <p>NovaHarvest · IES Valle del Jerte · {year}</p>
        </footer>
    </body>
    </html>
    """, "text/html; charset=utf-8");
});

// Status page
app.MapGet("/status", () =>
{
    var now = DateTime.Now;
    var date = now.ToString("dd/MM/yyyy HH:mm:ss");
    var year = now.Year;

    return Results.Content($"""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <meta name="viewport" content="width=device-width, initial-scale=1.0">
        <title>NovaHarvest - Estado del servidor</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <img src="/img/logo.png" class="logo" alt="Logo NovaHarvest">
            <p>NovaHarvest</p>
            <nav>
                <a href="/">Inicio</a>
                <a href="/about">Acerca de</a>
                <a href="/status">Estado</a>
            </nav>
        </header>

        <main>
            <h1>Estado del servidor</h1>
            <p>Fecha y hora del servidor: {date}</p>
            <p>Página generada por ASP.NET Core (Kestrel)</p>
        </main>

        <footer>
            <p>NovaHarvest · IES Valle del Jerte · {year}</p>
        </footer>
    </body>
    </html>
    """, "text/html; charset=utf-8");
});

app.Run();
