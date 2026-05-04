namespace Data
{
    public class Ball : IBall
    {
        public Vector Position { get; set; }

        public Vector Velocity { get; set; }

        public double Diameter { get;}

        public double Mass { get;}

        public double Id { get; set; }

        public Ball(double positionX, double positionY, double velocityX, double velocityY, double diameter, double mass)
        {
            Position = new Vector(positionX, positionY);
            Velocity = new Vector(velocityX, velocityY);
            this.Diameter = diameter;
            this.Mass = mass;
        }
        public Ball(double positionX, double positionY, double velocityX, double velocityY, double diameter) {
            Position = new Vector(positionX, positionY);
            Velocity = new Vector(velocityX, velocityY);
            this.Diameter = diameter;
            this.Mass = 1; //dla uproszczenia - jeśli nie podano masy, masa jest równa 1 kg
        }

        public void Collide(IBall other) {
            double otherBallMass=other.Mass;
            Vector newVelocity;
            Vector newOtherBallVelocity;
            if (Mass == otherBallMass) 
            {
                newVelocity = other.Velocity;
                newOtherBallVelocity = Velocity;
            }
            else 
            {
                double sumOfMasses = Mass + otherBallMass;
                newVelocity = Velocity.MultiplyByNumber((Mass - otherBallMass) / sumOfMasses)
                                      + other.Velocity.MultiplyByNumber(2 * otherBallMass / sumOfMasses);
                newOtherBallVelocity = Velocity.MultiplyByNumber(2 * Mass / sumOfMasses)
                                     + other.Velocity.MultiplyByNumber((otherBallMass - Mass) / sumOfMasses);
            }
            Velocity = newVelocity;
            other.Velocity = newOtherBallVelocity;
        }
    }
}
