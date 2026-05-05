using Data;
using Logic;
using System.Collections.ObjectModel;
using System.Windows.Threading;

public sealed class SimulationModel : Base
{
    private readonly ISimulationLogic _logic;

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

    public SimulationModel(ISimulationLogic logic)
    {
        _logic = logic;
    }

    public void Start()
    {
        if (IsRunning) return;

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

        Dispatcher uiDispatcher = Dispatcher.CurrentDispatcher;

        for (int i = 0; i < _modelBalls.Count; i++)
        {
            IBall modelBall = _modelBalls[i];
            BallExtended ballVm = Balls[i];

            _ = Task.Run(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var start = Environment.TickCount;

                    _logic.MoveBallOneStep(modelBall, PlayfieldWidth, PlayfieldHeight);

                    await uiDispatcher.BeginInvoke(() =>
                    {
                        ballVm.UpdateFrom(modelBall);
                    });

                    int elapsed = Environment.TickCount - start;
                    int delay = Math.Max(0, 16 - elapsed);
                    await Task.Delay(delay, token);
                }
            }, token);
        }

        IsRunning = true;
    }

    public void Stop()
    {
        if (!IsRunning) return;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        IsRunning = false;
    }
}