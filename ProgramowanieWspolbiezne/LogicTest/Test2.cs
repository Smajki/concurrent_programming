using Data;
using Logic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LogicTest
{
    [TestClass]
    public sealed class Test2
    {
        [TestMethod]
        public void UpdateFromSetsCorrectValues()
        {
            var b = new BallExtended();
            var ball = new Ball(100, 200, 0, 0, 50);
            b.UpdateFrom(ball);
            Assert.AreEqual(100 - 25, b.X);
            Assert.AreEqual(200 - 25, b.Y);
            Assert.AreEqual(50, b.Diameter);
        }
        [TestMethod]
        public void UpdateFromRaisesPropertyChangedForAllPropertiesAndDoesNotRaisePropertyChangedWhenValuesAreSame()
        {
            var b = new BallExtended();
            var ball = new Ball(10, 20, 0, 0, 10);

            var changed = new List<string>();
            b.PropertyChanged += (_, e) => changed.Add(e.PropertyName!);
            b.UpdateFrom(ball);
            CollectionAssert.Contains(changed, "X");
            CollectionAssert.Contains(changed, "Y");
            CollectionAssert.Contains(changed, "Diameter");
            changed.Clear();
            b.UpdateFrom(ball); // te same wartości
            Assert.IsEmpty(changed);
        }
    }

    [TestClass]
    public sealed class SimulationLogicCheckBallsCollisionsMinimalTests
    {
        [TestMethod]
        public void CheckBallsCollisions_SeparatesOverlappingBalls()
        {
            // robimy overlap kul
            var repo = new BallRepository();
            var logic = new SimulationLogic(repo);
            string fName = "logicTest.txt";
            logic.Initialize(0, 10, 10);
            logic.setLogsFileName(fName);

            var b0 = new Ball(0, 0, 0, 0, 10) { Id = 0 };
            var b1 = new Ball(9, 0, 0, 0, 10) { Id = 1 };

            repo.Add(b0);
            repo.Add(b1);

            double minDist = SimulationLogic.SumOfBallsRadiuses(b0, b1);
            Assert.IsTrue(SimulationLogic.DistanceBetweenBallsCenters(b0, b1) < minDist);

            logic.CheckBallsCollisions(b0);

            // po wywołaniu nie powinno być overlapu
            Assert.IsTrue(SimulationLogic.DistanceBetweenBallsCenters(b0, b1) >= minDist);

            if (File.Exists("logicTest.txt"))
                File.Delete("logicTest.txt");
        }
    }

}
