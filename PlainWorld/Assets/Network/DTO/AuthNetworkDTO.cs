using System;

namespace Assets.Network.DTO
{
    [Serializable]
    public class LoginRequest
    {
        public string email;
        public string password;
        public string roleCode;
    }

    [Serializable]
    public class RegisterRequest
    {
        public string email;
        public string password;
        public string fullName;
        public string gender;
        public string dob;
    }

    [Serializable]
    public class HttpResponse<T>
    {
        public T payload;
    }

    [Serializable]
    public class TokenPayload
    {
        public string accessToken;
        public string refreshToken;
    }
}
