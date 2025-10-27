namespace Products.SharedKernel
{
    public record Result
    {
        public bool IsSuccess { get; init; }
        public bool IsFailure => !IsSuccess;
        public string? Error { get; init; }

        public static Result Ok() => new() { IsSuccess = true };
        public static Result Fail(string error) => new() { IsSuccess = false, Error = error };
    }

    public record Result<T> : Result
    {
        public T? Value { get; init; }
        public static Result<T> Ok(T value) => new() { IsSuccess = true, Value = value };
        public new static Result<T> Fail(string error) => new() { IsSuccess = false, Error = error };
    }
}
