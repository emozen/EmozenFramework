using EOF.Utilities.ResultModel;

namespace GT.EOF.Utilities.ResultModel
{
    public class ErrorDataResult<T> : DataResult<T>
    {
        public ErrorDataResult(T data) : base(data, false)
        {

        }

        public ErrorDataResult(T data, string message) : base(data, false, message)
        {

        }
    }
}
