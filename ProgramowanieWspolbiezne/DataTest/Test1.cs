
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
            Ball b = new Ball(initPositionX, initPositionY, initVelocityX, initVelocityY, initDiamater);
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
            Vector v = new Vector(initX, initY);
            Assert.AreEqual(initX, v.X);
            Assert.AreEqual(initY, v.Y);
            v.X = newX;
            Assert.AreEqual(newX, v.X);
            Assert.AreEqual(initY, v.Y);
            v.Y = newY;
            Assert.AreEqual(newY, v.Y);
        } 
    }
}
