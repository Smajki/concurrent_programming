namespace Data
{
    public class Ball : IBall
    {
        public Vector Position { get; private set; }

        public Vector Velocity { get; set; }

        public double Diameter { get;}

        public Ball(double positionX, double positionY, double velocityX, double velocityY, double diameter)
        {
            Position = new Vector(positionX, positionY);
            Velocity = new Vector(velocityX, velocityY);
            this.Diameter = diameter;
        }
        public void move()
        {
            Position.X += Velocity.X;
            Position.Y += Velocity.Y;
        }


    }
}
