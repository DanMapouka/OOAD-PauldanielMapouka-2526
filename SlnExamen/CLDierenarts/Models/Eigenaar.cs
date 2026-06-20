namespace CLDierenarts.Models;

/// <summary>
/// Stelt de eigenaar van één of meerdere dieren voor.
/// </summary>
public class Eigenaar
{
    public string Id { get; set; } = string.Empty;

    public string Voornaam { get; set; } = string.Empty;

    public string Achternaam { get; set; } = string.Empty;

    public string VolledigeNaam => $"{Voornaam} {Achternaam}".Trim();

    public override string ToString()
    {
        return VolledigeNaam;
    }
}
