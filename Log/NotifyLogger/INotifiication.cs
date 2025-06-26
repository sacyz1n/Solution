namespace Log
{
    public interface INotifiication
    {

        Task SendNotification(int eventId, string message);
    }
}
