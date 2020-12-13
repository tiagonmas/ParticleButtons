using System;

namespace ParticleButtons.Models
{
    public class pfButtons
    {
        public pfButtons()
        {
            pFunc = new ParticleFunction();
            pFunc.Enabled = true;
            pFunc.Order = 100;
            pFunc.Saved = false;
        }
        public string Filename { get; set; }
        public string Text { get; set; }
        public DateTime Date { get; set; }

        public ParticleFunction pFunc { get; set; }


    }
}
