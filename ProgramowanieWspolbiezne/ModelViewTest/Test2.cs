using Data;
using Logic;
using Model;
using ModelView;
using System;

namespace ModelViewTest;

[TestClass]
public sealed class MainViewModelTests_RealLogic
{
    private static MainViewModel CreateVM()
    {
        IBallRepository repo = new BallRepository();
        ISimulationLogic logic = new SimulationLogic(repo);

        return new MainViewModel(logic, new ImmediateDispatcher());
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
    public void CommandsRaiseCanExecuteChangedWhenIsRunningChanges()
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

    internal sealed class ImmediateDispatcher : IUiDispatcher
    {
        public void Post(Action action) => action();
    }
}