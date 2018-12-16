namespace AirView.Application.Core
{
    public interface ICommand<out TResult> :
        ICommand
    {
    }
}