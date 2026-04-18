using Data;
using Logic;
using ModelView;



[TestClass]
public sealed class MainViewModelTests
{
    private MainViewModel CreateVM()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);
        return new MainViewModel(logic);
    }

    [TestMethod]
    public void StartCommandExecutesAndStartsSimulation()
    {
        var vm = CreateVM();

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
        var vm = CreateVM();

        vm.StartCommand.Execute(null);
        Assert.IsTrue(vm.Model.IsRunning);

        vm.StopCommand.Execute(null);

        Assert.IsFalse(vm.Model.IsRunning);
        Assert.IsTrue(vm.StartCommand.CanExecute(null));
        Assert.IsFalse(vm.StopCommand.CanExecute(null));
    }

    [TestMethod]
    public void StartCommandRaisesCanExecuteChangedWhenIsRunningChanges()
    {
        var vm = CreateVM();
        bool startChanged = false;
        bool stopChanged = false;
        vm.StartCommand.CanExecuteChanged += (_, __) => startChanged = true;
        vm.StopCommand.CanExecuteChanged += (_, __) => stopChanged = true;
        vm.StartCommand.Execute(null);
        Assert.IsTrue(startChanged);
        Assert.IsTrue(stopChanged);
    }

    [TestMethod]
    public void TickDelegatesToModelTick()
    {
        var vm = CreateVM();
        vm.Model.BallsCount = 1;
        vm.StartCommand.Execute(null);
        double oldX = vm.Model.Balls[0].X;
        double oldY = vm.Model.Balls[0].Y;
        vm.Tick();
        Assert.AreNotEqual(oldX, vm.Model.Balls[0].X);
        Assert.AreNotEqual(oldY, vm.Model.Balls[0].Y);
    }
    [TestMethod]
    public void ConstructorRelayCommandThrowsWhenExecuteIsNull()
    {
        Assert.Throws<ArgumentNullException>(() => new RelayCommand(null!));
    }

    [TestMethod]
    public void CanExecuteRelayCommandReturnsTrue_WhenCanExecuteIsNull()
    {
        var cmd = new RelayCommand(() => { });
        Assert.IsTrue(cmd.CanExecute(null));
    }
}
