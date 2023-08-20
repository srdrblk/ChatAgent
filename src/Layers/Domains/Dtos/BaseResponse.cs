using Common.Enums;

namespace Dtos
{
    public class BaseResponse<T>
    {
        public string Message { get; set; }
        public T Data { get; set; }

        public ResponseStatu Statu { get; set; }
    }
}