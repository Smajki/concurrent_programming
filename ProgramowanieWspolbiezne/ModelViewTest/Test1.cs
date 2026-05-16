using Data;
using Logic;
using Model;
using ModelView;


namespace ModelViewTest;

[TestClass]
public sealed class MainViewModelTests
{
    private static MainViewModel CreateVM(FakeSimulationLogic logic)
        => new MainViewModel(logic, new ImmediateDispatcher());

    [TestMethod]
    public void StartCommandExecutesAndStartsSimulation()
    {
        var logic = new FakeSimulationLogic();
        var vm = CreateVM(logic);

        Assert.IsTrue(vm.StartCommand.CanExecute(null));
        Assert.IsFalse(vm.StopCommand.CanExecute(null));

        vm.StartCommand.Execute(null);

        Assert.IsTrue(vm.Model.IsRunning);

        Assert.IsFalse(vm.StartCommand.CanExecute(null));
        Assert.IsTrue(vm.StopCommand.CanExecute(null));
    }

    [TestMethod]
    public void StopCommandExecutesAndStopsSimulation()
    {
        var logic = new FakeSimulationLogic();
        var vm = CreateVM(logic);

        vm.StartCommand.Execute(null);
        Assert.IsTrue(vm.Model.IsRunning);

        vm.StopCommand.Execute(null);

        Assert.IsFalse(vm.Model.IsRunning);
        Assert.IsTrue(vm.StartCommand.CanExecute(null));
        Assert.IsFalse(vm.StopCommand.CanExecute(null));
    }

    [TestMethod]
    public void CommandsRaiseCanExecuteChangedWhenIsRunningChanges()
    {
        var logic = new FakeSimulationLogic();
        var vm = CreateVM(logic);

        bool startChanged = false;
        bool stopChanged = false;

        vm.StartCommand.CanExecuteChanged += (_, __) => startChanged = true;
        vm.StopCommand.CanExecuteChanged += (_, __) => stopChanged = true;

        vm.StartCommand.Execute(null);

        Assert.IsTrue(startChanged);
        Assert.IsTrue(stopChanged);
    }

    [TestMethod]
    public void RelayCommand_Execute_InvokesAction()
    {
        bool executed = false;
        var cmd = new RelayCommand(() => executed = true);

        cmd.Execute(null);

        Assert.IsTrue(executed);
    }

    [TestMethod]
    public void CanExecuteRelayCommandReturnsTrue_WhenCanExecuteIsNull()
    {
        var cmd = new RelayCommand(() => { });
        Assert.IsTrue(cmd.CanExecute(null));
    }

    internal sealed class ImmediateDispatcher : IUiDispatcher
    {
        public void Post(Action action) => action();
    }

    internal sealed class FakeSimulationLogic : ISimulationLogic
    {
        public event EventHandler<BallStateChangedEventArgs>? BallStateChanged;

        private readonly List<IBall> _balls = new();
        public IReadOnlyList<IBall> Balls => _balls;

        public void Initialize(int ballsCount, double areaWidth, double areaHeight)
        {
            _balls.Clear();
            int count = Math.Max(1, ballsCount);

            for (int i = 0; i < count; i++)
            {
                var b = new Ball(100, 100, 0, 0, 25);
                b.Id = i;
                _balls.Add(b);
            }
        }

        public void checkCollisonsWithWalls(IBall ball, double areaWidth, double areaHeight)
        {
        }

        public Task MoveBallAsync(CancellationToken token, IBall ball, double areaWidth, double areaHeight)
            => Task.CompletedTask;

        public void Clear() => _balls.Clear();

        public ValueTask Dispose()
        {
            return ValueTask.CompletedTask;
        }
    }
}