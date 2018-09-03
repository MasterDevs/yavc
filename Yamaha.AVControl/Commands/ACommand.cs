using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using System.IO;
using System.Net;
using Yamaha.AVControl.Commands.Data;

namespace Yamaha.AVControl.Commands {

	public struct RequestInfo {
		public string Method { get; set; }
		public string Body { get; set; }
		public string RelativeUri { get; set; }

		private const string XML_FS = @"<?xml version=""1.0"" encoding=""utf-8""?>{0}";
		public static RequestInfo GenRequest(string method, string body) {
			RequestInfo ri = new RequestInfo();
			ri.Method = method;
			ri.Body = string.Format(XML_FS, body);
			ri.RelativeUri = "YamahaRemoteControl/ctrl";
			return ri;
		}
		public static RequestInfo GenRequest(string method, string body, string relativeUri) {
			RequestInfo ri = GenRequest(method, body);
			ri.RelativeUri = relativeUri;
			return ri;
		}
	}

	public abstract class ACommand : ICommand {

		public SendResult LastSendResult { get; protected set; }
		public XElement LastResponse { get; protected set; }

		protected virtual string Method { get { return "POST"; } }
		protected virtual string UserAgent { get { return @"AVcontrol/1.03 CFNetwork/485.12.30 Darwin/10.4.0"; } }
		protected virtual string ContentType { get { return @"text/xml; charset=UTF-8"; } }
		protected virtual Encoding CommandEncoding { get { return Encoding.UTF8; } }

		protected abstract SendResult ParseResponseImp(XElement xml);
		protected abstract RequestInfo[] GetRequests();

		public Zone Zone;

		protected ACommand(Zone z) {
			Zone = z;
		}

		public void ParseResponse(XElement responseXML) {
			LastSendResult = ParseResponseImp(responseXML);
		}

		public override string ToString() {
			if (LastResponse != null)
				return LastResponse.ToString();
			else
				return this.GetType().ToString();
		}

		public SendResult Send(Controller c) {

			SendResult result = SendResult.Empty;
			foreach (var data in GetRequests()) {
				HttpWebRequest request = c.GenerateRequest(data.RelativeUri);

				//-- Setup the common Request properties
				request.UserAgent = UserAgent;
				request.ContentType = ContentType;

				result = Process(request, data);
				if (!result.Success)
					return result;
			}

			return result;
		}

		#region Helpers
		protected SendResult Process(HttpWebRequest request, RequestInfo info) {
			try {
				request.Method = info.Method;

				if (request.Method == "POST") {
					byte[] body = CommandEncoding.GetBytes(info.Body);
					request.ContentLength = body.LongLength; 
					using (Stream s = request.GetRequestStream()) {
						s.Write(body, 0, body.Length);
					}
				}

				//-- Lets Handle the Reponse Now
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				using (StreamReader sr = new StreamReader(response.GetResponseStream(), CommandEncoding)) {
					XElement result = XElement.Parse(sr.ReadToEnd());

					ParseResponse(result);

					return LastSendResult;
				}
			} catch (Exception exp) {
				return SendResult.Error(exp, "There was an error processing the Request");
			}
		}
		#endregion
	}
}
