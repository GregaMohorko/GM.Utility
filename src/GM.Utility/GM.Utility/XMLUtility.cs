/*
MIT License

Copyright (c) 2017 Grega Mohorko

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
Created: 2017-10-27
Author: Grega Mohorko
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Schema;

namespace GM.Utility
{
	/// <summary>
	/// Utilities for XML.
	/// </summary>
	public static class XMLUtility
	{
		/// <summary>
		/// Returns a collection of the child comment nodes of this element or document, in document order.
		/// </summary>
		/// <param name="xml">The xml from which to get the comment nodes.</param>
		public static IEnumerable<XComment> Comments(this XContainer xml)
		{
			return xml
				.Nodes()
				.Where(n => n.NodeType == XmlNodeType.Comment)
				.Cast<XComment>();
		}

		/// <summary>
		/// Returns a collection of the descendant comment nodes for this document or element, in document order.
		/// </summary>
		/// <param name="xml">The xml from which to get the descendant comment nodes.</param>
		public static IEnumerable<XComment> DescendantComments(this XContainer xml)
		{
			return xml
				.DescendantNodes()
				.Where(n => n.NodeType == XmlNodeType.Comment)
				.Cast<XComment>();
		}

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

		/// <summary>
		/// Removes all descendant comment nodes for this document or element.
		/// </summary>
		/// <param name="xml">The XML from which to remove all comment nodes.</param>
		public static void RemoveAllComments(this XContainer xml)
		{
			xml.DescendantComments().Remove();
		}

		/// <summary>
		/// Removes all information about namespaces in this and all descendant elements.
		/// </summary>
		/// <param name="xml">The XML from which to remove the namespace information.</param>
		public static void RemoveNamespaces(this XElement xml)
		{
			xml.DescendantsAndSelf().Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
			foreach(XElement element in xml.DescendantsAndSelf()) {
				element.Name = element.Name.LocalName;
			}
		}

		/// <summary>
		/// Validates the provided XML text according to XML Schema definition language (XSD) schemas and returns a list of any validation events (warnings or errors) that occured.
		/// <para>XML Schemas are associated with namespace URIs either by using the schemaLocation attribute or the provided Schemas property.</para>
		/// </summary>
		/// <param name="xml">The XML text to validate.</param>
		/// <param name="validationFlags">Set your schema validation settigns. Default is: <see cref="XmlSchemaValidationFlags.ProcessInlineSchema"/>, <see cref="XmlSchemaValidationFlags.ProcessSchemaLocation"/>, <see cref="XmlSchemaValidationFlags.ProcessIdentityConstraints"/> and <see cref="XmlSchemaValidationFlags.ReportValidationWarnings"/>.</param>
		public static List<ValidationEventArgs> ValidateSchema(string xml, XmlSchemaValidationFlags? validationFlags = null)
		{
			return ValidateSchema(xml, null, validationFlags);
		}

		/// <summary>
		/// Validates the provided XML text according to XML Schema definition language (XSD) schemas and returns a list of any validation events (warnings or errors) that occured.
		/// <para>XML Schemas are associated with namespace URIs either by using the schemaLocation attribute or the provided Schemas property.</para>
		/// </summary>
		/// <param name="xml">The XML text to validate.</param>
		/// <param name="additionalSchemas">A set of additional custom XML schemas to use.</param>
		/// <param name="validationFlags">Set your schema validation settigns. Default is: <see cref="XmlSchemaValidationFlags.ProcessInlineSchema"/>, <see cref="XmlSchemaValidationFlags.ProcessSchemaLocation"/>, <see cref="XmlSchemaValidationFlags.ProcessIdentityConstraints"/> and <see cref="XmlSchemaValidationFlags.ReportValidationWarnings"/>.</param>
		public static List<ValidationEventArgs> ValidateSchema(string xml, XmlSchemaSet additionalSchemas, XmlSchemaValidationFlags? validationFlags = null)
		{
			var result = new List<ValidationEventArgs>();

			// Set the validation settings.
			var settings = new XmlReaderSettings
			{
				ValidationType = ValidationType.Schema
			};

			// set validation flags
			if(validationFlags != null) {
				settings.ValidationFlags = validationFlags.Value;
			} else {
				settings.ValidationFlags = XmlSchemaValidationFlags.ProcessInlineSchema;
				settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessSchemaLocation;
				settings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
				settings.ValidationFlags |= XmlSchemaValidationFlags.ProcessIdentityConstraints;
			}

			if(additionalSchemas != null) {
				settings.Schemas = additionalSchemas;
			}

			settings.ValidationEventHandler += (object sender, ValidationEventArgs args) =>
			{
				result.Add(args);
			};

			using(MemoryStream stream = xml.ToStream()) {
				// Create the XmlReader object.
				using(XmlReader reader = XmlReader.Create(stream, settings)) {
					// Parse the file.
					while(reader.Read()) ;
				}
			}

			return result;
		}
	}
}
