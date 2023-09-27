namespace Hustle.Repositories
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }

        public static ApiResponse<T> DoMethod(Action<ApiResponse<T>> action)
        {
            ApiResponse<T> response = new ApiResponse<T>();
            try
            {
                action(response);
            }
            catch (Exception e)
            {
                response.Message = e.Message;
                response.Code = -1;
            }

            return response;
        }
    }
}