using System;
using System.Collections.Generic;
using System.Text;

namespace OpenSprinklerApp.ViewModels
{
    public class AppModel : ViewModelBase
    {
		private AppModel()
		{
#if DEBUG
			m_Connection = new OpenSprinklerNet.OpenSprinklerConnection("http://1.1.1.1", "opendoor", new OpenSprinklerNet.MockService.MockOpenSprinklerService("1.1.1.1", "opendoor"));
#else
			m_Connection = new OpenSprinklerNet.OpenSprinklerConnection("http://192.168.1.15:80", "opendoor");
#endif
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
