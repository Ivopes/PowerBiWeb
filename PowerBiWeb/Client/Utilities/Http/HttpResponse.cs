namespace PowerBiWeb.Client.Utilities.Http
{
    public class HttpResponse
    {
        public bool IsSuccess { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
