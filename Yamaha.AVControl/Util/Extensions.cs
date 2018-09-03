using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Yamaha.AVControl.Util {
	public static class Extensions {

		public static bool CompareElementVal(this XElement me, string elementName, string elementValue) {
			string temp = me.GetStrFromEV(elementName, string.Empty);
			if (string.Empty == temp)
				return false;
			else
				return temp == elementValue ? true : false;

		}

		public static int GetIntFromEV(this XElement me, string elementName, int defaultValue) {
			return int.Parse(me.GetStrFromEV(elementName, defaultValue.ToString()));
		}

		public static double GetDbFromEV(this XElement me, string elementName, double defaultValue) {
			return double.Parse(me.GetStrFromEV(elementName, defaultValue.ToString()));
		}

		public static string GetStrFromEV(this XElement me, string elementName, string defaultValue) {
			if (me == null || string.IsNullOrEmpty(elementName))
				return defaultValue;

			XElement e = me.Descendants(elementName).FirstOrDefault();
			if (e == null)
				return defaultValue;
			else
				return e.Value;
		}

		public static bool HasAttribute(this XElement me, string attributeName) {
			if (me == null) return false;

			return me.Attribute(attributeName) != null;
		}

		public static bool CompareAttributeVal(this XElement me, string attributeName, string attributeValue) {
			return me.CompareAttributeVal(attributeName, attributeValue, true);
		}

		public static bool CompareAttributeVal(this XElement me, string attributeName, string attributeValue, bool ignoreCase) {
			if (me.HasAttribute(attributeName))
				return string.Compare(me.Attribute(attributeName).Value, attributeValue, ignoreCase) == 0;
			else
				return false;
		}
	}
}
