namespace PulseMates.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;
    using Infrastructure.Extensions;

    [DataContract]
    public class Event : DataModel
    {
        [DataMember]
        public TimeDuration Duration { get; set; }
        [DataMember]
        public Point[] Area { get; set; }
    }

    [DataContract]
    public class EventModel
    {
        [DataMember(IsRequired=true)]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember(IsRequired = true)]
        public TimeDuration Duration { get; set; }
        [DataMember(IsRequired = true)]
        public Point[] Area { get; set; }

        [DataMember]
        public string[] Tags { get; set; }
        [DataMember]
        public IDictionary<string, object> PropertyBag { get; set; }

        public Event ToEvent(string id = "")
        {
            return new Event
            {
                Id = id,
                Name = Name,
                Description = Description,
                PropertyBag = PropertyBag.ToSafeDictionary(),
                Tags = new Tags(Tags),
                Duration = Duration,
                Area = Area
            };
        }
    }
    
}