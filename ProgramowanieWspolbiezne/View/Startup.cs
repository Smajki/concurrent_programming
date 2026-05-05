using System.Windows;
using Data;
using Logic;
using ModelView;

namespace View
{
    public static class Startup
    {
        public static void Run(Application app)
        {
            IBallRepository repository = new BallRepository();
            ISimulationLogic logic = new SimulationLogic(repository);

            var vm = new MainViewModel(logic);

            var window = new MainWindow
            {
                DataContext = vm
            };

            app.MainWindow = window;
            window.Show();
        }
    }
}