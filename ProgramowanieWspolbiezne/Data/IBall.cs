using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IBall
    {
        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public double Diameter {  get;}   
    }
}
