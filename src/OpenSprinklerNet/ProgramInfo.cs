using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OpenSprinklerNet
{
	[DataContract]
	public class ProgramDetailsInfo
	{
		// /jp
		// {"nprogs":5,"nboards":1,"mnp":53,
		//  "pd":[
		//		[1,255,1,270,360,1439,600,8],
		//		[1,255,1,270,480,1439,600,16],
		//		[1,255,1,270,360,1439,480,32],
		//		[1,255,1,270,480,1439,480,64],
		//		[1,255,1,270,600,1439,720,128]
		//	]}

		[OnDeserialized]
		private void OnDeserialized(StreamingContext context)
		{
			if (_Data != null)
			{
				List<Program> programs = new List<Program>();
				foreach (var item in _Data)
				{
					programs.Add(new Program(item));
				}
				Programs = programs.ToArray();
			}
		}

		[DataMember(Name = "nprogs")]
		public int Count { get; private set; }
		[DataMember(Name = "nboards")]
		public int BoardCoard { get; private set; }
		[DataMember(Name = "mnp")]
		public int mnp { get; set; } //???

		[DataMember(Name = "pd")]
		private int[][] _Data { get; set; }

		public Program[] Programs { get; private set; }
	}

	public class Program
	{
		byte[] m_data;
		internal Program(int[] data)
		{
			// 0: enabled/disabled
			// 1+2: days
			// 3: start (seconds)
			// 4: end (seconds)
			// 5: interval
			// 6: duration
			// 7: stations
			if (data[0] == 1)
				Enabled = true;
			else if (data[0] == 0)
				Enabled = false;
			else
				throw new ArgumentOutOfRangeException();
			Days = new List<DayOfWeek>();
			if (data[2] > 128)
			{
				//every n days
				EveryNDays = data[2] - 128;
			}
			else
			{
				if ((data[1] & 1) > 0) Days.Add(DayOfWeek.Monday);
				if ((data[1] & 2) > 0) Days.Add(DayOfWeek.Tuesday);
				if ((data[1] & 4) > 0) Days.Add(DayOfWeek.Wednesday);
				if ((data[1] & 8) > 0) Days.Add(DayOfWeek.Thursday);
				if ((data[1] & 16) > 0) Days.Add(DayOfWeek.Friday);
				if ((data[1] & 32) > 0) Days.Add(DayOfWeek.Saturday);
				if ((data[1] & 64) > 0) Days.Add(DayOfWeek.Sunday);

				if ((data[1] & 128) > 1) //skip days
				{
					if (data[2] == 1)
						SkipDays = OpenSprinklerNet.SkipDays.Odd;
					else if (data[2] == 0)
						SkipDays = OpenSprinklerNet.SkipDays.Even;
				}
			}
			Start = TimeSpan.FromSeconds(data[3]);
			End = TimeSpan.FromSeconds(data[4]);
			Interval = TimeSpan.FromSeconds(data[5]);
			Duration = TimeSpan.FromSeconds(data[6]);
			List<bool> stations = new List<bool>();
			for (int n = 7; n < data.Length; n++)
			{
				var bits = data[n];
				for (var s = 0; s < 8; s++)
				{
					{

						bool isOn = (bits & (1 << s)) > 0;
						stations.Add(isOn);
					}
				}
			}
			Stations = stations.ToArray();
		}
		public int? EveryNDays { get; set; } //between 2 and 128
		public bool Enabled { get; set; }
		public List<DayOfWeek> Days { get; set; }
		public TimeSpan Start { get; set; }
		public TimeSpan End { get; set; }
		public TimeSpan Interval { get; set; }
		public TimeSpan Duration { get; set; }
		public bool[] Stations { get; set; }
		public SkipDays SkipDays { get; set; }
	}

	public enum SkipDays
	{
		None,
		Odd,
		Even
	}

}
