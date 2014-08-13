using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenSprinklerNet.MockService
{
	[DataContract]
	internal class MockControllerInfo : ControllerInfo
	{
		public void SetTimeZone(double zone)
		{
			base.TimeZone = zone;
		}
		public void SetSequentialMode(Status status)
		{
			base.SequentialMode = status;
		}
	}
}
