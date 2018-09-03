using System;
using System.Collections.Generic;
using System.Text;

namespace yavc.Base.Commands {
	public struct RequestInfo {
		public const string DefaultUri = @"YamahaRemoteControl/ctrl";
		public const int DefaultPort = 80;

		public int Port { get; set; }
		public yavcMethod Method { get; set; }
		public string Body { get; set; }
		public string RelativeUri { get; set; }
		public Dictionary<string, string> Headers { get; private set; }

		private const string XML_FS = @"<?xml version=""1.0"" encoding=""utf-8""?>{0}";
		
		public static RequestInfo GenRequest(yavcMethod method, string body) {
			return GenRequest(method, body, DefaultUri, DefaultPort);
		}
		
		public static RequestInfo GenRequest(yavcMethod method, string body, string relativeUri, int port) {
			RequestInfo ri = new RequestInfo();
			ri.Body = string.Format(XML_FS, body);
			ri.Headers = new Dictionary<string, string>();
			ri.Method = method;
			ri.Port = port;
			ri.RelativeUri = relativeUri;
			return ri;
		}

		public void AddHeaderIfNotExists(string key, string value) {
			if (Headers.ContainsKey(key)) return;
			Headers.Add(key, value);
		}
	}
}
