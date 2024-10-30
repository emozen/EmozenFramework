using EOF.Utilities.ResultModel.Interfaces;

namespace EOF.Utilities.ResultModel
{
    public abstract class Result : IEOFResult
    {
        public Result(bool isSuccess)
        {
            IsSuccess = isSuccess;
        }

        public Result(bool isSuccess, string message) : this(isSuccess)
        {
            Message = message;
        }

        public bool IsSuccess { get; }

        public string Message { get; }
    }
}
