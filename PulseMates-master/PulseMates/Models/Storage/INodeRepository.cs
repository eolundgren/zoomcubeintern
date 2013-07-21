using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseMates.Models.Storage
{
    public interface INodeRepository : IDisposable
    {
        Node Find(string id);
        IQueryable<Node> FindAll();
        IQueryable<Node> FindAll(string[] tags);
        IQueryable<Node> FindAll(double longitude, double latitude, double radius);
        IQueryable<Node> FindAll(string[] tags, double longitude, double latitude, double radius);

        Tag[] GetUniqueTags();
            
        bool Create(Node node);
        bool Update(Node node);
        bool Delete(string id);
    }
}
