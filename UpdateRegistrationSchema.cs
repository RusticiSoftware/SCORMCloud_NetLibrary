using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace RusticiSoftware.HostedEngine.Client
{
    [XmlRoot(ElementName = "registrationreport")]
    public class UpdateRegistrationSchema
    {
        [XmlAttribute(AttributeName = "regid")]
        public String RegId { get; set; }

        [XmlAttribute(AttributeName = "instanceid")]
        public Int16 InstanceId { get; set; }

        [XmlElement(ElementName = "complete")]
        public String Complete { get; set; }

        [XmlElement(ElementName = "success")]
        public String Success { get; set; }

        [XmlElement(ElementName = "totaltime")]
        public Int16 TotalTime { get; set; }

        [XmlElement(ElementName = "score")]
        public Double Score { get; set; }

        [XmlArray(ElementName = "interactions")]
        public List<Interaction> Interactions { get; set; }
    }


    [XmlType(TypeName = "interaction")]
    public class Interaction
    {
        [XmlAttribute(AttributeName = "id")]
        public String Id { get; set; }

        [XmlElement(ElementName = "type")]
        public String Type { get; set; }

        [XmlArray(ElementName = "objectives")]
        public List<Objective> Objectives { get; set; }

        [XmlElement(ElementName = "timestamp")]
        public String Timestamp { get; set; }

        [XmlArray(ElementName = "correct_responses")]
        public List<Response> CorrectResponses { get; set; }

        [XmlElement(ElementName = "weighting")]
        public String Weighting { get; set; }

        [XmlElement(ElementName = "learner_response")]
        public String LearnerResponse { get; set; }

        [XmlElement(ElementName = "result")]
        public String Result { get; set; }

        [XmlElement(ElementName = "latency")]
        public String Latency { get; set; }

        [XmlElement(ElementName = "description")]
        public String Description { get; set; }
    }


    [XmlType(TypeName = "objective")]
    public class Objective
    {
        [XmlAttribute(AttributeName = "id")]
        public String Id { get; set; }
    }


    [XmlType(TypeName = "response")]
    public class Response
    {
        [XmlAttribute(AttributeName = "id")]
        public String Id { get; set; }
    }
}
