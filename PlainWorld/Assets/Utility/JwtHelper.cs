using System;
using System.Text;

/*
 * Lightweight JWT decoding and claim extraction helper.
 *
 * Purpose:
 * - Decode JWT payload without external JSON or JWT libraries.
 * - Extract required authentication claims for client-side usage.
 *
 * Design notes:
 * - Does NOT validate signature or token integrity.
 * - Intended for trusted tokens issued by the game backend.
 * - Avoids additional DLL dependencies (Unity-friendly).
 *
 * Behavior:
 * - Decodes Base64Url JWT payload into JSON string.
 * - Extracts claims using string-based parsing.
 * - Supports standard and custom claim keys.
 *
 * Usage:
 * - DecodePayload(token) → JSON payload string.
 * - ParseClaims(json) → strongly-typed JwtClaims object.
 */

namespace Assets.Utility
{
    public static class JwtHelper
    {
        public static string DecodePayload(string jwt)
        {
            var parts = jwt.Split('.');
            if (parts.Length != 3)
                throw new Exception("Invalid JWT");

            var payload = parts[1]
                .Replace('-', '+')
                .Replace('_', '/');

            switch (payload.Length % 4)
            {
                case 2: payload += "=="; break;
                case 3: payload += "="; break;
            }

            var bytes = Convert.FromBase64String(payload);
            return Encoding.UTF8.GetString(bytes);
        }

        public static JwtClaims ParseClaims(string payloadJson)
        {
            return new JwtClaims
            {
                UserId = GetClaim(
                    payloadJson,
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"
                ),

                Email = GetClaim(
                    payloadJson,
                    "http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"
                ),

                FullName = GetClaim(payloadJson, "FullName"),

                Role = GetClaim(
                    payloadJson,
                    "http://schemas.microsoft.com/ws/2008/06/identity/claims/role"
                ),

                Privileges = GetClaim(payloadJson, "Privileges"),

                Exp = GetLong(payloadJson, "exp")
            };
        }

        private static string GetClaim(string json, string key)
        {
            var pattern = $"\"{key}\":\"";
            var start = json.IndexOf(pattern);
            if (start < 0) return null;

            start += pattern.Length;
            var end = json.IndexOf('"', start);
            return json.Substring(start, end - start);
        }

        private static long GetLong(string json, string key)
        {
            var pattern = $"\"{key}\":";
            var start = json.IndexOf(pattern);
            if (start < 0) return 0;

            start += pattern.Length;
            var end = json.IndexOfAny(new[] { ',', '}' }, start);
            return long.Parse(json.Substring(start, end - start));
        }
    }

    public class JwtClaims
    {
        public string UserId;
        public string Email;
        public string FullName;
        public string Role;
        public string Privileges;
        public long Exp;
    }
}