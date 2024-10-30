namespace EOF.Utilities.ResultModel.Interfaces
{
    public interface IDataResult<T> : IEOFResult
    {
        T Data { get; }
    }
}
