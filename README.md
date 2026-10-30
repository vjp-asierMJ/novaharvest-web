
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
- **backend:** aplicación ASP.NET Core 10 que utiliza Kestrel para generar las páginas.

### Diagrama de arquitectura

```text
                 NAVEGADOR
                     |
                     |
             http://localhost:8080
                     |
                     v
          +----------------------+
          |     WEB (NGINX)      |
          |                      |
          |     Puerto 80        |
          |     Reverse Proxy    |
          +----------------------+
                     |
                     |
              backend:8080
                     |
                     v
        +-------------------------+
        |        BACKEND          |
        |                         |
        |    ASP.NET Core 10      |
        |       Kestrel           |
        |                         |
        |     Puerto 8080         |
        +-------------------------+
                     |
                     v
             NOVAHARVEST
                     |
          +----------+----------+
          |          |          |
         Inicio    About      Status
                     |
                     v
              wwwroot/
              |       |
             CSS    Logo
```

### Recorrido de una petición

1. El usuario accede a NovaHarvest mediante el navegador y la dirección `http://localhost:8080`.
2. Docker Compose dirige la petición al servicio `web`, que utiliza Nginx.
3. Nginx recibe la petición y la reenvía al servicio `backend` mediante el puerto 8080.
4. ASP.NET Core recibe la petición a través de Kestrel.
5. La aplicación ejecuta la ruta correspondiente y genera la página HTML mediante código C#.
6. Los recursos estáticos, como CSS e imágenes, se sirven desde la carpeta `wwwroot`.
7. La respuesta vuelve a través de Nginx hasta el navegador.

El único servicio que publica un puerto al exterior es `web`. El backend permanece accesible únicamente dentro de la red de Docker.
