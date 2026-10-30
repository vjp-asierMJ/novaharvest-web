# Primera etapa: utilizamos el SDK de .NET 10 para compilar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

# Establecemos /src como carpeta de trabajo dentro del contenedor
WORKDIR /src

# Copiamos el archivo del proyecto para poder restaurar sus dependencias
COPY NovaHarvest.Web/NovaHarvest.Web.csproj NovaHarvest.Web/

# Descargamos las dependencias necesarias para el proyecto
RUN dotnet restore NovaHarvest.Web/NovaHarvest.Web.csproj

# Copiamos el resto de archivos del proyecto al contenedor
COPY NovaHarvest.Web/ NovaHarvest.Web/

# Compilamos y publicamos la aplicación en la carpeta /app/publish
RUN dotnet publish NovaHarvest.Web/NovaHarvest.Web.csproj -c Release -o /app/publish


# Segunda etapa: utilizamos ASP.NET Core 10 para ejecutar la aplicación
FROM mcr.microsoft.com/dotnet/aspnet:10.0

# Establecemos /app como carpeta de trabajo
WORKDIR /app

# Copiamos los archivos publicados en la primera etapa
COPY --from=build /app/publish .

# Configuramos ASP.NET Core para escuchar en el puerto 8080
ENV ASPNETCORE_URLS=http://+:8080

# Indicamos el puerto que utiliza la aplicación dentro del contenedor
EXPOSE 8080

# Ejecutamos la aplicación NovaHarvest al iniciar el contenedor
ENTRYPOINT ["dotnet", "NovaHarvest.Web.dll"]