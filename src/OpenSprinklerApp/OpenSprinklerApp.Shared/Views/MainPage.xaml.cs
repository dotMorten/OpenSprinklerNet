using OpenSprinklerNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace OpenSprinklerApp
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
			Test();
        }

		private async void Test()
		{
			try
			{
				//Open connection
				var conn = await OpenSprinklerNet.OpenSprinklerConnection.OpenAsync("http://192.168.1.15:80", "opendoor");
				//Get controller metadata
				ControllerInfo controllerInfo = await conn.GetControllerInfoAsync();
				controllerInfoView.DataContext = controllerInfo;
				//Get current settings
				ControllerSettingsInfo settings = await conn.GetControllerSettingsAsync();
				//Get all programs
				ProgramDetailsInfo programs = await conn.GetProgamDetailsAsync();
				//Get info about the stations
				StationInfo stationInfo = await conn.GetStationsAsync();
				//Check whether sprinkler '1' is on
				bool isOn = await conn.QueryStationStatusAsync(1);
				//Check on/off status of all sprinklers
				var stations = await conn.QueryStationStatusesAsync();
				//Check if in manual mode
				if (settings.ManualMode == OpenSprinklerNet.Status.On)
				{
					//Turn sprinkler 3 on
					await conn.SetStationStatusAsync(3, OpenSprinklerNet.Status.On);
					await Task.Delay(2000);
					var bit = await conn.QueryStationStatusAsync(3);
					stations = await conn.QueryStationStatusesAsync();
					System.Diagnostics.Debug.Assert(bit);
					//Turn sprinkler 3 off
					await conn.SetStationStatusAsync(3, OpenSprinklerNet.Status.Off);
				}
			}
			catch { }
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(Views.ProgramsCalendarPage));
		}
    }
}
