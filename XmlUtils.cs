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
    public class XmlUtils
    {
        /// <summary>
        /// Utility function to retrieve inner text of first elem with tag elementName, or null if not found
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static String GetNamedElemValue(XmlElement parent, String elementName)
        {
            String val = null;
            XmlNodeList list = parent.GetElementsByTagName(elementName);
            if (list.Count > 0) {
                val = ((XmlElement)list[0]).InnerText;
            }
            return val;
        }

        /// <summary>
        /// Utility function to retrieve inner text of first elem with tag elementName, or null if not found
        /// </summary>
        /// <param name="parent"></param>
        /// <param name="elementName"></param>
        /// <returns></returns>
        public static String GetNamedElemXml(XmlElement parent, String elementName)
        {
            String val = null;
            XmlNodeList list = parent.GetElementsByTagName(elementName);
            if (list.Count > 0) {
                val = ((XmlElement)list[0]).InnerXml;
            }
            return val;
        }

        public static String GetChildElemText(XmlElement parent, String tagName)
        {
            String val = null;
            XmlElement childElem = GetFirstChildByTagName(parent, tagName);
            if (childElem != null) {
                val = childElem.InnerText;
            }
            return val;
        }

        public static XmlElement GetFirstChildByTagName(XmlNode parent, String tagName)
        {
            List<XmlElement> children = GetChildrenByTagName(parent, tagName);
            return (children.Count == 0) ? null : children[0];
        }

        public static List<XmlElement> GetChildrenByTagName(XmlNode parent, String tagName)
        {
            List<XmlElement> elements = new List<XmlElement>();
            XmlNodeList children = parent.ChildNodes;
            for (int i = 0; i < children.Count; i++) {
                XmlNode child = children[i];
                if (child is XmlElement) {
                    XmlElement elem = (XmlElement)child;
                    if (elem.Name.Equals(tagName)) {
                        elements.Add(elem);
                    }
                }
            }
            return elements;
        }
    }
}
