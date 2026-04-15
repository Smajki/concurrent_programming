using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IBall
    {
        public Vector Position { get;}

        public Vector Velocity { get; set; }

        public double Diameter {  get;}
        public void move();       
    }
}
