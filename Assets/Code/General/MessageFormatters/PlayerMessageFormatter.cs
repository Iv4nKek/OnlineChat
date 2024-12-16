namespace Code.General.MessageFormatters
{
    public class PlayerMessageFormatter : IMessageFormatter
    {
        private readonly UserData _userData;

        public PlayerMessageFormatter(UserData userData)
        {
            _userData = userData;
        }

        public string Format(string message)
        {
            return $"<color=#F59C7A></b>{_userData.Username}</b>:<color=\"white\"> {message}";
        }
    }
}