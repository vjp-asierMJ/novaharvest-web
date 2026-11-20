using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

public class StatusModel : PageModel
{
    // Fecha y hora actual del servidor
    public DateTime ServerTime { get; private set; }

    // Nombre del servidor, puede estar vacio
    public string? ServerName { get; private set; }

    // Número de visitas desde que arrancó el servidor
    public int Visits { get; private set; }

    public void OnGet()
    {
        // Obtiene la fecha y hora del servidor
        ServerTime = DateTime.Now;

        // Obtiene el nombre de la máquina donde se ejecuta
        ServerName = Environment.MachineName;

        // Incrementa el contador compartido de visitas
        SiteStats.Visits++;

        // Guarda el número de visitas para mostrarlo en la página
        Visits = SiteStats.Visits;
    }
}
