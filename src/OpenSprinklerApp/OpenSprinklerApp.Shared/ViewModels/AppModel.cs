using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
    public class AppModel : ViewModelBase
    {
		private AppModel()
		{
#if !DEBUG
			if (IsDesignTime)
			{
#endif
				m_Connection = new OpenSprinklerNet.OpenSprinklerConnection("http://1.1.1.1", "opendoor", new OpenSprinklerNet.MockService.MockOpenSprinklerService("1.1.1.1", "opendoor"));
#if !DEBUG
			}
			else
			{
				m_Connection = new OpenSprinklerNet.OpenSprinklerConnection("http://192.168.1.5:80", "opendoor", new Windows.Web.Http.Filters.HttpBaseProtocolFilter());
			}
#endif
		}

		protected bool IsDesignTime
		{
			get
			{
				return Windows.ApplicationModel.DesignMode.DesignModeEnabled;
			}
		}

		private static AppModel s_Current;

		public static AppModel Current
		{
			get
			{
				if (s_Current == null)
					s_Current = new AppModel();
				return s_Current;
			}
		}

		private OpenSprinklerNet.OpenSprinklerConnection m_Connection;

		public OpenSprinklerNet.OpenSprinklerConnection Connection
		{
			get { return m_Connection; }
			set
			{
				if (m_Connection != value)
				{
					m_Connection = value;
					OnPropertyChanged();
				}
			}
		}		
    }
}
