using System;
using Data;

namespace Logic
{
    public interface ISimulationLogic
    {
        event EventHandler<BallStateChangedEventArgs>? BallStateChanged;

        IReadOnlyList<IBall> Balls { get; }
        void Initialize(int ballsCount, double areaWidth, double areaHeight, IDiagnosticLogger? logger = null);
        void checkCollisonsWithWalls(IBall ball, double areaWidth, double areaHeight);
        Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight, Func<Task> waitForTick);
        void Clear();
        public ValueTask Dispose();
    }
}
