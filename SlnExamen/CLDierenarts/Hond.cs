using System;

namespace CLDierenarts
{
    /// <summary>
    /// Afgeleide klasse voor een hond. Een hond heeft als extra eigenschap Ras.
    /// </summary>
    public class Hond : Dier
    {
        public string Ras { get; set; }

        public override string Type
        {
            get { return "Hond"; }
        }

        public Hond()
        {
            Ras = "";
        }

        public override string GeefInfo()
        {
            return base.GeefInfo() + Environment.NewLine +
                   $"Ras: {Ras}";
        }
    }
}
