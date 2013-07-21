namespace PulseMates.Models
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Dynamic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;

    using Storage;
    using Infrastructure.Extensions;
    
    [DataContract]
    public class Node : DataModel
    {
        [DataMember]
        public DateTime Time { get; set; }

        [DataMember]
        public Location Location { get; set; }
        [IgnoreDataMember]
        public Image Image { get; set; }
    }

    [DataContract]
    public class NodeStack
    {
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public IEnumerable<Node> Nodes { get; set; }
    }

    [DataContract]
    public class Image
    {
        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string MediaType { get; set; }

        [DataMember]
        public string Url { get; set; }
    }

    [DataContract]
    public class Location
    {
        [DataMember]
        public double Latitude { get; set; }
        [DataMember]
        public double Longitude { get; set; }
    }

    public class NodeModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime Time { get; set; }

        //// note the order [longitude, latitude]
        //public double[] Location { get; set; }
        //[DataMember]
        public Location Location { get; set; }

        public string[] Tags { get; set; }
        public IDictionary<string, object> PropertyBag { get; set; }

        public Node ToNode(string id = "", Image image = null)
        {
            return new Node
            {
                Id = id,
                Name = Name,
                Description = Description,
                Time = Time,
                Location = Location,
                PropertyBag = PropertyBag.ToSafeDictionary(),
                Tags = new Tags(Tags),
                Image = image
            };
        }
    }
}