var builder = WebApplication.CreateBuilder(args);

var app = builder.Build();

app.MapStaticAssets();

app.MapGet("/", () =>
    Results.Content("""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <title>NovaHarvest</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <h1>NovaHarvest</h1>
        </header>

        <main>
            <img src="/img/logo.png" class="logo" alt="Logo NovaHarvest">
            <h2>Bienvenido a NovaHarvest</h2>
            <p>Una nueva forma de entender la agricultura.</p>

            <nav>
                <a href="/about">Sobre nosotros</a>
                <a href="/status">Estado</a>
            </nav>
        </main>

        <footer>
            Proyecto NovaHarvest
        </footer>
    </body>
    </html>
    """, "text/html; charset=utf-8"));

app.MapGet("/about", () =>
    Results.Content("""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <title>Sobre NovaHarvest</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <h1>Sobre nosotros</h1>
        </header>

        <main>
            <h2>¿Qué es NovaHarvest?</h2>
            <p>NovaHarvest es un proyecto relacionado con la innovación
            y la tecnología aplicada a la agricultura.</p>
            <a href="/">Volver al inicio</a>
        </main>
    </body>
    </html>
    """, "text/html; charset=utf-8"));

app.MapGet("/status", () =>
    Results.Content("""
    <!DOCTYPE html>
    <html lang="es">
    <head>
        <meta charset="UTF-8">
        <title>Estado del servicio</title>
        <link rel="stylesheet" href="/css/site.css">
    </head>
    <body>
        <header>
            <h1>Estado de NovaHarvest</h1>
        </header>

        <main>
            <h2>Backend funcionando correctamente</h2>
            <p>La aplicación ASP.NET Core está respondiendo.</p>
            <a href="/">Volver al inicio</a>
        </main>
    </body>
    </html>
    """, "text/html; charset=utf-8"));

app.Run();