using System.Text.Json.Serialization;

namespace WebApplication1.DTOs.Common
{
    public class ResponseWrapper<T>
    {
        [JsonPropertyOrder(1)]
        public T Data { get; set; }

        public ResponseWrapper(T data)
        {
            Data = data;
        }
    }
}