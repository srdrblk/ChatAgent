using Dtos;

namespace Business.IServices
{
    public interface ISupportService
    {
        Task<bool> CheckCreateSupportIsAvailable();
    }
}
