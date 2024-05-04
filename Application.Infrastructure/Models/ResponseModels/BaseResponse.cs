using Newtonsoft.Json;
using System.Net;

namespace Application.Infrastructure.Models.ResponseModels;

public class BaseResponse<T>
{
    public T? Data { get; set; }
    public bool Succeeded { get; set; }
    public string? Message { get; set; }
    public string[]? Errors { get; set; }
    public HttpStatusCode StatusCode { get; set; }
    public BaseResponse(bool success, string msg, T? data)
    {
        Data = data;
        Succeeded = success;
        Message = msg;
    }

    public BaseResponse()
    {
    }

    /// <summary>
    /// Sets the data to the appropriate response
    /// at run time
    /// </summary>
    /// <param name="errorMessage"></param>
    /// <returns></returns>
    public static BaseResponse<T> Fail(string errorMessage, string[]? errors = null, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        => new() { Succeeded = false, Message = errorMessage, Errors = errors, StatusCode = statusCode };

    public static BaseResponse<T> Success(string successMessage, T? data = default, HttpStatusCode statusCode = HttpStatusCode.OK)
       => new() { Succeeded = true, Message = successMessage, Data = data, StatusCode = statusCode };

    public override string ToString() => JsonConvert.SerializeObject(this);
}
