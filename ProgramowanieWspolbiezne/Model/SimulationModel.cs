using Data;
using Logic;
using Model;
using System.Collections.ObjectModel;

public sealed class SimulationModel : Base
{
    private readonly ISimulationLogic _logic;
    private readonly IUiTimer _uiTimer;

    public double PlayfieldWidth { get; } = 730;
    public double PlayfieldHeight { get; } = 430;

    private CancellationTokenSource? _cts;

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

    public SimulationModel(ISimulationLogic logic, IUiTimer uiTimer)
    {
        _logic = logic;
        _uiTimer = uiTimer;
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

        _cts = new CancellationTokenSource();
        CancellationToken token = _cts.Token;

        foreach (var ball in _modelBalls)
        {
            _ = Task.Run(() =>
                _logic.MoveBallAsync(token, ball, PlayfieldWidth, PlayfieldHeight),
                token
            );
        }

        IsRunning = true;

        _uiTimer.Start(TimeSpan.FromMilliseconds(16), Tick);
    }

    public void Stop()
    {
        _uiTimer.Stop();

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        IsRunning = false;
    }

    public void Tick()
    {
        if (!IsRunning) return;

    //    _logic.Step(PlayfieldWidth, PlayfieldHeight);

        for (int i = 0; i < _modelBalls.Count; i++)
            Balls[i].UpdateFrom(_modelBalls[i]);
    }
}