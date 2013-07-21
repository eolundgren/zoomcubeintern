using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseMates.Models.Storage
{
    public interface IEventRepository : IDisposable
    {
        Event Find(string id);
        IQueryable<Event> FindAll();

        bool Create(Event node);
        bool Update(Event node);
        bool Delete(string id);
    }
}
