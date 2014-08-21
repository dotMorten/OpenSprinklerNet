using OpenSprinklerNet;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace OpenSprinklerApp.ViewModels
{
	[Windows.UI.Xaml.Data.Bindable]
	public class MainPageVM : ViewModelBase
	{
		public class Station
		{
			public int ID { get; set; }
			public string Name { get; set; }
			public Status Status { get; set; }
			public bool IsOn { get { return Status == OpenSprinklerNet.Status.On; } }
		}

		public MainPageVM()
		{
			LoadControllerInfo();
			LoadStatuses();
		}

		private async void LoadControllerInfo()
		{
			//Open connection
			var conn = AppModel.Current.Connection;
			//Get controller metadata
			ControllerInfo = await conn.GetControllerInfoAsync();
		}
		private async void LoadStatuses()
		{
			//Open connection
			var conn = AppModel.Current.Connection;
			//Get station info metadata
			var statusesT = conn.QueryStationStatusesAsync();
			var stationInfoT = conn.GetStationsAsync();
			await System.Threading.Tasks.Task.WhenAll(new Task[] { statusesT, stationInfoT }); //run both simultanously
			var statuses = statusesT.Result;
			var stationInfo = stationInfoT.Result;
			//join data
			List<Station> stations = new List<Station>(statuses.Statuses.Length);
			for (int i = 0; i < statuses.Statuses.Length; i++)
			{
				stations.Add(new Station()
				{
					ID = i + 1,
					Status = statuses.Statuses[i],
					Name = stationInfo.StationNames.Length > i ? stationInfo.StationNames[i] : i.ToString()
				});
			}
			Stations = stations.ToArray();
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


		private Station[] m_Stations;

		public Station[] Stations
		{
			get { return m_Stations; }
			set
			{
				if (m_Stations != value)
				{
					m_Stations = value;
					OnPropertyChanged();
				}
			}
		}
    }
}
