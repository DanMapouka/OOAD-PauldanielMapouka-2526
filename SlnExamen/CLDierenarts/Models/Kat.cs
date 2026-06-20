namespace CLDierenarts.Models;

/// <summary>
/// Een kat met als extra eigenschap of ze gevaccineerd is.
/// </summary>
public sealed class Kat : Dier
{
    public bool IsGevaccineerd { get; set; }

    public override string GeefInfo()
    {
        string vaccinatieInfo = IsGevaccineerd ? "Ja" : "Nee";

        return $"{base.GeefInfo()}{Environment.NewLine}" +
               $"Gevaccineerd: {vaccinatieInfo}";
    }
}
