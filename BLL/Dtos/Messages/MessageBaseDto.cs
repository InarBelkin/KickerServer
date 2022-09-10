namespace BLL.Dtos.Messages;

public class MessageBaseDto
{
    public bool Success { get; set; } = false;
    public string Message { get; set; } = string.Empty;
}