using Microsoft.AspNetCore.Mvc.RazorPages;
using NovaHarvest.Web.Models;

namespace NovaHarvest.Web.Pages;

// Modelo que proporciona los datos del excedente a la página.
public class SurplusModel : PageModel
{
    // Contiene el excedente destacado en un único lugar.
    public Surplus Surplus { get; } = new(
        "Cereza Picota",
        "Huerta Los Castaños",
        "Cabezuela del Valle",
        DateTime.Today.AddDays(-2),
        343,
        120,
        3.20m,
        "<b>Recoger antes de las 14:00</b>"
    );

    // Porcentaje de descuento aplicado al excedente.
    private const decimal SurplusDiscount = 0.30m;

    // Kilos que contiene cada caja.
    private const int BoxKg = 5;

    // Días disponibles para consumir el producto.
    private const int ShelfLifeDays = 7;

    // Cantidad de kilos por debajo de la cual se considera poco stock.
    private const int LowStockKg = 50;

    // Precio por kilo después de aplicar el descuento.
    public decimal SurplusPrice { get; private set; }

    // Kilos que quedan después de descontar los reservados.
    public int RemainingKg { get; private set; }

    // Número de cajas completas.
    public int Boxes { get; private set; }

    // Kilos que quedan fuera de las cajas completas.
    public int LooseKg { get; private set; }

    // Valor total de los kilos que quedan disponibles.
    public decimal SurplusValue { get; private set; }

    // Fecha límite para consumir el excedente.
    public DateTime ConsumeBefore { get; private set; }

    // Días que quedan para consumir el producto.
    public int DaysToConsume { get; private set; }

    // Estado actual del excedente.
    public string StockStatus { get; private set; }

    // Indica si el plazo de consumo es urgente.
    public bool IsUrgent { get; private set; }

    // Indica si el excedente está agotado.
    public bool IsSoldOut { get; private set; }

    public void OnGet()
    {
        //Precio con descuento
        SurplusPrice = Surplus.PricePerKg * (1 - SurplusDiscount);

        //Kilos restantes
        RemainingKg = Surplus.AvailableKg - Surplus.ReservedKg;

        //Reparto de las cajas

        Boxes = RemainingKg / BoxKg;
        LooseKg = RemainingKg % BoxKg;

        //Valor excedente
        SurplusValue = RemainingKg * SurplusPrice;

        //Fecha de caducidad
        ConsumeBefore = Surplus.HarvestDate.AddDays(ShelfLifeDays);

        //Dias restantes
        DaysToConsume = (ConsumeBefore - DateTime.Today).Days;

        //Mensajes de estado segun disponibilidad
        if (RemainingKg == 0)
        {
            StockStatus = "Agotado";
        }
        else if (RemainingKg < LowStockKg)
        {
            StockStatus = "¡Últimos kilos!";
        }
        else
        {
            StockStatus = "Disponible";
        }

        //Si es urgente
        IsUrgent = DaysToConsume <= 2 && RemainingKg > 0;

        //Si es 0 es true 
        IsSoldOut = RemainingKg == 0;
    }
}

