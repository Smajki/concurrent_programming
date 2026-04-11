namespace Data
{
    public class Ball : IBall
    {
        public double positionX { get; private set; }
        public double positionY { get; private set; }

        public double velocityX { get; set; }
        public double velocityY { get; set;}

        public double diameter { get;}

        public Ball(double positionX, double positionY, double velocityX, double velocityY, double diameter)
        {
            this.positionX = positionX;
            this.positionY = positionY;
            this.velocityX = velocityX;
            this.velocityY = velocityY;
            this.diameter = diameter;
        }
        public void move()
        {
            positionX += velocityX;
            positionY += velocityY;
        }


    }
}
