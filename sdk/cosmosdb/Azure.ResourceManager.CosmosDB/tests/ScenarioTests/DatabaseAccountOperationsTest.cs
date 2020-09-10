// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.CosmosDB.Models;
using NUnit;
using NUnit.Framework;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    public class DatabaseAccountOperationsTest: CosmosDBManagementClientBase
    {
        private string location ;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName ;
        private string databaseAccountName;
        public DatabaseAccountOperationsTest(bool isAsync) : base(isAsync)
        {

        }
        private void GenerateRandomVariables()
        {
            location = CosmosDBTestUtilities.Location;
            resourceGroupName = Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
            databaseAccountName = Recording.GenerateAssetName("cosmosdb");
        }
        [SetUp]
        public async Task ClearAndInitialize()
        {
            GenerateRandomVariables();
            if (Mode == RecordedTestMode.Record || Mode == RecordedTestMode.Playback)
            {
                InitializeClients();
                //Creates or updates a resource group
                await CosmosDBTestUtilities.TryRegisterResourceGroupAsync(ResourceGroupsOperations,
                    location,
                    resourceGroupName);
            }
        }
        [TearDown]
        public async Task CleanupResourceGroup()
        {
            await CleanupResourceGroupsAsync();
        }
        [Test]
        public async Task DatabaseAccountCRUDTests()
        {
            // Create client
            var locations = new List<Location>()
                            {
                                {new Location(id:default(string),locationName: location, documentEndpoint:default(string), provisioningState: default(string), failoverPriority: default(int?), isZoneRedundant: default(bool?)) }
                            };
            CosmosDBManagementClient cosmosDBManagementClient = GetCosmosDBManagementClient();
            DatabaseAccountCreateUpdateParameters databaseAccountCreateUpdateParameters = new DatabaseAccountCreateUpdateParameters
            (
                id: default(string),
                name: default(string),
                type: default(string),
                location: location,
                tags: new Dictionary<string, string>
                    {
                        {"key1","value1"},
                        {"key2","value2"}
                    },
                kind: DatabaseAccountKind.MongoDB,
                consistencyPolicy: new ConsistencyPolicy(DefaultConsistencyLevel.BoundedStaleness)
                {
                    MaxStalenessPrefix = 300,
                    MaxIntervalInSeconds = 1000
                },
                locations: locations,
                databaseAccountOfferType: "Standard",
                ipRules: new List<IpAddressOrRange>
                {
                    new IpAddressOrRange("23.43.230.120")
                },
                isVirtualNetworkFilterEnabled: true,
                enableAutomaticFailover: true,
                capabilities: new List<Capability>(),
                virtualNetworkRules: new List<VirtualNetworkRule>(),
                enableMultipleWriteLocations: true,
                enableCassandraConnector: false,
                connectorOffer: "Small",
                disableKeyBasedMetadataWriteAccess: true,
                keyVaultKeyUri: default(string),
                publicNetworkAccess: default(PublicNetworkAccess),
                enableFreeTier: false,
                apiProperties: default(ApiProperties),
                enableAnalyticalStorage: true,
                cors: new List<CorsPolicy>()
                );
            DatabaseAccountGetResults databaseAccount =  await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartCreateOrUpdateAsync(resourceGroupName, databaseAccountName, databaseAccountCreateUpdateParameters));

            VerifyCosmosDBAccount(databaseAccount, databaseAccountCreateUpdateParameters);
            Assert.AreEqual(databaseAccountName, databaseAccount.Name);

            DatabaseAccountGetResults readDatabaseAccount = (await cosmosDBManagementClient.DatabaseAccounts.GetAsync(resourceGroupName, databaseAccountName)).Value;
            VerifyCosmosDBAccount(readDatabaseAccount, databaseAccountCreateUpdateParameters);
            Assert.AreEqual(databaseAccountName, readDatabaseAccount.Name);

            DatabaseAccountCreateUpdateParameters databaseAccountUpdateParameters = new DatabaseAccountCreateUpdateParameters
            (
                id: default(string),
                name: default(string),
                type: default(string),
                location: location,
                tags: new Dictionary<string, string>
                    {
                        {"key1","value1"},
                        {"key2","value2"}
                    },
                kind: DatabaseAccountKind.MongoDB,
                consistencyPolicy: new ConsistencyPolicy(DefaultConsistencyLevel.BoundedStaleness)
                {
                    MaxStalenessPrefix = 1300,
                    MaxIntervalInSeconds = 12000
                },
                locations: locations,
                databaseAccountOfferType: "Standard",
                ipRules: new List<IpAddressOrRange>
                {
                    new IpAddressOrRange("23.43.230.120")
                },
                isVirtualNetworkFilterEnabled: false,
                enableAutomaticFailover: true,
                capabilities: new List<Capability>(),
                virtualNetworkRules: new List<VirtualNetworkRule>(),
                enableMultipleWriteLocations: true,
                enableCassandraConnector: false,
                connectorOffer: "Small",
                disableKeyBasedMetadataWriteAccess: true,
                keyVaultKeyUri: default(string),
                publicNetworkAccess: default(PublicNetworkAccess),
                enableFreeTier: default(bool),
                apiProperties: default(ApiProperties),
                enableAnalyticalStorage: true,
                cors: new List<CorsPolicy>()
            );

            DatabaseAccountGetResults updatedDatabaseAccount = await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartCreateOrUpdateAsync(resourceGroupName, databaseAccountName, databaseAccountUpdateParameters));

            VerifyCosmosDBAccount(updatedDatabaseAccount, databaseAccountUpdateParameters);
            Assert.AreEqual(databaseAccountName, updatedDatabaseAccount.Name);

            IAsyncEnumerable<DatabaseAccountGetResults> databaseAccounts = cosmosDBManagementClient.DatabaseAccounts.ListAsync();
            Assert.NotNull(databaseAccounts);

            IAsyncEnumerable<DatabaseAccountGetResults> databaseAccountsByResourceGroupName = cosmosDBManagementClient.DatabaseAccounts.ListByResourceGroupAsync(resourceGroupName);
            Assert.NotNull(databaseAccountsByResourceGroupName);

            DatabaseAccountListKeysResult databaseAccountListKeysResult = (await cosmosDBManagementClient.DatabaseAccounts.ListKeysAsync(resourceGroupName, databaseAccountName)).Value;

            Assert.NotNull(databaseAccountListKeysResult.PrimaryMasterKey);
            Assert.NotNull(databaseAccountListKeysResult.SecondaryMasterKey);
            Assert.NotNull(databaseAccountListKeysResult.PrimaryReadonlyMasterKey);
            Assert.NotNull(databaseAccountListKeysResult.SecondaryReadonlyMasterKey);

            DatabaseAccountListConnectionStringsResult databaseAccountListConnectionStringsResult = (await cosmosDBManagementClient.DatabaseAccounts.ListConnectionStringsAsync(resourceGroupName, databaseAccountName)).Value;
            Assert.NotNull(databaseAccountListConnectionStringsResult);

            DatabaseAccountListReadOnlyKeysResult databaseAccountGetReadOnlyKeysResult = (await cosmosDBManagementClient.DatabaseAccounts.GetReadOnlyKeysAsync(resourceGroupName, databaseAccountName)).Value;
            Assert.NotNull(databaseAccountGetReadOnlyKeysResult);

            DatabaseAccountListReadOnlyKeysResult databaseAccountListReadOnlyKeysResult = (await cosmosDBManagementClient.DatabaseAccounts.ListReadOnlyKeysAsync(resourceGroupName, databaseAccountName)).Value;
            Assert.NotNull(databaseAccountListReadOnlyKeysResult);

            await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartRegenerateKeyAsync(resourceGroupName, databaseAccountName, new DatabaseAccountRegenerateKeyParameters("primary" )));
            await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartRegenerateKeyAsync(resourceGroupName, databaseAccountName, new DatabaseAccountRegenerateKeyParameters("secondary")));
            await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartRegenerateKeyAsync(resourceGroupName, databaseAccountName, new DatabaseAccountRegenerateKeyParameters("primaryReadonly")));
            await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartRegenerateKeyAsync(resourceGroupName, databaseAccountName, new DatabaseAccountRegenerateKeyParameters("secondaryReadonly")));

            DatabaseAccountListKeysResult databaseAccountListRegeneratedKeysResult =(await cosmosDBManagementClient.DatabaseAccounts.ListKeysAsync(resourceGroupName, databaseAccountName)).Value;

            int isNameExists = (await cosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName)).Status;
            Assert.AreEqual(isNameExists,200);

            await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartDeleteAsync(resourceGroupName, databaseAccountName));
        }
        private static void VerifyCosmosDBAccount(DatabaseAccountGetResults databaseAccount, DatabaseAccountCreateUpdateParameters parameters)
        {
            Assert.AreEqual(databaseAccount.Location.ToLower(), parameters.Location.ToLower());
            Assert.AreEqual(databaseAccount.Tags.Count, parameters.Tags.Count);
            Assert.True(databaseAccount.Tags.SequenceEqual(parameters.Tags));
            Assert.AreEqual(databaseAccount.Kind, parameters.Kind);
            VerifyConsistencyPolicy(databaseAccount.ConsistencyPolicy, parameters.ConsistencyPolicy);
            Assert.AreEqual(databaseAccount.IsVirtualNetworkFilterEnabled, parameters.IsVirtualNetworkFilterEnabled);
            Assert.AreEqual(databaseAccount.EnableAutomaticFailover, parameters.EnableAutomaticFailover);
            Assert.AreEqual(databaseAccount.EnableMultipleWriteLocations, parameters.EnableMultipleWriteLocations);
            //Assert.AreEqual(databaseAccount.EnableCassandraConnector, parameters.EnableCassandraConnector);
            //Assert.AreEqual(databaseAccount.ConnectorOffer, parameters.ConnectorOffer);
            Assert.AreEqual(databaseAccount.DisableKeyBasedMetadataWriteAccess, parameters.DisableKeyBasedMetadataWriteAccess);
        }

        private static void VerifyCosmosDBAccount(DatabaseAccountGetResults databaseAccount, DatabaseAccountUpdateParameters parameters)
        {
            VerifyConsistencyPolicy(databaseAccount.ConsistencyPolicy, parameters.ConsistencyPolicy);
            Assert.AreEqual(databaseAccount.IsVirtualNetworkFilterEnabled, parameters.IsVirtualNetworkFilterEnabled);
            Assert.AreEqual(databaseAccount.EnableAutomaticFailover, parameters.EnableAutomaticFailover);
            Assert.AreEqual(databaseAccount.EnableCassandraConnector, parameters.EnableCassandraConnector);
            Assert.AreEqual(databaseAccount.ConnectorOffer, parameters.ConnectorOffer);
            Assert.AreEqual(databaseAccount.DisableKeyBasedMetadataWriteAccess, parameters.DisableKeyBasedMetadataWriteAccess);
        }

        private static void VerifyConsistencyPolicy(ConsistencyPolicy actualValue, ConsistencyPolicy expectedValue)
        {
            Assert.AreEqual(actualValue.DefaultConsistencyLevel, expectedValue.DefaultConsistencyLevel);

            if (actualValue.DefaultConsistencyLevel == DefaultConsistencyLevel.BoundedStaleness)
            {
                Assert.AreEqual(actualValue.MaxIntervalInSeconds, expectedValue.MaxIntervalInSeconds);
                Assert.AreEqual(actualValue.MaxStalenessPrefix, expectedValue.MaxStalenessPrefix);
            }
        }
    }
}
