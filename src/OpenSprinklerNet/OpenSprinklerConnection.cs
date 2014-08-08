using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace OpenSprinklerNet
{

	/*
	GET /jo?_=1407434691298 HTTP/1.1
	{"fwv":207,"tz":16,"ntp":0,"dhcp":1,"ip1":192,"ip2":168,"ip3":1,"ip4":22,"gw1":192,"gw2":168,"gw3":1,"gw4":1,"hp0":80,"hp1":0,"ar":0,"ext":0,"seq":0,"sdt":0,"mas":0,"mton":0,"mtof":0,"urs":0,"rso":0,"wl":100,"stt":10,"ipas":0,"devid":0,"con":110,"lit":100,"dim":5,"ntp1":204,"ntp2":9,"ntp3":54,"ntp4":119,"reset":0}

	/jp
	{"nprogs":5,"nboards":1,"mnp":53,"pd":[[1,255,1,270,360,1439,600,8],[1,255,1,270,480,1439,600,16],[1,255,1,270,360,1439,480,32],[1,255,1,270,480,1439,480,64],[1,255,1,270,600,1439,720,128]]}


	/jc
	{"devt":1407409574,"nbrd":1,"en":1,"rd":0,"rs":1,"mm":0,"rdst":0,"loc":"Redlands,CA","sbits":[0,0],"ps":[[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0]],"lrun":[2,99,2,1407408388]}

	 */

	public class OpenSprinklerConnection
    {
		private string m_hostname;
		private string m_password;
		private OpenSprinklerConnection(string hostname, string password)
		{
			m_hostname = hostname;
			m_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
		}
		private string GetPassword()
		{
			var bytes = Convert.FromBase64String(m_password);
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}
		public static async Task<OpenSprinklerConnection> OpenAsync(string hostname, string password = null)
		{
			OpenSprinklerConnection conn = new OpenSprinklerConnection(hostname, password);
			await conn.GetControllerInfoAsync().ConfigureAwait(false); //Test connection
			return conn;
		}
		public Task<ProgramDetailsInfo> GetProgamDetailsAsync()
		{
			return GetJsonAsync<ProgramDetailsInfo>("jp");
		}

		public Task<ControllerInfo> GetControllerInfoAsync()
		{
			return GetJsonAsync<ControllerInfo>("jo");
		}
		public Task<ControllerSettingsInfo> GetControllerSettingsAsync()
		{
			return GetJsonAsync<ControllerSettingsInfo>("jc");
		}

		public Task<StationStatusInfo> QueryStationStatusesAsync()
		{
			return GetJsonAsync<StationStatusInfo>("js");
		}

		public async Task<bool> QueryStationStatusAsync(int stationID)
		{
			if (stationID < 1)
				throw new ArgumentOutOfRangeException("stationID", "Station ID must be 1 or greater");
			HttpClient client = new HttpClient();
			Uri uri = new Uri(string.Format("{0}/sn{1}", m_hostname, stationID));
			var result = await client.GetAsync(uri);
			var str = await result.Content.ReadAsStringAsync();
			if (str == "0")
				return false;
			else if (str == "1")
				return true;
			else 
				throw new Exception("Invalid response");
		}
		public async Task SetStationStatusAsync(int stationID, Status status)
		{
			if (stationID < 1)
				throw new ArgumentOutOfRangeException("stationID", "Station ID must be 1 or greater");
			HttpClient client = new HttpClient();
			Uri uri = new Uri(string.Format("{0}/sn{1}={2}&pw={3}", m_hostname, stationID, (int)status, GetPassword()));
			var result = await client.GetAsync(uri);
			var str = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync();
		}

		public Task<StationInfo> GetStationsAsync()
		{
			return GetJsonAsync<StationInfo>("jn");
		}

		private static string AppendParameter(string url, string parameter, string value = null)
		{
			if (url.Contains("?"))
				url += "&";
			else
				url += "?";
			url += parameter;
			if (value != null)
				url += "=" + value;
			return url;
		}

		private async Task<T> GetJsonAsync<T>(string query, bool includePwd = false)
		{
			HttpClient client = new HttpClient();
			string url = m_hostname + "/" + query;
			if(includePwd) {
				url = AppendParameter(url, "pw", GetPassword());
			}
			url = AppendParameter(url, "_t", DateTime.Now.Ticks.ToString());
			Uri uri = new Uri(url);
			//if(m_password != null)
			//{
			//	client.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Basic", m_password);
			//}
			var result = await client.GetAsync(uri).AsTask().ConfigureAwait(false);
			var str = await result.EnsureSuccessStatusCode().Content.ReadAsStringAsync().AsTask().ConfigureAwait(false);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			T status = (T)ser.ReadObject((await result.Content.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false)).AsStreamForRead());
			return status;
		}
	}
}
