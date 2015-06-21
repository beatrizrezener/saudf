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
using saudfhub.Data;
using saudfhub.Common;
using Windows.UI.Popups;
using Windows.ApplicationModel.Resources;
using System.Threading.Tasks;
using Windows.Devices.Geolocation;
using System.Globalization;

namespace saudfhub
{
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private Geoposition geoposition;
        private bool podeProsseguir = false;
        private Unidade usMaisProxima = new Unidade();

        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        public HubPage()
        {
            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
        }

        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
         
        }

        public void Unidades_Loaded(object sender, RoutedEventArgs e)
        {
            var listView = (ListView)sender;
            CarregarListView(listView);
        }

        private void USMaisProxima_Click(object sender, RoutedEventArgs e)
        {
            //DefineVisibilidadeDoAnelDeProgresso(visivel: true, eOpacidadeDe: 0.2f);
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
                MessageDialog popup = new MessageDialog("Não foi possível obter\na sua localização.", "Verifique sua conexão com a internet\ne tente novamente.");
                await popup.ShowAsync();
            }
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
        private void DefineVisibilidadeDoAnelDeProgresso(bool visivel, float eOpacidadeDe)
        {
            ProgressRing meuAneldeProgresso = BuscarControleFilho<ProgressRing>(HubSaudf, "ProgressRingPesquisaUSMaisProxima") as ProgressRing;
            Image imagemDeFundo = BuscarControleFilho<Image>(HubSaudf, "ImageMapa") as Image;
            imagemDeFundo.Opacity = eOpacidadeDe;
            HubSaudf.IsEnabled = !visivel;
            meuAneldeProgresso.IsActive = visivel;
        }

        void ItemView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var unidadeSelecionada = ((Unidade)e.ClickedItem);
            if (!Frame.Navigate(typeof(UnidadePage), unidadeSelecionada.IdUnidade))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }
        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="Common.NavigationHelper.LoadState"/>
        /// and <see cref="Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </summary>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void CarregarListView(ListView listView)
        {
           TextBox filtro = BuscarControleFilho<TextBox>(HubSaudf, "TextBoxFiltro") as TextBox;
            string chave = filtro.Text.ToUpper();
            
            List<Unidade> unidadesPesquisadas;

            if (string.IsNullOrEmpty(chave))
            {
                unidadesPesquisadas = new UnidadeDAO().Listar();
            }
            else
            {
                unidadesPesquisadas = new UnidadeDAO().Listar(chave);
            }

            if (unidadesPesquisadas.Count == 0)
	        {
                MessageDialog mensagem = new MessageDialog("Sua pesquisa não retornou resultados.");
                await mensagem.ShowAsync();
                unidadesPesquisadas = new UnidadeDAO().Listar();
                filtro.Text = "";
            }
            listView.ItemsSource = unidadesPesquisadas;
        }

        private void Click_FiltraUnidades(object sender, RoutedEventArgs e)
        {
            ListView lstView = BuscarControleFilho<ListView>(HubSaudf, "ListViewUnidadeSaude") as ListView;
            CarregarListView(lstView);
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
    }
}
