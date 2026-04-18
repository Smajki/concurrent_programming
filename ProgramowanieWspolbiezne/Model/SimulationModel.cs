using Data;
using Logic;
using System.Collections.ObjectModel;

public sealed class SimulationModel : Base
{
    private readonly ISimulationLogic _logic;

    public double PlayfieldWidth { get; } = 730;
    public double PlayfieldHeight { get; } = 430;

    public ObservableCollection<BallExtended> Balls { get; } = new();

    private IReadOnlyList<IBall> _modelBalls = Array.Empty<IBall>();

    private int _ballsCount = 10;
    public int BallsCount
    {
        get => _ballsCount;
        set => SetField(ref _ballsCount, value);
    }

    private bool _isRunning;
    public bool IsRunning
    {
        get => _isRunning;
        private set => SetField(ref _isRunning, value);
    }

    public SimulationModel(ISimulationLogic logic)
    {
        _logic = logic;

    }

    public void Start()
    {
        int count = Math.Max(1, BallsCount);

        _logic.Initialize(count, PlayfieldWidth, PlayfieldHeight);
        _modelBalls = _logic.Balls;

        Balls.Clear();
        foreach (var ball in _modelBalls)
        {
            var vm = new BallExtended();
            vm.UpdateFrom(ball);
            Balls.Add(vm);
        }

        IsRunning = true;
    }

    public void Stop()
    {
        IsRunning = false;
    }

    public void Tick()
    {
        if (!IsRunning) return;

        _logic.Step(PlayfieldWidth, PlayfieldHeight);

        for (int i = 0; i < _modelBalls.Count; i++)
            Balls[i].UpdateFrom(_modelBalls[i]);
    }
}
