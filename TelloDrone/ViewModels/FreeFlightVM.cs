using System;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using TelloSharp;

namespace TelloDrone.ViewModels
{
    public class FreeFlightVM : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Tello drone;
        private bool flying = false;
        IDispatcherTimer timer;

        private int _temperature = 80;

        public int Temperature
        {
            get { return _temperature; }
            set
            {
                _temperature = value;
                OnPropertyChanged("Temperature");
            }
        }

        private int _fb = 15;

        public int FB
        {
            get { return _fb; }
            set
            {
                _fb = value;
                OnPropertyChanged("FB");
            }
        }

        private int _up = 15;

        public int UP
        {
            get { return _up; }
            set
            {
                _up = value;
                OnPropertyChanged("UP");
            }
        }

        private int _lf = 15;

        public int LR
        {
            get { return _lf; }
            set
            {
                _lf = value;
                OnPropertyChanged("LR");
            }
        }
        private int _speed = 30;

        public int Speed
        {
            get { return _speed; }
            set
            {
                _speed = value;
                OnPropertyChanged("Speed");
            }
        }
        private int _battery = 90;

        public int Battery
        {
            get { return _battery; }
            set
            {
                _battery = value;
                OnPropertyChanged("Battery");
            }
        }
        private int _width = 4;

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                OnPropertyChanged("Width");
            }
        }
        private int _height = 10;

        public int Height
        {
            get { return _height; }
            set
            {
                _height = value;
                OnPropertyChanged("Height");
            }
        }


        private bool _droneConnected = false;

        public bool DroneConnected
        {
            get { return _droneConnected; }
            set
            {
                _droneConnected = value;
                OnPropertyChanged("DroneConnected");
            }
        }
        public ICommand TakeOffOrLand
        {
            get
            {
                return new Command(() =>
                {
                    if (flying)
                    {
                        Land();
                    }
                    else
                    {
                        TakeOff();
                    }
                    
                });
            }
            private set { }
        }
        public ICommand Connect
        {
            get
            {
                return new Command(() =>
                {
                    try
                    {
                        var result = drone.Connect();
                        Console.WriteLine(result);

                        timer = Application.Current.Dispatcher.CreateTimer();
                        timer.Interval = TimeSpan.FromMilliseconds(1000);
                        Battery = drone.GetBattery();
                        timer.Tick += (s, e) =>
                        {
                            if (!drone.isFlying)
                            {
                                DroneConnected = false;
                                timer.Stop();
                            }
                            DroneConnected = true;
                            Battery = drone.GetBattery();

                            //move drone
                            if (Math.Abs(FB) >= Math.Abs(LR))
                            {
                                if (FB >= 30)
                                {
                                    drone.Forward(30);
                                }
                                else if (FB <= -30)
                                {
                                    drone.Back(30);
                                }
                                
                            }
                            else
                            {
                                if (LR >= 30)
                                {
                                    drone.Right(30);
                                }
                                else if (LR <= -30)
                                {
                                    drone.Left(30);
                                }
                            }

                            //Altitude
                        };

                        Shell.Current.DisplayAlert("Connection Result", result, "Ok");
                        if (Battery > 20)
                        {
                            timer.Start();
                        }
                        else
                        {
                            Shell.Current.DisplayAlert("Alert", "Battery too low to charge", "Ok");
                        }

                    }
                    catch (Exception ex)
                    {
                        Shell.Current.DisplayAlert("Failed", ex.Message,"Ok");
                    }
                    
                });
            }
            private set { }
        }
        public FreeFlightVM()
        {
            drone = new Tello();
        }

        private void TakeOff()
        {
            try
            {
                string result = drone.Takeoff();
                flying = true;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Failed", ex.Message, "Ok");
            }
        }
        private void Land()
        {
            try
            {
                string result = drone.Land();
                flying = false;
            }
            catch (Exception ex)
            {
                Shell.Current.DisplayAlert("Failed", ex.Message, "Ok");
            }
        }
    }
}

