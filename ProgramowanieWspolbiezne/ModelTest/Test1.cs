using Data;
using Logic;

[TestClass]
public sealed class SimulationModelTests
{
    [TestMethod]
    public void StartInitializesBallsCorrectly()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        model.BallsCount = 3;
        model.Start();

        Assert.HasCount(3, model.Balls);
        Assert.IsTrue(model.IsRunning);
        Assert.IsTrue(model.Balls.All(b => b.Diameter > 0));
    }

    [TestMethod]
    public void StartUsesMinimumOneBall()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        model.BallsCount = 0;
        model.Start();
        Assert.IsGreaterThanOrEqualTo(1, model.Balls.Count);
    }

    [TestMethod]
    public void StartRaisesPropertyChangedForIsRunning()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        var changed = new List<string>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName!);

        model.Start();

        CollectionAssert.Contains(changed, "IsRunning");
    }

    [TestMethod]
    public void StopSetsIsRunningToFalse()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        model.Start();
        model.Stop();

        Assert.IsFalse(model.IsRunning);
    }

    
    [TestMethod]
    public void TickDoesNothingWhenNotRunning()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);
        // brak Start()
        model.Tick();
        Assert.IsEmpty(model.Balls);
    }

    [TestMethod]
    public void TickUpdatesBallPositions()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        model.BallsCount = 1;
        model.Start();
        double oldX = model.Balls[0].X;
        double oldY = model.Balls[0].Y;

        model.Tick();

        Assert.AreNotEqual(oldX, model.Balls[0].X);
        Assert.AreNotEqual(oldY, model.Balls[0].Y);
    }

    [TestMethod]
    public void TickUpdatesAllBalls()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        var model = new SimulationModel(logic);

        model.BallsCount = 3;
        model.Start();

        var oldPositions = model.Balls.Select(b => (b.X, b.Y)).ToList();

        model.Tick();

        for (int i = 0; i < 3; i++)
        {
            Assert.AreNotEqual(oldPositions[i].X, model.Balls[i].X);
            Assert.AreNotEqual(oldPositions[i].Y, model.Balls[i].Y);
        }
    }
}
