using System;
using System.Collections.Generic;
using System.Text;
using System.Xml;

namespace RusticiSoftware.HostedEngine.Client
{
    // We're returning the xml as-is now so I stopped development of this class.  If
    // we change our minds, it's here

//    class Metadata
//    {
//        private string id;
//        private string title;
//        private string description;
//        private long duration;
//        private long typicalTime;
//        private List<string> keywords;
//        private double masteryScore;
//        private List<Metadata> children;
//
//
//        public Metadata(XmlElement metadataObjectEl)
//        {
//            this.id = metadataObjectEl.Attributes["id"].Value;
//            foreach (XmlElement node in metadataObjectEl.ChildNodes)
//            {
//                if (node.Name == "metadata")
//                {
//                    foreach (XmlElement child in node.ChildNodes)
//                    {
//                        switch (child.Name)
//                        {
//                            case "title":
//                                this.title = child.InnerText;
//                                break;
//                            case "description":
//                                this.description = child.InnerText;
//                                break;
//                            case "duration":
//                                this.duration = Convert.ToInt64(child.InnerText);
//                                break;
//                            case "typicaltime":
//                                this.typicalTime = Convert.ToInt64(child.InnerText);
//                                break;
//                            case "masteryscore":
//                                this.masteryScore = Convert.ToDouble(child.InnerText);
//                                break;
//                            case "keywords":
//                                foreach (XmlElement keyword in child.ChildNodes)
//                                {
//                                    this.keywords.Add(keyword.InnerText);
//                                }
//                                break;
//                            default:
//                                break;
//                        }
//                    }
//                }
//
//                // Recurse through children
//                if (node.Name == "children")
//                {
//                    foreach (XmlElement objNode in node.ChildNodes)
//                    {
//                        if (objNode.Name == "object")
//                        {
//                            children.Add(new Metadata(objNode));
//                        }
//                    }
//                }
//            }
//
//        }
//
//        public string Title
//        {
//            get { return title; }
//        }
//
//        public string Description
//        {
//            get { return description; }
//        }
//
//        public long Duration
//        {
//            get { return duration; }
//        }
//
//        public long TypicalTime
//        {
//            get { return typicalTime; }
//        }
//
//        public List<string> Keywords
//        {
//            get { return keywords; }
//        }
//
//        public double MasteryScore
//        {
//            get { return masteryScore; }
//        }
//
//        public List<Metadata> Children
//        {
//            get { return children; }
//        }
//    }
}
