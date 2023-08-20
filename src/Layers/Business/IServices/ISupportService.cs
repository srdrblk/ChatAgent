using Dtos;

namespace Business.IServices
{
    public interface ISupportService
    {
        Task<BaseResponse<string>> CreateSupport(SupportDto supportDto, Guid userId);
        Task<bool> CheckCreateSupportIsAvailable();
    }
}
