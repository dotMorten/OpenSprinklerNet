using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;

namespace OpenSprinklerNet.MockService
{
	public class MockOpenSprinklerService : MockHttpFilter
	{
		private MockControllerInfo controllerInfo;
		private ProgramDetailsInfo programs;
		private string m_hostname;
		private string m_password;
		public MockOpenSprinklerService(string hostname, string password)
			: base(null)
		{
			m_hostname = hostname;
			m_password = password;
			controllerInfo = CreateFromJson<MockControllerInfo>("{\"fwv\":207,\"tz\":16,\"ntp\":0,\"dhcp\":1,\"ip1\":192,\"ip2\":168,\"ip3\":1,\"ip4\":22,\"gw1\":192,\"gw2\":168,\"gw3\":1,\"gw4\":1,\"hp0\":80,\"hp1\":0,\"ar\":0,\"ext\":0,\"seq\":1,\"sdt\":0,\"mas\":0,\"mton\":0,\"mtof\":0,\"urs\":0,\"rso\":0,\"wl\":100,\"stt\":10,\"ipas\":0,\"devid\":0,\"con\":110,\"lit\":100,\"dim\":5,\"ntp1\":204,\"ntp2\":9,\"ntp3\":54,\"ntp4\":119,\"reset\":0}");
			controllerInfo.SetTimeZone(5);
			programs = CreateFromJson<ProgramDetailsInfo>("{\"nprogs\":5,\"nboards\":1,\"mnp\":53,\"pd\":[[1,255,1,270,360,1439,600,8],[1,255,1,270,480,1439,600,16],[1,255,1,270,360,1439,480,32],[1,255,1,270,480,1439,480,64],[1,127,1,270,600,1439,720,128]]}");
			var progs = programs.Programs as IList<Program>;
			progs[2].Duration = TimeSpan.FromMinutes(30);
		}

		private static T CreateFromJson<T>(string str)
		{
			System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(typeof(T));
			return (T)s.ReadObject(new MemoryStream(Encoding.UTF8.GetBytes(str)));
		}

		public override Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, Windows.Web.Http.HttpProgress> SendRequestAsync(Windows.Web.Http.HttpRequestMessage request)
		{
			if (request.RequestUri.Host != m_hostname)
				return Get404();

			string[] segments = request.RequestUri.Segments;
			if (segments.Length < 2)
				return Get404();

			switch(segments[1])
			{
				case "jo":
					return GetJo(request);
				case "jc":
					return GetJc(request);
				case "jp":
					return GetJp(request);
				default:
					return Get404();
			}
		}

		private Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> GetJc(HttpRequestMessage request)
		{
			// {"devt":1407409574,"nbrd":1,"en":1,"rd":0,"rs":1,"mm":0,"rdst":0,
			// "loc":"Redlands,CA","sbits":[0,0],
			// "ps":[[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0]],
			// "lrun":[2,99,2,1407408388]}
			return ResponseFromString("{\"devt\":1407409574,\"nbrd\":1,\"en\":1,\"rd\":0,\"rs\":1,\"mm\":0,\"rdst\":0,\"loc\":\"Redlands,CA\",\"sbits\":[0,0],\"ps\":[[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0],[0,0]],\"lrun\":[2,99,2,1407408388]}");
		}

		private Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> GetJo(HttpRequestMessage request)
		{
			return ResponseFromObject(controllerInfo);
		}
		private Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> GetJp(HttpRequestMessage request)
		{
			return ResponseFromObject(programs);
		}

		private static Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> Get404()
		{
			return Operation<HttpResponseMessage>.FromResult(new HttpResponseMessage(HttpStatusCode.NotFound)
			{
				Content = new HttpStringContent("NOT FOUND")
			});
		}
		private static Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> GetAccessDenied()
		{
			return Operation<HttpResponseMessage>.FromResult(new HttpResponseMessage(HttpStatusCode.Unauthorized)
			{
				Content = new HttpStringContent("UNAUTHORIZED")
			});
		}

		private Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> ResponseFromObject(object response)
		{
			System.Runtime.Serialization.Json.DataContractJsonSerializer s = new System.Runtime.Serialization.Json.DataContractJsonSerializer(response.GetType());
			System.IO.MemoryStream ms = new System.IO.MemoryStream();
			s.WriteObject(ms, response);
			ms.Seek(0, SeekOrigin.Begin);
			return Operation<HttpResponseMessage>.FromResult(new HttpResponseMessage()
			{
				Content = new HttpStreamContent(ms.AsInputStream())
			});
		}


		private Windows.Foundation.IAsyncOperationWithProgress<HttpResponseMessage, HttpProgress> ResponseFromString(string response)
		{
			return Operation<HttpResponseMessage>.FromResult(new HttpResponseMessage()
			{
				 Content = new HttpStringContent(response)
			});
		}
	}
}
