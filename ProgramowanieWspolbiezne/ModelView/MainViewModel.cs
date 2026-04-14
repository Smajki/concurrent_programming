using System;
using System.Collections.ObjectModel;
using Logic;

namespace ModelView
{
    public sealed class MainViewModel : ViewModelBase
    {
        private readonly ISimulationLogic _logic;

        private int _ballsCount = 10;
        private bool _isRunning;

        public double PlayfieldWidth { get; } = 730;
        public double PlayfieldHeight { get; } = 430;

        public ObservableCollection<BallViewModel> Balls { get; } = new();

        public int BallsCount
        {
            get => _ballsCount;
            set => SetField(ref _ballsCount, value);
        }

        public bool IsRunning
        {
            get => _isRunning;
            private set
            {
                if (SetField(ref _isRunning, value))
                {
                    StartCommand.RaiseCanExecuteChanged();
                    StopCommand.RaiseCanExecuteChanged();
                }
            }
        }

        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

        public MainViewModel(ISimulationLogic logic)
        {
            _logic = logic ?? throw new ArgumentNullException(nameof(logic));

            StartCommand = new RelayCommand(Start, () => !IsRunning);
            StopCommand = new RelayCommand(Stop, () => IsRunning);
        }

        public void Tick()
        {
            if (!IsRunning) return;

            _logic.Step(PlayfieldWidth, PlayfieldHeight);

            var modelBalls = _logic.Balls;
            int n = Math.Min(modelBalls.Count, Balls.Count);

            for (int i = 0; i < n; i++)
                Balls[i].UpdateFrom(modelBalls[i]);
        }

        private void Start()
        {
            int count = BallsCount;
            if (count < 1) count = 1;
            if (count > 300) count = 300;

            _logic.Initialize(count, PlayfieldWidth, PlayfieldHeight);

            Balls.Clear();
            foreach (var ball in _logic.Balls)
            {
                var vm = new BallViewModel();
                vm.UpdateFrom(ball);
                Balls.Add(vm);
            }

            IsRunning = true;
        }

        private void Stop()
        {
            IsRunning = false;
        }
    }
}