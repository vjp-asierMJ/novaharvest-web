# Cuestiones UT3 - NovaHarvest

**Autor: Asier Martín Jiménez**

## 1. De .cshtml a HTML

Al ejecutar dotnet build -p:EmitCompilerGeneratedFiles=true, el proyecto se compila y genera el archivo NovaHarvest.Web.dll. También se genera la clase para las páginas Razor. En mi caso, he localizado la página; dentro de ella aparece información de la propia página, como, por ejemplo, el título: `ViewData["Title"] = "Excedente destacado";`.

Por lo que Razor combina los dos valores, tanto el HTML como los valores del PageModel, para generar la página según la entrada del navegador.

El WriteLiteral sale directamente de lo que se escribe en el archivo .cshtml.

El Write sale de las expresiones Razor que empiezan por @, como `@Model.ServerTime.ToString("dd/MM/yyyy HH:mm:ss")`, y muestra el valor obtenido de esa expresión.

No hay ningún archivo .cshtml porque las páginas Razor se compilan y se convierten en código C#, que es el que utiliza la aplicación para ejecutarse. Por eso, no incluye los archivos .cshtml.

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/img1.png)

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/img2.png)

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/img3.png)

 