namespace Business.IServices
{
    public interface IChatHubService
    {
        Task ChatCompleted(long userId, long agentId, long chatId);
        Task ChatCreated(long userId, long agentId, long chatId);
        //... etc
    }
}
