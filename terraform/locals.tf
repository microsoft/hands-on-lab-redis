locals {
  resource_lowercase_array  = [lower(var.environment), substr(lower(var.location), 0, 2), lower(var.domain), random_id.resource_group_name_suffix.hex]
  resource_suffix_kebabcase = join("-", local.resource_lowercase_array)
  resource_suffix_lowercase = join("", local.resource_lowercase_array)

  tags = merge(
    var.tags,
    tomap(
      {
        "Environment" = var.environment,
        "Domain"      = var.domain,
      }
    )
  )
}

