resource "azurerm_static_web_app" "this" {
  name                = format("stapp-%s", local.resource_suffix_kebabcase)
  resource_group_name = local.resource_group_name
  location            = local.resource_group_location
  tags                = local.tags
}
