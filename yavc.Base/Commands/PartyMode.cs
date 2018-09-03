using System;
using System.Linq;
using yavc.Base.Data;
using yavc.Base.Util;
using System.Xml.Linq;

namespace yavc.Base.Commands
{
    public class SetPartyMode : ACommand
    {

        private const string REQ = @"<YAMAHA_AV cmd=""PUT""><System><Party_Mode><Mode>{0}</Mode></Party_Mode></System></YAMAHA_AV>";
        private const string SUCCESS_RESULT = @"<YAMAHA_AV rsp=""PUT"" RC=""0""><System><Party_Mode><Mode></Mode></Party_Mode></System></YAMAHA_AV>";
        private readonly string OnOff;

        public SetPartyMode(bool partyModeOn, Zone z)
        {
            OnOff = partyModeOn ? "On" : "Off";
        }

        protected override SendResult ParseResponseImp(string xml)
        {
            if (xml == SUCCESS_RESULT)
                return SendResult.Succcess;
            else
                return SendResult.Error(new Exception("Invalid response returned."));
        }

        protected override RequestInfo[] GetRequestInfo()
        {
            return new RequestInfo[] {
				RequestInfo.GenRequest(yavcMethod.Post, string.Format(REQ, OnOff))
			};
        }
    }

    public class GetPartyMode : ACommand
    {
        private const string PartyMode = @"<YAMAHA_AV cmd=""GET""><System><Party_Mode><Mode>GetParam</Mode></Party_Mode></System></YAMAHA_AV>";

        public bool IsOn { get; private set; }

        public GetPartyMode(Zone z, IProcessRequest processor) { }

        protected override SendResult ParseResponseImp(string xmlS)
        {
            var xml = XElement.Parse(xmlS);

            var partyMode = xml.Descendants("Party_Mode").FirstOrDefault();
            if (null != partyMode)
            {
                IsOn = partyMode.CompareElementVal("Mode", "On");
                return SendResult.Succcess;
            }

            return SendResult.Empty;
        }

        protected override RequestInfo[] GetRequestInfo()
        {
            return new RequestInfo[] { RequestInfo.GenRequest(yavcMethod.Post, PartyMode) };
        }
    }
}
