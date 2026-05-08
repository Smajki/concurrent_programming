namespace Model;

public interface IUiTimer
{
    void Start(TimeSpan interval, Action tick);
    void Stop();
}