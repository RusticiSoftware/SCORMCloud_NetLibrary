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
