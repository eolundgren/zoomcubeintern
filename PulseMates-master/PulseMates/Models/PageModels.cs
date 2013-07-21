namespace PulseMates.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Runtime.Serialization;
    using System.Web;
    using Infrastructure;
    using Infrastructure.Extensions;

    [DataContract]
    public class Page : DataModel {
        [DataMember]
        public string Slug { get; set; }
    }


    public class PageModel
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }

        [Required]
        public Tags Tags { get; set; }
        public IDictionary<string, object> PropertyBag { get; set; }

        public Page ToPage(string id = "")
        {
            return new Page
            {
                Id = id,
                Name = Name,
                Slug = Name.Slugify(),
                Description = Description,
                Tags = Tags,
                PropertyBag = PropertyBag.ToSafeDictionary()
            };
        }
    }
}