/*
MIT License

Copyright (c) 2020 Gregor Mohorko

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.

Project: GM.Utility
Created: 2020-10-30
Author: Gregor Mohorko
*/

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GM.Utility.Net
{
	/// <summary>
	/// Uses <see cref="HttpClient"/> internally.
	/// </summary>
	public class GMHttpClient : IDisposable
	{
		private readonly HttpClient httpClient;

		/// <summary>
		/// Creates a new instance of <see cref="GMHttpClient"/> with the specified timeout.
		/// </summary>
		/// <param name="timeout">The timespan to wait before the request times out.</param>
		public GMHttpClient(TimeSpan timeout)
		{
			httpClient.Timeout = timeout;
		}

		/// <summary>
		/// Creates a new instance of <see cref="GMHttpClient"/>.
		/// </summary>
		public GMHttpClient()
		{
			httpClient = new HttpClient();
		}

		/// <summary>
		/// Releases all resources used by this http client.
		/// </summary>
		public void Dispose()
		{
			httpClient?.Dispose();
		}

		/// <summary>
		/// Gets or sets the timespan to wait before the request times out.
		/// </summary>
		public TimeSpan Timeout
		{
			get => httpClient.Timeout;
			set => httpClient.Timeout = value;
		}

		/// <summary>
		/// Sends an HTTP request as an asynchronous operation with the specified name/value collection.
		/// </summary>
		/// <param name="address">A string that represents the request <see cref="Uri"/>.</param>
		/// <param name="nameValueCollection">A collection of name/value pairs.</param>
		/// <param name="method">The HTTP method.</param>
		/// <param name="cancellationToken">The cancellation token to cancel operation.</param>
		/// <exception cref="HttpRequestException" />
		public async Task<string> UploadValuesAsync(string address, IEnumerable<KeyValuePair<string, string>> nameValueCollection, System.Net.Http.HttpMethod method, CancellationToken cancellationToken)
		{
			using(var request = new HttpRequestMessage(method, address)) {
				request.Content = new FormUrlEncodedContent(nameValueCollection);

				HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, cancellationToken);
				try {
					if(response.Content == null) {
						return null;
					}
					return await response.Content.ReadAsStringAsync();
				} finally {
					response?.Dispose();
				}
			}
		}
	}
}
