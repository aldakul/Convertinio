namespace HtmlConverter.Application.Common.Exceptions
{
    public class HtmlSizeException : Exception
    {
        public HtmlSizeException(string name, object key)
            :base($"Entity \"{name}\" ({key})."){ }
    }
}
