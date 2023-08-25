resource "azurerm_redis_cache" "this" {
  name                = format("redis-%s", local.resource_suffix_kebabcase)
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  capacity            = 1
  family              = "P"
  sku_name            = "Premium"
  enable_non_ssl_port = false
  minimum_tls_version = "1.2"

  tags = local.tags

  redis_configuration {}
}

resource "azurerm_api_management_redis_cache" "this" {
  name              = format("apim-redis-%s", local.resource_suffix_kebabcase)
  api_management_id = azurerm_api_management.this.id
  connection_string = azurerm_redis_cache.this.primary_connection_string
  description       = "Redis cache instances"
  redis_cache_id    = azurerm_redis_cache.this.id
  cache_location    = azurerm_resource_group.this.location
}