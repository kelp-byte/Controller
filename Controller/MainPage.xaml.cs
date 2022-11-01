using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Controller.Schnittstellen;
using System.Timers;
using Acr.UserDialogs;

namespace Controller
{
    public partial class MainPage : ContentPage
    {
        private IBluetooth bluetooth;

        #region const
        private const string go = "\x01" + "1" + "\x02" + "1" + "\x03";
        private const string back = "\x01" + "1" + "\x02" + "2" + "\x03";
        private const string still = "\x01" + "1" + "\x02" + "0" + "\x03";
        private const string lightOn = "\x01" + "4" + "\x02" + "1" + "\x03";
        private const string lightOff = "\x01" + "4" + "\x02" + "0" + "\x03";
        private const string hornOn = "\x01" + "5" + "\x02" + "1" + "\x03";
        private const string hornOff = "\x01" + "5" + "\x02" + "0" + "\x03";
        private const string left = "\x01" + "2" + "\x02" + "1" + "\x03";
        private const string right = "\x01" + "2" + "\x02" + "2" + "\x03";
        private const string neutral = "\x01" + "2" + "\x02" + "0" + "\x03";
        #endregion;

        private bool is_connect = false;
        public MainPage()
        {
            bluetooth = DependencyService.Get<Schnittstellen.IBluetooth>().GetBluetooth();
            bluetooth.CheckBt();
            InitializeComponent();
        }

        private void CheckConnection_Elapsed(object sender, ElapsedEventArgs e)
        {

        }

        private void ConnectTbi_Clicked(object sender, EventArgs e)
        {
            if (is_connect)
            {
                bluetooth.Disconnect();

                ConnectTbi.Text = "Verbinden";
                StateLbl.Text = "nicht Verbunden";
                StateLbl.TextColor = Color.Red;

                is_connect = false;
                //checkConnection.Stop();
            }
            else
            {
                if (bluetooth.Connect())
                {

                    bluetooth.preperListenData();
                    bluetooth.listenToData(velocityLbl);

                    StateLbl.Text = "Verbunden";
                    ConnectTbi.Text = "Trennen";
                    StateLbl.TextColor = Color.Green;

                    is_connect = true;
                    //checkConnection.Start();
                    UserDialogs.Instance.Toast("Verbunden mit Gerät 98:D3:31:F6:47:00");

                }
                else
                {
                    StateLbl.TextColor = Color.Red;
                    StateLbl.Text = "Verbindung nicht möglich";

                    is_connect = false;
                    //checkConnection.Stop();
                    UserDialogs.Instance.Toast("Verbindung mit 98:D3:31:F6:47:00 nicht möglich");
                }
            }
        }
        private void GoBtn_Released(object sender, EventArgs e)
        {
            bluetooth.Write(still);
        }
        private void BackBtn_Released(object sender, EventArgs e)
        {
            bluetooth.Write(still);
        }
        private void HornBtn_Pressed(object sender, EventArgs e)
        {
            bluetooth.Write(hornOn);
        }
        private void HornBtn_Released(object sender, EventArgs e)
        {
            bluetooth.Write(hornOff);
        }
        private void LightSwh_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (LightSwh.IsToggled)
            {
                bluetooth.Write(lightOn);
            }
            else
            {
                bluetooth.Write(lightOff);
            }
        }
        private void GoBtn_Pressed(object sender, EventArgs e)
        {
            bluetooth.Write(go);
        }
        private void BackBtn_Pressed(object sender, EventArgs e)
        {
            bluetooth.Write(back);
        }




        private void MointorTbi_Clicked(object sender, EventArgs e)
        {
            var page = new Monitor(bluetooth.getInputData(), bluetooth.getOutputData());
            Navigation.PushAsync(page);

        }

        private void RightBtn_Pressed(object sender, EventArgs e)
        {
            bluetooth.Write(right);
        }

        private void RightBtn_Released(object sender, EventArgs e)
        {
            bluetooth.Write(neutral);

        }

        private void LeftBtn_Pressed(object sender, EventArgs e)
        {
            bluetooth.Write(left);
        }

        private void LeftBtn_Released(object sender, EventArgs e)
        {
            bluetooth.Write(neutral);
        }
    }
}

