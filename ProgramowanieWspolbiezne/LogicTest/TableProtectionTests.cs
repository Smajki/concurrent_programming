using Data;
using Logic;
using System.Collections.Concurrent;

namespace LogicTest
{
    [TestClass]
    public sealed class AbstractTableProtectionTests
    {
        [TestMethod]
        public async Task ConcurrentCollisionDetection_DoesNotThrow_AndKeepsStateConsistent()
        {
            int ballsCount = 100;
            double width = 400;
            double height = 300;

            var repo = new BallRepository();
            var logic = new SimulationLogic(repo, new Random(123));

            logic.Initialize(ballsCount, width, height, new NullDiagnosticLogger());

            var exceptions = new ConcurrentQueue<Exception>();

            using var cts = new CancellationTokenSource();
            cts.CancelAfter(TimeSpan.FromSeconds(1.5));

            int workers = Environment.ProcessorCount;

            Task[] tasks = Enumerable.Range(0, workers).Select(_ => Task.Run(() =>
            {
                try
                {
                    while (!cts.Token.IsCancellationRequested)
                    {
                        var balls = logic.Balls.ToList();

                        foreach (var b in balls)
                        {
                            logic.checkCollisonsWithWalls(b, width, height);
                            logic.CheckBallsCollisions(b);
                        }
                    }
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                }
            })).ToArray();

            await Task.WhenAll(tasks);

            if (!exceptions.IsEmpty)
            {
                Exception ex = exceptions.First();
                Assert.Fail("Mamy wyjątek podczas równoległego wykrywania kolizji. Pierwszy wyjątek: " + ex);
            }

            Assert.AreEqual(ballsCount, logic.Balls.Count, "Liczba kulek zmieniła się w podczas testu.");

            foreach (var b in logic.Balls)
            {
                Assert.IsFalse(double.IsNaN(b.Position.X) || double.IsInfinity(b.Position.X), $"Pozycja X jest niepoprawna dla kuli id={b.Id}");
                Assert.IsFalse(double.IsNaN(b.Position.Y) || double.IsInfinity(b.Position.Y), $"Pozycja Y jest niepoprawna dla kuli id={b.Id}");
            }

            await logic.Dispose().AsTask();
        }
    }
}