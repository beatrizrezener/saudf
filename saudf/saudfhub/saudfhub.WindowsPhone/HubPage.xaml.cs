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
using System.Threading.Tasks;

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace saudfhub
{
    public sealed partial class HubPage : Page
    {
        private readonly NavigationHelper navigationHelper;
        private readonly ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private Geoposition geoposition;
        private Unidade usMaisProxima = new Unidade();
        private bool podeProsseguir = false;

        public HubPage()
        {
            this.InitializeComponent();

            // Hub is only supported in Portrait orientation
            DisplayInformation.AutoRotationPreferences = DisplayOrientations.Portrait;

            this.NavigationCacheMode = NavigationCacheMode.Required;

            this.navigationHelper = new NavigationHelper(this);
        }

        public void Unidades_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            CarregarListView(listView);
        }

        public void TelefonesEmergencia_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;

            List<TelefoneEmergencia> numerosTelefoneEmergencia = new List<TelefoneEmergencia>();
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Atendimento à Mulher", Numero: "180", CaminhoFoto: "Assets/Emergencia/ligue_180.jpg"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Disque Denúncia", Numero: "181", CaminhoFoto: "Assets/Emergencia/disque-denuncia.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia Militar", Numero: "190", CaminhoFoto: "Assets/Emergencia/policia_militar.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "SAMU", Numero: "192", CaminhoFoto: "Assets/Emergencia/Samu.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Bombeiros", Numero: "193", CaminhoFoto: "Assets/Emergencia/Bombeiros.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia Federal", Numero: "194", CaminhoFoto: "Assets/Emergencia/Logo_Policia_Federal_DF.png"));
            numerosTelefoneEmergencia.Add(new TelefoneEmergencia(Nome: "Polícia Civil", Numero: "197", CaminhoFoto: "Assets/Emergencia/Logo_Policia_Civil_DF.png"));

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

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

       private void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var unidadeSelecionada = ((Unidade)e.ClickedItem);
            if (!Frame.Navigate(typeof(UnidadePage), unidadeSelecionada.IdUnidade))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private void ItemTelefone_ItemClick(object sender, ItemClickEventArgs e)
        {
            var telefoneSelecionado = ((TelefoneEmergencia)e.ClickedItem);
            Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(telefoneSelecionado.Numero, telefoneSelecionado.Nome);
        }

        #region Geoposition

        private async Task getMyPosition()
        {
            Geolocator geolocator = new Geolocator();
            geolocator.DesiredAccuracyInMeters = 50;

            geoposition = null;

            try
            {
                geoposition = await geolocator.GetGeopositionAsync(
                    maximumAge: TimeSpan.FromMinutes(5),
                    timeout: TimeSpan.FromSeconds(10));
                podeProsseguir = true;
                getUnidadeMaisProxima();
            }
            catch (Exception)
            {
                podeProsseguir = false;
            }

            if (!podeProsseguir)
            {
                ContentDialog popup = new ContentDialog();
                popup.Title = "Não foi possível obter\na sua localização.";
                popup.Content = "Verifique sua conexão com a internet\ne tente novamente.";
                popup.PrimaryButtonText = "Ok";
                await popup.ShowAsync().AsTask().ConfigureAwait(false);
            }
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

        private void getUnidadeMaisProxima()
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

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void USMaisProxima_Click(object sender, RoutedEventArgs e)
        {
            DefineVisibilidadeDoAnelDeProgresso(visivel: true, eOpacidadeDe: 0.2f);
            UnidadeMaisProxima();
        }
        private async void UnidadeMaisProxima()
        {
            await getMyPosition();
            DefineVisibilidadeDoAnelDeProgresso(visivel: false, eOpacidadeDe: 1.0f);
            if (podeProsseguir)
            {
                Frame.Navigate(typeof(UnidadePage), usMaisProxima.IdUnidade);                
            }
        }
        
        private void DefineVisibilidadeDoAnelDeProgresso(bool visivel, float eOpacidadeDe)
        {
            ProgressRing meuAneldeProgresso = BuscarControleFilho<ProgressRing>(HubSaudf, "ProgressRingPesquisaUSMaisProxima") as ProgressRing;
            Image imagemDeFundo = BuscarControleFilho<Image>(HubSaudf, "ImageMapa") as Image;
            imagemDeFundo.Opacity = eOpacidadeDe;
            HubSaudf.IsEnabled = !visivel;
            meuAneldeProgresso.IsActive = visivel;
        }
    }
}