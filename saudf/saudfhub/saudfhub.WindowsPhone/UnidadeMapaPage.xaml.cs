using saudfhub.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.Services.Maps;
using Windows.Storage.Streams;
using Windows.UI;
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

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace saudfhub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnidadeMapaPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Unidade unidade = new Unidade();
        public Geoposition currentPosition { get; set; }

        public UnidadeMapaPage()
        {
            this.InitializeComponent();

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
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            MapService.ServiceToken = "9sS3k8A_lN-OP2NWhVxW5g";
            unidade = new UnidadeDAO().Buscar((int)e.NavigationParameter);
            TextBlockNome.Text = (unidade as Unidade).Nome;
            showUnidadeNoMapa();
        }

        private void showUnidadeNoMapa()
        {
            MapIcon mapIcon = new MapIcon();
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinkPushPin.png"));
            mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);

            BasicGeoposition queryHint = new BasicGeoposition();
            queryHint.Latitude = double.Parse(unidade.Latitude, CultureInfo.InvariantCulture);
            queryHint.Longitude = double.Parse(unidade.Longitude, CultureInfo.InvariantCulture);

            Geopoint toPoint = new Geopoint(queryHint);

            mapIcon.Location = toPoint;
            mapIcon.Title = unidade.Nome;
            myMapControl.MapElements.Add(mapIcon);

            myMapControl.Center = toPoint;
            myMapControl.ZoomLevel = 15;

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
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void Click_TracarRota(object sender, RoutedEventArgs e)
        {
            ShowRotaNoMapa();
        }

        private async void ShowRotaNoMapa()
        {
            Geolocator geolocator = new Geolocator();
            currentPosition = null;
            try
            {
                currentPosition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
            }
            catch (Exception)
            {
                Debug.WriteLine("Erro ao pegar localizacao do usuario");
            }

            // Start
            BasicGeoposition startLocation = new BasicGeoposition();
            startLocation.Latitude = currentPosition.Coordinate.Point.Position.Latitude;
            startLocation.Longitude = currentPosition.Coordinate.Point.Position.Longitude;
            Geopoint startPoint = new Geopoint(startLocation);

            //Image iconStart = new Image();
            //iconStart.Source = new BitmapImage(new Uri("ms-appx:///Assets/PinkPushPin.png"));
            //myMapControl.Children.Add(iconStart);
            //MapControl.SetLocation(iconStart, startPoint);
            //MapControl.SetNormalizedAnchorPoint(iconStart, new Point(0.5, 0.5));

            // End
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = double.Parse(unidade.Latitude, CultureInfo.InvariantCulture);
            endLocation.Longitude = double.Parse(unidade.Longitude, CultureInfo.InvariantCulture);

            Geopoint endPoint = new Geopoint(endLocation);

            // Get the route between the points.
            MapRouteFinderResult routeResult =
                await MapRouteFinder.GetDrivingRouteAsync(
                startPoint,
                endPoint,
                MapRouteOptimization.Time,
                MapRouteRestrictions.None);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Use the route to initialize a MapRouteView.
                MapRouteView viewOfRoute = new MapRouteView(routeResult.Route);
                viewOfRoute.RouteColor = Colors.Yellow;
                viewOfRoute.OutlineColor = Colors.Black;

                // Add the new MapRouteView to the Routes collection
                // of the MapControl.
                myMapControl.Routes.Add(viewOfRoute);

                // Fit the MapControl to the route.
                await myMapControl.TrySetViewBoundsAsync(
                    routeResult.Route.BoundingBox,
                    null,
                    Windows.UI.Xaml.Controls.Maps.MapAnimationKind.None);
            }
        }
    }
}
