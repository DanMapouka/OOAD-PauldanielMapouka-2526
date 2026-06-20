using System;

namespace CLDierenarts.Models;

/// <summary>
/// Basisklasse voor alle dieren in de dierenartsenpraktijk.
/// </summary>
public abstract class Dier
{
    public int Id { get; set; }

    public string Naam { get; set; } = string.Empty;

    public string EigenaarId { get; set; } = string.Empty;

    public Eigenaar? Eigenaar { get; set; }

    public DateTime Geboortedatum { get; set; }

    public double Gewicht { get; set; }

    public Urgentie Urgentie { get; set; } = Urgentie.Normaal;

    public bool IsOpgenomen { get; set; }

    public DateTime? DatumOpgenomen { get; set; }

    /// <summary>
    /// Geeft de gemeenschappelijke informatie van een dier terug.
    /// Afgeleide klassen voegen hier hun eigen informatie aan toe.
    /// </summary>
    public virtual string GeefInfo()
    {
        string eigenaarNaam = Eigenaar is null
            ? EigenaarId
            : Eigenaar.VolledigeNaam;

        string opnameInfo = IsOpgenomen
            ? DatumOpgenomen.HasValue
                ? $"Ja, sinds {DatumOpgenomen.Value:dd/MM/yyyy HH:mm}"
                : "Ja"
            : "Nee";

        return $"Naam: {Naam}{Environment.NewLine}" +
               $"Type: {GetType().Name}{Environment.NewLine}" +
               $"Eigenaar: {eigenaarNaam}{Environment.NewLine}" +
               $"Geboortedatum: {Geboortedatum:dd/MM/yyyy}{Environment.NewLine}" +
               $"Gewicht: {Gewicht:0.##} kg{Environment.NewLine}" +
               $"Urgentie: {Urgentie}{Environment.NewLine}" +
               $"Opgenomen: {opnameInfo}";
    }

    /// <summary>
    /// Tekst die in de ListBox wordt getoond.
    /// </summary>
    public override string ToString()
    {
        string opgenomenTekst = IsOpgenomen ? " - OPGENOMEN" : string.Empty;
        return $"{Naam} ({GetType().Name}){opgenomenTekst}";
    }
}
