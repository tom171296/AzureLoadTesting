targetScope= 'resourceGroup'

param target string
param location string

resource cosmosdb 'Microsoft.DocumentDB/databaseAccounts@2022-02-15-preview' = {
  name: '${target}20cosmosdb'
  location: location
  kind: 'GlobalDocumentDB'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    publicNetworkAccess: 'Enabled'
    databaseAccountOfferType: 'Standard'
    capacity: {
      totalThroughputLimit: 100
    }
    locations: [
      {
        locationName: location
        failoverPriority: 0
        isZoneRedundant: false
      }
    ]
  }
}

output connectionString string = cosmosdb.properties.locations[0].documentEndpoint
