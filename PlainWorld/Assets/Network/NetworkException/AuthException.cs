using System;

namespace Assets.Network.NetworkException
{
    public class AuthException : Exception
    {
        public AuthException(string message) : base(message) { }
    }
}
