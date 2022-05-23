targetScope='resourceGroup'

param targetName string
param location string
param cosmosConnectionString string

var cosmosDbAccountReaderRole = 'fbdf93bf-df7d-467e-a4d2-9458aa1360c8'

resource serverfarm 'Microsoft.Web/serverfarms@2021-03-01' = {
  name: '${targetName}_serverfarm'
  location: location
  sku: {
    name: 'P1V2'
    tier: 'PremiumV2'
    size: 'P1V2'
    family: 'P1V2'
    capacity: 1
  }
  kind: 'Linux'
  properties: {
    perSiteScaling: false
    elasticScaleEnabled: false
    maximumElasticWorkerCount: 1
  }
}

resource appservice 'Microsoft.Web/sites@2021-03-01' = {
  name: '${targetName}20site'
  location: location
  kind: 'container'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: serverfarm.id
    enabled: true
    hostNameSslStates: [
      {
        name: '${targetName}20appservice.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
      {
        name: '${targetName}20appservice.scm.azurewebsites.net'
        sslState: 'Disabled'
        hostType: 'Standard'
      }
    ]
    siteConfig: {
      acrUseManagedIdentityCreds: true
      appSettings: [
        {
          name: 'CONNECTION_STRING'
          value: cosmosConnectionString
        }
      ]
    }
  }
}

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2020-10-01-preview' = {
  name: guid(targetName, cosmosDbAccountReaderRole, 'appservice')
  properties: {
    principalType: 'ServicePrincipal'
    principalId: appservice.identity.principalId
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', cosmosDbAccountReaderRole)
  }
}
