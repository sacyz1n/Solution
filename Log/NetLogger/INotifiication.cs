namespace Log
{
    public interface INotifiication
    {
        Task SendNotification(string message);
    }
}
