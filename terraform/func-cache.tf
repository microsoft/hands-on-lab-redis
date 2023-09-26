resource "azurerm_linux_function_app" "func_cache" {
  name                = format("func-cache-%s", local.resource_suffix_kebabcase)
  resource_group_name = azurerm_resource_group.this.name
  location            = azurerm_resource_group.this.location

  storage_account_name       = azurerm_storage_account.func.name
  storage_account_access_key = azurerm_storage_account.func.primary_access_key
  service_plan_id            = azurerm_service_plan.this.id

  tags = local.tags

  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    FUNCTIONS_WORKER_RUNTIME              = "dotnet-isolated"
    AZURE_REDIS_CONNECTION_STRING         = azurerm_redis_cache.this.primary_connection_string
    REDIS_PRODUCT_ALL                     = "products:all"
    CATALOG_API_URL                       = azurerm_api_management.this.gateway_url
    APPINSIGHTS_INSTRUMENTATIONKEY        = azurerm_application_insights.this.instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.this.connection_string
  }

  site_config {
    application_stack {
      dotnet_version = "7.0"
      use_dotnet_isolated_runtime = true
    }
  }
}
