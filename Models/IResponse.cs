namespace proxy_net.Models
{
    public interface IResponse
    {
        ResponseError @return { get; set; }
    }

    public class ResponseError
    {
        public int code { get; set; }
        public string? msg { get; set; }
        public bool error { get; set; }
    }

    public interface IErrorResponse : IResponse
    {
        int? code { get; set; }
        string? msg { get; set; }
        bool? error { get; set; }
    }
}