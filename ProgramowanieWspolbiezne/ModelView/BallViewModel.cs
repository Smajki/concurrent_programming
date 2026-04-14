using Data;

namespace ModelView
{
    public sealed class BallViewModel : ViewModelBase
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
            X = ball.positionX;
            Y = ball.positionY;
            Diameter = ball.diameter;
        }
    }
}