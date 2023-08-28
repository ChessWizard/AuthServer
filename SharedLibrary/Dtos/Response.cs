using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SharedLibrary.Dtos
{
    public class Response<TData> where TData : class
    {
        public TData Data { get; set; }

        public int HttpStatusCode { get; set; }

        [JsonIgnore]
        public bool IsSuccessful { get; set; }

        public ErrorDto ErrorDto { get; set; }

        public static Response<TData> Success(TData data, int statusCode)
        {
            return new Response<TData> { Data = data, HttpStatusCode = statusCode };
        }

        public static Response<TData> Success(int statusCode)
        {
            return new Response<TData> { Data = default, HttpStatusCode = statusCode };
        }

        public static Response<TData> Error(ErrorDto errorDto, int statusCode)
        {
            return new Response<TData> { ErrorDto = errorDto, HttpStatusCode = statusCode, IsSuccessful = false };
        }

        public static Response<TData> Error(string errorMessage, int statusCode, bool isShow = true) 
        {
            return new Response<TData>
            {
                ErrorDto = new(errorMessage, isShow),
                HttpStatusCode = statusCode,
                IsSuccessful = false
            };
        }
    }
}
