﻿//
// Copyright (c) 2012 Morten Nielsen
//
// Licensed under the Microsoft Public License (Ms-PL) (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//    http://opensource.org/licenses/Ms-PL.html
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//

using System.Runtime.Serialization;

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
