namespace GadgetShop.Models;

public class ApiResponse<T>
{
    public int Code { get; set; }
    public string? Message { get; set; }
    public List<string> Errors { get; set; } = new();
    public T? Data { get; set; }
    public bool IsSuccess => Code is >= 200 and < 300 && Errors.Count == 0;
}
