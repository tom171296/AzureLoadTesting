# Azure Load Testing
This repo shows you how to integrate azure load testing into your CI/CD pipeline using github actions.

## Prerequisites
- [Github account](https://www.github.com);
- Azure account and subscription;
- Azure Cli installed and logged into azure;
- [VsCode](https://code.visualstudio.com/) installed;
- [Azure extension](https://code.visualstudio.com/docs/azure/extensions) for VsCode installed.

## Getting started
First we have to create the infrastucture needed for our load test to run. To get this infrastructure deployed, you can use the bicep files that are in the `./infrastructure` folder.This includes the following resources:
- Azure Load Testing;
- App service plan;
- App service;
- Azure cosmos DB account
- Key vault
- Role assignment to read data from keyvault;
- Role assignment to perform actions on cosmos DB.

To deploy the infrastructure to azure, run the following command from the root of this project. 

`az deployment sub create --location {location} --template-file .\infrastructure\main.bicep`

## 