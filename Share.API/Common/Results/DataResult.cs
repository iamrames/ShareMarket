using Share.API.Enums;

namespace Share.API.Common.Results
{
    public class DataResult<T>: DataResult where T : class
    {
        public T Data { get; set; }
    }

    public class DataResult
    {
        public string Message { get; set; }
        public ResultTypeOption ResultType { get; set; }
    }
}