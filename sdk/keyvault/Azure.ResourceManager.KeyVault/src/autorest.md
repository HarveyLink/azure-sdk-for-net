# Generated code configuration

Run `dotnet build /t:GenerateCode` to generate code.

``` yaml
azure-arm: true
library-name: KeyVault
require: https://raw.githubusercontent.com/Azure/azure-rest-api-specs/87027c383aa7dd9e7ba5cf016e523a7837470329/specification/keyvault/resource-manager/readme.md
clear-output-folder: true
skip-csproj: true
namespace: Azure.ResourceManager.KeyVault
modelerfour:
  lenient-model-deduplication: true

model-namespace: false
public-clients: false
head-as-boolean: false
payload-flattening-threshold: 2
operation-group-to-resource-type:
    PrivateLinkResources: Microsoft.KeyVault/vaults/privateLinkResources
    MHSMPrivateLinkResources: Microsoft.KeyVault/managedHSMs/privateLinkResources
    Operations: Microsoft.KeyVault/operations
operation-group-to-resource:
    PrivateLinkResources: PrivateLinkResource
    MHSMPrivateLinkResources: MHSMPrivateLinkResource
    Operations: NonResource
```