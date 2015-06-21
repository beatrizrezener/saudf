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

// The Item Detail Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234232

namespace saudfhub
{
    public sealed partial class UnidadePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private Unidade unidade = new Unidade();
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

        private void Click_TracarRota(object sender, RoutedEventArgs e)
        {
            
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

        private void Click_ListarRota(object sender, RoutedEventArgs e)
        {

        }
    }
}
