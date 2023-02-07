using System.Runtime.Serialization;

namespace PowerBiWeb.Server.Utilities.Exceptions
{
    public class MessageException : Exception
    {
        public string ExcptMessage { get; set; } = string.Empty;
        public MessageException()
        {
        }

        public MessageException(string? message) : base(message)
        {
        }

        public MessageException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
