
using System.Runtime.Intrinsics;

namespace Data.Test
{
    [TestClass]
    public sealed class DataTest
    {
        [TestMethod]
        public void ballMethodsTest()
        {
            double initPositionX = 1;
            double initPositionY = 2;
            double initVelocityX = -3;
            double initVelocityY = 2;
            double initDiamater = 8;
            double mass = 4;
            Ball b = new Ball(initPositionX, initPositionY, initVelocityX, initVelocityY, initDiamater, mass);
            Assert.AreEqual(initPositionX, b.Position.X);
            Assert.AreEqual(initPositionY, b.Position.Y);
            Assert.AreEqual(initVelocityX, b.Velocity.X);
            Assert.AreEqual(initVelocityY, b.Velocity.Y);
            Assert.AreEqual(initDiamater, b.Diameter);
            b.Position.X = initPositionX + initVelocityX;
            b.Position.Y = initPositionY + initVelocityY;
            Assert.AreEqual(initPositionX + initVelocityX, b.Position.X);
            Assert.AreEqual(initPositionY + initVelocityY, b.Position.Y);
            b.Velocity.X = 0;
            Assert.AreEqual(0, b.Velocity.X);
            b.Velocity.Y = 4;
            Assert.AreEqual(4, b.Velocity.Y);
            Assert.AreEqual(mass, b.Mass);

        }
        [TestMethod]
        public void ballColideMethodTest()
        {
            double initPositionX = 1;
            double initPositionY = 2;
            double initVelocityX = -3;
            double initVelocityY = 2;
            double initVelocityX2 = 9;
            double initVelocityY2 = -8;
            double initDiamater = 8;
            double mass = 4;
            Ball b = new Ball(initPositionX, initPositionY, initVelocityX, initVelocityY, initDiamater, mass);
            Ball b1 = new Ball(initPositionX, initPositionY, initVelocityX2, initVelocityY2, initDiamater);
            Ball b2 = new Ball(initPositionX, initPositionY, initVelocityX2, initVelocityY2, initDiamater);
            Ball b3 = new Ball(initPositionX, initPositionY, initVelocityX, initVelocityY, initDiamater);
            b.Collide(b1);
            Assert.AreNotEqual(initVelocityX, b.Velocity.X);
            Assert.AreNotEqual(initVelocityY, b.Velocity.Y);
            Assert.AreNotEqual(initVelocityX2, b1.Velocity.X);
            Assert.AreNotEqual(initVelocityY2, b1.Velocity.Y);
            b2.Collide(b3); //mają równe masy
            Assert.AreEqual(initVelocityX2, b3.Velocity.X);
            Assert.AreEqual(initVelocityY2, b3.Velocity.Y);
            Assert.AreEqual(initVelocityX, b2.Velocity.X);
            Assert.AreEqual(initVelocityY, b2.Velocity.Y);

        }
        [TestMethod]
        public void ballRepositoryTest() {
            BallRepository repo = new Data.BallRepository();
            Assert.IsEmpty(repo.GetAll());
            Ball b = new Ball(1, 2, 1, 1, 6);
            repo.Add(b);
            Assert.AreEqual(1, repo.GetAll().Count());
            Ball b2 = new Ball(2, 2, 1, 1, 6);
            repo.Add(b2);
            Assert.AreEqual(2, repo.GetAll().Count());
            Assert.Contains(b, repo.GetAll());
            Assert.Contains(b2, repo.GetAll());
            repo.Remove(b);
            Assert.AreEqual(1, repo.GetAll().Count());
            Assert.Contains(b2, repo.GetAll());
            Assert.DoesNotContain(b, repo.GetAll());
            repo.RemoveAll();
            Assert.IsEmpty(repo.GetAll());
        }
        [TestMethod]
        public void vectorTest() {
            double initX = 10;
            double initY = -2;
            double newX = 10;
            double newY = -2;
            double n = 2.5;
            Vector v = new Vector(initX, initY);
            Assert.AreEqual(initX, v.X);
            Assert.AreEqual(initY, v.Y);

            v = v.MultiplyByNumber(n);
            Assert.AreEqual(initX * n, v.X);
            Assert.AreEqual(initY * n, v.Y);

            v.X = newX;
            Assert.AreEqual(newX, v.X);
            v.Y = newY;
            Assert.AreEqual(newY, v.Y);

            double X2 = 20;
            double Y2 = -4.5;
            Vector v2 = new Vector(X2, Y2);
            Vector v3 = v2 + v;
            Assert.AreEqual(v2.X + v.X, v3.X);
            Assert.AreEqual(v2.Y + v.Y, v3.Y);
        } 
    }
}
