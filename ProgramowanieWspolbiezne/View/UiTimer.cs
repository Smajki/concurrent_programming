using System.Windows.Threading;
using Model;

namespace ProgramowanieWspolbiezne.Services;

public sealed class DispatcherUiTimer : IUiTimer
{
    private readonly DispatcherTimer _timer = new();
    private Action? _tick;

    public void Start(TimeSpan interval, Action tick)
    {
        _tick = tick;

        _timer.Stop();
        _timer.Interval = interval;

        _timer.Tick -= OnTick;
        _timer.Tick += OnTick;

        _timer.Start();
    }

    public void Stop()
    {
        _timer.Stop();
    }

    private void OnTick(object? sender, EventArgs e) => _tick?.Invoke();
}