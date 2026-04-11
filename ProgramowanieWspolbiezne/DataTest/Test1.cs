
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
            Ball b= new Ball(initPositionX, initPositionY, initVelocityX, initVelocityY, initDiamater);
            Assert.AreEqual(initPositionX, b.positionX);
            Assert.AreEqual(initPositionY, b.positionY);
            Assert.AreEqual(initVelocityX, b.velocityX);
            Assert.AreEqual(initVelocityY, b.velocityY);
            Assert.AreEqual(initDiamater, b.diameter);
            b.move();
            Assert.AreEqual(initPositionX + initVelocityX, b.positionX);
            Assert.AreEqual(initPositionY + initVelocityY, b.positionY);
            b.velocityX = 0;
            Assert.AreEqual(0, b.velocityX);
            b.velocityY = 4;
            Assert.AreEqual(4, b.velocityY);

        }
        [TestMethod]
        public void ballRepositoryTest() {
            BallRepository repo = new Data.BallRepository();
            Assert.IsEmpty(repo.GetAll());
            Ball b = new Ball(1,2,1,1,6);
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
    }
}
