// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.CosmosDB.Models;
using NUnit;
using NUnit.Framework;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    public class MongoResourcesOperationsTests: CosmosDBManagementClientBase
    {
        private string location ;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName ;
        private string databaseAccountName;

        private string databaseName;
        private string databaseName2;
        private string collectionName;

        private string mongoDatabaseThroughputType;

        private int sampleThroughput;
        private Dictionary<string, string> additionalProperties = new Dictionary<string, string>
        {
            {"foo","bar" }
        };
        private Dictionary<string, string> tags = new Dictionary<string, string>
        {
            {"key3","value3"},
            {"key4","value4"}
        };

        public MongoResourcesOperationsTests(bool isAsync) : base(isAsync)
        {

        }
        private void GenerateRandomVariables()
        {
            location = CosmosDBTestUtilities.Location;
            resourceGroupName = Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
            databaseAccountName = Recording.GenerateAssetName("cosmosdb");
            databaseName = Recording.GenerateAssetName("databaseName");
            databaseName2 = Recording.GenerateAssetName("databaseName");
            collectionName = Recording.GenerateAssetName("collectionName");
            mongoDatabaseThroughputType = "Microsoft.DocumentDB/databaseAccounts/mongodbDatabases/throughputSettings";
            sampleThroughput = 700;
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
        public async Task MongoCRUDTests()
        {
            // Create client
            CosmosDBManagementClient cosmosDBManagementClient = GetCosmosDBManagementClient();

            int isDatabaseNameExists = (await cosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName)).Status;

            DatabaseAccountGetResults databaseAccount = null;
            if (isDatabaseNameExists != 200)
            {
                var locations = new List<Location>()
                {
                  {new Location(id:default(string),locationName: location, documentEndpoint:default(string), provisioningState: default(string), failoverPriority: default(int?), isZoneRedundant: default(bool?)) }
                };
                DatabaseAccountCreateUpdateParameters databaseAccountCreateUpdateParameters = new DatabaseAccountCreateUpdateParameters(locations)
                {
                    Location = location,
                    Kind = DatabaseAccountKind.MongoDB,
                };
                databaseAccount = await WaitForCompletionAsync(await cosmosDBManagementClient.DatabaseAccounts.StartCreateOrUpdateAsync(resourceGroupName, databaseAccountName, databaseAccountCreateUpdateParameters));
                Assert.AreEqual(databaseAccount.Name, databaseAccountName);
            }

            MongoDBDatabaseCreateUpdateParameters mongoDBDatabaseCreateUpdateParameters = new MongoDBDatabaseCreateUpdateParameters
            (
                new MongoDBDatabaseResource(databaseName),
                new CreateUpdateOptions()
            );

            MongoDBDatabaseGetResults mongoDBDatabaseGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.MongoDBResources.StartCreateUpdateMongoDBDatabaseAsync(resourceGroupName, databaseAccountName, databaseName, mongoDBDatabaseCreateUpdateParameters));
            Assert.AreEqual(databaseName, mongoDBDatabaseGetResults.Name);
            Assert.NotNull(mongoDBDatabaseGetResults);

            MongoDBDatabaseGetResults mongoDBDatabaseGetResults1 = (await cosmosDBManagementClient.MongoDBResources.GetMongoDBDatabaseAsync(resourceGroupName, databaseAccountName, databaseName)).Value;
            Assert.NotNull(mongoDBDatabaseGetResults1);
            Assert.AreEqual(databaseName, mongoDBDatabaseGetResults1.Name);

            VerifyEqualMongoDBDatabases(mongoDBDatabaseGetResults, mongoDBDatabaseGetResults1);

            MongoDBDatabaseCreateUpdateParameters mongoDBDatabaseCreateUpdateParameters2 = new MongoDBDatabaseCreateUpdateParameters
            (
                new MongoDBDatabaseResource(databaseName2),
                new CreateUpdateOptions
                {
                    Throughput = sampleThroughput
                }
            );

            MongoDBDatabaseGetResults mongoDBDatabaseGetResults2 = await WaitForCompletionAsync(await cosmosDBManagementClient.MongoDBResources.StartCreateUpdateMongoDBDatabaseAsync(resourceGroupName, databaseAccountName, databaseName2, mongoDBDatabaseCreateUpdateParameters2));
            Assert.NotNull(mongoDBDatabaseGetResults2);
            Assert.AreEqual(databaseName2, mongoDBDatabaseGetResults2.Name);

            IAsyncEnumerable<MongoDBDatabaseGetResults> mongoDBDatabases = cosmosDBManagementClient.MongoDBResources.ListMongoDBDatabasesAsync(resourceGroupName, databaseAccountName);
            Assert.NotNull(mongoDBDatabases);

            ThroughputSettingsGetResults throughputSettingsGetResults = (await cosmosDBManagementClient.MongoDBResources.GetMongoDBDatabaseThroughputAsync(resourceGroupName, databaseAccountName, databaseName2)).Value;
            Assert.NotNull(throughputSettingsGetResults);
            Assert.NotNull(throughputSettingsGetResults.Name);
            Assert.AreEqual(throughputSettingsGetResults.Resource.Throughput, sampleThroughput);
            Assert.AreEqual(mongoDatabaseThroughputType, throughputSettingsGetResults.Type);

            MongoDBCollectionCreateUpdateParameters mongoDBCollectionCreateUpdateParameters = new MongoDBCollectionCreateUpdateParameters
            (
                new MongoDBCollectionResource(collectionName),
                new CreateUpdateOptions()
            );

            MongoDBCollectionGetResults mongoDBCollectionGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.MongoDBResources.StartCreateUpdateMongoDBCollectionAsync(resourceGroupName, databaseAccountName, databaseName, collectionName, mongoDBCollectionCreateUpdateParameters));
            Assert.NotNull(mongoDBCollectionGetResults);
            VerfiyMongoCollectionCreation(mongoDBCollectionGetResults, mongoDBCollectionCreateUpdateParameters);

            IAsyncEnumerable<MongoDBCollectionGetResults> mongoDBCollections = cosmosDBManagementClient.MongoDBResources.ListMongoDBCollectionsAsync(resourceGroupName, databaseAccountName, databaseName);
            Assert.NotNull(mongoDBCollections);

            await foreach (MongoDBCollectionGetResults mongoDBCollection in mongoDBCollections)
            {
                await cosmosDBManagementClient.MongoDBResources.StartDeleteMongoDBCollectionAsync(resourceGroupName, databaseAccountName, databaseName, mongoDBCollection.Name);
            }

            await foreach (MongoDBDatabaseGetResults mongoDBDatabase in mongoDBDatabases)
            {
                await cosmosDBManagementClient.MongoDBResources.StartDeleteMongoDBDatabaseAsync(resourceGroupName, databaseAccountName, mongoDBDatabase.Name);
            }
        }

        private void VerfiyMongoCollectionCreation(MongoDBCollectionGetResults mongoDBCollectionGetResults, MongoDBCollectionCreateUpdateParameters mongoDBCollectionCreateUpdateParameters)
        {
            Assert.AreEqual(mongoDBCollectionGetResults.Resource.Id, mongoDBCollectionCreateUpdateParameters.Resource.Id);
        }

        private void VerifyEqualMongoDBDatabases(MongoDBDatabaseGetResults expectedValue, MongoDBDatabaseGetResults actualValue)
        {
            Assert.AreEqual(expectedValue.Resource.Id, actualValue.Resource.Id);
            Assert.AreEqual(expectedValue.Resource.Rid, actualValue.Resource.Rid);
            Assert.AreEqual(expectedValue.Resource.Ts, actualValue.Resource.Ts);
            Assert.AreEqual(expectedValue.Resource.Etag, actualValue.Resource.Etag);
        }
    }
}
