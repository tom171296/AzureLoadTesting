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
