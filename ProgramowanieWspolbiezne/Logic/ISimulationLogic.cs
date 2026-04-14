using System.Collections.Generic;
using Data;

namespace Logic
{
    public interface ISimulationLogic
    {
        IReadOnlyList<IBall> Balls { get; }

        void Initialize(int ballsCount, double areaWidth, double areaHeight);

        void Step(double areaWidth, double areaHeight);

        void Clear();
    }
}
