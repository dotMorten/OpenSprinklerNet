using OpenSprinklerNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
	[Windows.UI.Xaml.Data.Bindable]
	public class ControllerInfoVM : ViewModelBase
    {
		
		public ControllerInfoVM()
		{
			LoadControllerInfo();
		}
		private async void LoadControllerInfo()
		{
			//Open connection
			var conn = AppModel.Current.Connection;
			//Get controller metadata
			ControllerInfo = await conn.GetControllerInfoAsync();
		}

		private ControllerInfo m_ControllerInfo;

		public ControllerInfo ControllerInfo
		{
			get { return m_ControllerInfo; }
			private set
			{
				if (m_ControllerInfo != value)
				{
					m_ControllerInfo = value;
					OnPropertyChanged();
				}
			}
		}
		
		
    }
}
