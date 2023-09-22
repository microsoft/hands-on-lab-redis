using System.Text.Json.Serialization;

namespace History.Api
{
    public class ProductView {
        [JsonPropertyName("productId")]
        public string ProductId { get; set; }

        [JsonPropertyName("productTitle")]
        public string ProductTitle { get; set; }

        [JsonPropertyName("date")]
        public string Date { get; set; }
    }
}
