using Data;

namespace Logic
{
    public sealed class BallExtended : Base
    {
        private double _x;
        private double _y;
        private double _diameter;

        public double X
        {
            get => _x;
            private set => SetField(ref _x, value);
        }

        public double Y
        {
            get => _y;
            private set => SetField(ref _y, value);
        }

        public double Diameter
        {
            get => _diameter;
            private set => SetField(ref _diameter, value);
        }

        public void UpdateFrom(IBall ball)
        {
            X = ball.Position.X - ball.Diameter / 2;
            Y = ball.Position.Y - ball.Diameter / 2;
            Diameter = ball.Diameter;
        }
    }
}