/* Software License Agreement (BSD License)
 * 
 * Copyright (c) 2010-2011, Rustici Software, LLC
 * All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *     * Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *     * Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *     * Neither the name of the <organization> nor the
 *       names of its contributors may be used to endorse or promote products
 *       derived from this software without specific prior written permission.
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND
 * ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED
 * WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE
 * DISCLAIMED. IN NO EVENT SHALL Rustici Software, LLC BE LIABLE FOR ANY
 * DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES
 * (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND
 * ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT
 * (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS
 * SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */

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
