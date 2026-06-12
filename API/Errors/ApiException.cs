namespace API.Errors;

public class ApiException(int statusCode, string Message, string? Details )
{
    public int StatusCode { get; set; } = statusCode;
    public string Message { get; set; } = Message;
    public string? Details { get; set; } = Details;
}
