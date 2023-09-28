resource "azurerm_linux_web_app" "this" {
  name                = format("app-%s", local.resource_suffix_kebabcase)
  resource_group_name = local.resource_group_name
  location            = azurerm_service_plan.this.location
  service_plan_id     = azurerm_service_plan.this.id
  tags                = local.tags

  https_only = true

  identity {
    type = "SystemAssigned"
  }

  app_settings = {
    AZURE_COSMOS_CONNECTION_STRING        = azurerm_cosmosdb_account.this.connection_strings[0]
    AZURE_COSMOS_DATABASE                 = "catalogdb"
    AZURE_REDIS_CONNECTION_STRING         = azurerm_redis_cache.this.primary_connection_string
    PRODUCT_LIST_CACHE_DISABLE            = "0"
    SIMULATED_DB_LATENCY_IN_SECONDS       = "2"
    APPINSIGHTS_INSTRUMENTATIONKEY        = azurerm_application_insights.this.instrumentation_key
    APPLICATIONINSIGHTS_CONNECTION_STRING = azurerm_application_insights.this.connection_string
  }

  site_config {
    application_stack {
      dotnet_version = "7.0"
    }
  }

  lifecycle {
    ignore_changes = [
      site_config["application_insights_connection_string"],
      site_config["application_insights_key"]
    ]
  }
}
