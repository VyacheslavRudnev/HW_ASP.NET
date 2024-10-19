namespace WebApplication01.Models;

public class ErrorViewModel //Цей клас використовується для відображення помилок на сторінці Error.
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
