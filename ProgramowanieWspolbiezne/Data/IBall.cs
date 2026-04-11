using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public interface IBall
    {
        public double positionX { get;}
        public double positionY { get;}

        public double velocityX { get; set; }
        public double velocityY { get; set; }

        public double diameter {  get;}
        public void move();       
    }
}
