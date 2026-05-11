using Data;
using Logic;
using Model;

namespace ModelTest;

[TestClass]
public sealed class SimulationModelTests_RealLogic
{
    [TestMethod]
    public void StartInitializesBallsCorrectly()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var dispatcher = new ImmediateDispatcher();
        var model = new SimulationModel(logic, dispatcher);

        model.BallsCount = 3;

        model.Start();

        Assert.AreEqual(3, model.Balls.Count);
        Assert.IsTrue(model.IsRunning);

        Assert.IsTrue(model.Balls.All(b => b.Diameter > 0));
    }

    [TestMethod]
    public void StartUsesMinimumOneBall()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 0;

        model.Start();

        Assert.IsTrue(model.Balls.Count >= 1);
        Assert.IsTrue(model.IsRunning);
    }

    [TestMethod]
    public void StartRaisesPropertyChangedForIsRunning()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var model = new SimulationModel(logic, new ImmediateDispatcher());

        var changed = new List<string>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName!);

        model.Start();

        CollectionAssert.Contains(changed, nameof(model.IsRunning));
    }

    [TestMethod]
    public void StopSetsIsRunningToFalse()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.Start();
        Assert.IsTrue(model.IsRunning);

        model.Stop();

        Assert.IsFalse(model.IsRunning);
    }

    internal sealed class ImmediateDispatcher : IUiDispatcher
    {
        public void Post(Action action) => action();
    }
}