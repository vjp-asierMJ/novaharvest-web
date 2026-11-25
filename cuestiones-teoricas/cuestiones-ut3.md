# Cuestiones UT3 - NovaHarvest

**Autor: Asier Martín Jiménez**

## 1. De .cshtml a HTML

Al ejecutar dotnet build -p:EmitCompilerGeneratedFiles=true, el proyecto se compila y genera el archivo NovaHarvest.Web.dll. También se genera la clase para las páginas Razor. En mi caso, he localizado la página; dentro de ella aparece información de la propia página, como, por ejemplo, el título: `ViewData["Title"] = "Excedente destacado";`.

Por lo que Razor combina los dos valores, tanto el HTML como los valores del PageModel, para generar la página según la entrada del navegador.

El WriteLiteral sale directamente de lo que se escribe en el archivo .cshtml.

El Write sale de las expresiones Razor que empiezan por @, como `@Model.ServerTime.ToString("dd/MM/yyyy HH:mm:ss")`, y muestra el valor obtenido de esa expresión.

No hay ningún archivo .cshtml porque las páginas Razor se compilan y se convierten en código C#, que es el que utiliza la aplicación para ejecutarse. Por eso, no incluye los archivos .cshtml.

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-1-img1.png)

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-1-img2.png)

 ![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-1-img3.png)

 
 # 2. Tecnologías.

 Actualmente en mi pagina de Surplus.cshtml el precio lo muestro con esta linea:

`<p>Precio de excedente: @Model.SurplusPrice.ToString("C") / kg</p>`

En php se utiliza <?= ... ?> para mostrar el resultado de la expresion.

En Asp clásico se utiliza <%= ... %>.

En Razor se utiliza @ para comenzar el codigo, una diferencia con las otras es que estas utilizan apertura y cierre <?= ... ?> pero con Razor únicamente @ al comienzo y este reconoce automaticamente dónde termina por lo que no necesita de cierre.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-2-img7.png)

Al cambiar de MapGet a razor hemos dejado de escribir el HTML en Strings, lo que hacia que el HTML estaba mezclado con el C#. Ahora al utilizar Razor Pages cada pagina tiene su propio .cshtml donde se escribe directamente HTML y añado C# donde sea necesario mediante @ Además, ya no tengo que registrar cada página manualmente porque la URL sale de la ubicación del archivo.

Antiguo Program.cs
![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-2-img4.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-2-img5.png)

Nuevo Program.cs

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-2-img6.png)


# 3. Etiquetas de código. 

En mi proyecto uso varias formas de introducir codigo C# dentro del HTML gracias a Razor

**Expresión implícita**

En Surplus.cshtml utilizo:

`<h2>@Model.Surplus.Product</h2>`


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img8.png)

En el Navegador aparece:

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img9.png)


**Expresión explícita**

`<p>Nota del productor: @(Model.Surplus.Note ?? "Sin observaciones")</p>`

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img10.png)

Y el HTML al ver el código fuente es:

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img11.png)

Utilizo los paréntesis porque es una expresión explícita y dentro de ellos puedo utilizar operadores.

**Bloque de código @{ }**

En Surplus.cshtml utilizo:

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img12.png)

Este bloque se utiliza para guardar el título de la página, recoge el layout compartido y lo utiliza en la etiqueta tittle

**Carácter @@**

En About.cshtml utilizo:

`<p>Escríbenos a info@novaharvest.es o síguenos en @@novaharvest</p>`

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img13.png)

En el navegador aparece:

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img14.png)


**Comentario Razor @* *@**

En Surplus.cshtml tengo este comentario:

@* Datos del productor y del producto. *@

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img15.png)


Este comentario no aparece en el HTML que recibe el navegador.

**¿Por qué info@novaharvest.es no necesita @@ y @novaharvest sí?**

En el correo electrónico hay letras justo delante del arroba, por lo que Razor no lo interpreta como el comienzo de código. En cambio, en @novaharvest el arroba aparece al principio y Razor intenta interpretarla como una expresión de C#. Por eso utilizo @@ para mostrarla como un carácter normal.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-3-img14.png)

# 4. Sintexis

**Cambiar SurplusPrice por SurplusPrize**

El problema ocurre en que en mi Surplus.cshtml.cs la propiedad se llama SurplusPrice, por lo que al intentar buscar "SurplusPrize" da error de compilación ya que no encuentra el nombre de la variable

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-4-img16.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-4-img17.png)

**Añadir @Model.Surplus.PricePerKg * 0.7m**

Como esta explicado en el bloque 2 diapositiva 8, no da error ya que Razor separa los contenidos en 2 partes

1. @Model.Surplus.PricePerKg - es decir lo que va detras del @, esto se interpreta como codigo C#

2. *0.7m - Lo interpreta como texto normal, porque encuentra un espacio y termina la expresión implícita.


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-4-img18.png)


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-4-img19.png)


**Solución**

Para que la operación funcione hay que excribirla de maneria explícita:

@(Model.Surplus.PricePerKg * 0.7m)

# 5. Efectos en el documento.

**Nota del productor**

Esto ocurre porque al comenzar con @ Razor lo toma como codigo C#, entonces los símbolos < y > pasan a  &lt; y &gt; por lo que el navegador muestra las etiquetas como texto y no las interpreta.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img20.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img21.png)


**¿Qué pasaría si utilizara @Html.Raw?**

Al poner Html.Raw interpreta el texto como etiquetas HTML por lo que aparece en negrita

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img22.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img23.png)

El problema de esto es que un usuario malintencionado puede poner texto que se tomará  como etiquetas lo que provoca potenciales problemas de seguridad por lo que solo se deberia utilizar para HTML que controlas tú, escrito en el propio código y nunca con datos que vengan de usuarios: es abrir la puerta al XSS


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img24.png)

**Escenario A**

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img30.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img31.png)

No aparece el atributo class porque no es urgente y tampoco aparece disabled porque el excedente no está agotado.

**Escenario C**


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img27.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img28.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img29.png)

No aparece el atributo class porque no quedan kilos disponibles. En cambio, el botón sí tiene disabled porque el excedente está agotado.

**Escenario D**

Si modifico los dias restantes 

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img25.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img26.png)

Aparece class="urgent" porque queda un día para consumirlo y todavía hay kilos. El botón no tiene disabled porque el excedente no está agotado.

**¿Por qué a veces no aparecen los atributos class y disabled?**

Por la configuracion de mi Surplus.cshtml ya que tengo condicionales cuando se cumplen x condiciones, por lo que varia las clases y el boton segun la informacion del Surplus.cshtml.cs

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img32.png)

# 6. Directivas

En mi proyecto utilizo varias directivas y archivos especiales de Razor que cambian el comportamiento de las páginas.

| Directiva o archivo | Para qué lo utilizo |
|---|---|
| `@page` | Convierte el archivo `.cshtml` en una página Razor con su propia URL. |
| `@page "/ticket"` | Cambia la URL de la página. En este caso, la URL deja de depender de la ubicación del archivo. |
| `@model` | Conecta la página `.cshtml` con su clase C# (PageModel) para poder acceder a ella mediante `Model`. |
| `@using` | Importa un espacio de nombres para poder utilizar sus clases en la página. |
| `@namespace` | Elige el espacio de nombres de la clase que Razor genera para la página, haciendo que coincida con el de su PageModel. |
| `@inject` | Permite obtener un objeto que el servidor ya tiene creado, como `IWebHostEnvironment`, sin tener que crearlo con `new`. |
| `_ViewStart.cshtml` | Es un archivo que se ejecuta antes de cada página y establece el layout que utilizan por defecto. |
| `Layout = null` | Hace que una página concreta no utilice el layout. | |

**Quitar @namespace de _ViewImports.cshtml**

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-6-img33.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-6-img34.png)

**¿Por qué /reservations/ticket da 404?**

En mi archivo Ticket.cshtml utilizo @page "/ticket". Esto cambia la URL que Razor obtiene normalmente de la ubicación del archivo. Por eso la página responde en /ticket y cuando entro en /reservations/ticket aparece un error 404.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-6-img35.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-6-img60.png)


# 7. Tipos y operadores. 

| Dato calculado | Tipo C# | Operadores u operación | Por qué utilizo ese tipo |
|---|---|---|---|
| `SurplusPrice` | `decimal` | `*` y `-` | Es un precio, por lo que utilizo decimal para trabajar con dinero. |
| `RemainingKg` | `int` | `-` | Guarda los kilos disponibles después de restar los reservados. |
| `Boxes` | `int` | `/` | Guarda las cajas completas. Al dividir dos int se realiza una división entera. |
| `LooseKg` | `int` | `%` | Guarda el resto de la división, es decir, los kilos sueltos. |
| `SurplusValue` | `decimal` | `*` | Es el valor del excedente. Al multiplicar decimal por int el resultado es decimal. |
| `ConsumeBefore` | `DateTime` | `AddDays()` | Guarda una fecha. El tipo DateTime aparece en el temario, pero AddDays() no se explica. |
| `DaysToConsume` | `int` | `-` y `.Days` | Guarda los días restantes. La operación con `.Days` no aparece explicada en las diapositivas. |
| `StockStatus` | `string` | `==` y `<` | Guarda un texto con el estado del excedente. |
| `IsUrgent` | `bool` | `<=`, `&&` y `>` | Es una comparación que devuelve True o False. |
| `IsSoldOut` | `bool` | `==` | Es una comparación que devuelve True o False. |

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img36.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img37.png)

**¿Por qué RemainingKg / BoxKg da cajas completas?**


Por que ambos valores son enteros (int), al dividirlos se elimina la parte decimal.

En mi proyecto, con 223 kilos disponibles y cajas de 5 kg, el resultado es 44 cajas completas. Para calcular los kilos que sobran utilizo %, que devuelve el resto de la división, en este caso 3 kg.

**¿Qué pasaría si los kilos fueran decimal?**

Si los kilos fueran decimal, con 223 kilos quedarian 44,6 cajas de 5kg

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img38.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img39.png)

**¿Qué escribe Razor si muestras directamente un bool? ¿Y un null?**
Si muestro directamente un bool, Razor escribe True o False, dependiendo de su valor. En cambio, cuando el valor es null, Razor no escribe nada, ni siquiera la palabra null.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img40.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-7-img41.png)

# 8. Cultura.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-8-img42.png)

**Con dotnet run**

Al ejecutar la aplicación con dotnet run, utiliza la cultura configurada en mi ordenador. Por eso los precios y las fechas aparecen con el formato correspondiente a la configuración designada en el program.cs

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-8-img43.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-8-img44.png)

**Con docker**

En Docker el contenedor no tiene un idioma configurado, por lo que utiliza la cultura estandarizada. Esto hace que los precios y las fechas aparezcan con un formato diferente al español.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-8-img45.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-8-img46.png)

**¿Por qué UseRequestLocalization("es-ES") elimina las diferencias**

Al añadir de nuevo app.UseRequestLocalization("es-ES") en Program.cs, todas las peticiones utilizan la cultura española, independientemente de dónde se ejecute la aplicación.

# 9. Ámbito.
**Variable local**

En mi página Status.cshtml he probado a cambiar temporalmente el contador por una variable local dentro de @{ }. Al recargar varias veces la página, el contador siempre muestra 1.

Esto ocurre porque cada petición ejecuta de nuevo el código de la página. La variable empieza en 0, aumenta a 1 y desaparece al terminar la petición. Por eso, al volver a entrar, empieza otra vez desde 0.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img47.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img48.png)

**SiteStats**

Al abrir /status desde otro navegador, aparece el mismo contador compartido y las visitas de ambos navegadores aumentan el mismo valor.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img49.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img50.png)

Al ejecutar docker compose restart backend, el contador vuelve a 0 porque se reinicia el proceso. Cuando vuelvo a entrar en /status, el contador aparece en 1 porque se incrementa con la nueva petición.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img51.png)

**¿Por qué el layout no puede leer una variable declarada en Status.cshtml?**

Esto ocurre porque Status.cshtml y _Layout.cshtml son clases diferentes. Las variables que declaro dentro de @{ } son locales del método ExecuteAsync() de la página, por lo que el layout no puede acceder a ellas.

Para pasar el título utilizo ViewData["Title"], que comparten la página y el layout durante la petición. En Status.cshtml asigno el título y en _Layout.cshtml lo recojo mediante @ViewData["Title"].

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img52.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img53.png)


![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img54.png)

# 10. PageModel.

En mi proyecto he separado la logica de negocio de los datos que se muestran en pantalla. Para ello utilizo SurplusModel en Surplus.cshtml.cs y Surplus.cshtml para mostrar el contenido.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-5-img30.png)




**SurplusModel**

En Surplus.cshtml.cs tengo los datos del excedente, las constantes, las propiedades calculadas y el método OnGet().

Aquí realizo los cálculos y preparo los datos que después utiliza la página. Por ejemplo, calculo los kilos disponibles restando los kilos reservados a los kilos totales:

`RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;`

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-10-img56.png)

**Surplus.cshtml**

En Surplus.cshtml tengo el HTML y las expresiones Razor que utilizo para mostrar los datos del SurplusModel. También establezco el título de la página mediante ViewData["Title"] y utilizo formatos para mostrar los precios y las fechas.

De esta forma, los cálculos se realizan en el PageModel y la página se encarga de mostrar los resultados.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-10-img57.png)

**¿Cuántos objetos SurplusModel se crean si cargo /surplus tres veces y cuánto vive cada uno?**

Cada vez que entro en /surplus se crea un objeto SurplusModel nuevo. Por tanto, si cargo la página tres veces, se crean tres objetos diferentes.

Cada objeto vive durante una petición. Cuando termina esa petición, sus propiedades también dejan de existir.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-10-img59.png)

**¿Y el contador de SiteStats?**
En cambio, SiteStats utiliza una variable static, por lo que existe una sola vez para toda la aplicación mientras el proceso está activo. Es compartida por todos los usuarios y no desaparece al terminar una petición.

En mi proyecto, el contador se incrementa al entrar en /status, mediante SiteStats.Visits++. Entrar tres veces en /surplus no aumenta ese contador, porque esa página no realiza el incremento.

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-10-img58.png)

![alt text](/backend/NovaHarvest.Web/wwwroot/img/ut3-9-img50.png)