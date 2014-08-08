﻿//
// Copyright (c) 2014 Morten Nielsen
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
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace OpenSprinklerNet
{
	public class OpenSprinklerConnection
    {
		private string m_hostname;
		private string m_password;

		private OpenSprinklerConnection(string hostname, string password)
		{
			m_hostname = hostname;
			m_password = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
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
			var response = await GetHttpContentAsync("sn" + stationID.ToString());
			var str = await response.Content.ReadAsStringAsync();
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
			await GetHttpContentAsync(string.Format("{0}/sn{1}={2}", m_hostname, stationID, (int)status), true);
		}

		public Task<StationInfo> GetStationsAsync()
		{
			return GetJsonAsync<StationInfo>("jn");
		}

		#region Http Request

		private async Task<T> GetJsonAsync<T>(string query, bool includePwd = false)
		{
			var response = await GetHttpContentAsync(query, includePwd);
			var str = await response.Content.ReadAsStringAsync().AsTask().ConfigureAwait(false);
			DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));
			T status = (T)ser.ReadObject((await response.Content.ReadAsInputStreamAsync().AsTask().ConfigureAwait(false)).AsStreamForRead());
			return status;
		}

		private async Task<HttpResponseMessage> GetHttpContentAsync(string query, bool requiresAuth = false)
		{
			HttpClient client = new HttpClient();
			string url = m_hostname + "/" + query;
			if (requiresAuth)
			{
				url = AppendParameter(url, "pw", GetPassword());
				//if(m_password != null)
				//{
				//	client.DefaultRequestHeaders.Authorization = new Windows.Web.Http.Headers.HttpCredentialsHeaderValue("Basic", m_password);
				//}
			}
			url = AppendParameter(url, "_t", DateTime.Now.Ticks.ToString());
			Uri uri = new Uri(url);
			var result = await client.GetAsync(uri).AsTask().ConfigureAwait(false);
			return result.EnsureSuccessStatusCode();
		}

		private static string AppendParameter(string url, string parameter, string value = null)
		{
			if (url.Contains("?"))
			{
				if (!url.EndsWith("&") && !url.EndsWith("?"))
					url += "&";
			}
			else
			{
				url += "?";
			}
			url += parameter;
			if (value != null)
				url += "=" + Uri.EscapeDataString(value);
			return url;
		}

		private string GetPassword()
		{
			var bytes = Convert.FromBase64String(m_password);
			return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
		}

		#endregion
	}
}
