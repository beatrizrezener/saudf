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

// The Universal Hub Application project template is documented at http://go.microsoft.com/fwlink/?LinkID=391955

namespace saudfhub
{
    /// <summary>
    /// A page that displays a grouped collection of items.
    /// </summary>
    public sealed partial class HubPage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");

        /// <summary>
        /// Gets the NavigationHelper used to aid in navigation and process lifetime management.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the DefaultViewModel. This can be changed to a strongly typed view model.
        /// </summary>
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

        private void USMaisProxima_Click(object sender, RoutedEventArgs e)
        {

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
        private async void ItemTelefone_ItemClick(object sender, ItemClickEventArgs e)
        {
            MessageDialog popup = new MessageDialog("Essa funcionalidade só está disponível em celulares.","Desculpe!");
            await popup.ShowAsync();
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
