using Logic;
using ModelView;

public sealed class MainViewModel : Base
{
    public SimulationModel Model { get; }

    public RelayCommand StartCommand { get; }
    public RelayCommand StopCommand { get; }

    public MainViewModel(ISimulationLogic logic)
    {
        Model = new SimulationModel(logic);
        StartCommand = new RelayCommand(Model.Start, () => !Model.IsRunning);
        StopCommand = new RelayCommand(Model.Stop, () => Model.IsRunning);
        Model.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName == nameof(Model.IsRunning))
            {
                StartCommand.RaiseCanExecuteChanged();
                StopCommand.RaiseCanExecuteChanged();
            }
        };
    }

    public void Tick()
    {
        Model.Tick();
    }
}
