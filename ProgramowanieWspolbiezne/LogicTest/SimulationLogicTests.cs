using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LogicTest
{
    [TestClass]
    public sealed class SimulationLogicTests
    {
        [TestMethod]
        public void Step_BallMovesWhenVelocityIsNonZero()
        {
            // Czy kula porusza się, gdy ma nadaną prędkość?
            // Robimy kulę z prędkością (2, 3).
            // Robimy step.
            // Sprawdzamy, czy się poturlała.
            
            IBallRepository repo = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repo);

            IBall ball = new Ball(positionX: 10, positionY: 10, velocityX: 2, velocityY: 3, diameter: 10);
            repo.Add(ball);

            double width = 100;
            double height = 100;

            double oldX = ball.Position.X;
            double oldY = ball.Position.Y;

            logic.checkCollisonsWithWalls(ball,width, height);

            Assert.AreNotEqual(oldX, ball.Position.X);
            Assert.AreNotEqual(oldY, ball.Position.Y);
            Assert.AreEqual(2, ball.Velocity.X);
            Assert.AreEqual(3, ball.Velocity.Y);
        }

        [TestMethod]
        public void Step_BouncesOnRightWallAndFlipsVelocityX()
        {
            // Czy kula odbije się od ściany?
            // Robimy kulkę z prędkością 5 zaraz przy ścianie.
            // Sprawdzamy, czy po zrobieniu step jej velocity się odwróci.
            
            IBallRepository repo = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repo);

            double width = 100;
            double height = 100;
            double diameter = 10;
            double radious = diameter / 2;
            double velocityX = 5;
            double previousPositionToHitBoundary = width - radious - velocityX;

            IBall ball = new Ball(positionX: width - diameter - 1, positionY: 10, velocityX: 5, velocityY: 0, diameter: diameter);
            IBall ball2 = new Ball(positionX: previousPositionToHitBoundary, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            IBall ball3 = new Ball(positionX: previousPositionToHitBoundary - 1, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            IBall ball4 = new Ball(positionX: previousPositionToHitBoundary + 1, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            repo.Add(ball);
            repo.Add(ball2);
            repo.Add(ball3);
            repo.Add(ball4);

            logic.checkCollisonsWithWalls(ball, width, height);
            logic.checkCollisonsWithWalls(ball2, width, height);
            logic.checkCollisonsWithWalls(ball3, width, height);
            logic.checkCollisonsWithWalls(ball4, width, height);

            Assert.IsTrue(ball.Velocity.X > 0, "Kulka nie powinna odwrócić velocity po uderzeniu.");
            Assert.IsTrue(ball2.Velocity.X < 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru.");
            Assert.IsTrue(ball3.Velocity.X > 0, "Kulka nie powinna odwrócić prędkości po dotraciu do granicy obszaru..");
            Assert.IsTrue(ball4.Velocity.X < 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru..");
        }
        [TestMethod]
        public void Step_BouncesOnLeftWallAndFlipsVelocityX()
        {
            // Czy kula odbije się od ściany?
            // Robimy kulkę z prędkością 5 zaraz przy ścianie.
            // Sprawdzamy, czy po zrobieniu step jej velocity się odwróci.

            IBallRepository repo = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repo);

            double width = 100;
            double height = 100;
            double diameter = 10;
            double radious = diameter / 2;
            double velocityX = -5;


            IBall ball = new Ball(positionX: radious - velocityX, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            IBall ball2 = new Ball(positionX: radious - velocityX - 1, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            IBall ball3 = new Ball(positionX: radious - velocityX + 1, positionY: 10, velocityX: velocityX, velocityY: 0, diameter: diameter);
            repo.Add(ball);
            repo.Add(ball2);
            repo.Add(ball3);

            logic.checkCollisonsWithWalls(ball, width, height);
            logic.checkCollisonsWithWalls(ball2, width, height);
            logic.checkCollisonsWithWalls(ball3, width, height);

            Assert.IsTrue(ball.Velocity.X > 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru.");
            Assert.IsTrue(ball2.Velocity.X > 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru.");
            Assert.IsTrue(ball3.Velocity.X < 0, "Kulka nie powinna odwrócić prędkość po dotraciu do granicy obszaru..");
        }

        [TestMethod]
        public void Step_BouncesOnTopWallAndFlipsVelocityY()
        {
            // Czy kula odbije się od sufitu?
            // Robimy kulkę z prędkością -4 zaraz przy suficie.
            // Sprawdzamy, czy po zrobieniu step jej velocity się odwróci.
            
            IBallRepository repo = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repo);

            double width = 100;
            double height = 100;
            double diameter = 10;
            double radious = diameter / 2;
            double velocityY = -4;

            IBall ball = new Ball(positionX: diameter , positionY: 1, velocityX: 0, velocityY: velocityY, diameter: diameter);

            IBall ball2 = new Ball(positionX: diameter, positionY: radious - velocityY, velocityX: 0, velocityY: velocityY, diameter: diameter);
            IBall ball3 = new Ball(positionX: diameter, positionY: radious - velocityY + 1, velocityX: 0, velocityY: velocityY, diameter: diameter);
            IBall ball4 = new Ball(positionX: diameter, positionY: radious - velocityY - 1, velocityX: 0, velocityY: velocityY, diameter: diameter);
            repo.Add(ball);
            repo.Add(ball2);
            repo.Add(ball3);
            repo.Add(ball4);

            logic.checkCollisonsWithWalls(ball, width, height);
            logic.checkCollisonsWithWalls(ball2, width, height);
            logic.checkCollisonsWithWalls(ball3, width, height);
            logic.checkCollisonsWithWalls(ball4, width, height);

            Assert.IsTrue(ball.Velocity.Y > 0, "Kulka powinna odwrócić velocity po uderzeniu.");
            Assert.IsTrue(ball2.Velocity.Y > 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru.");
            Assert.IsTrue(ball3.Velocity.Y < 0, "Kulka nie powinna odwrócić prędkość po dotraciu do granicy obszaru..");
            Assert.IsTrue(ball4.Velocity.Y > 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru..");
        }

        [TestMethod]
        public void Step_BouncesOnDownWallAndFlipsVelocityY()
        {
            // Czy kula odbije się od sufitu?
            // Robimy kulkę z prędkością 4 zaraz przy podłodze.
            // Sprawdzamy, czy po zrobieniu step jej velocity się odwróci.

            IBallRepository repo = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repo);

            double width = 100;
            double height = 100;
            double diameter = 10;
            double radious = diameter / 2;
            double velocityY = 4;
            double previousPositionToHitBoundary = height - radious - velocityY;


            IBall ball = new Ball(positionY: previousPositionToHitBoundary, positionX: 1, velocityX: 0, velocityY: velocityY, diameter: diameter);

            IBall ball2 = new Ball(positionY: previousPositionToHitBoundary - 1, positionX: radious - velocityY, velocityX: 0, velocityY: velocityY, diameter: diameter);
            IBall ball3 = new Ball(positionY: previousPositionToHitBoundary + 1, positionX: radious - velocityY + 1, velocityX: 0, velocityY: velocityY, diameter: diameter);
          
            repo.Add(ball);
            repo.Add(ball2);
            repo.Add(ball3);

            logic.checkCollisonsWithWalls(ball, width, height);
            logic.checkCollisonsWithWalls(ball2, width, height);
            logic.checkCollisonsWithWalls(ball3, width, height);

            Assert.IsTrue(ball.Velocity.Y< 0, "Kulka powinna odwrócić velocity po uderzeniu.");
            Assert.IsTrue(ball2.Velocity.Y > 0, "Kulka nie powinna odwrócić prędkość po dotraciu do granicy obszaru..");
            Assert.IsTrue(ball3.Velocity.Y < 0, "Kulka powinna odwrócić prędkość po dotraciu do granicy obszaru..");
        }
        [TestMethod]
        public void constructorExceptionTest()
        {
            Assert.Throws<ArgumentNullException>(() => new SimulationLogic(null));

        }
        [TestMethod]
        public void checkCollisonsWithWallsMethodExceptionTest()
        {
            SimulationLogic sl = new SimulationLogic(new BallRepository());
            IBall ball = new Ball(0,0,1,1,1);
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.checkCollisonsWithWalls(ball,- 1,5));
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.checkCollisonsWithWalls(ball, 5, -5));

        }
        [TestMethod]
        public void initalizeMethodTest()
        {
            SimulationLogic sl = new SimulationLogic(new BallRepository());
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.Initialize(-1,4,5));
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.Initialize(2, 0, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.Initialize(2, -1, 5));
            Assert.Throws<ArgumentOutOfRangeException>(() => sl.Initialize(2, 5, 0));
            int ballsNumber = 2;
            sl.Initialize(ballsNumber, 20, 25);
            Assert.HasCount(ballsNumber, sl.Balls);
        }
        [TestMethod]
        public void separteBallsTest()
        {
            double diameter = 10;
            double positionX = 19;
            double positionY = 10;
            //środki kulek pokrywją sie
            IBall ball = new Ball(positionX: positionX, positionY: positionY, velocityX: 0, velocityY: -4, diameter: diameter);
            IBall ball2 = new Ball(positionX: positionX, positionY: positionY, velocityX: 0, velocityY: 3, diameter: diameter);
            SimulationLogic.SeparateBalls(ball, ball2);
            Assert.AreNotEqual(ball.Position.X, ball2.Position.X);
            
            //kulki częściowo się pokrywają
            ball.Position = new Vector(positionX, positionY);
            ball2.Position = new Vector(positionX - 0.5*diameter, positionY - 0.5 * diameter);
            double distance = SimulationLogic.DistanceBetweenBallsCenters(ball, ball2);
            double minDistance = SimulationLogic.SumOfBallsRadiuses(ball, ball2);
            Assert.IsLessThan(minDistance, distance);
            SimulationLogic.SeparateBalls(ball, ball2);
            double distanceAfetr = SimulationLogic.DistanceBetweenBallsCenters(ball, ball2);
            Assert.IsGreaterThanOrEqualTo(distanceAfetr, minDistance);
            
            //kulki się nie pokrywają - nic nie powinno się zmienić
            ball.Position = new Vector(positionX, positionY);
            double positionX2 = positionX + 2 * diameter;
            double positionY2 = positionY + 2 * diameter;
            ball2.Position = new Vector(positionX2, positionY2);
            Assert.AreEqual(positionX, ball.Position.X);
            Assert.AreEqual(positionY, ball.Position.Y);
            Assert.AreEqual(positionX2, ball2.Position.X);
            Assert.AreEqual(positionY2, ball2.Position.Y);
            SimulationLogic.SeparateBalls(ball, ball2);
            Assert.AreEqual(positionX, ball.Position.X);
            Assert.AreEqual(positionY, ball.Position.Y);
            Assert.AreEqual(positionX2, ball2.Position.X);
            Assert.AreEqual(positionY2, ball2.Position.Y);

        }
    }
        
}