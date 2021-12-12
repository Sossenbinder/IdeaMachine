terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "=2.46.0"
    }
  }
}

variable "ideamachineenvironment" {
	type = string
	default = "ideamachine_${var.environment}"
}

# Configure the Microsoft Azure Provider
provider "azurerm" {
  features {}
  subscription_id = "8a636ffa-13b3-4b02-a482-e7d7a69593ec"
}

# Create a resource group
resource "azurerm_resource_group" "ideamachinerg" {
  name     = var.ideamachineenvironment
  location = "East US"
}

resource "azurerm_kubernetes_cluster" "example" {
  name                = var.ideamachineenvironment
  location            = azurerm_resource_group.ideamachinerg.location
  resource_group_name = azurerm_resource_group.ideamachinerg.name
  dns_prefix          = var.ideamachineenvironment

  default_node_pool {
    name       = "default"
    node_count = 1
    vm_size    = "Standard_D2_v2"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    Environment = var.environment
  }
}