namespace Code.General.MessageFormatters
{
    public class NotificationMessageFormatter : IMessageFormatter
    {
        public string Format(string message)
        {
            return $"<color=\"yellow\">c{message}</b>";
        }
    }
}