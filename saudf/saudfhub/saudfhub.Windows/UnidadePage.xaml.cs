using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using saudfhub.Common;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Bing.Maps;
using System.Globalization;
using Windows.UI.Popups;
using Windows.Devices.Geolocation;
using System.Threading;
using System.Threading.Tasks;
using Windows.UI.Xaml.Documents;
using System.Diagnostics;

namespace saudfhub
{
    public sealed partial class UnidadePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Unidade unidade = new Unidade();
        Location userPosition = null;
        
        // maps variables 
        private Geolocator _geolocator = null;
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public UnidadePage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;

            _geolocator = new Geolocator();
        }

        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
            int idUnidade = (int)e.NavigationParameter;

            unidade = new UnidadeDAO().Buscar(idUnidade);
            TextBlockNome.Text = (unidade as Unidade).Nome;
            TextBlockTelefone.Text = (unidade as Unidade).Telefone;
            TextBlockEndereco.Text = (unidade as Unidade).Bairro + " " + (unidade as Unidade).Endereco;
            TextBlockTipo.Text = (unidade as Unidade).Tipo;
            InitializeMap();
        }
        void InitializeMap()
        {
            try
            {
                double lat = double.Parse(unidade.Latitude, CultureInfo.InvariantCulture);
                double lon = double.Parse(unidade.Longitude, CultureInfo.InvariantCulture);

                Pushpin pushpin = new Pushpin();
                pushpin.Tapped += new TappedEventHandler(pushpinTapped);

                MapLayer.SetPosition(pushpin, new Location(lat, lon));
                myMap.Children.Add(pushpin);

                myMap.Center = new Location(lat, lon);
                myMap.ZoomLevel = 16;
                myMap.MapType = MapType.Road;
            }
            catch (Exception)
            {
               
            }
        }

        private async void pushpinTapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            MessageDialog dialog = new MessageDialog(unidade.Nome);
            await dialog.ShowAsync();
        }

        private async void Click_TracarRota(object sender, RoutedEventArgs e)
        {
            myMap.Visibility = Windows.UI.Xaml.Visibility.Visible;
            scrollView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            await GetDirections();
        }

        Bing.Maps.Directions.RouteResponse response = null;

        public async Task GetDirections()
        {
            await setUserPosition();

            if (response == null)
            {
                double endLat = double.Parse(unidade.Latitude, CultureInfo.InvariantCulture);
                double endLong = double.Parse(unidade.Longitude, CultureInfo.InvariantCulture);

                Location endPoint = new Location(endLat, endLong);

                // Set the start and end waypoints
                Bing.Maps.Directions.Waypoint startWaypoint = new Bing.Maps.Directions.Waypoint(userPosition);
                Bing.Maps.Directions.Waypoint endWaypoint = new Bing.Maps.Directions.Waypoint(endPoint);

                Bing.Maps.Directions.WaypointCollection waypoints = new Bing.Maps.Directions.WaypointCollection();
                waypoints.Add(startWaypoint);
                waypoints.Add(endWaypoint);
            
                Bing.Maps.Directions.DirectionsManager directionsManager = myMap.DirectionsManager;
                directionsManager.Waypoints = waypoints;

                // Calculate route directions
                response = await directionsManager.CalculateDirectionsAsync();

                // Display the route on the map
                directionsManager.ShowRoutePath(response.Routes[0]);

                myMap.Center = userPosition;
            }
        }

        private async Task setUserPosition()
        {
            if (userPosition == null)
            {
                try
                {
                    scrollView.Visibility = Windows.UI.Xaml.Visibility.Visible;
                    messageTextBox.Text = "Traçando rota. Por favor, aguarde um momento.";
                    // Get the location.
                    Geoposition pos = await _geolocator.GetGeopositionAsync().AsTask();
                    messageTextBox.Text = "";
                    scrollView.Visibility = Windows.UI.Xaml.Visibility.Collapsed;

                    userPosition = new Location(pos.Coordinate.Point.Position.Latitude, pos.Coordinate.Point.Position.Longitude);


                    // Setting the zoom level of the map based on the accuracy of user location data.
                    if (pos.Coordinate.Accuracy <= 10)
                    {
                        myMap.ZoomLevel = 15.0f;
                    }
                    else if (pos.Coordinate.Accuracy <= 100)
                    {
                        myMap.ZoomLevel = 14.0f;
                    }
                }
                catch (System.UnauthorizedAccessException)
                {
                    messageTextBox.Text = "Localização desabilitada.";
                }
            }

        }

        private void Click_ReportarErro(object sender, RoutedEventArgs e)
        {
           
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void Click_ListarRota(object sender, RoutedEventArgs e)
        {

            await GetDirections();
            myMap.Visibility = Windows.UI.Xaml.Visibility.Collapsed;
            scrollView.Visibility = Windows.UI.Xaml.Visibility.Visible;
            
            messageTextBox.Inlines.Add(new Run()
            {
                Text = "Tempo estimado (minutos) = "
                  + (response.Routes[0].TravelDuration / 60).ToString("F1")
            });
            messageTextBox.Inlines.Add(new LineBreak());
            messageTextBox.Inlines.Add(new Run()
            {
                Text = "Distância (quilômetros) = "
                  + (response.Routes[0].TravelDistance ).ToString("F1")
            });
            messageTextBox.Inlines.Add(new LineBreak());
            messageTextBox.Inlines.Add(new LineBreak());
            // Display the directions.
            messageTextBox.Inlines.Add(new Run()
            {
                Text = "Direções",
                FontSize = 25
            });
            messageTextBox.Inlines.Add(new LineBreak());

            var listItems = response.Routes[0].RouteLegs[0].ItineraryItems.ToList();

            for (int i = 0; i < listItems.Count; i++)
            {
                messageTextBox.Inlines.Add(new Run()
                {
                    Text = (i + 1).ToString() + ". "
                });
                messageTextBox.Inlines.Add(new Run()
                {
                    Text = listItems[i].Instruction.Text
                });
                messageTextBox.Inlines.Add(new LineBreak());
            }

            Debug.WriteLine(messageTextBox.Text);
        }
    }
}
