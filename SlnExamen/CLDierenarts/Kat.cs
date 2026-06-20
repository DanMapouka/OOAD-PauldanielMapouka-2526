using System;

namespace CLDierenarts
{
    /// <summary>
    /// Afgeleide klasse voor een kat. Een kat houdt bij of ze gevaccineerd is.
    /// </summary>
    public class Kat : Dier
    {
        public bool IsGevaccineerd { get; set; }

        public override string Type
        {
            get { return "Kat"; }
        }

        public Kat()
        {
            IsGevaccineerd = false;
        }

        public override string GeefInfo()
        {
            string vaccinatieTekst = IsGevaccineerd ? "Ja" : "Nee";

            return base.GeefInfo() + Environment.NewLine +
                   $"Gevaccineerd: {vaccinatieTekst}";
        }
    }
}
