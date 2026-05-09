using Data;
using Logic;
using Model;

namespace ModelTest;

[TestClass]
public sealed class SimulationModelTests
{
    [TestMethod]
    public void StartInitializesBallsCorrectly()
    {
        var logic = new FakeSimulationLogic();
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
        var logic = new FakeSimulationLogic();
        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 0;

        model.Start();

        Assert.IsTrue(model.Balls.Count >= 1);
        Assert.IsTrue(model.IsRunning);
    }

    [TestMethod]
    public void StartRaisesPropertyChangedForIsRunning()
    {
        var logic = new FakeSimulationLogic();
        var model = new SimulationModel(logic, new ImmediateDispatcher());

        var changed = new List<string>();
        model.PropertyChanged += (_, e) => changed.Add(e.PropertyName!);

        model.Start();

        CollectionAssert.Contains(changed, nameof(model.IsRunning));
    }

    [TestMethod]
    public void StopSetsIsRunningToFalse()
    {
        var logic = new FakeSimulationLogic();
        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.Start();
        Assert.IsTrue(model.IsRunning);

        model.Stop();

        Assert.IsFalse(model.IsRunning);
    }

    [TestMethod]
    public void BallStateChangedUpdatesBallExtended()
    {
        var logic = new FakeSimulationLogic();
        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 1;
        model.Start();

        double oldX = model.Balls[0].X;
        double oldY = model.Balls[0].Y;

        logic.Emit(id: 0, centerX: 200, centerY: 210, diameter: 25);

        Assert.AreNotEqual(oldX, model.Balls[0].X);
        Assert.AreNotEqual(oldY, model.Balls[0].Y);

        Assert.AreEqual(200 - 12.5, model.Balls[0].X, 0.0001);
        Assert.AreEqual(210 - 12.5, model.Balls[0].Y, 0.0001);
        Assert.AreEqual(25, model.Balls[0].Diameter, 0.0001);
    }

    [TestMethod]
    public void StopUnsubscribesFromBallUpdates()
    {
        var logic = new FakeSimulationLogic();
        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 1;
        model.Start();
        model.Stop();

        double x = model.Balls[0].X;
        double y = model.Balls[0].Y;

        logic.Emit(id: 0, centerX: 300, centerY: 300, diameter: 25);

        Assert.AreEqual(x, model.Balls[0].X, 0.0001);
        Assert.AreEqual(y, model.Balls[0].Y, 0.0001);
    }

    
    internal sealed class ImmediateDispatcher : IUiDispatcher
    {
        public void Post(Action action) => action();
    }

    internal sealed class FakeSimulationLogic : ISimulationLogic
    {
        public event EventHandler<BallStateChangedEventArgs>? BallStateChanged;

        private readonly List<IBall> _balls = new();
        public IReadOnlyList<IBall> Balls => _balls;

        public void Initialize(int ballsCount, double areaWidth, double areaHeight)
        {
            _balls.Clear();

            int count = Math.Max(1, ballsCount);

            for (int i = 0; i < count; i++)
            {
                var b = new Ball(100, 100, 0, 0, 25);
                b.Id = i;
                _balls.Add(b);
            }
        }

        public void checkCollisonsWithWalls(IBall ball, double areaWidth, double areaHeight)
        {

        }

        public Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight)
        {
            return Task.CompletedTask;
        }

        public void Clear() => _balls.Clear();

        public void Emit(int id, double centerX, double centerY, double diameter)
        {
            BallStateChanged?.Invoke(this, new BallStateChangedEventArgs(id, centerX, centerY, diameter));
        }
    }
}