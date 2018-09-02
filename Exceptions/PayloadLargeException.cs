using System;

namespace OfflineMessaging.Exceptions
{
    public class PayloadLargeException : Exception
    {
        public PayloadLargeException()
            : base()
        {
        }

        public PayloadLargeException(string message)
            : base(message)
        {
        }
    }
}