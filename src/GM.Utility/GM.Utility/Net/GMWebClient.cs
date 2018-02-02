using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GM.Utility.Net
{
	/// <summary>
	/// A version of <see cref="WebClient"/> with added functionalities.
	/// <para>Added support for request timeout. Default timeout is 10000 ms.</para>
	/// </summary>
	public class GMWebClient:IDisposable
    {
		/// <summary>
		/// Default timeout.
		/// </summary>
		public const int DEFAULT_TIMEOUT = 10000;

		private readonly GMWebClientInternal webClient;

		/// <summary>
		/// Creates a new instance of <see cref="GMWebClient"/> with default timeout.
		/// </summary>
		public GMWebClient() : this(DEFAULT_TIMEOUT) { }

		/// <summary>
		/// Creates a new instance of <see cref="GMWebClient"/> with the specified timeout.
		/// </summary>
		/// <param name="timeout">The length of time, in milliseconds, before the request times out.</param>
		public GMWebClient(int timeout)
		{
			webClient = new GMWebClientInternal();
			Timeout = timeout;
		}

		/// <summary>
		/// Releases all resources used by this web client.
		/// </summary>
		public void Dispose()
		{
			webClient?.Dispose();
		}

		/// <summary>
		/// Gets or sets the length of time, in milliseconds, before the request times out.
		/// </summary>
		public int Timeout
		{
			get => webClient.Timeout;
			set => webClient.Timeout = value;
		}

		/// <summary>
		/// Uploads the specified name/value collection to the resource identified by the specified URI.
		/// </summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The NameValueCollection to send to the resource.</param>
		/// <param name="method">The http method used to send data to the resource. Allowed methods are POST and GET.</param>
		public string UploadValues(string address, NameValueCollection data, HttpMethod method)
		{
			switch(method) {
				case HttpMethod.GET:
					return UploadValuesGet(address, data);
				case HttpMethod.POST:
					return UploadValuesPost(address, data);
			}
			throw new NotImplementedException("Unsupported http method.");
		}

		/// <summary>
		/// Asynchronously uploads the specified name/value collection to the resource identified by the specified URI.
		/// </summary>
		/// <param name="address">The URI of the resource to receive the collection.</param>
		/// <param name="data">The NameValueCollection to send to the resource.</param>
		/// <param name="method">The http method used to send data to the resource. Allowed methods are POST and GET.</param>
		public Task<string> UploadValuesAsyncTask(string address, NameValueCollection data, HttpMethod method)
		{
			switch(method) {
				case HttpMethod.GET:
					return UploadValuesGetAsyncTask(address, data);
				case HttpMethod.POST:
					return UploadValuesPostAsyncTask(address, data);
			}
			throw new NotImplementedException("Unsupported http method.");
		}

		private string UploadValuesGet(string address, NameValueCollection data)
		{
			AddGetParameters(ref address, data);
			return webClient.DownloadString(address);
		}

		private Task<string> UploadValuesGetAsyncTask(string address, NameValueCollection data)
		{
			AddGetParameters(ref address, data);
			return webClient.DownloadStringTaskAsync(address);
		}

		private void AddGetParameters(ref string address, NameValueCollection data)
		{
			if(data.Count == 0) {
				return;
			}

			List<string> collection = new List<string>(data.Count);
			foreach(string name in data.AllKeys) {
				string value = data.Get(name);
				collection.Add($"{name}={value}");
			}
			address += "?" + string.Join("&", collection);
		}

		private string UploadValuesPost(string address, NameValueCollection data)
		{
			byte[] byteArray = webClient.UploadValues(address, data);
			return BytesToString(byteArray);
		}

		private async Task<string> UploadValuesPostAsyncTask(string address, NameValueCollection data)
		{
			byte[] byteArray = await webClient.UploadValuesTaskAsync(address, data);
			return BytesToString(byteArray);
		}

		private string BytesToString(byte[] bytes)
		{
			return Encoding.UTF8.GetString(bytes);
		}

		/// <summary>
		/// Solution for timeout came from here: http://w3ka.blogspot.si/2009/12/how-to-fix-webclient-timeout-issue.html
		/// </summary>
		private class GMWebClientInternal : WebClient
		{
			/// <summary>
			/// Gets or sets the length of time, in milliseconds, before the request times out.
			/// </summary>
			public int Timeout { get; set; }

			/// <summary>
			/// Overriden just for added functionality of timeout.
			/// </summary>
			protected override WebRequest GetWebRequest(Uri address)
			{
				WebRequest request = base.GetWebRequest(address);

				if(request != null) {
					request.Timeout = Timeout;
				}

				return request;
			}
		}
	}
}
