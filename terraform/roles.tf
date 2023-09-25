// Cosmos Db role definition
resource "azurerm_cosmosdb_sql_role_definition" "this" {
  resource_group_name = azurerm_resource_group.this.name
  account_name        = azurerm_cosmosdb_account.this.name
  name                = "CosmosDBPasswordlessReadWrite"
  assignable_scopes   = [azurerm_cosmosdb_account.this.id]

  permissions {
    data_actions = [
      "Microsoft.DocumentDB/databaseAccounts/readMetadata",
      "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/items/*",
      "Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers/*"
    ]
  }
}

// App Service and Function App role assignments

resource "azurerm_cosmosdb_sql_role_assignment" "app_service" {
  resource_group_name = azurerm_resource_group.this.name
  account_name        = azurerm_cosmosdb_account.this.name
  role_definition_id  = azurerm_cosmosdb_sql_role_definition.this.id
  principal_id        = azurerm_linux_web_app.this.identity[0].principal_id
  scope               = azurerm_cosmosdb_account.this.id
}

// Function Apps

resource "azurerm_cosmosdb_sql_role_assignment" "function_app_cache" {
  resource_group_name = azurerm_resource_group.this.name
  account_name        = azurerm_cosmosdb_account.this.name
  role_definition_id  = azurerm_cosmosdb_sql_role_definition.this.id
  principal_id        = azurerm_linux_function_app.func_cache.identity[0].principal_id
  scope               = azurerm_cosmosdb_account.this.id
}

resource "azurerm_cosmosdb_sql_role_assignment" "function_app_history" {
  resource_group_name = azurerm_resource_group.this.name
  account_name        = azurerm_cosmosdb_account.this.name
  role_definition_id  = azurerm_cosmosdb_sql_role_definition.this.id
  principal_id        = azurerm_linux_function_app.func_history.identity[0].principal_id
  scope               = azurerm_cosmosdb_account.this.id
}
