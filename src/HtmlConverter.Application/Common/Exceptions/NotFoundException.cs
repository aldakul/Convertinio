namespace HtmlConverter.Application.Common.Exceptions
{
    public class NotFoundException:Exception
    {
        public NotFoundException(object key)
        : base($"Entity with ({key}) not found.") { }
    }
}
