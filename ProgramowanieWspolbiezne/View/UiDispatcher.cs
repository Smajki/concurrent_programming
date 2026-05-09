using System.Windows;
using Model;

namespace ProgramowanieWspolbiezne.Services;

public sealed class WpfUiDispatcher : IUiDispatcher
{
    public void Post(Action action)
    {
        Application.Current.Dispatcher.BeginInvoke(action);
    }
}