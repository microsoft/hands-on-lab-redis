public class Product
{
    public string id { get; set; }
    public string? title { get; set; }
    public string? description { get; set; }

    public string? image { get; set; }

    public int? quantity { get; set; }

    // Price in cents
    public int? price { get; set; }
}
