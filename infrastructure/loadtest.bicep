targetScope = 'resourceGroup'

param targetName string
param location string

resource loadtest 'Microsoft.LoadTestService/loadTests@2022-04-15-preview' = {
  name: '${targetName}_loadtest'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
}

resource KeyVaultUserRole 'Microsoft.Authorization/roleDefinitions@2018-01-01-preview' existing = {
  scope: subscription()
  name: '4633458b-17de-408a-b874-0445c86b69e6'
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  scope: loadtest
  name: guid(loadtest.id, KeyVaultUserRole.id)
  properties: {
    roleDefinitionId: KeyVaultUserRole.id
    principalId: loadtest.identity.principalId
    principalType: 'ServicePrincipal'
  }
}
