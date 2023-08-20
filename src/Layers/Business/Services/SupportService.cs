using Business.IServices;
using Business.Queues;
using Dtos;
using Entities;

namespace Business.Services
{
    public class SupportService : ISupportService
    {
        SupportQueue supportQueue;
        public SupportService(SupportQueue _supportQueue)
        {
            supportQueue = _supportQueue;
        }
        public async Task<BaseResponse<string>> CreateSupport(SupportDto supportDto, Guid userId)
        {
            try
            {
                var support = new Support()
                {
                    Subject = supportDto.Subject,
                    CreatedDate = DateTime.Now,
                    User = new User()
                    {
                        Id = userId,
                        FullName = supportDto.User.FullName,
                    }
                };
                supportQueue.AddSupport(support);
            }
            catch (Exception)
            {

                throw;
            }
            return await Task.Run(() => new BaseResponse<string> { Statu = Common.Enums.ResponseStatu.Success });
        }
        public async Task<bool> CheckCreateSupportIsAvailable()
        {
            return await Task.Run(() => true);
        }
    }
}
