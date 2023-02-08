namespace B2BPriceAdmin.Core.Exceptions
{
    public class UserFriendlyException : Exception
    {
        public UserFriendlyException() : base()
        {
        }

        public UserFriendlyException(string message) : base(message)
        {
        }
    }
}
