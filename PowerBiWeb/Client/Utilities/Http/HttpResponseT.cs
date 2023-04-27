namespace PowerBiWeb.Client.Utilities.Http
{
    public class HttpResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T? Value { get; set; } = default;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
