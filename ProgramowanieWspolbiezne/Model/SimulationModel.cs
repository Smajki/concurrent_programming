using Data;
using Logic;
using Model;
using System.Collections.ObjectModel;

public sealed class SimulationModel : Base
{
    private readonly ISimulationLogic _logic;

    private readonly IUiDispatcher _uiDispatcher;
    private readonly Dictionary<int, BallExtended> _byId = new();
    public double PlayfieldWidth { get; } = 730;
    public double PlayfieldHeight { get; } = 430;

    private CancellationTokenSource? _cts;

    public ObservableCollection<BallExtended> Balls { get; } = new();


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

    public SimulationModel(ISimulationLogic logic, IUiDispatcher uiDispatcher)
    {
        _logic = logic;
        _uiDispatcher = uiDispatcher;
    }

    public void Start()
    {
        int count = Math.Max(1, BallsCount);

        _logic.Initialize(count, PlayfieldWidth, PlayfieldHeight);

        Balls.Clear();
        _byId.Clear();

        foreach (var ball in _logic.Balls)
        {
            var vm = new BallExtended();
            vm.UpdateFrom(ball);
            Balls.Add(vm);

            _byId[(int)ball.Id] = vm;
        }

        _logic.BallStateChanged += OnBallStateChanged;

        _cts = new CancellationTokenSource();
        CancellationToken token = _cts.Token;

        foreach (var ball in _logic.Balls)
        {
            _ = Task.Run(() => _logic.MoveBallAsync(token, ball, PlayfieldWidth, PlayfieldHeight), token);
        }

        IsRunning = true;
    }

    public void Stop()
    {
        _logic.BallStateChanged -= OnBallStateChanged;

        _cts?.Cancel();
        _cts?.Dispose();
        _cts = null;

        IsRunning = false;
    }

    private void OnBallStateChanged(object? sender, BallStateChangedEventArgs e)
    {
        if (!IsRunning) return;

        if (_byId.TryGetValue(e.Id, out var vm))
        {
            _uiDispatcher.Post(() =>
            {
                vm.UpdateFromState(e.CenterX, e.CenterY, e.Diameter);
            });
        }
    }

}