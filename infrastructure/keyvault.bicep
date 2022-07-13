targetScope = 'resourceGroup'

param targetName string
param location string
param tenantId string

resource keyvault 'Microsoft.KeyVault/vaults@2021-11-01-preview' = {
  name: '${targetName}-keyvault'
  location: location
  properties: {
    sku: {
      family: 'A'
      name: 'standard'
    }
    tenantId: tenantId
    enableRbacAuthorization: true
  }
}
