using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PulseMates.Models.Storage
{
    public interface IPageRepository : IDisposable
    {
        Page Find(string id);
        IQueryable<Page> FindAll();

        bool Create(Page page);
        bool Update(Page page);
        bool Delete(string id);
    }
}
