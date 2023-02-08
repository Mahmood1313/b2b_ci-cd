namespace B2BPriceAdmin.Core.Common.PagedResponse
{
    public class Response<T>
    {
        public Response()
        {
        }
        public Response(T data, string message = "")
        {
            Succeeded = true;
            Message = message;
            Data = data;
        }
        public static Response<T> Success(T data, string successMessage = "")
        {
            return new Response<T> { Succeeded = true, Data = data, Message = successMessage };
        }

        public static Response<T> Success(string successMessage)
        {
            return new Response<T> { Succeeded = true, Message = successMessage };
        }

        public static Response<T> Fail(string errorMessage)
        {
            return new Response<T> { Succeeded = false, Message = errorMessage };
        }

        public T Data { get; set; }
        public bool Succeeded { get; set; }
        public string Message { get; set; }
    }
}
