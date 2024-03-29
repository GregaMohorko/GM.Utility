﻿/*
MIT License

Copyright (c) 2023 Gregor Mohorko

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
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GM.Utility.Net;

/// <summary>
/// Uses <see cref="HttpClient"/> internally.
/// </summary>
public class GMHttpClient : IDisposable
{
	private readonly HttpClient _httpClient;
	private readonly bool _disposeHttpClient;

	/// <summary>
	/// Creates a new instance of <see cref="GMHttpClient"/>.
	/// <param name="httpClient">The http client to use. If null, creates a new one.</param>
	/// <param name="disposeHttpClient">Determines whether to dispose the http client when this instance is disposed. Is automatically set to true if <paramref name="httpClient"/> is null.</param>
	/// </summary>
	public GMHttpClient(HttpClient httpClient = null, bool disposeHttpClient = true)
	{
		_httpClient = httpClient ?? new HttpClient();
		_disposeHttpClient = disposeHttpClient || httpClient == null;
	}

	/// <summary>
	/// Releases all resources used by this http client.
	/// </summary>
	public void Dispose()
	{
		if(_disposeHttpClient) {
			_httpClient?.Dispose();
		}
	}

	/// <summary>
	/// Gets or sets the timespan to wait before the request times out.
	/// </summary>
	public TimeSpan Timeout
	{
		get => _httpClient.Timeout;
		set
		{
			if(_disposeHttpClient == false) {
				throw new InvalidOperationException("Since http client will not be disposed, this class assumes it will be needed later and thus modifying the timeout is not possible. Modify the timeout directly on the http client that was provided to create this instance.");
			}
			_httpClient.Timeout = value;
		}
	}

	/// <summary>
	/// Obsolete. Please use SendGet or Send.
	/// </summary>
	// FIXME obsolete 2023-12-22
	[Obsolete("Please use SendGet or Send.", error: true)]
	public Task<string> UploadValuesAsync(string address, IEnumerable<KeyValuePair<string, string>> nameValueCollection, System.Net.Http.HttpMethod method, CancellationToken cancellationToken)
	{
		throw new NotImplementedException();
	}

	/// <summary>
	/// Send a GET HTTP request.
	/// </summary>
	/// <param name="requestUri">A string that represents the base request <see cref="Uri"/> (without the query).</param>
	/// <param name="nameValueCollection">A list of query values.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>Serialized response HTTP content.</returns>
	public async Task<string> SendGet(string requestUri, IEnumerable<KeyValuePair<string, string>> nameValueCollection, CancellationToken ct)
	{
		// the constructor of FormUrlEncodedContent has a problem with large amount of data (>2000 characters)
		//request.Content = new FormUrlEncodedContent(nameValueCollection);

		// therefore, let's do it ourselves:
		// (idea from: https://stackoverflow.com/a/51854330/6277755)
		var encodedItems = nameValueCollection
			.Select(x => $"{WebUtility.UrlEncode(x.Key)}={WebUtility.UrlEncode(x.Value)}");

		string requestAndQueryUri = requestUri + "?" + string.Join("&", encodedItems);

		return await Send(requestAndQueryUri, HttpMethod.Get, null, ct);
	}

	/// <summary>
	/// Send an HTTP request.
	/// </summary>
	/// <param name="requestUri">A string that represents the request <see cref="Uri"/>.</param>
	/// <param name="httpMethod">The HTTP method</param>
	/// <param name="httpContent">The contents of the HTTP message. Should be NULL for GET.</param>
	/// <param name="ct">Cancellation token.</param>
	/// <returns>Serialized response HTTP content.</returns>
	public async Task<string> Send(string requestUri, HttpMethod httpMethod, HttpContent httpContent, CancellationToken ct)
	{
		using(var request = new HttpRequestMessage(httpMethod, requestUri)) {
			if(httpContent != null) {
				request.Content = httpContent;
			}

			HttpResponseMessage response = await _httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead, ct);
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
