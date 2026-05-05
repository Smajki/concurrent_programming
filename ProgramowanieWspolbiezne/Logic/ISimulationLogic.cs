using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ISimulationLogic
    {
        IReadOnlyList<IBall> Balls { get; }

        void Initialize(int ballsCount, double areaWidth, double areaHeight);

        void Step(double areaWidth, double areaHeight);

        public Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight);
        
        void MoveBallOneStep(IBall ball, double areaWidth, double areaHeight);
        void Clear();
    }
}
