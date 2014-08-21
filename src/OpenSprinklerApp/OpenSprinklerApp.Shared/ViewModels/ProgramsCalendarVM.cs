using OpenSprinklerNet;
using System;
using System.Linq;
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
			var conn = AppModel.Current.Connection; 
			//Get controller metadata
			ControllerInfo controllerInfo = await conn.GetControllerInfoAsync();

			ProgramDetailsInfo programs = await conn.GetProgamDetailsAsync();
			Programs = new List<OpenSprinklerNet.Program>(programs.Programs); //Unfortunately I have to do this because stupid WinRT type system can't do proper type conversion
			IsSequential = controllerInfo.SequentialMode == Status.On;
			OnPropertyChanged("Programs");
		}

		public List<OpenSprinklerNet.Program> Programs { get; set; }
		
		private bool m_IsSequential;

		public bool IsSequential
		{
			get { return m_IsSequential; }
			private set { m_IsSequential = value; OnPropertyChanged(); }
		}
		
    }
}
