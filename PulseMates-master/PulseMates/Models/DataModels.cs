namespace PulseMates.Models
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Linq;

    public interface DataModels
    {
        string Id { get; set; }
        string Name { get; set; }
        string Description { get; set; }

        Tags Tags { get; set; }
        IDictionary<string, object> PropertyBag { get; set; }
    }

    [DataContract]
    public abstract class DataModel : DataModels
    {
        protected DataModel()
        {
            Tags = new Tags();
            PropertyBag = new Dictionary<string, object>();
        }

        #region IDataModel Members

        [DataMember]
        public string Id { get; set; }
        [DataMember]
        public string Name { get; set; }
        [DataMember]
        public string Description { get; set; }

        [DataMember]
        public Tags Tags { get; set; }
        [DataMember]
        public IDictionary<string, object> PropertyBag { get; set; }

        #endregion
    }

    public class Tag
    {
        public string Name { get; set; }
        public int Sum { get; set; }
    }

    public class Tags : ICollection<string>
    {
        #region Field Members

        static readonly char[] tagsSplits = new char[] { ',', ';', '|' };

        private List<string> tagList;

        #endregion

        #region Constructor Members

        public Tags() { tagList = new List<string>(); }
        public Tags(string tags)
        {
            if (!string.IsNullOrEmpty(tags))
            {
                var args = tags.Split(tagsSplits).Select(x => x.Trim().ToLower());
                tagList = new List<string>(args);
            }
        }
        public Tags(IEnumerable<string> tags) { tagList = new List<string>(tags); }

        #endregion

        #region ICollection<string> Members

        public void Add(string tag)
        {
            if (!Contains(tag))
                tagList.Add(tag.ToLower());
        }

        public void Clear()
        {
            tagList.Clear();
        }

        public bool Contains(string tag)
        {
            return tagList.Contains(tag.ToLower());
        }

        void ICollection<string>.CopyTo(string[] array, int arrayIndex)
        {
            tagList.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return tagList.Count; }
        }

        bool ICollection<string>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(string tag)
        {
            return tagList.Remove(tag.ToLower());
        }

        #endregion

        #region IEnumerable<string> Members

        public IEnumerator<string> GetEnumerator()
        {
            return tagList.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        #endregion

        #region Override Members

        public override string ToString()
        {
            return string.Join(", ", tagList.Select(x => x.Trim().ToLower()));
        }

        #endregion
    }

    public struct TimeDuration
    {
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
    }

    public struct Point
    {
        public Point(double latitude, double longitude) : this() {
            Latitude = latitude; Longitude = longitude;
        }

        public double Latitude { get; set; }
        public double Longitude { get; set; }
    }

    public struct Polygon
    {
        public Point[] Points { get; set; }
    }
}
