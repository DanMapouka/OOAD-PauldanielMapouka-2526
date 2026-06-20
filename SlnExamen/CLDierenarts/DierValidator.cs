namespace CLDierenarts
{
    /// <summary>
    /// Valideert de invoer voor een nieuw dier.
    /// </summary>
    public class DierValidator
    {
        public int MinimaalAantalTekensRas { get; set; }

        public DierValidator()
        {
            MinimaalAantalTekensRas = 3;
        }

        /// <summary>
        /// Een naam bevat minstens één letter en verder alleen letters,
        /// spaties en koppeltekens.
        /// </summary>
        public bool IsGeldigeNaam(string naam)
        {
            if (string.IsNullOrWhiteSpace(naam))
            {
                return false;
            }

            bool bevatLetter = false;

            foreach (char teken in naam.Trim())
            {
                if (char.IsLetter(teken))
                {
                    bevatLetter = true;
                }
                else if (teken != ' ' && teken != '-')
                {
                    return false;
                }
            }

            return bevatLetter;
        }

        /// <summary>
        /// Een ras bestaat uit minstens drie tekens (standaardwaarde).
        /// </summary>
        public bool IsGeldigRas(string ras)
        {
            if (string.IsNullOrWhiteSpace(ras))
            {
                return false;
            }

            return ras.Trim().Length >= MinimaalAantalTekensRas;
        }
    }
}
