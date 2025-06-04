namespace TicketingSystem.SharedKernel.Results
{
    public class Result
    {
        public bool IsSuccess { get; protected set; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; protected set; }

        protected Result(bool isSuccess, string? error)
        {
            if (isSuccess && error != null)
                throw new InvalidOperationException();
            if (!isSuccess && error == null)
                throw new InvalidOperationException();

            IsSuccess = isSuccess;
            Error = error;
        }

        public static Result Success() => new Result(true, null);

        public static Result Failure(string error) => new Result(false, error);

        public static Result<T> Success<T>(T value) => Result<T>.Success(value);

        public static Result<T> Failure<T>(string error) => Result<T>.Failure(error);
    }

    public class Result<T> : Result
    {
        public T? Value { get; private set; }

        protected internal Result(T value) : base(true, null)
        {
            Value = value;
        }

        protected internal Result(string error) : base(false, error)
        {
            Value = default;
        }

        public static Result<T> Success(T value) => new Result<T>(value);

        public static new Result<T> Failure(string error) => new Result<T>(error);
    }
}
