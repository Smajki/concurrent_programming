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


            var uiDispatcher = new WpfUiDispatcher();
            DataContext = new MainViewModel(logic, uiDispatcher);

        }
    }
}