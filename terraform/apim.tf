resource "azurerm_api_management" "this" {
  name                = format("apim-%s", local.resource_suffix_kebabcase)
  location            = azurerm_resource_group.this.location
  resource_group_name = azurerm_resource_group.this.name
  publisher_name      = "My Company"
  publisher_email     = "company@me.io"

  sku_name = var.apim_sku

  tags = local.tags
}