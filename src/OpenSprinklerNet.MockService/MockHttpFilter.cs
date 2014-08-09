using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace OpenSprinklerNet.MockService
{
	public class MockHttpFilter : IHttpFilter
	{
		private HttpResponseMessage m_response = new HttpResponseMessage();

		public MockHttpFilter(HttpResponseMessage response)
		{
			m_response = response;
		}
		public MockHttpFilter(IHttpContent content, HttpStatusCode code = HttpStatusCode.Ok)
		{
			m_response = new HttpResponseMessage(code) { Content = content };
		}
		public MockHttpFilter(string response, HttpStatusCode code = HttpStatusCode.Ok)
		{
			m_response = new HttpResponseMessage(code)
			{
				Content = new HttpStringContent(response)
			};
		}
		public virtual Windows.Foundation.IAsyncOperationWithProgress<Windows.Web.Http.HttpResponseMessage, Windows.Web.Http.HttpProgress> SendRequestAsync(Windows.Web.Http.HttpRequestMessage request)
		{
			return Operation<HttpResponseMessage>.FromResult(m_response);
		}

		public void Dispose()
		{
			//
		}

		public class Operation<T> : Windows.Foundation.IAsyncOperationWithProgress<T, HttpProgress>
		{
			private T m_result;
			private Operation(T result)
			{
				m_result = result;
			}
			public static Operation<T> FromResult(T result)
			{
				return new Operation<T>(result);
			}

			private Windows.Foundation.AsyncOperationWithProgressCompletedHandler<T, HttpProgress> m_Completed;
			public Windows.Foundation.AsyncOperationWithProgressCompletedHandler<T, HttpProgress> Completed
			{
				get { return m_Completed; 
				}
				set {
					m_Completed = value;
					m_Completed.Invoke(this, Windows.Foundation.AsyncStatus.Completed);
				}
			}

			public T GetResults()
			{
				return m_result;
			}

			private Windows.Foundation.AsyncOperationProgressHandler<T, HttpProgress> m_Progress;
			public Windows.Foundation.AsyncOperationProgressHandler<T, HttpProgress> Progress
			{
				get
				{
					return m_Progress;
				}
				set
				{
					m_Progress = value;
					m_Progress.Invoke(this, new HttpProgress() { });
				}
			}

			public void Cancel()
			{
				throw new NotImplementedException();
			}

			public void Close()
			{
				throw new NotImplementedException();
			}

			public Exception ErrorCode
			{
				get { throw new NotImplementedException(); }
			}

			public uint Id
			{
				get { throw new NotImplementedException(); }
			}

			public Windows.Foundation.AsyncStatus Status
			{
				get { return Windows.Foundation.AsyncStatus.Completed; }
			}
		}
	}
}
