using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Windows.Devices.WiFi;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using System.Net.NetworkInformation;
using Windows.Networking;
using Windows.Networking.Sockets;
using System.Text;
using Windows.Devices.Sensors;
using Windows.Graphics.Display;
using Windows.UI.Input.Spatial;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App3
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
       
        WiFiAdapter wifiAdapter;
        List<string> ls = new List<string>();
        StringBuilder sb = new StringBuilder();
        public MainPage()
        {
            this.InitializeComponent();
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {

            HostName host = new HostName("www.mail.ru");
            var eps = await DatagramSocket.GetEndpointPairsAsync(host, "80");
            if (eps.Count >= 1)
            {
                messageb.Text = eps.Count.ToString()+"\n";
            }
            else
            {
                messageb.Text="error\n";
            }
            //Ping ping = new Ping();
            // Send a ping.
            //PingReply reply = await ping.SendPingAsync("dotnetperls.com");
            // Display the result.
            //Text.Text = "ADDRESS:" + reply.Address.ToString();
            //Text.Text = Text.Text + "TIME:" + reply.RoundtripTime;
        }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {

            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            sb.AppendLine("time;ssid1;strength1;ssid2;strength2;ssid3;strength3;ssid4;strength4;ssid5;strength5");
            WiFiAccessStatus access = await WiFiAdapter.RequestAccessAsync();
            DateTimeOffset time = DateTimeOffset.Now;
            sb.AppendLine(time.ToString()+time.Millisecond);
            if (access == WiFiAccessStatus.Allowed)
            {
                DataContext = this;
                var result = await Windows.Devices.Enumeration.DeviceInformation.FindAllAsync(WiFiAdapter.GetDeviceSelector());
                //time = DateTimeOffset.Now;
                //sb.AppendLine(time.ToString() + time.Millisecond);
                if (result.Count >= 1)
                {
                    wifiAdapter = await WiFiAdapter.FromIdAsync(result[0].Id);
                    //time = DateTimeOffset.Now;
                    //sb.AppendLine(time.ToString() + time.Millisecond);
                }
            }
            time = DateTimeOffset.Now;
            //sb.AppendLine(time.ToString() + time.Millisecond);
           var result1= await WiFiAdapter.FindAllAdaptersAsync();
            sb.AppendLine(result1.GetType().ToString());
           while(true)
            {
                var firstadapter = result1[0];
                await firstadapter.ScanAsync();
                 time= DateTimeOffset.Now;
                
                //await Task.Delay(5000);
                sb.Append("  " +time.Hour+":"+time.Minute+":"+time.Second+":"+time.Millisecond + "   ;   ");
                
               // List<string> sortedList = new List<string>();
                foreach (var network in firstadapter.NetworkReport.AvailableNetworks)
                {
                    //var _dist = Math.Pow(10, 0.05 * (-20 * Math.Log10(network.ChannelCenterFrequencyInKilohertz / 1000) + 27.55 + Math.Abs(network.NetworkRssiInDecibelMilliwatts)));
                    //ls.Add(Math.Round(_dist, 1) + "m " + " Name : " + network.Ssid + " , dB signal RSSID : " + network.NetworkRssiInDecibelMilliwatts+network.Uptime.ToString(@"\:mm\:ss\:fff"));
                    sb.Append( network.Ssid+"    ;    "+network.NetworkRssiInDecibelMilliwatts+" ;  "+ network.Bssid+" ;  "+network.ChannelCenterFrequencyInKilohertz+" ; ");
                }

                sb.AppendLine();
                messageb.Text = sb.ToString();
                //connectpanel.Visibility = Visibility.Collapsed;
                await Windows.Storage.FileIO.WriteTextAsync(file, sb.ToString());
                //sortedList = ls.OrderBy(s => double.Parse(s.Substring(0, s.IndexOf('m')))).ToList();
               
            }
            

        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();

            for(int i=0;i<100;i++)
            {                
                DateTimeOffset time = DateTimeOffset.Now;
                //await Task.Delay(5000);
                sb.Append("Time:  " + time.Hour + ":" + time.Minute + ":" + time.Second + ":" + time.Millisecond + "   :   ");
                

                //sortedList = ls.OrderBy(s => double.Parse(s.Substring(0, s.IndexOf('m')))).ToList();

                
                //connectpanel.Visibility = Visibility.Collapsed;

            }
            //messageb.Text = sb.ToString();
            messageb.Text = sb.ToString();
            await Windows.Storage.FileIO.WriteTextAsync(file, sb.ToString());
        }

        private void ReadingChanged(object sender, GyrometerReadingChangedEventArgs e)
        {
            double x_Axis=0;
            double y_Axis=0;
            double z_Axis=0;

            GyrometerReading reading = e.Reading;

            // Calculate the gyrometer axes based on
            // the current display orientation.
            DisplayInformation displayInfo = DisplayInformation.GetForCurrentView();
            switch (displayInfo.CurrentOrientation)
            {
                case DisplayOrientations.Landscape:
                    x_Axis = reading.AngularVelocityX;
                    y_Axis = reading.AngularVelocityY;
                    z_Axis = reading.AngularVelocityZ;
                    break;
                case DisplayOrientations.Portrait:
                    x_Axis = reading.AngularVelocityY;
                    y_Axis = -1 * reading.AngularVelocityX;
                    z_Axis = reading.AngularVelocityZ;
                    break;
                case DisplayOrientations.LandscapeFlipped:
                    x_Axis = -1 * reading.AngularVelocityX;
                    y_Axis = -1 * reading.AngularVelocityY;
                    z_Axis = reading.AngularVelocityZ;
                    break;
                case DisplayOrientations.PortraitFlipped:
                    x_Axis = -1 * reading.AngularVelocityY;
                    y_Axis = reading.AngularVelocityX;
                    z_Axis = reading.AngularVelocityZ;
                    break;
            }
            sb.AppendLine(x_Axis + " ; " + y_Axis + " ; " + z_Axis);
            messageb.Text = sb.ToString();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
   