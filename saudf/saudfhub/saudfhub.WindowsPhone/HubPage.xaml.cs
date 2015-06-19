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
using System.Diagnostics;

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
        private Geoposition geoposition;
        private Unidade usMaisProxima = new Unidade();

        public HubPage()
        {//Desenvolvido como projeto de conclusao do concurso S2B 1º/2015 - MIC Brasilia/DF
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
        }

        public void Unidades_Loaded(object sender, RoutedEventArgs e)
        {
            BasicGeoposition queryHint = new BasicGeoposition();
            queryHint.Latitude = -15.780148200000;
            queryHint.Longitude = -47.92916980000001;

            Geopoint pointBSB = new Geopoint(queryHint);

            MapControl myMapControl = BuscarControleFilho<MapControl>(HubSaudf, "myMapControl") as MapControl;
            myMapControl.Center = pointBSB;
            myMapControl.ZoomLevel = 10;

            MapService.ServiceToken = "9sS3k8A_lN-OP2NWhVxW5g";
            ShowMyPosition();

            var listView = (ListView)sender;

            CarregarListView(listView);
        }

        public void TelefonesEmergencia_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;

            List<TelefoneEmergencia> numerosTelefoneEmergencia = new List<TelefoneEmergencia>();
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Delegacia da Mulher", Numero: "180", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Disque Denúncia", Numero: "181", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia", Numero: "190", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "SAMU", Numero: "192", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Bombeiros", Numero: "193", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia Federal", Numero: "194", CaminhoFoto: "Assets/DarkGray.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia Civil", Numero: "197", CaminhoFoto: "Assets/DarkGray.png"));

            listView.ItemsSource = numerosTelefoneEmergencia;

        }

        private void CarregarListView(ListView listView)
        {
            TextBox filtro = BuscarControleFilho<TextBox>(HubSaudf, "TextBoxFiltro") as TextBox;
            string chave = filtro.Text.ToUpper();

            if (string.IsNullOrEmpty(chave))
            {
                listView.ItemsSource = new UnidadeDAO().Listar();
            }
            else
            {
                listView.ItemsSource = new UnidadeDAO().Listar(chave);
            }
        }

        private void Click_FiltraUnidades(object sender, RoutedEventArgs e)
        {
            ListView lstView = BuscarControleFilho<ListView>(HubSaudf, "ListViewUnidadeSaude") as ListView;
            CarregarListView(lstView);
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
            var unidadeSelecionada = ((Unidade)e.ClickedItem);
            if (!Frame.Navigate(typeof(UnidadePage), unidadeSelecionada))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void ItemTelefone_ItemClick(object sender, ItemClickEventArgs e)
        {
#if WINDOWS_PHONE_APP
            var telefoneSelecionado = ((TelefoneEmergencia)e.ClickedItem);
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(telefoneSelecionado.Numero, telefoneSelecionado.Nome);
#else
            var popup = new MessageDialog("Desculpe!", "Essa funcionalidade só está disponível em celulares.");
#endif
        }

        #region Mapa
        private void MaisInfo_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(UnidadePage), usMaisProxima);
        }
        private void Atualiza_Click(object sender, RoutedEventArgs e)
        {
            ShowMyPosition();
        }

        private async void ShowMyPosition()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            geoposition = null;

            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
            }
            catch (Exception)
            {
                // Handle errors like unauthorized access to location
                // services or no Internet access.
            }

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
            List<Unidade> unidadesProximas = new UnidadeDAO().Listar();

            var fromLatFloat = geoposition.Coordinate.Point.Position.Latitude;
            var fromLonFloat = geoposition.Coordinate.Point.Position.Longitude;

            double menor = 20000;

            for (int i = 0; i < unidadesProximas.Count; i++)
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

            MapControl myMapControl = BuscarControleFilho<MapControl>(HubSaudf, "myMapControl") as MapControl;

            MapIcon mapIcon = new MapIcon();
            mapIcon.Image = RandomAccessStreamReference.CreateFromUri(new Uri("ms-appx:///Assets/PinkPushPin.png"));
            mapIcon.NormalizedAnchorPoint = new Point(0.25, 0.9);

            BasicGeoposition queryHint = new BasicGeoposition();
            queryHint.Latitude = double.Parse(usMaisProxima.Latitude, CultureInfo.InvariantCulture);
            queryHint.Longitude = double.Parse(usMaisProxima.Longitude, CultureInfo.InvariantCulture);

            Geopoint toPoint = new Geopoint(queryHint);

            mapIcon.Location = toPoint;
            mapIcon.Title = usMaisProxima.Nome;
            myMapControl.MapElements.Add(mapIcon);

            myMapControl.Center = toPoint;
            myMapControl.ZoomLevel = 15;

        }
        #endregion
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

            //if (e.Parameter as List<Unidade> != null)
            //{
            //    listaDeUnidadesFiltradas = e.Parameter as List<Unidade>;
            //}
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

    }
}