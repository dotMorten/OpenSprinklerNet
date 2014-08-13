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

using System;
using System.Runtime.Serialization;

namespace OpenSprinklerNet
{
	[DataContract]

	public class ControllerInfo
	{
		// Endpoint: jo
		//{"fwv":207,"tz":16,"ntp":0,"dhcp":1,"ip1":192,"ip2":168,"ip3":1,"ip4":22,
		//"gw1":192,"gw2":168,"gw3":1,"gw4":1,"hp0":80,"hp1":0,"ar":0,"ext":0,"seq":0,
		//"sdt":0,"mas":0,"mton":0,"mtof":0,"urs":0,"rso":0,"wl":100,"stt":10,"ipas":0,
		//"devid":0,"con":110,"lit":100,"dim":5,"ntp1":204,"ntp2":9,"ntp3":54,"ntp4":119,"reset":0}

		/// <summary>
		/// Firmware version
		/// </summary>
		[DataMember(Name = "fwv")]
		public int FirmwareVersion { get; protected set; }

		/// <summary>
		/// number of extension boards (up to 3) 
		/// </summary>
		[DataMember(Name = "ext")]
		public int NumberOfExtensionBoards { get; protected set; }
		/// <summary>
		/// Use DHCP
		/// </summary>
		[DataMember(Name = "dhcp")]
		public Status Dhcp { get; private set; }

		[DataMember(Name = "ip1")]
		private int _ip1 { get; set; }
		[DataMember(Name = "ip2")]
		private int _ip2 { get; set; }
		[DataMember(Name = "ip3")]
		private int _ip3 { get; set; }
		[DataMember(Name = "ip4")]
		private int _ip4 { get; set; }
		public Windows.Networking.HostName Endpoint
		{
			get
			{
				return new Windows.Networking.HostName(string.Format("{0}.{1}.{2}.{3}", _ip1, _ip2, _ip3, _ip4));
			}
		}
		[DataMember(Name = "hp0")]
		public int HostPort { get; protected set; }

		[DataMember(Name = "ntp1")]
		private int _Ntp1 { get; set; }
		[DataMember(Name = "ntp2")]
		private int _Ntp2 { get; set; }
		[DataMember(Name = "ntp3")]
		private int _Ntp3 { get; set; }
		[DataMember(Name = "ntp4")]
		private int _Ntp4 { get; set; }
		public Windows.Networking.HostName NtpEndpoint
		{
			get
			{
				return new Windows.Networking.HostName(string.Format("{0}.{1}.{2}.{3}", _Ntp1, _Ntp2, _Ntp3, _Ntp4));
			}
		}


		//time zone: (floating point time zone value + 12) x 4. e.g. GMT-5:00 will be (-5+12) *4=28, GMT+8:15 will be (8 ¼+12)*4 = 81 
		[DataMember(Name = "tz")]
		private double _TimeZoneInternal { get; set; }
		public double TimeZone
		{
			get { return _TimeZoneInternal / 4 - 12; }
			protected set
			{
				if (value < -12 || value > 12)
					throw new ArgumentOutOfRangeException();
				_TimeZoneInternal = 4 * value + 48;
			}
		}

		/// <summary>
		/// Whether to user network time sync
		/// </summary>
		[DataMember(Name = "ntp")]
		public Status NtpSync { get; protected set; }

		/// <summary>
		/// Station delay time (in seconds). must be between 0 and 240.
		/// e.g. 15 means a 15 seconds delay between two consecutive stations. 
		/// </summary>
		[DataMember(Name = "sdt")]
		public int StationDelayTime { get; protected set; }
		/// <summary>
		/// Master station index (0 means no master station, 1 means station 1 is the master station, and so on) 
		/// </summary>
		[DataMember(Name = "mas")]
		public int MasterStationIndex { get; protected set; }
		/// <summary>
		/// master on delay time (in seconds). Must be between 0 and 60. 
		/// e.g. 5 means the master station will turn on 5 seconds after a station opens. 
		
		[DataMember(Name = "mton")]
		public int MasterOnDelayTime { get; protected set; }
		/// <summary>
		///  master off delay time (in seconds). Must be between -60 and +60. 
		///  e.g. -5 means the master station will turn off 5 seconds before a station closes. 
		/// </summary>
		[DataMember(Name = "mtof")]
		public int MasterOffDelayTime { get; protected set; }
		/// <summary>
		/// Use rain sensor 
		/// </summary>
		[DataMember(Name = "urs")]
		public Status UseRainSensor { get; protected set; }
		/// <summary>
		/// Rain sensor type
		/// </summary>
		[DataMember(Name = "rso")]
		public bool RainSensorNormallyOpen { get; protected set; }
		/// <summary>
		/// Water level in percentage (from 0 to 250). e.g. 150 means water time will increase to 150%. 
		/// </summary>
		[DataMember(Name = "wl")]
		public int WaterLevel { get; protected set; }
		/// <summary>
		/// Ignore password
		/// </summary>
		[DataMember(Name = "ipos")]
		public Status IgnorePassword { get; protected set; }
		/// <summary>
		/// Use sequential mode
		/// </summary>
		[DataMember(Name = "seq")]
		public Status SequentialMode { get; protected set; }
	}
}
