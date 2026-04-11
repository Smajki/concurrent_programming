using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class BallRepository : IBallRepository
    {
        private List<IBall> balls;
        public BallRepository()
        {
            balls = new List<IBall>();
        }
        public void Add(IBall b)
        {
            balls.Add(b);
        }

        public IEnumerable<IBall> GetAll()
        {
            return balls.AsReadOnly();
        }

        public void Remove(IBall b)
        {
            balls.Remove(b);
        }

        public void RemoveAll()
        {
            balls.Clear();
        }
    }
}
