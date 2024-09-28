namespace Service.Contracts;

public class ServiceResult
{
    public bool Success { get; set; } = true;
    public List<string> Errors { get; set; } = new();

    public void AddError(string errorMessage)
    {
        Success = false;
        Errors.Add(errorMessage);
    }
}