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
using Windows.Devices.Geolocation;


using Windows.UI.Xaml.Controls.Maps;
using Windows.Services.Maps;
using System.Text;
// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace App13
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        StringBuilder sb = new StringBuilder();
        public MainPage()
        {
            this.InitializeComponent();

        }

        private void Seattle_Click(object sender, RoutedEventArgs e)
        {
            Geopoint seattlePoint = new Geopoint
                (new BasicGeoposition { Latitude = 47.6062, Longitude = -122.3321 });

            PlaceInfo spaceNeedlePlace = PlaceInfo.Create(seattlePoint);

            FrameworkElement targetElement = (FrameworkElement)sender;

            GeneralTransform generalTransform =
                targetElement.TransformToVisual((FrameworkElement)targetElement.Parent);

            Rect rectangle = generalTransform.TransformBounds(new Rect(new Point
                (targetElement.Margin.Left, targetElement.Margin.Top), targetElement.RenderSize));

            spaceNeedlePlace.Show(rectangle, Windows.UI.Popups.Placement.Below);
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Specify a known location.
            BasicGeoposition cityPosition = new BasicGeoposition() { Latitude = 47.604, Longitude = -122.329 };
            Geopoint cityCenter = new Geopoint(cityPosition);

            // Set the map location.
            MapControl1.Center = cityCenter;
            MapControl1.ZoomLevel = 12;
            MapControl1.LandmarksVisible = true;
        }
        private void SpaceNeedle_Click(object sender, RoutedEventArgs e)
        {
            Geopoint spaceNeedlePoint = new Geopoint
                (new BasicGeoposition { Latitude = 47.6205, Longitude = -122.3493 });

            PlaceInfoCreateOptions options = new PlaceInfoCreateOptions();

            options.DisplayAddress = "400 Broad St, Seattle, WA 98109";
            options.DisplayName = "Seattle Space Needle";

            PlaceInfo spaceNeedlePlace = PlaceInfo.Create(spaceNeedlePoint, options);

            FrameworkElement targetElement = (FrameworkElement)sender;

            GeneralTransform generalTransform =
                targetElement.TransformToVisual((FrameworkElement)targetElement.Parent);

            Rect rectangle = generalTransform.TransformBounds(new Rect(new Point
                (targetElement.Margin.Left, targetElement.Margin.Top), targetElement.RenderSize));

            spaceNeedlePlace.Show(rectangle, Windows.UI.Popups.Placement.Below);
        }

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var accessStatus = await Geolocator.RequestAccessAsync();
            Windows.Storage.Pickers.FileSavePicker savePicker = new Windows.Storage.Pickers.FileSavePicker();
            savePicker.FileTypeChoices.Add("Plain Text", new List<string>() { ".txt" });
            Windows.Storage.StorageFile file = await savePicker.PickSaveFileAsync();
            switch (accessStatus)
            {
                case GeolocationAccessStatus.Allowed:

                    // Get the current location.
                    Geolocator geolocator = new Geolocator();
                    while (true)
                    {
                        Geoposition pos = await geolocator.GetGeopositionAsync();
                        Geopoint myLocation = pos.Coordinate.Point;
                        string accuracy = pos.Coordinate.Accuracy.ToString();
                        string speed = pos.Coordinate.Speed.ToString();
                        string stdata = pos.Coordinate.SatelliteData.ToString();
                        string heading = pos.Coordinate.Heading.ToString();
                        string positionSource = pos.Coordinate.PositionSource.ToString();
                        string positionSourceTS = pos.Coordinate.PositionSourceTimestamp.ToString();
                        string altitude = myLocation.Position.Altitude.ToString();
                        string altitudeAccuracy = pos.Coordinate.AltitudeAccuracy.ToString();
                        string longtitude = myLocation.Position.Longitude.ToString();
                        string latitude = myLocation.Position.Latitude.ToString();
                        // Set the map location.
                        string time1 = pos.Coordinate.Timestamp.ToString();
                        MapControl1.Center = myLocation;
                        sb.AppendLine("Time Stamp: " + time1 +
                            " Latitude: " + latitude +
                            " Longtitude: " + longtitude +
                            " Accuracy: " + accuracy);
                          //  " Heading: " + heading+
                          //  " Speed: "+ speed+
                          //  " SatelliteData: "+stdata+
                          //  " Point: "+myLocation.ToString()+
                          //  " Position Source: "+ positionSource+
                          //  " Position Source Time Stamp: "+positionSourceTS+
                          //  " Altitude: "+altitude+
                          //  " Altitude Accuracy: "+altitudeAccuracy);
                        MapControl1.ZoomLevel = 20;
                        MapControl1.LandmarksVisible = true;
                        messageb.Text = sb.ToString();
                        await Windows.Storage.FileIO.WriteTextAsync(file, sb.ToString());
                    }
                    break;

                case GeolocationAccessStatus.Denied:
                    // Handle the case  if access to location is denied.
                    break;

                case GeolocationAccessStatus.Unspecified:
                    // Handle the case if  an unspecified error occurs.
                    break;
            }
        }
    }
}
