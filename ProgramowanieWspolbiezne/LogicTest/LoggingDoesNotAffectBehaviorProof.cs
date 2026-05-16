using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace LogicTest
{
    [TestClass]
    public sealed class LoggingDoesNotAffectBehaviorProof
    {
        [TestMethod]
        public void LoggingDoesNotAffectBallsBehavior_ForSameSeedAndSameSteps()
        {
            int seed = 12345;
            int ballsCount = 10;
            double width = 200;
            double height = 200;

            // bez logowania
            var repoA = new BallRepository();
            var logicA = new SimulationLogic(repoA, new Random(seed), new NullDiagnosticLogger());
            logicA.Initialize(ballsCount, width, height);

            // z logowaniem do pliku
            string logFile = "logging_impact_test.txt";
            if (File.Exists(logFile)) File.Delete(logFile);

            var repoB = new BallRepository();
            var logicB = new SimulationLogic(repoB, new Random(seed), new Logger(logFile));
            logicB.Initialize(ballsCount, width, height);

  
            int steps = 200;
            for (int s = 0; s < steps; s++)
            {
                var ballsA = logicA.Balls.OrderBy(b => b.Id).ToList();
                var ballsB = logicB.Balls.OrderBy(b => b.Id).ToList();

                Assert.AreEqual(ballsA.Count, ballsB.Count);

                for (int i = 0; i < ballsA.Count; i++)
                {
                    logicA.checkCollisonsWithWalls(ballsA[i], width, height);
                    logicA.CheckBallsCollisions(ballsA[i]);

                    logicB.checkCollisonsWithWalls(ballsB[i], width, height);
                    logicB.CheckBallsCollisions(ballsB[i]);
                }
            }

            // porównujemy stan końcowy
            var endA = logicA.Balls.OrderBy(b => b.Id).ToList();
            var endB = logicB.Balls.OrderBy(b => b.Id).ToList();

            Assert.AreEqual(endA.Count, endB.Count);

            for (int i = 0; i < endA.Count; i++)
            {
                Assert.AreEqual(endA[i].Id, endB[i].Id);

                Assert.AreEqual(endA[i].Position.X, endB[i].Position.X, 0.0000001, $"X differs for ball {endA[i].Id}");
                Assert.AreEqual(endA[i].Position.Y, endB[i].Position.Y, 0.0000001, $"Y differs for ball {endA[i].Id}");

                Assert.AreEqual(endA[i].Velocity.X, endB[i].Velocity.X, 0.0000001, $"Vx differs for ball {endA[i].Id}");
                Assert.AreEqual(endA[i].Velocity.Y, endB[i].Velocity.Y, 0.0000001, $"Vy differs for ball {endA[i].Id}");
            }

            logicA.Dispose().AsTask().Wait();
            logicB.Dispose().AsTask().Wait();

            if (File.Exists(logFile)) File.Delete(logFile);
        }
    }
}