using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Services.Maps;
using Windows.UI;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Maps;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace saudfhub
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class UnidadeRotaPage : Page
    {

        private Geopoint startPoint;
        private Geopoint endPoint;

        public UnidadeRotaPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            List<Object> parameters = e.Parameter as List<Object>;
            TextBlockNome.Text = parameters[0] as String;
            startPoint = parameters[1] as Geopoint;
            endPoint = parameters[2] as Geopoint;
            ListarRota();
        }

        private async void ListarRota()
        {
            // Get the route between the points.
            MapRouteFinderResult routeResult =
            await MapRouteFinder.GetDrivingRouteAsync(
              startPoint,
              endPoint,
              MapRouteOptimization.Time,
              MapRouteRestrictions.None,
              290);

            if (routeResult.Status == MapRouteFinderStatus.Success)
            {
                // Display summary info about the route.
                tbTurnByTurn.Inlines.Add(new Run()
                {
                    Text = "Tempo estimado (minutos) = "
                      + routeResult.Route.EstimatedDuration.TotalMinutes.ToString("F1")
                });
                tbTurnByTurn.Inlines.Add(new LineBreak());
                tbTurnByTurn.Inlines.Add(new Run()
                {
                    Text = "Distância (quilômetros) = "
                      + (routeResult.Route.LengthInMeters / 1000).ToString("F1")
                });
                tbTurnByTurn.Inlines.Add(new LineBreak());
                tbTurnByTurn.Inlines.Add(new LineBreak());
                // Display the directions.
                tbTurnByTurn.Inlines.Add(new Run()
                {
                    Text = "Direções",
                    FontSize = 35
                });
                tbTurnByTurn.Inlines.Add(new LineBreak());
                // Loop through the legs and maneuvers.
                int legCount = 0;
                foreach (MapRouteLeg leg in routeResult.Route.Legs)
                {
                    foreach (MapRouteManeuver maneuver in leg.Maneuvers)
                    {
                        legCount++;
                        tbTurnByTurn.Inlines.Add(new Run()
                        {
                            Text = legCount.ToString() + ". "
                        });
                        tbTurnByTurn.Inlines.Add(new Run()
                        {
                            Text = maneuver.InstructionText
                        });
                        tbTurnByTurn.Inlines.Add(new LineBreak());
                    }
                }
            }
            else
            {
                tbTurnByTurn.Text =
                "Um problema ocorreu: " + routeResult.Status.ToString();
            }

        }

        private void Click_TracarRota(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }

        private void Click_ListarRota(object sender, RoutedEventArgs e)
        {

        }
    }
}
