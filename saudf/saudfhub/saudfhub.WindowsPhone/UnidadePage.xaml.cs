using saudfhub.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.Resources;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace saudfhub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnidadePage : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private readonly ResourceLoader resourceLoader = ResourceLoader.GetForCurrentView("Resources");
        private Unidade unidade = new Unidade();

        public UnidadePage()
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
            unidade = e.NavigationParameter as Unidade;
            TextBlockNome.Text = (unidade as Unidade).Nome;
            TextBlockTelefone.Text = (unidade as Unidade).Telefone;
            TextBlockEndereco.Text = (unidade as Unidade).Bairro + " " + (unidade as Unidade).Endereco;
            TextBlockTipo.Text = (unidade as Unidade).Tipo;
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

        private void Click_VerNoMapa(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(UnidadeMapaPage), unidade.IdUnidade))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void Click_LigarUnidade(object sender, RoutedEventArgs e)
        {
            string numero = unidade.Telefone;

            if (numero == "Nao disponivel")
            {
                string titulo = "Desculpe!";
                string mensagem = "Aparentemente não existe nenhum\ntelefone para esta unidade.";
#if WINDOWS_PHONE_APP
                ContentDialog popup = new ContentDialog();
                popup.Title = titulo;
                popup.Content = mensagem;
                popup.PrimaryButtonText = "Ok";
#else
            var popup = new MessageDialog(titulo, mensagem);
#endif
                await popup.ShowAsync().AsTask().ConfigureAwait(false);
            }
            else
            {
                string numeroTelefone = numero.Remove(0, 5);
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(numeroTelefone, unidade.Nome);
            }
        }

        private async void Click_ReportarErro(object sender, RoutedEventArgs e)
        {
            Windows.ApplicationModel.Email.EmailRecipient sendToDev1 = new Windows.ApplicationModel.Email.EmailRecipient()
            {
                Address = "beatrizrezener@gmail.com"
            };

            Windows.ApplicationModel.Email.EmailRecipient sendToDev2 = new Windows.ApplicationModel.Email.EmailRecipient()
            {
                Address = "jgbs@outlook.com"
            };

            Windows.ApplicationModel.Email.EmailMessage mail = new Windows.ApplicationModel.Email.EmailMessage();
            mail.Subject = "Reporte um erro ou dê uma sugestão";
            mail.To.Add(sendToDev1);
            mail.To.Add(sendToDev2);
            
            await Windows.ApplicationModel.Email.EmailManager.ShowComposeNewEmailAsync(mail);
        }
    }
}
