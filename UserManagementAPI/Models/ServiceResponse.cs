namespace UserManagementAPI.Models
{
    public class ServiceResponse<T>
    {
        public T? data { get; set; }
        public Boolean Success { get; set; } = true;
        public string Message { get; set; } = string.Empty;

        public static ServiceResponse<T> NotFound(string message)
        {
            return new ServiceResponse<T> { Success = false, Message = message };
        }
    }
}
