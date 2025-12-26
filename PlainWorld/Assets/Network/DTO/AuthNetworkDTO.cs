using System;

/*
 * NOTE: Auth Service call HTTP APIs only
 */

namespace Assets.Network.DTO
{
    // Request DTO
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

    // Response DTO
    [Serializable]
    public class TokenPayload
    {
        public string accessToken;
        public string refreshToken;
    }
}
