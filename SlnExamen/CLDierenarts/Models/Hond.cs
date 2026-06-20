namespace CLDierenarts.Models;

/// <summary>
/// Een hond met als extra eigenschap het ras.
/// </summary>
public sealed class Hond : Dier
{
    public string Ras { get; set; } = string.Empty;

    public override string GeefInfo()
    {
        return $"{base.GeefInfo()}{Environment.NewLine}" +
               $"Ras: {Ras}";
    }
}
