using System;

namespace Inveni.App.Elementi
{
    public class OggettoCaccia
    {
        public Guid Id { get; set; }
        public int IdGroup { get; set; }
        public Modelli.Scheda IndizioEnigma { get; set; } = new Modelli.Scheda();
        public Modelli.Scheda Caccia { get; set; } = new Modelli.Scheda();
        public int Index { get; set; }

        public OggettoCaccia()
        {
            Id = Guid.NewGuid();
        }
    }
}