using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Xml.Linq;
using yavc.Base.Data;
using yavc.Base.Util;

namespace yavc.Base.Commands {
	public class GetList : ACommand {

        public const int PAGE_SIZE = 8;

		public GetList(Input i) {
			InputName = i.Src_Name;
		}

        public Dictionary<int, ListItem> Items { get; protected set; }
        public int CurrentLine { get; private set; }
        public int CurrentPage { get; private set; }
        public int MaxLine { get; private set; }
        public int MenuLayer { get; private set; }

        public string InputName { get; private set; }
        public string MenuName { get; private set; }

        private int GetFirstItemNumber(int currentPage) 
        {
            return ((currentPage - 1) * PAGE_SIZE) + 1; 
        } 

        public static int GetPageForLine(int line)
        {
            //return Math.Ceiling((double)line / (double)PAGE_SIZE);
            var remainder = (double)line % (double)PAGE_SIZE;
            var flat = line / PAGE_SIZE;

            if (remainder > 0)
                return flat + 1;

            return flat;
        }

        protected override RequestInfo[] GetRequestInfo()
        {
            Items = new Dictionary<int, ListItem>();
            return new RequestInfo[] { 
				RequestInfo.GenRequest(yavcMethod.Post, ListRequests.GetListRequest(InputName)),
			};
        }

		protected override SendResult ParseResponseImp(string xml) {
			try {
				var root = XElement.Parse(xml);

                return TryParseList(root);
			} catch (Exception exp) {
				return SendResult.Error(exp);
			}
		}

        protected ListItem ParseItem(XElement i, int line) {
			return new ListItem(
				i.GetStrFromEV("Txt", string.Empty).HtmlDecode(),
				i.GetStrFromEV("Attribute", string.Empty),
				line);
		}

        protected SendResult TryParseList(XElement root)
        {
            var el = root.Element(InputName);

            if (null == el)
                return SendResult.Empty;

            MenuName = el.GetStrFromEV("Menu_Name", string.Empty);
            MenuLayer = el.GetIntFromEV("Menu_Layer", 0);
            MaxLine = el.GetIntFromEV("Max_Line", 0);

            var items = root.Descendants("List_Info").FirstOrDefault();

            var item_list = items.Element("Current_List");
            var cursor_position = items.Element("Cursor_Position");
            CurrentLine = cursor_position.GetIntFromEV("Current_Line", -1);

            if (CurrentLine == -1) return SendResult.Error(new Exception("Could not parse Current_Line from response."));

            CurrentPage = GetPageForLine(CurrentLine);

            if (null != item_list)
            {
                var line = GetFirstItemNumber(CurrentPage);
                foreach (var i in item_list.Elements())
                {
                    Items[line] = ParseItem(i, line);
                    line++;
                }
            }

            return SendResult.Succcess;
        }

        
	}
}
