using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Yamaha.AVControl;
using System.Net;
using System.Xml.Linq;
using Yamaha.AVControl.Util;

namespace TestRunner {
	class Program {
		static void Main(string[] args) {

			Controller c = new Controller(@"192.168.0.103");

			if (c.TryConnect()) {
				c.UpdateStatus();
		
				Console.ReadLine();
			} else {
				Console.WriteLine("There was an error attempting to connet to the service");
			}
		}
	}
}
