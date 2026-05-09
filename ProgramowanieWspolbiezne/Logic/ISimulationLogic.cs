using System;
using Data;

namespace Logic
{
    public interface ISimulationLogic
    {
        event EventHandler<BallStateChangedEventArgs>? BallStateChanged;

        IReadOnlyList<IBall> Balls { get; }
        void Initialize(int ballsCount, double areaWidth, double areaHeight);
        void checkCollisonsWithWalls(IBall ball, double areaWidth, double areaHeight);
        Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight);
        void Clear();
    }
}
