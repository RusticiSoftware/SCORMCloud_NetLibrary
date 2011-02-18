using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class RegistrationSummary
    {
        private String _complete;
        private String _success;
        private String _totaltime;
        private String _score;

        public RegistrationSummary(XmlElement reportElem)
        {
            this.Complete = reportElem.GetElementsByTagName("complete")[0].InnerText;
            this.Success = reportElem.GetElementsByTagName("success")[0].InnerText;
            this.TotalTime = reportElem.GetElementsByTagName("totaltime")[0].InnerText;
            this.Score = reportElem.GetElementsByTagName("score")[0].InnerText;
        }

        public String Complete
        {
            get { return _complete; }
            set { _complete = value; }
        }

        public String Success
        {
            get { return _success; }
            set { _success = value; }
        }

        public String TotalTime
        {
            get { return _totaltime; }
            set { _totaltime = value; }
        }

        public String Score
        {
            get { return _score; }
            set { _score = value; }
        }

    }
}
