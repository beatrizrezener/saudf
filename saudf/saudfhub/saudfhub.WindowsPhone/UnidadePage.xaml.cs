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
            int idUnidade = (int)e.NavigationParameter;

            unidade = new UnidadeDAO().Buscar(idUnidade);
            TextBlockNome.Text = (unidade as Unidade).Nome;
            TextBlockTelefone.Text = (unidade as Unidade).Telefone;
            TextBlockEndereco.Text = (unidade as Unidade).Bairro + " " + (unidade as Unidade).Endereco;
            TextBlockTipo.Text = (unidade as Unidade).Tipo;
        }

        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
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

        private void Click_VerNoMapa(object sender, RoutedEventArgs e)
        {
            if (!Frame.Navigate(typeof(UnidadeMapaPage), unidade.IdUnidade))
            {
                throw new Exception(this.resourceLoader.GetString("NavigationFailedExceptionMessage"));
            }
        }

        private async void Click_LigarUnidade(object sender, RoutedEventArgs e)
        {
#if WINDOWS_PHONE_APP
            string numero = unidade.Telefone;

            if (numero == "Nao disponivel")
            {
                ContentDialog popup = new ContentDialog();
                popup.Title = "Desculpe!";
                popup.Content = "Aparentemente não existe nenhum\ntelefone para esta unidade.";
                popup.PrimaryButtonText = "Ok";
                await popup.ShowAsync().AsTask().ConfigureAwait(false);
            }
            else
            {
                string numeroTelefone = numero.Remove(0, 5);
                Windows.ApplicationModel.Calls.PhoneCallManager.ShowPhoneCallUI(numeroTelefone, unidade.Nome);
            }
#else
            var popup = new MessageDialog("Desculpe!", "Essa funcionalidade só está disponível em celulares.");
#endif
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
