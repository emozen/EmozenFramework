using EOF.Utilities.ResultModel.Interfaces;

namespace EOF.Utilities.ResultModel
{
    public abstract class DataResult<T> : Result, IDataResult<T>
    {
        public DataResult(T data, bool isSuccess) : base(isSuccess)
        {
            Data = data;
        }

        public DataResult(T data, bool isSuccess, string message) : base(isSuccess, message)
        {
            Data = data;
        }

        public T Data { get; }
    }
}
