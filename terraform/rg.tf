resource "azurerm_resource_group" "this" {
  count    = var.resource_group_name != "" ? 0 : 1
  name     = format("rg-%s", local.resource_suffix_kebabcase)
  location = var.location
  tags     = local.tags
}
