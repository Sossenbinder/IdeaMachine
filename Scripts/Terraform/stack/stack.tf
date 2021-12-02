terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.46.0"
    }
  }
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  features {}
  subscription_id = "8a636ffa-13b3-4b02-a482-e7d7a69593ec"
}

# Create a resource group
resource "azurerm_resource_group" "ideamachinerg" {
  name     = "ideamachine_${var.environment}"
  location = "East US"
}