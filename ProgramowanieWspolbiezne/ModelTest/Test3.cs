using Data;
using Logic;
using Model;
using System;
using System.Threading.Tasks;

namespace ModelTest;

[TestClass]
public sealed class SimulationModelTests_RealLogic_Events
{
    [TestMethod]
    public async Task BallStateChangedUpdatesBallExtended_RealLogic()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 1;
        model.Start();

        Assert.AreEqual(1, model.Balls.Count);

        double oldX = model.Balls[0].X;
        double oldY = model.Balls[0].Y;

        await WaitForPositionChangeAsync(model.Balls[0], oldX, oldY, timeoutMs: 1000);

        model.Stop();

        Assert.AreNotEqual(oldX, model.Balls[0].X);
        Assert.AreNotEqual(oldY, model.Balls[0].Y);
    }

    [TestMethod]
    public async Task StopUnsubscribesFromBallUpdates_RealLogic()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        var model = new SimulationModel(logic, new ImmediateDispatcher());

        model.BallsCount = 1;
        model.Start();

        Assert.AreEqual(1, model.Balls.Count);

        double x0 = model.Balls[0].X;
        double y0 = model.Balls[0].Y;
        await WaitForPositionChangeAsync(model.Balls[0], x0, y0, timeoutMs: 1000);

        model.Stop();

        double xAfterStop = model.Balls[0].X;
        double yAfterStop = model.Balls[0].Y;

        await Task.Delay(200);

        Assert.AreEqual(xAfterStop, model.Balls[0].X, 0.0001);
        Assert.AreEqual(yAfterStop, model.Balls[0].Y, 0.0001);
    }

    private static async Task WaitForPositionChangeAsync(BallExtended ball, double oldX, double oldY, int timeoutMs)
    {
        var tcs = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);

        void Handler(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName is nameof(BallExtended.X) or nameof(BallExtended.Y))
            {
                if (ball.X != oldX || ball.Y != oldY)
                    tcs.TrySetResult();
            }
        }

        ball.PropertyChanged += Handler;
        try
        {
            Task completed = await Task.WhenAny(tcs.Task, Task.Delay(timeoutMs));
            Assert.AreSame(tcs.Task, completed, "Timeout");
        }
        finally
        {
            ball.PropertyChanged -= Handler;
        }
    }

    internal sealed class ImmediateDispatcher : IUiDispatcher
    {
        public void Post(Action action) => action();
    }
}