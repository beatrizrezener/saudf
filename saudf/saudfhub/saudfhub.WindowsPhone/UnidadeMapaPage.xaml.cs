using saudfhub.Common;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
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

    public sealed partial class UnidadeMapaPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Unidade unidade = new Unidade();
        public Geoposition currentPosition { get; set; }
        private Geopoint startPoint;
        private Geopoint endPoint;
        private bool podeProsseguir = false;
        MapRouteFinderResult routeResult = null;

        public UnidadeMapaPage()
        {
            this.InitializeComponent();

            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

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

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            setEndPoint();
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

            // Start
            await setCurrentPosition();

            if (podeProsseguir)
            {
                routeResult =
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
                    myMapControl.Center = startPoint;
                    myMapControl.ZoomLevel = 15;
                   
                }
                else
                {
                    await CriaAlerta("Não foi possível traçar\na rota.");
                }
            }
            else
            {
                await CriaAlerta("Não foi possível obter\na sua localização.");
            }
        }

        private async Task CriaAlerta(string titulo)
        {
            ContentDialog popup = new ContentDialog();
            popup.Title = titulo;
            popup.Content = "Habilite a localização do seu\ndispositivo e tente novamente.";
            popup.PrimaryButtonText = "Ok";
            await popup.ShowAsync().AsTask().ConfigureAwait(false);
        }

        private async Task setCurrentPosition()
        {
            if (startPoint == null)
            {
                Geolocator geolocator = new Geolocator();
                currentPosition = null;
                try
                {
                    currentPosition = await geolocator.GetGeopositionAsync(
                        maximumAge: TimeSpan.FromMinutes(5),
                        timeout: TimeSpan.FromSeconds(10));
                    BasicGeoposition startLocation = new BasicGeoposition();
                    startLocation.Latitude = currentPosition.Coordinate.Point.Position.Latitude;
                    startLocation.Longitude = currentPosition.Coordinate.Point.Position.Longitude;
                    startPoint = new Geopoint(startLocation);
                    podeProsseguir = true;
                }
                catch (Exception)
                {
                    podeProsseguir = false;
                    Debug.WriteLine("Erro ao pegar localizacao do usuario");
                }
            }
        }

        private void setEndPoint() {
            BasicGeoposition endLocation = new BasicGeoposition();
            endLocation.Latitude = double.Parse(unidade.Latitude, CultureInfo.InvariantCulture);
            endLocation.Longitude = double.Parse(unidade.Longitude, CultureInfo.InvariantCulture);

            endPoint= new Geopoint(endLocation);
        }
        private async void ListarRota()
        {
            await setCurrentPosition();

            if (routeResult != null)
            {
                Frame.Navigate(typeof(UnidadeRotaPage), routeResult);
            }
            else
            {
                if (podeProsseguir)
                {
                    List<Object> ListParameters = new List<Object>()
                    {
                         unidade.Nome,
                         startPoint,
                         endPoint,
                    };
                    Frame.Navigate(typeof(UnidadeRotaPage),ListParameters);
                }
                else
                {
                    await CriaAlerta("Não foi possível obter\na sua localização.");
                }
            }
        }

        private void Click_ListarRota(object sender, RoutedEventArgs e)
        {
            ListarRota();
        }
    }
}
