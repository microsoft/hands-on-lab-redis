resource "azurerm_service_plan" "this" {
  name                = format("asp-%s", local.resource_suffix_kebabcase)
  resource_group_name = local.resource_group_name
  location            = local.resource_group_location
  os_type             = "Linux"
  sku_name            = var.app_service_plan_sku
  tags                = local.tags
}
