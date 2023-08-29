public class Product
{
    public int Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }

    public string? Image { get; set; }

    public int? Quantity { get; set; }

    // Price in cents
    public int? Price { get; set; }
}
