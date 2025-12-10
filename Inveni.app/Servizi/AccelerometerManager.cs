using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using UIKit;
using Xamarin.Essentials;

namespace Palmipedo.iOS.Core
{
    public class AccelerometerManager
    {
        private static object _lock = new object();

        public event EventHandler<AccelerometerInfo> ReadingChanged = delegate { };

        private Models.Scheda _card;

        public AccelerometerData LatestAccelerometerData { get; private set; }

        #region Singleton
        private static AccelerometerManager instance;

        public static AccelerometerManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AccelerometerManager();
                }
                return instance;
            }
        }
        #endregion

        private AccelerometerManager()
        {

        }

        public void StartMonitor()
        {
            StopMonitor();

            //_card = card;

            Accelerometer.Start(SensorSpeed.Default);
            Accelerometer.ReadingChanged += Accelerometer_ReadingChanged;
        }


        public void StopMonitor()
        {
            //_card = null;

            Accelerometer.Stop();
            Accelerometer.ReadingChanged -= Accelerometer_ReadingChanged;
        }

        private void Accelerometer_ReadingChanged(object sender, AccelerometerChangedEventArgs e)
        {
            lock (_lock)
            {
                LatestAccelerometerData = e.Reading;
            }

            ReadingChanged?.Invoke(sender, new AccelerometerInfo { AccelerometerData = e.Reading });
        }
    }

    public class AccelerometerInfo
    {
        //public Models.Scheda Card { get; set; }
        public AccelerometerData AccelerometerData { get; set; }
    }
}