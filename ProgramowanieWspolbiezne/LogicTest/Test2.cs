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
}
