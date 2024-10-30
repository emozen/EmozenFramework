namespace EOF.Utilities.ResultModel.Interfaces
{
    public interface IEOFResult
    {
        bool IsSuccess { get; }
        string Message { get; }
    }
}
