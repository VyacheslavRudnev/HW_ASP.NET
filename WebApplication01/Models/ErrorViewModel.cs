namespace WebApplication01.Models;

public class ErrorViewModel //��� ���� ��������������� ��� ����������� ������� �� ������� Error.
{
    public string? RequestId { get; set; }

    public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
}
