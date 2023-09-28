resource "azurerm_api_management" "this" {
  name                = format("apim-%s", local.resource_suffix_kebabcase)
  location            = local.resource_group_location
  resource_group_name = local.resource_group_name
  publisher_name      = "My Company"
  publisher_email     = "company@me.io"

  sku_name = var.apim_sku

  tags = local.tags
}

resource "azurerm_api_management_logger" "this" {
  name                = format("apimlogs-%s", local.resource_suffix_kebabcase)
  api_management_name = azurerm_api_management.this.name
  resource_group_name = local.resource_group_name
  resource_id         = azurerm_application_insights.this.id

  application_insights {
    instrumentation_key = azurerm_application_insights.this.instrumentation_key
  }
}

resource "azurerm_api_management_product" "this" {
  product_id            = "catalog"
  api_management_name   = azurerm_api_management.this.name
  resource_group_name   = local.resource_group_name
  display_name          = "Catalog API"
  description           = "Catalog API for demo purposes."
  subscription_required = false
  approval_required     = false
  published             = true
}

resource "azurerm_api_management_api" "products" {
  name                  = "products"
  resource_group_name   = local.resource_group_name
  api_management_name   = azurerm_api_management.this.name
  subscription_required = azurerm_api_management_product.this.subscription_required
  revision              = "1"
  display_name          = "Products"
  path                  = "products"
  protocols             = ["https"]
  service_url           = format("https://%s", azurerm_linux_web_app.this.default_hostname)
}

resource "azurerm_api_management_product_api" "catalog_products" {
  api_name            = azurerm_api_management_api.products.name
  product_id          = azurerm_api_management_product.this.product_id
  api_management_name = azurerm_api_management.this.name
  resource_group_name = azurerm_api_management.this.resource_group_name
}

resource "azurerm_api_management_api_operation" "get_people" {
  operation_id        = "get-products"
  api_name            = azurerm_api_management_api.products.name
  api_management_name = azurerm_api_management_api.products.api_management_name
  resource_group_name = azurerm_api_management_api.products.resource_group_name
  display_name        = "Get Products"
  method              = "GET"
  url_template        = "/products"
  description         = "Get products."

  response {
    status_code = 200
  }
}
