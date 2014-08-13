using OpenSprinklerNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
	[Windows.UI.Xaml.Data.Bindable]
	public class ProgramsCalendarVM : ViewModelBase
    {
		public ProgramsCalendarVM()
		{
			LoadData();
		}
		private async void LoadData()
		{
			//Open connection
			var conn = AppModel.Current.Connection; // await OpenSprinklerNet.OpenSprinklerConnection.OpenAsync("http://192.168.1.15:80", "opendoor");
			//Get controller metadata
			ControllerInfo controllerInfo = await conn.GetControllerInfoAsync();

			ProgramDetailsInfo programs = await conn.GetProgamDetailsAsync();
			Programs = programs.Programs;
			IsSequential = controllerInfo.SequentialMode == Status.On;
			OnPropertyChanged("Programs");
		}

		public IEnumerable<OpenSprinklerNet.Program> Programs { get; private set; }


		private bool m_IsSequential;

		public bool IsSequential
		{
			get { return m_IsSequential; }
			private set { m_IsSequential = value; OnPropertyChanged(); }
		}
		
    }
}
