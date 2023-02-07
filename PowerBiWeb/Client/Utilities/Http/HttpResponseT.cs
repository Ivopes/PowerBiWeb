namespace PowerBiWeb.Client.Utilities.Http
{
    public class HttpResponse<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
