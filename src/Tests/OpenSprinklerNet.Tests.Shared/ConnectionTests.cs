using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.UnitTestFramework;
using OpenSprinklerNet.MockService;

namespace OpenSprinklerNet.Tests
{
    [TestClass]
    public class ConnectionTests
    {
        [TestMethod]
        public async System.Threading.Tasks.Task TestConnection()
        {
			await OpenSprinklerConnection.OpenAsync("http://1.1.1.1", "pwd",
				new MockOpenSprinklerService("1.1.1.1", "pwd")
				);
        }
			

		[TestMethod]
		public async System.Threading.Tasks.Task TestControllerInfo()
		{
			var conn = new OpenSprinklerConnection("http://1.1.1.1", "pwd", new MockOpenSprinklerService("1.1.1.1", "pwd"));

			var info = await conn.GetControllerInfoAsync();
			Assert.AreEqual(Status.On, info.Dhcp);
			Assert.AreEqual(80, info.HostPort);
			Assert.AreEqual(-8, info.TimeZone);
			Assert.AreEqual(Status.Off, info.UseRainSensor);
			Assert.AreEqual(100, info.WaterLevel);
			Assert.AreEqual(0, info.StationDelayTime);
			Assert.AreEqual(207, info.FirmwareVersion);
			Assert.AreEqual(0, Windows.Networking.HostName.Compare("192.168.1.22", info.Endpoint.RawName));
		}

		[TestMethod]
		public async System.Threading.Tasks.Task TestControllerSettingsInfo()
		{
			var conn = new OpenSprinklerConnection("http://1.1.1.1", "pwd", new MockOpenSprinklerService("1.1.1.1", "pwd"));
			var info = await conn.GetControllerSettingsAsync();
			Assert.AreEqual(new DateTime(2014, 8, 7, 11, 6, 14), info.Date);
			Assert.AreEqual("Redlands,CA", info.Location);
			Assert.AreEqual(1, info.NumberOfBoards);
			Assert.AreEqual(Status.Off, info.ManualMode);
		}
	}
}
