resource "azurerm_storage_account" "func" {
  name                     = format("stfunc%s", local.resource_suffix_lowercase)
  resource_group_name      = azurerm_resource_group.this.name
  location                 = azurerm_resource_group.this.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.tags
}
