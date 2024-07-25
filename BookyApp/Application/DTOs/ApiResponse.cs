using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.DTOs
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data { get; set; }
        //public List<T> ListOfData { get; set; }
        public Object DataResult { get; set; }
        public List<string> Errors { get; set; } = new List<string>();

        public ApiResponse()
        {
            Status = true;
        }
    }


    public class BaseResponse
    {
        public BaseResponse()
        {
        }

        public BaseResponse(int statusCode, bool? isSuccess, string responseMessage)
        {
            StatusCode = statusCode;
            IsSuccess = isSuccess;
            ResponseMessage = responseMessage;
        }
        public BaseResponse(bool success, int statusCode, string responseMessage, List<ValidationError> validationErrors = null)
        {
            IsSuccess = success;
            StatusCode = statusCode;
            ResponseMessage = responseMessage;
            ValidationErrors = validationErrors;
        }

        public int? StatusCode { get; set; }
        public bool? IsSuccess { get; set; }
        public string? ResponseMessage { get; set; }
        public List<ValidationError>? ValidationErrors { get; set; }
    }

    public class ValidationError
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }






}
