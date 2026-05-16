using Data;
using Logic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModelTest;

[TestClass]
public sealed class ScreenProtection_DispatcherTests
{
    [TestMethod]
    public async Task BallExtended_IsUpdatedOnlyViaDispatcher_FlushRequired()
    {
        IBallRepository repo = new BallRepository();

        ISimulationLogic logic = new SimulationLogic(repo, random: new Random(123));

        var dispatcher = new QueuedDispatcher();
        var model = new SimulationModel(logic, dispatcher);

        model.BallsCount = 1;
        model.Start();

        Assert.AreEqual(1, model.Balls.Count);

        var ball = model.Balls[0];
        double x0 = ball.X;
        double y0 = ball.Y;
        bool gotWork = await WaitUntilAsync(() => dispatcher.PendingCount > 0, timeoutMs: 1500);
        Assert.IsTrue(gotWork, "Dispatcher nie zebrał żadnej akcji wiec BallStateChanged nie jest wywoływane");

        Assert.AreEqual(x0, ball.X, 0.0000001, "BallExtended.X zmienił się przed FlushAll(), co oznacza, że aktualizacja pozycji nie została zsynchronizowana przez IUiDispatcher.");
        Assert.AreEqual(y0, ball.Y, 0.0000001, "BallExtended.Y zmienił się przed FlushAll(), co oznacza, że aktualizacja pozycji nie została zsynchronizowana przez IUiDispatcher.");

        dispatcher.FlushAll();

        bool changedAfterFlush = await WaitUntilAsync(
            () => ball.X != x0 || ball.Y != y0,
            timeoutMs: 1500);

        model.Stop();

        Assert.IsTrue(changedAfterFlush, "pozycja kulek się nie zmienila");
    }

    private static async Task<bool> WaitUntilAsync(Func<bool> predicate, int timeoutMs, int stepMs = 10)
    {
        var start = DateTime.UtcNow;
        while ((DateTime.UtcNow - start).TotalMilliseconds < timeoutMs)
        {
            if (predicate()) return true;
            await Task.Delay(stepMs);
        }
        return predicate();
    }

    internal sealed class QueuedDispatcher : IUiDispatcher
    {
        private readonly object _lock = new object();
        private readonly Queue<Action> _queue = new Queue<Action>();

        public int PendingCount
        {
            get
            {
                lock (_lock) return _queue.Count;
            }
        }

        public void Post(Action action)
        {
            lock (_lock) _queue.Enqueue(action);
        }

        public void FlushAll()
        {
            while (true)
            {
                Action? a = null;
                lock (_lock)
                {
                    if (_queue.Count == 0) break;
                    a = _queue.Dequeue();
                }
                a();
            }
        }
    }
}