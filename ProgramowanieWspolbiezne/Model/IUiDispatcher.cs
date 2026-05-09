namespace Model;

public interface IUiDispatcher
{
    void Post(Action action);
}