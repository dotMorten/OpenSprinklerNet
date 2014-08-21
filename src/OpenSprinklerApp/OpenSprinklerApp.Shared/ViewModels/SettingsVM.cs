using OpenSprinklerNet;
using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
	[Windows.UI.Xaml.Data.Bindable]
	public class SettingsVM : ViewModelBase
    {
		
		public SettingsVM()
		{
			LoadSettingsInfo();
		}
		private async void LoadSettingsInfo()
		{
			//Open connection
			var conn = AppModel.Current.Connection;
			//Get settings metadata
			SettingsInfo = await conn.GetControllerSettingsAsync();
		}

		private ControllerSettingsInfo m_SettingsInfo;

		public ControllerSettingsInfo SettingsInfo
		{
			get { return m_SettingsInfo; }
			private set
			{
				if (m_SettingsInfo != value)
				{
					m_SettingsInfo = value;
					OnPropertyChanged();
				}
			}
		}
		
		
    }
}
