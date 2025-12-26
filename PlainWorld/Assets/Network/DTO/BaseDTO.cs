using System;

namespace Assets.Network.DTO
{
    // HTTP Base
    [Serializable]
    public class HttpResponse<T>
    {
        public T payload;
    }

    [Serializable]
    public class ErrorResponse
    {
        public string message;
    }
}
