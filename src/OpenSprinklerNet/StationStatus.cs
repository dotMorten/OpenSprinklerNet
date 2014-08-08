using System.Runtime.Serialization;

namespace OpenSprinklerNet
{
	[DataContract]
	public sealed class StationStatusInfo
	{		
		// Endpoint: /js
		//{"sn":[0,0,0,1,0,1,0,0],"nstations":8}

		[DataMember(Name = "nstations")]
		public int StationCount { get; set; }
		[DataMember(Name = "sn")]
		public Status[] Statuses { get; set; }
	}
}
