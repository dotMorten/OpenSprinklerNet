using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenSprinklerNet
{

	[DataContract]
	public class ControllerSettingsInfo
	{
		// Endpoint: /jc
		// {"devt":1407409574,"nbrd":1,"en":1,"rd":0,"rs":1,"mm":0,"rdst":0,
		// "loc":"Redlands,CA","sbits":[0,0],
		// "ps":[[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0]],
		// "lrun":[2,99,2,1407408388]}

		[DataMember(Name = "loc")]
		public string Location { get; private set; }

		[DataMember(Name = "nbrd")]
		public int NumberOfBoards { get; set; }
		[DataMember(Name = "mm")]
		public Status ManualMode { get; set; }
	}
}
