targetScope= 'subscription'

param targetName string

var location = deployment().location

resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: targetName
  location: location
}

module loadtest 'loadtest.bicep' = {
  name: 'loadtest'
  scope: rg
  params: {
    targetName: targetName
    location: rg.location
  }
}

module cosmosdb 'cosmosdb.bicep' = {
  name: 'cosmosdb'
  scope: rg
  params: {
    target: targetName
    location: rg.location
  }
}

module site 'testapp.bicep' = {
  name: 'site'
  scope: rg
  params: {
    targetName: targetName
    location: rg.location
    cosmosConnectionString: cosmosdb.outputs.connectionString
  }
}
