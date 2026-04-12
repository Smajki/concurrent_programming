using System;
using System.Collections.Generic;
using System.Linq;
using Data;

namespace Logic
{
    public sealed class SimulationLogic : ISimulationLogic
    {
        private readonly IBallRepository _repository;
        private readonly Random _random;

        public SimulationLogic(IBallRepository repository, Random? random = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _random = random ?? new Random();
        }

        public IReadOnlyList<IBall> Balls
        {
            get
            {
                return _repository.GetAll().ToList();
            }
        }

        public void Initialize(int ballsCount, double areaWidth, double areaHeight)
        {
            if (ballsCount < 0) throw new ArgumentOutOfRangeException(nameof(ballsCount));
            if (areaWidth <= 0) throw new ArgumentOutOfRangeException(nameof(areaWidth));
            if (areaHeight <= 0) throw new ArgumentOutOfRangeException(nameof(areaHeight));

            Clear();

            for (int i = 0; i < ballsCount; i++)
            {
                double diameter = NextDouble(10, 30);

                double x = NextDouble(0, Math.Max(0, areaWidth - diameter));
                double y = NextDouble(0, Math.Max(0, areaHeight - diameter));

                double vx = NextNonZeroVelocity(-3, 3);
                double vy = NextNonZeroVelocity(-3, 3);

                IBall ball = new Ball(x, y, vx, vy, diameter);
                _repository.Add(ball);
            }
        }

        public void Step(double areaWidth, double areaHeight)
        {
            if (areaWidth <= 0) throw new ArgumentOutOfRangeException(nameof(areaWidth));
            if (areaHeight <= 0) throw new ArgumentOutOfRangeException(nameof(areaHeight));

            foreach (var ball in _repository.GetAll())
            {
                ball.move();

                if (ball.positionX <= 0 && ball.velocityX < 0)
                    ball.velocityX = -ball.velocityX;

                if (ball.positionX + ball.diameter >= areaWidth && ball.velocityX > 0)
                    ball.velocityX = -ball.velocityX;

                if (ball.positionY <= 0 && ball.velocityY < 0)
                    ball.velocityY = -ball.velocityY;

                if (ball.positionY + ball.diameter >= areaHeight && ball.velocityY > 0)
                    ball.velocityY = -ball.velocityY;
            }
        }

        public void Clear()
        {
            _repository.RemoveAll();
        }

        private double NextDouble(double min, double max)
        {
            return min + _random.NextDouble() * (max - min);
        }

        private double NextNonZeroVelocity(int minInclusive, int maxInclusive)
        {
            int v = 0;
            while (v == 0)
                v = _random.Next(minInclusive, maxInclusive + 1);
            return v;
        }
    }
}