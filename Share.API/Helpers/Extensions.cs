using System;
using Microsoft.AspNetCore.Http;

namespace Share.API.Helpers
{
    public static class Extensions
    {
        public static void AddApplicationError(this HttpResponse response, string message)
        {
            response.Headers.Add("Application-Errors", message);
            response.Headers.Add("Access-Control-Expose-Headers", "Application-Errors");
            response.Headers.Add("Allow-Control-Allow-Origin", "*");
        }
    }
}