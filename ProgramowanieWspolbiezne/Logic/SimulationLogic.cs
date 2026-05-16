using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using System.Diagnostics;

namespace Logic
{
    public sealed class SimulationLogic : ISimulationLogic
    {
        public event EventHandler<BallStateChangedEventArgs>? BallStateChanged;

        private readonly IBallRepository _repository;
        private readonly Random _random;
        private readonly object _lock = new object();
        private IDiagnosticLogger logger;
        private string _logsFileName = "logs.txt";

        public SimulationLogic(IBallRepository repository, Random? random = null, IDiagnosticLogger? logger = null)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _random = random ?? new Random();
            this.logger = logger ?? new Logger(_logsFileName);
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
                double diameter = 25;

                double x = NextDouble(0, Math.Max(0, areaWidth - diameter));
                double y = NextDouble(0, Math.Max(0, areaHeight - diameter));

                double vx = NextNonZeroVelocity(-3, 3);
                double vy = NextNonZeroVelocity(-3, 3);

                IBall ball = new Ball(x, y, vx, vy, diameter);
                ball.Id = i;
                _repository.Add(ball);
            }

            //logger = new Logger(_logsFileName);

        }

        public async Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight)
        {
            Stopwatch stopwach = new Stopwatch();
            while (!token.IsCancellationRequested)
            {
                stopwach.Restart();

                checkCollisonsWithWalls(ball, areaWidth, areaHeight);
                CheckBallsCollisions(ball);

                int id = (int)ball.Id;
                double x = ball.Position.X;
                double y = ball.Position.Y;
                double d = ball.Diameter;

                BallStateChanged?.Invoke(this, new BallStateChangedEventArgs(id, x, y, d));

                stopwach.Stop();
                int delay = Math.Max(0, 16 - (int)stopwach.Elapsed.Milliseconds);
                await Task.Delay(delay, token);
            }
        }


        public void checkCollisonsWithWalls(IBall ball, double areaWidth, double areaHeight)
        {
            if (areaWidth <= 0) throw new ArgumentOutOfRangeException(nameof(areaWidth));
            if (areaHeight <= 0) throw new ArgumentOutOfRangeException(nameof(areaHeight));
            double ballRadious = ball.Diameter / 2;
            double newPositionX = ball.Position.X + ball.Velocity.X;
            double newPositionY = ball.Position.Y + ball.Velocity.Y;

            if (newPositionX <= ballRadious && ball.Velocity.X < 0)
            {
                ball.Position.X = ballRadious - (newPositionX - ballRadious);
                ball.Velocity.X = -ball.Velocity.X;
            }

            else if (newPositionX >= (areaWidth - ballRadious) && ball.Velocity.X > 0)
            {
                ball.Position.X = areaWidth - ballRadious - (newPositionX - (areaWidth - ballRadious));
                ball.Velocity.X = -ball.Velocity.X;
            }
            else
            {
                ball.Position.X = newPositionX;
            }

            if (newPositionY <= ballRadious && ball.Velocity.Y < 0)
            {
                ball.Position.Y = ballRadious - (newPositionY - ballRadious);
                ball.Velocity.Y = -ball.Velocity.Y;

            }
            else if (newPositionY >= (areaHeight - ballRadious) && ball.Velocity.Y > 0)
            {
                ball.Position.Y = areaHeight - ballRadious - (newPositionY - (areaHeight - ballRadious));
                ball.Velocity.Y = -ball.Velocity.Y;
            }
            else
            {
                ball.Position.Y = newPositionY;
            }
        }
        public static double DistanceBetweenBallsCenters(IBall b1, IBall b2)
        {
            double dx = b1.Position.X - b2.Position.X;
            double dy = b1.Position.Y - b2.Position.Y;
            return Math.Sqrt(dx * dx + dy * dy);
        }

        public static double SumOfBallsRadiuses(IBall b1, IBall b2)
        {
            return 0.5 * (b1.Diameter + b2.Diameter);
        }
        public bool CheckBallsCollision(IBall b1, IBall b2, bool onlyOverlap)
        {
            double distance = DistanceBetweenBallsCenters(b1, b2);
            double sumOfRadiuses = SumOfBallsRadiuses(b1, b2);

            return onlyOverlap ? distance < sumOfRadiuses : distance <= sumOfRadiuses;
        }
        public static void SeparateBalls(IBall b1, IBall b2)
        {
            double distance = DistanceBetweenBallsCenters(b1, b2);
            double minDistance = SumOfBallsRadiuses(b1, b2);

            if (distance == 0)
            {
                distance = 0.0001;
                b1.Position.X += 0.00005;
                b2.Position.X -= 0.00005;
            }

            double overlap = minDistance - distance;
            if (overlap <= 0)
                return;

            double dx = (b1.Position.X - b2.Position.X) / distance;
            double dy = (b1.Position.Y - b2.Position.Y) / distance;

            double shift = overlap / 2.0;

            b1.Position.X += dx * shift;
            b1.Position.Y += dy * shift;

            b2.Position.X -= dx * shift;
            b2.Position.Y -= dy * shift;
        }

        public void CheckBallsCollisions(IBall ball)
        {
            List<IBall> balls = _repository.GetAll().ToList();
            int nmbrOfBalls = balls.Count;
            for (int i = 0; i < nmbrOfBalls; i++)
            {
                if (balls[i].Id > ball.Id)
                {
                    lock (_lock)
                    {
                        if (CheckBallsCollision(ball, balls[i], true))
                        {
                            SeparateBalls(ball, balls[i]);
                        }
                        if (CheckBallsCollision(ball, balls[i], false))
                        {
                            ball.Collide(balls[i]);
                            logger.logAsync(DateTime.Now.ToString() + " collision: " + balls[i].ToString() + " and " + ball.ToString());
                        }
                    }
                }
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

        public void setLogsFileName(string filename)
        {
            _logsFileName = filename;
        }

        public async ValueTask Dispose()
        {
            await logger.DisposeAsync();
        }
    }
}