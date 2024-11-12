namespace SavingsInvestment.Models
{
    public class ServiceResponse<T>
    {
        public bool Success { get; set; }
        public T Data { get; set; }
        public string Message { get; set; }
        public string ErrorDetails { get; set; }
        public DateTime Timestamp { get; set; }

        private ServiceResponse()
        {
            Timestamp = DateTime.UtcNow;
        }

        public static ServiceResponse<T> SuccessResponse(T data, string message = null)
        {
            return new ServiceResponse<T>
            {
                Success = true,
                Data = data,
                Message = message ?? "Operation completed successfully"
            };
        }

        public static ServiceResponse<T> ErrorResponse(string message, string errorDetails = null)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                ErrorDetails = errorDetails,
                Data = default
            };
        }

        public static ServiceResponse<T> NotFoundResponse(string message = "Requested resource not found")
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                Data = default
            };
        }

        public static ServiceResponse<T> ValidationErrorResponse(string message, string validationDetails)
        {
            return new ServiceResponse<T>
            {
                Success = false,
                Message = message,
                ErrorDetails = validationDetails,
                Data = default
            };
        }
    }
}
