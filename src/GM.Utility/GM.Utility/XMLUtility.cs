using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for XML.
	/// </summary>
	public static class XMLUtility
	{
		/// <summary>
		/// Loads an XElement from the web. Supports HTTP/S and S/FTP.
		/// </summary>
		/// <param name="address">The address from which to download the XML.</param>
		/// <param name="credentials">The network credentials that are sent to the host and used to authenticate the request.</param>
		public static XElement LoadFromWeb(string address, ICredentials credentials=null)
		{
			if(credentials == null) {
				return XElement.Load(address);
			}

			using(var webClient = new WebClient()) {
				webClient.Credentials = credentials;
				string xmlContent = webClient.DownloadString(address);
				return XElement.Parse(xmlContent);
			}
		}
	}
}
