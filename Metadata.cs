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
