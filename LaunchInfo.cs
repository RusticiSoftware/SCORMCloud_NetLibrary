using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    public class LaunchInfo
    {
        private String _id;
        private String _completion;
        private String _satisfaction;
        private String _measureStatus;
        private String _normalizedMeasure;
        private String _experiencedDurationTracked;
        private String _launchTime;
        private String _exitTime;
        private String _lastUpdated;
        private String _log;

        /// <summary>
        /// The id associated with this launch
        /// </summary>
        public String Id
        {
            get { return _id; }
            set { _id = value; }
        }

        /// <summary>
        /// The completion value of the launch
        /// </summary>
        public String Completion
        {
            get { return _completion; }
            set { _completion = value; }
        }

        /// <summary>
        /// The satisfaction value of the launch
        /// </summary>
        public String Satisfaction
        {
            get { return _satisfaction; }
            set { _satisfaction = value; }
        }

        /// <summary>
        /// The measure status of the launch
        /// </summary>
        public String MeasureStatus
        {
            get { return _measureStatus; }
            set { _measureStatus = value; }
        }

        /// <summary>
        /// The normalized measure of the launch
        /// </summary>
        public String NormalizedMeasure
        {
            get { return _normalizedMeasure; }
            set { _normalizedMeasure = value; }
        }

        /// <summary>
        /// The experienced duration tracked for this launch
        /// </summary>
        public String ExperiencedDurationTracked
        {
            get { return _experiencedDurationTracked; }
            set { _experiencedDurationTracked = value; }
        }

        /// <summary>
        /// The launch time
        /// </summary>
        public String LaunchTime
        {
            get { return _launchTime; }
            set { _launchTime = value; }
        }

        /// <summary>
        /// The exit time
        /// </summary>
        public String ExitTime
        {
            get { return _exitTime; }
            set { _exitTime = value; }
        }

        /// <summary>
        /// The last update time for this launch
        /// </summary>
        public String LastUpdated
        {
            get { return _lastUpdated; }
            set { _lastUpdated = value; }
        }

        /// <summary>
        /// The log which contains the execution history of this launch
        /// </summary>
        public String Log
        {
            get { return _log; }
            set { _log = value; }
        }

        /// <summary>
        /// Inflate launch info object from passed in xml element
        /// </summary>
        /// <param name="launchInfoElem"></param>
        public LaunchInfo(XmlElement launchInfoElem)
        {
            this.Id = launchInfoElem.GetAttribute("id");
            this.Completion = XmlUtils.GetNamedElemValue(launchInfoElem, "completion");
            this.Satisfaction = XmlUtils.GetNamedElemValue(launchInfoElem, "satisfaction");
            this.MeasureStatus = XmlUtils.GetNamedElemValue(launchInfoElem, "measure_status");
            this.NormalizedMeasure = XmlUtils.GetNamedElemValue(launchInfoElem, "normalized_measure");
            this.ExperiencedDurationTracked = XmlUtils.GetNamedElemValue(launchInfoElem, "experienced_duration_tracked");
            this.LaunchTime = XmlUtils.GetNamedElemValue(launchInfoElem, "launch_time");
            this.ExitTime = XmlUtils.GetNamedElemValue(launchInfoElem, "exit_time");
            this.LastUpdated = XmlUtils.GetNamedElemValue(launchInfoElem, "update_dt");
            this.Log = XmlUtils.GetNamedElemXml(launchInfoElem, "log");
        }

        /// <summary>
        /// Return list of launch info objects from xml element
        /// </summary>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static List<LaunchInfo> ConvertToLaunchInfoList(XmlElement rootElem)
        {
            List<LaunchInfo> launchList = new List<LaunchInfo>();
            XmlNodeList launches = rootElem.GetElementsByTagName("launch");
            foreach (XmlNode launch in launches) {
                launchList.Add(new LaunchInfo((XmlElement)launch));
            }
            return launchList;
        }
    }
}
