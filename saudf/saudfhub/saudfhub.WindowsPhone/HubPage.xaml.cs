using saudfhub.Common;
using saudfhub.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Resources;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI.Core;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace saudfhub
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private async void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            // TODO: Create an appropriate data model for your problem domain to replace the sample data
            var sampleDataGroups = await SampleDataSource.GetGroupsAsync();
            this.DefaultViewModel["Groups"] = sampleDataGroups;
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
            // TODO: Save the unique state of the page here.
        }

        /// <summary>
        /// Shows the details of a clicked group in the <see cref="SectionPage"/>.
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Details about the click event.</param>
        private void GroupSection_ItemClick(object sender, ItemClickEventArgs e)
        {
            var groupId = ((SampleDataGroup)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(SectionPage), groupId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        /// <summary>
        /// Shows the details of an item clicked on in the <see cref="ItemPage"/>
        /// </summary>
        /// <param name="sender">The source of the click event.</param>
        /// <param name="e">Defaults about the click event.</param>
        private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var itemId = ((SampleDataItem)e.ClickedItem).UniqueId;
            if (!Frame.Navigate(typeof(ItemPage), itemId))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MapService.ServiceToken = "9sS3k8A_lN-OP2NWhVxW5g";
            ShowMyPosition();
        }

        private Geoposition geoposition;

        private async void ShowMyPosition()
        {
            Geolocator geolocator = new Geolocator();
            //geolocator.DesiredAccuracyInMeters = 50;
            geoposition = null;
            TextBlock geolocation = BuscarControleFilho<TextBlock>(Hub, "geolocation") as TextBlock;
            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
                geolocation.Text = "GPS:" + geoposition.Coordinate.Point.Position.Latitude.ToString("0.0000000") + ", " + geoposition.Coordinate.Point.Position.Longitude.ToString("0.0000000");
            }
            catch (Exception)
            {
                // Handle errors like unauthorized access to location
                // services or no Internet access.
                geolocation.Text = "Error";
            }

            //MapControl myMapControl = BuscarControleFilho<MapControl>(Hub, "myMapControl") as MapControl;

            //myMapControl.Center = geoposition.Coordinate.Point;
            //myMapControl.ZoomLevel = 15;

            //MapIcon mapIcon = new MapIcon();
            //mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinkPushPin.png"));
            //mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);
            //mapIcon.Location = geoposition.Coordinate.Point;
            //mapIcon.Title = "You are here";
            //myMapControl.MapElements.Add(mapIcon);
            showUnidadeMaisProxima();
        }

        private Double rad2deg(Double rad)
        {
            return (rad / Math.PI * 180.0);
        }

        private Double deg2rad(Double deg)
        {
            return (deg * Math.PI / 180.0);
        }

        private double computeDistanceBetweenTwoLatLon(double fromLat, double fromLon, double toLat, double toLon)
        {
            //If the same point
            if ((fromLat == toLat) && (fromLon == toLon))
            {
                return 0.0;
            }
            // Compute the distance with the haversine formula
            var distanceRad = Math.Acos(Math.Sin(deg2rad(fromLat)) * Math.Sin(deg2rad(toLat)) +
                        Math.Cos(deg2rad(fromLat)) * Math.Cos(deg2rad(toLat)) *
                        Math.Cos(deg2rad(fromLon - toLon)));
            var distanceDegree = rad2deg(distanceRad);
            // Distance in miles and KM - Add others if needed
            var miles = (double)distanceDegree * 69.0;
            var kilometers = (double)miles * 1.61;
            // return km = miles * 1.61
            return Math.Round(kilometers, 2);
        }

        private void showUnidadeMaisProxima()
        {
            List<Unidade> unidadesProximas = new List<Unidade>();
            Unidade u1 = new Unidade();
            u1.Nome = "Hospital Santa Marta";
            u1.Endereco = "Taguatinga";
            u1.Latitude = "-15.859855";
            u1.Longitude = "-48.042606";
            unidadesProximas.Add(u1);
            Unidade u2 = new Unidade();
            u2.Nome = "Hospital Santa Helena";
            u2.Endereco = "Asa Norte";
            u2.Latitude = "-15.735693";
            u2.Longitude = "-47.897239";
            unidadesProximas.Add(u2);
            Unidade u3 = new Unidade();
            u3.Nome = "Hospital Anchieta";
            u3.Endereco = "Taguatinga";
            u3.Latitude = "-15.823967";
            u3.Longitude = "-48.066628";
            unidadesProximas.Add(u3);
                
            var fromLatFloat = double.Parse(geoposition.Coordinate.Point.Position.Latitude.ToString("0.0000000"), CultureInfo.InvariantCulture); 
            var fromLonFloat = double.Parse(geoposition.Coordinate.Point.Position.Longitude.ToString("0.0000000"), CultureInfo.InvariantCulture);

            var usMaisProxima = new Unidade();
            double menor = 20000;

            for (int i = 0; i < 3; i++)
            {
                var usCorrente = unidadesProximas[i];

                var toLatFloat = double.Parse(usCorrente.Latitude, CultureInfo.InvariantCulture);
                var toLonFloat = double.Parse(usCorrente.Longitude, CultureInfo.InvariantCulture);

                var distance = computeDistanceBetweenTwoLatLon(fromLatFloat, fromLonFloat, toLatFloat, toLonFloat);
                if (distance < menor)
                {
                    menor = distance;
                    usMaisProxima = usCorrente;
                }
            }

            TextBlock unidadeMaisProxima = BuscarControleFilho<TextBlock>(Hub, "unidadeMaisProxima") as TextBlock;
            unidadeMaisProxima.Text = usMaisProxima.Nome + "\n" + usMaisProxima.Endereco;

        }

        private DependencyObject BuscarControleFilho<T>(DependencyObject controle, string controleFilho)
        {
            int childNumber = VisualTreeHelper.GetChildrenCount(controle);
            for (int i = 0; i < childNumber; i++)
            {
                DependencyObject child = VisualTreeHelper.GetChild(controle, i);
                FrameworkElement fe = child as FrameworkElement;
                // Not a framework element or is null
                if (fe == null) return null;

                if (child is T && fe.Name == controleFilho)
                {
                    // Found the control so return
                    return child;
                }
                else
                {
                    // Not found it - search children
                    DependencyObject nextLevel = BuscarControleFilho<T>(child, controleFilho);
                    if (nextLevel != null)
                        return nextLevel;
                }
            }
            return null;
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}