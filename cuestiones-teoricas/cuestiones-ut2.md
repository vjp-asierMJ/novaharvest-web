# Cuestiones UT2 - NovaHarvest

**Autor: Asier Martín Jiménez**

## 1. Cliente y servidor

Cuando abro la página `/status`, el navegador actúa como cliente y envía una petición HTTP GET al servidor. En NovaHarvest, Nginx recibe la petición en el servicio `web` y la reenvía al servicio `backend`, donde se encuentra ASP.NET Core con Kestrel. Kestrel ejecuta el código C# de `Program.cs` y genera la página con la fecha y hora actuales. El navegador recibe la respuesta HTML y muestra el contenido al usuario. Si en el futuro se permiten reservas, la regla de que no se pueden reservar más kilos de los disponibles debe comprobarse obligatoriamente en el servidor, porque el cliente no debe poder modificar las reglas de negocio.

## 2. Estático o dinámico

Una página estática contiene información que el servidor puede devolver tal como está guardada, por ejemplo un HTML, CSS o una imagen. En NovaHarvest, la página principal `/` podría ser estática si su presentación no cambiase, y la página `/about` también podría serlo porque contiene información del proyecto. Sin embargo, `/status` debe ser dinámica porque muestra la fecha y hora actuales del servidor. En el futuro, NovaHarvest necesitará páginas dinámicas para consultar la disponibilidad de los productos agrícolas. Otro ejemplo sería una página para realizar reservas, que tendría que comprobar los kilos disponibles antes de confirmar una operación.

## 3. Ejecución en el servidor

Cuando el navegador solicita `http://localhost:8080/status`, envía una petición HTTP al servicio `web` de Docker. Nginx recibe la petición en el puerto 80 del contenedor y utiliza `proxy_pass` para reenviarla a `backend:8080`. Kestrel recibe la petición y ejecuta la ruta `/status` definida en `Program.cs`. El código C# obtiene la fecha y hora mediante `DateTime.Now` y genera el HTML que se devuelve al navegador. Según lo visto en clase, con CGI clásico se crea un proceso nuevo por cada petición, lo que supone un mayor coste. Con FastCGI se mantienen procesos o workers preparados para atender varias peticiones. PHP-FPM es una implementación de FastCGI para PHP, mientras que con Apache y `mod_php` el intérprete PHP funciona dentro del propio proceso del servidor web.

## 4. Reverse proxy

En NovaHarvest, Nginx funciona como reverse proxy mediante la directiva `proxy_pass http://backend:8080` del archivo `web/nginx.conf`. Cuando accedo con Docker a `/status`, la cabecera `Server` esperada es `nginx`, porque Nginx es el servidor que responde directamente al navegador. Cuando ejecuto la aplicación con `dotnet run`, la cabecera esperada es `Kestrel`, porque accedo directamente al servidor de aplicaciones. Si detengo el backend con `docker compose stop backend`, las peticiones a `/status` y `/css/site.css` deben devolver un error 502 porque Nginx no puede comunicarse con el servicio de destino. El backend no publica ningún puerto porque solamente necesita comunicarse con Nginx dentro de la red interna de Docker.

Resultados de mis comprobaciones:
- Con Docker, cabecera Server: [ANOTAR RESULTADO].
- Con dotnet run, cabecera Server: [ANOTAR RESULTADO].
- Con el backend detenido, `/status`: [ANOTAR CÓDIGO HTTP].
- Con el backend detenido, `/css/site.css`: [ANOTAR CÓDIGO HTTP].

## 5. Nginx

En NovaHarvest, Nginx no sirve directamente `site.css` ni `logo.png` porque el archivo `web/nginx.conf` reenvía todas las peticiones al backend mediante `proxy_pass`. Es ASP.NET Core quien sirve los archivos estáticos desde `wwwroot` utilizando `MapStaticAssets()` en `Program.cs`. Según lo visto en clase, Nginx también puede servir ficheros estáticos directamente y actuar como servidor web y puerta de entrada. Si NovaHarvest se publicase en Internet, podría utilizar la compresión `gzip` para reducir el tamaño de las respuestas y mejorar la velocidad de carga. También podría utilizar `access_log` y `error_log` para registrar las peticiones y localizar errores.

## 6. Tecnologías

En NovaHarvest utilizo ASP.NET Core 10 con Kestrel como servidor de aplicaciones y Nginx como reverse proxy. En clase hemos trabajado también con PHP-FPM, que ejecuta código PHP mediante FastCGI. Si sustituyese ASP.NET Core por PHP-FPM, cambiarían los archivos del backend, porque utilizaría scripts PHP en lugar de `Program.cs`. También tendría que modificar el `Dockerfile` del backend para utilizar una imagen de PHP-FPM y adaptar `compose.yaml` para construir ese servicio. En `web/nginx.conf` cambiaría `proxy_pass` por `fastcgi_pass`, siguiendo el modelo de la sesión 9. Nginx seguiría siendo el servidor web y Docker Compose continuaría organizando los servicios.

## 7. Marcado y código

Un ejemplo de generación de HTML mediante C# está en la ruta `/status` de mi archivo `backend/NovaHarvest.Web/Program.cs`:

```csharp
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
        <title>NovaHarvest - Estado del servidor</title>
        ...
    </head>
    <body>
        ...
        <p>Fecha y hora del servidor: {date}</p>
        ...
    </body>
    </html>
    """, "text/html; charset=utf-8");
});
```

Un primer problema de generar HTML mediante cadenas es que se mezcla el código C# con el marcado HTML, haciendo más difícil leer y modificar las páginas. Un segundo problema es que se repiten muchas partes del HTML, como la cabecera, el menú y el pie de página, por lo que cualquier cambio común debe realizarse en las tres rutas. En mi proyecto, estas partes repetidas son el logotipo, el nombre NovaHarvest, los enlaces del menú, el enlace al CSS y el footer.

## 8. Docker

En NovaHarvest tengo dos imágenes principales para los servicios: una para `web`, que utiliza Nginx, y otra para `backend`, que ejecuta ASP.NET Core. Con el entorno levantado tengo dos contenedores, uno por cada servicio. Una imagen es una plantilla de solo lectura a partir de la que se crea un contenedor, mientras que el contenedor es una instancia de esa imagen en ejecución. La imagen final del backend no incluye el SDK porque solo necesita ASP.NET Core para ejecutar `NovaHarvest.Web.dll`; el SDK se utiliza en la primera etapa para compilar y publicar la aplicación. Sin `.dockerignore`, las carpetas `bin/` y `obj/` podrían entrar en el contexto de construcción y copiarse innecesariamente, aumentando su tamaño.

## 9. Herramientas

| Herramienta | Uso en NovaHarvest |
|---|---|
| Visual Studio Code | Crear y modificar los archivos del proyecto. |
| PowerShell | Ejecutar comandos y administrar el proyecto. |
| .NET 10 SDK | Crear, compilar y publicar la aplicación ASP.NET Core. |
| ASP.NET Core | Generar las páginas dinámicas y servir los recursos estáticos. |
| Kestrel | Recibir las peticiones y ejecutar la aplicación ASP.NET Core. |
| Docker | Construir y ejecutar las imágenes de los servicios. |
| Docker Compose | Definir y arrancar conjuntamente los servicios `web` y `backend`. |
| Nginx | Recibir las peticiones y reenviarlas al backend mediante reverse proxy. |
| Git | Registrar los cambios del proyecto mediante commits. |
| Navegador y curl | Acceder a las páginas y comprobar las respuestas y cabeceras HTTP. |

## 10. Observación

En NovaHarvest, la fecha y hora de `/status` se calculan mediante `DateTime.Now` en cada petición. Cuando ejecuto la aplicación con `dotnet run`, se utiliza la configuración horaria del sistema donde se ejecuta. Cuando la ejecuto con Docker, se utiliza la configuración horaria del contenedor. Por eso las horas pueden no coincidir si Windows y el contenedor utilizan zonas horarias diferentes. Por ejemplo, si Windows utiliza la hora de España y el contenedor utiliza UTC, puede existir una diferencia de una o dos horas según la época del año. Para comprobarlo, comparo las horas mostradas en ambas ejecuciones.

Resultados de mis comprobaciones:
- Hora con `dotnet run`: [ANOTAR RESULTADO].
- Hora con Docker: [ANOTAR RESULTADO].
- Diferencia observada: [ANOTAR RESULTADO].