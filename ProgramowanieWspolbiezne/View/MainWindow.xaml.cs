using Data;
using Logic;
using ModelView;
using ProgramowanieWspolbiezne.Services;
using System.Windows;

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            IBallRepository repository = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repository);


            var uiTimer = new DispatcherUiTimer();
            DataContext = new MainViewModel(logic, uiTimer);

        }
    }
}