namespace functions;

internal class Const
{
    internal const string CATALOG_API_CLIENT = "catalogApi";
    internal static readonly string REDIS_KEY_PRODUCTS_ALL = Environment.GetEnvironmentVariable("REDIS_KEY_PRODUCTS_ALL");
}