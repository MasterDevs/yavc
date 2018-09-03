using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using yavc.Base;

namespace System.Xml.Linq {
	public static class Extensions {
		#region XElement
		public static bool CompareElementVal(this XElement me, XName elementName, string elementValue) {
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

		public static string GetStrFromEV(this XElement me, XName elementName, string defaultValue) {
			if (me == null || elementName == null)
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
				return string.Compare(me.Attribute(attributeName).Value, attributeValue,
						ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture) == 0;
			else
				return false;
		}

		public static string ToStringSafe(this XElement me, string nullValue) {
			if (null == me)
				return nullValue;
			else
				return me.ToString();
		}
		public static string ToStringSafe(this XElement me) {
			return ToStringSafe(me, "null");
		}
		#endregion
	}
}

namespace System {
	public static class ActionExtensions {

		public static void NullableInvoke<T>(this Action<T> me, T obj) {
			if (null == me) return;

			me.Invoke(obj);
		}

		public static void NullableInvoke(this Action me) {
			if (null == me) return;

			me.Invoke();
		}

		public static Action NullableWrap(this Action me) {
			return () => me.NullableInvoke();
		}

		/// <summary>
		/// This method will invoke the specified action on the 
		/// UI thread.
		/// </summary>
		public static void OnUI(this Action me) {
			UI.Invoke(me);
		}
	}

	public static class StringExtension {
		public static bool IsNullOrEmpty(this string me) {
			return string.IsNullOrEmpty(me);
		}
	}

	public static class UI {
		/// <summary>
		/// Wrapper around the current dispatcher to invoke the action argument
		/// on the UI Thread.
		/// </summary>
		public static void Invoke(Action a) {
			if (Factory.Dispatcher == null) a.NullableInvoke();
			else
				Factory.Dispatcher.Invoke(a);
		}

        public static void Invoke<T>(Action<T> a, T obj)
        {
            Invoke(() => a.NullableInvoke(obj));
        }

		/// <summary>
		/// This method returns a new action that wraps the argument in a call to
		/// UI.Invoke. That way the action that's returned is safe to be invoked because
		/// it will be invoked on the UI thread.<para>
		/// Note: The argument will not be called until the returned action is invoked!</para>
		/// </summary>
		public static Action Wrap(Action a) {
			return () => Invoke(a);
		}
	}
}