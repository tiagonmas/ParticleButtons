using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace ParticleButtons.Models
{
    public class pfButtons : INotifyPropertyChanged
    {
        private bool _running = false;
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

        public bool Running { 
            get { return _running; }

            set { 
                if (_running!=value)
                {
                    _running = value;
                    OnPropertyChanged();
                }
            }
        
        }

        public ParticleFunction pFunc { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
