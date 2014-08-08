using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenSprinklerNet
{
	[DataContract]
	public class StationInfo
	{
		//	Endpoint: /jn
		//	{"snames":["Raised beds","S02","S03","Backyard lawn E","Backyard lawn W","Frontyard lawn","Sidewalk lawn","Flower beds"],"masop":[255],"ignore_rain":[128],"maxlen":16}

		/// <summary>
		/// Name of stations
		/// </summary>
		[DataMember(Name = "snames")]
		public string[] StationNames { get; private set; }
		
		/// <summary>
		/// Maximum name length
		/// </summary>
		[DataMember(Name = "maxlen")]
		public int MaxLength { get; private set; }
		
		/// <summary>
		/// Ignore rainsensor
		/// </summary>
		[DataMember(Name = "ignore_rain")]
		public int[] IgnoreRain { get; private set; }
		
		[DataMember(Name = "masop")]
		public int[] MasOp { get; private set; } //???
	}
}
