namespace ChargeSharedProto2.CustomExeptions
{
    public class UserNotFoundExeption : Exception
    {
        public UserNotFoundExeption()
        {
        }

        public UserNotFoundExeption(string message)
            : base(message)
        {
        }
    }
}
