namespace AirView.Application.Core
{
    // TODO(maximegelinas): Remove the possibility for a command to return a result.
    public interface ICommand<out TResult> :
        ICommand
    {
    }
}