
# NovaHarvest - Asier Martín Jiménez

Proyecto de infraestructura web inspirado en el aprovechamiento de excedentes agrícolas del Valle del Jerte.

NovaHarvest pretende conectar a productores agrícolas con compradores interesados en aprovechar los productos que no se han conseguido vender.

## 1. Arranque

### 1.1. Ejecutar con Docker

Para construir las imágenes y arrancar los servicios, ejecutar desde la carpeta principal del proyecto:

```powershell
docker compose up -d --build
```

Para comprobar que los contenedores están funcionando:

```powershell
docker compose ps
```

La aplicación estará disponible en:

http://localhost:8080/

Para detener y eliminar los contenedores:

```powershell
docker compose down
```

### 1.2. Ejecutar sin Docker

Entrar en la carpeta del proyecto ASP.NET Core:

```powershell
cd backend/NovaHarvest.Web
```

Ejecutar la aplicación:

```powershell
dotnet run
```

La consola mostrará la dirección local en la que se está ejecutando la aplicación.

Abrir en el navegador la dirección indicada por la consola.

Para detener la aplicación, pulsar `Ctrl + C`.

### 1.3. Direcciones de la aplicación

Con Docker:

- Página principal: http://localhost:8080/
- Acerca de: http://localhost:8080/about
- Estado: http://localhost:8080/status
- CSS: http://localhost:8080/css/site.css
- Logotipo: http://localhost:8080/img/logo.png

Sin Docker, utilizar la dirección local que muestra `dotnet run`, añadiendo las mismas rutas.

## 2. Arquitectura

NovaHarvest utiliza dos servicios principales:

- **web:** servidor Nginx que funciona como reverse proxy.
- **backend:** utiliza ASP.NET Core 10 y Kestrel para ejecutar la aplicación y gestionar las páginas mediante Razor Pages.

### Diagrama de arquitectura

                 NOVAHARVEST
                      |
                  NAVEGADOR
                      |
             http://localhost:8080
                      |
                      v
              +----------------+
              |   WEB (NGINX)  |
              |    Puerto 80   |
              +----------------+
                      |
          proxy_pass http://backend:8080
                      |
                      v
             +--------------------+
             |      BACKEND       |
             |   ASP.NET Core 10  |
             |      Kestrel       |
             +--------------------+
                      |
             +--------+---------+
             |                  |
             v                  v
        Razor Pages           wwwroot
             |                  |
      +------+------+       +---+---+
      |      |      |       |       |
     Inicio About Status   CSS    Imágenes
             |
          Surplus
             |
           Ticket

### Recorrido de una petición

1. El usuario accede a NovaHarvest mediante el navegador y la dirección `http://localhost:8080`.
2. Docker Compose dirige la petición al servicio `web`, que utiliza Nginx.
3. Nginx recibe la petición y la reenvía al servicio `backend` mediante el puerto 8080.
4. ASP.NET Core recibe la petición a través de Kestrel.
5. Razor Pages identifica la página correspondiente y genera el HTML.
6. Los archivos estáticos, como CSS e imágenes, se sirven desde la carpeta wwwroot.
7. La respuesta vuelve al navegador a través de Nginx.

De esta forma, el usuario solo accede directamente al servicio web.

## 3. Páginas

En NovaHarvest utilizo Razor Pages para organizar las distintas páginas de la aplicación. Cada página tiene su propio archivo `.cshtml` y algunas tienen también un PageModel asociado, donde se preparan los datos y se realizan los cálculos que después se muestran.

### 3.1. Localización de las páginas

| URL | Archivo `.cshtml` | PageModel |
|---|---|---|
| `/` | `Pages/Index.cshtml` | No tiene |
| `/about` | `Pages/About.cshtml` | No tiene |
| `/status` | `Pages/Status.cshtml` | `Pages/Status.cshtml.cs` (`StatusModel`) |
| `/surplus` | `Pages/Surplus.cshtml` | `Pages/Surplus.cshtml.cs` (`SurplusModel`) |
| `/ticket` | `Pages/Reservations/Ticket.cshtml` | `Pages/Reservations/Ticket.cshtml.cs` (`TicketModel`) |

### 3.2. Otros archivos que intervienen en las páginas

- **`Models/Surplus.cs`:** contiene la clase `Surplus`, que representa los datos del excedente utilizados por `SurplusModel`.
- **`Models/SiteStats.cs`:** contiene el contador de visitas compartido que utiliza `StatusModel`.

### 3.3. Ficheros especiales de Pages

- **`Pages/_ViewImports.cshtml`:** contiene las directivas comunes que utilizan las páginas, como `@using` y `@namespace`.
- **`Pages/_ViewStart.cshtml`:** se ejecuta antes de cada página y establece el layout que se utiliza por defecto.
- **`Pages/Shared/_Layout.cshtml`:** contiene la estructura común de las páginas, como la cabecera, el menú y el pie. Mediante `@RenderBody()` indica dónde aparece el contenido de cada página.

La página `/ticket` no utiliza el layout compartido porque tiene su propio documento HTML completo.