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

		[DataMember(Name = "devt")]
		private long _DateInternal { get; set; }

		public DateTime Date
		{
			get
			{
				return new DateTime(1970, 1, 1).AddSeconds(_DateInternal);
			}
		}
	}
}
