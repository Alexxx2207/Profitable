namespace Profitable.Common
{
    public class Result
    {
        public bool Succeeded { get; private set; }

        public string Error { get; private set; }

        public static implicit operator Result(bool succeded) => new Result
        {
            Succeeded = succeded
        };

        public static implicit operator Result(string error) => new Result
        {
            Succeeded = false,
            Error = error,
        };

    }
}