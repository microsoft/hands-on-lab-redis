resource "azurerm_storage_account" "func" {
  name                     = format("stfunc%s", local.resource_suffix_lowercase)
  resource_group_name      = local.resource_group_name
  location                 = local.resource_group_location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.tags
}
