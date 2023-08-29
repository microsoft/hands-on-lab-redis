resource "azurerm_storage_account" "func" {
  name                     = format("stfunc%s", local.resource_suffix_lowercase)
  resource_group_name      = azurerm_resource_group.this.name
  location                 = azurerm_resource_group.this.location
  account_tier             = "Standard"
  account_replication_type = "LRS"
  tags                     = local.tags
}

resource "azurerm_linux_function_app" "this" {
  name                = format("func-%s", local.resource_suffix_kebabcase)
  resource_group_name = azurerm_resource_group.this.name
  location            = azurerm_resource_group.this.location

  storage_account_name       = azurerm_storage_account.func.name
  storage_account_access_key = azurerm_storage_account.func.primary_access_key
  service_plan_id            = azurerm_service_plan.this.id

  tags = local.tags

  identity {
    type = "SystemAssigned"
  }

  site_config {}
}