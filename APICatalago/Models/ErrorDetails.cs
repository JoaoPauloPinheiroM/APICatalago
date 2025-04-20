using System.Text.Json;

namespace APICatalago.Models
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string? Message { get; set; }
        public string? Trace { get; set; }

        //Sobrescrevendo o método ToString para retornar a mensagem de erro em formato JSON
        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}