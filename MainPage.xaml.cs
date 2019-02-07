using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using FindMyCar.Resources;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.IO.IsolatedStorage;
using Microsoft.Phone.Tasks;
using System.Device.Location;
using Microsoft.Phone.Maps;
using System.Globalization;
using System.Windows.Media;
using System.Text.RegularExpressions;

using Nokia.Phone.HereLaunchers;



namespace FindMyCar
{
    public partial class MainPage : PhoneApplicationPage
    {
        public double latival, lonval;
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void HyperlinkButton_Click_1(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/Help.xaml", UriKind.Relative));
        }
        private void HyperlinkButton_Click_2(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/About.xaml", UriKind.Relative));
        }
         protected override void OnNavigatedTo(System.Windows.Navigation.NavigationEventArgs e)
        {
            if (IsolatedStorageSettings.ApplicationSettings.Contains("LocationConsent"))
              {
                 //User already gave us his agreement for using his position
                 if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] == true)
 
                 return;
                 //If he didn't we ask for it
            else
              {
                 MessageBoxResult result = MessageBox.Show("Can I use your position?","Location", MessageBoxButton.OKCancel);
 
                if (result == MessageBoxResult.OK)
                {
                   IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
                }
                else
                {
                   IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
                }
 
              IsolatedStorageSettings.ApplicationSettings.Save();
             }        
            }   
 
      //Ask for user agreement in using his position
     else 
     {
        MessageBoxResult result = MessageBox.Show("Can I use your position?", "Location", MessageBoxButton.OKCancel);
 
        if (result == MessageBoxResult.OK)
        {
            IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = true;
        }
        else
        {
            IsolatedStorageSettings.ApplicationSettings["LocationConsent"] = false;
        }
 
        IsolatedStorageSettings.ApplicationSettings.Save();
    }
  }

        private async void Button_Click_1(object sender, RoutedEventArgs e)
        {
                    
 
//Check for the user agreement in use his position. If not, method returns.
    if ((bool)IsolatedStorageSettings.ApplicationSettings["LocationConsent"] != true)
    {
        // The user has opted out of Location.
        return;
    }
    try
    {
        Geolocator geolocator = new Geolocator();
        geolocator.DesiredAccuracyInMeters = 50;


        Geoposition geoposition = await geolocator.GetGeopositionAsync(
             maximumAge: TimeSpan.FromMinutes(5),
             timeout: TimeSpan.FromSeconds(10)
            );

        //With this 2 lines of code, the app is able to write on a Text Label the Latitude and the Longitude, given by {{Icode|geoposition}}
        LatitudeVal.Text = geoposition.Coordinate.Latitude.ToString("0.00");
        LongitudeVal.Text = geoposition.Coordinate.Longitude.ToString("0.00");
        double latival = Convert.ToDouble(geoposition.Coordinate.Latitude.ToString("0.00"));
        double lonval = Convert.ToDouble(geoposition.Coordinate.Longitude.ToString("0.00"));

        IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        if (!settings.Contains("latival"))
        {
            settings.Add("latival", latival);
        }
        else
        {
            settings["latival"] = latival;
        }
        settings.Save();

        if (!settings.Contains("lonval"))
        {
            settings.Add("lonval", lonval);
        }
        else
        {
            settings["lonval"] = lonval;
        }
        settings.Save();

    }


   //If an error is catch 2 are the main causes: the first is that you forgot to include ID_CAP_LOCATION in your app manifest. 
    //The second is that the user doesn't turned on the Location Services
    catch (Exception ex)
    {
        if ((uint)ex.HResult == 0x80004004)
        {
            Status.Text = "location is disabled in phone settings.";
        }
        //else
        {
            // something else happened during the acquisition of the location
        }
    }
}

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {

                

                if (IsolatedStorageSettings.ApplicationSettings.Contains("latival"))
                {
                    string valu =
                    IsolatedStorageSettings.ApplicationSettings["latival"] as string;
                }
                if (IsolatedStorageSettings.ApplicationSettings.Contains("lonval"))
                {
                    string value =
                    IsolatedStorageSettings.ApplicationSettings["lonval"] as string;
                }
                double val = Convert.ToDouble(latival);
                double val1 = Convert.ToDouble(lonval);

                DirectionsRouteDestinationTask routeTo = new DirectionsRouteDestinationTask();
                routeTo.Destination = new GeoCoordinate(val, val1);
                routeTo.Mode = RouteMode.Car;
                routeTo.Show();

                 
            }
            catch (Exception ex)
            {
                if ((uint)ex.HResult == 0x80004004)
                {
                    Status.Text = "location is disabled in phone settings.";
                }
                //else
                {
                    // something else happened during the acquisition of the location
                }
            }

        }


   }

}

        