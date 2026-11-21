using Microsoft.AspNetCore.Mvc.RazorPages;

namespace NovaHarvest.Web.Pages.Reservations;

// Modelo que contiene los datos del justificante de reserva.
public class TicketModel : PageModel
{
    // Datos de la reserva indicados en el enunciado.

    public string Reservation { get; set; } = "RES-0001";
    public string Product { get; set; } = "Cereza Picota";
    public int Kg { get; set; } = 20;
    public string Pickup { get; set; } = "Cooperativa del Valle del Jerte";

    // Fecha y hora en la que se genera el justificante.
    public DateTime GeneratedAt { get; set; }

    // Se ejecuta al entrar en la página.
    public void OnGet()
    {
        GeneratedAt = DateTime.Now;
    }
}