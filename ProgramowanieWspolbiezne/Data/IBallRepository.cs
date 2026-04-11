using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IBallRepository
    {
        public void Add(IBall b);
        public void Remove(IBall b);
        public IEnumerable<IBall> GetAll();
        public void RemoveAll();
    }
}
