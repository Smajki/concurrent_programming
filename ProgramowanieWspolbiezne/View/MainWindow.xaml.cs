using System;
using System.Windows;
using System.Windows.Threading;
using Data;
using Logic;
using ModelView;

namespace View
{
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer _timer;

        public MainWindow()
        {
            InitializeComponent();

            IBallRepository repository = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repository);
            var vm = new MainViewModel(logic);

            DataContext = vm;

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(16)
            };

            _timer.Tick += (_, _) =>
            {
                if (DataContext is MainViewModel vm)
                    vm.Tick();
            };

            _timer.Start();
        }
    }
}