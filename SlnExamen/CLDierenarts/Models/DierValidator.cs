using System.Text.RegularExpressions;

namespace CLDierenarts.Models;

/// <summary>
/// Controleert de invoer voordat een nieuw dier wordt toegevoegd.
/// </summary>
public class DierValidator
{
    /// <summary>
    /// Het minimale aantal tekens waaruit een hondenras moet bestaan.
    /// </summary>
    public int MinimaalAantalTekensRas { get; set; } = 3;

    /// <summary>
    /// Een geldige naam bevat alleen letters, spaties en koppeltekens.
    /// </summary>
    public bool IsGeldigeNaam(string? naam)
    {
        if (string.IsNullOrWhiteSpace(naam))
        {
            return false;
        }

        return Regex.IsMatch(naam.Trim(), @"^[\p{L} -]+$");
    }

    /// <summary>
    /// Een geldig ras bevat minstens het ingestelde minimale aantal tekens.
    /// </summary>
    public bool IsGeldigRas(string? ras)
    {
        if (string.IsNullOrWhiteSpace(ras))
        {
            return false;
        }

        return ras.Trim().Length >= MinimaalAantalTekensRas;
    }
}
