using System.Text.Json;
using Api.Entities.Helpers;

namespace Api.Extentions
{
    public static class HttpExtentions
    {
        public static void AddPaginationHeader(this HttpResponse response, PaginationHeader header)
        {
            var jsonOptions = new JsonSerializerOptions{PropertyNamingPolicy = JsonNamingPolicy.CamelCase};
            response.Headers.Add("Pagination", JsonSerializer.Serialize(header,jsonOptions));
            response.Headers.Add("Access-Control-Expose-Hearders","Pagination");
        }
    }
}