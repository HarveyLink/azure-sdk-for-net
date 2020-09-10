<<<<<<< HEAD
﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.TestFramework;
using Azure.ResourceManager.CosmosDB.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    [TestFixture]
    public class CassandraResourcesOperationsTests : CosmosDBManagementClientBase
    {
        protected string resourceGroupName;
        protected string databaseAccountName = "db12345";
        protected string keyspaceName = "keyspaceName2510";
        protected string keyspaceName2 = "keyspaceName22510";
        protected string tableName = "tableName2510";
        protected string cassandraThroughputType = "Microsoft.DocumentDB/databaseAccounts/cassandraKeyspaces/throughputSettings";
        protected int sampleThroughput = 700;

        public CassandraResourcesOperationsTests()
            : base(true)
        {
=======
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
    //Message: The CosmosDB SDK does not support create the Cassandra type accoun.Becaue need to manually create a Cassandra type account.
    //TODO: When SDK support.
    public class CassandraResourcesOperationsTests: CosmosDBManagementClientBase
    {
        private string location ;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName ;
        private string databaseAccountName ;

        private string keyspaceName;
        private string keyspaceName2;
        private string tableName;
        private string cassandraThroughputType;
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
        public CassandraResourcesOperationsTests(bool isAsync) : base(isAsync)
        {

        }
        private void GenerateRandomVariables()
        {
            location = "West US"; //CosmosDBTestUtilities.Location;
            //Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix)
            resourceGroupName = "cosmosdbrg-test-8381";//Create manually
            //Recording.GenerateAssetName("cosmos")
            databaseAccountName = "cassandra-test01"; //Create manually
            keyspaceName = Recording.GenerateAssetName("keyspaceName");
            keyspaceName2 = Recording.GenerateAssetName("keyspaceName");
            tableName = Recording.GenerateAssetName("tableName");
            cassandraThroughputType = "Microsoft.DocumentDB/databaseAccounts/cassandraKeyspaces/throughputSettings";
            sampleThroughput = 700;
>>>>>>> pr/7
        }

        [SetUp]
        public async Task ClearAndInitialize()
        {
<<<<<<< HEAD
            if (Mode == RecordedTestMode.Record || Mode == RecordedTestMode.Playback)
            {
                InitializeClients();
                resourceGroupName = Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
                await CosmosDBTestUtilities.TryRegisterResourceGroupAsync(ResourceGroupsOperations, CosmosDBTestUtilities.Location, resourceGroupName);
            }
        }

        [TearDown]
        public async Task CleanupResourceGroup()
        {
           await CleanupResourceGroupsAsync();
        }

        [TestCase]
        public async Task CassandraCRUDTestsAsync()
        {
            var locations = new List<Location>();
            Location loc = new Location();
            loc.LocationName = CosmosDBTestUtilities.Location;
            loc.FailoverPriority = 0;
            loc.IsZoneRedundant = false;
            locations.Add(loc);

            DatabaseAccountCreateUpdateParameters databaseAccountCreateUpdateParameters = new DatabaseAccountCreateUpdateParameters(locations);
            databaseAccountCreateUpdateParameters.Location = CosmosDBTestUtilities.Location;
            databaseAccountCreateUpdateParameters.Capabilities.Add(new Capability("EnableCassandra"));
            databaseAccountCreateUpdateParameters.ConsistencyPolicy = new ConsistencyPolicy(DefaultConsistencyLevel.Eventual);

            await WaitForCompletionAsync(await CosmosDBManagementClient.DatabaseAccounts.StartCreateOrUpdateAsync(resourceGroupName, databaseAccountName, databaseAccountCreateUpdateParameters));

            Task<Response> taskIsDatabaseNameExists = CosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName);
            Response isDatabaseNameExists = taskIsDatabaseNameExists.ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.AreEqual(200, isDatabaseNameExists.Status);

            CassandraKeyspaceCreateUpdateParameters cassandraKeyspaceCreateUpdateParameters = new CassandraKeyspaceCreateUpdateParameters(new CassandraKeyspaceResource(keyspaceName), new CreateUpdateOptions());

            var cassandraKeyspaceResponse = await WaitForCompletionAsync (await CosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName, cassandraKeyspaceCreateUpdateParameters));

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults = cassandraKeyspaceResponse.Value;

            Assert.NotNull(cassandraKeyspaceGetResults);
            Assert.AreEqual(keyspaceName, cassandraKeyspaceGetResults.Name);

            var cassandraKeyspaceResponse1 = await WaitForCompletionAsync(await CosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName, cassandraKeyspaceCreateUpdateParameters));

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults1 = cassandraKeyspaceResponse1.Value;
=======
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
            // await CleanupResourceGroupsAsync();
            await Task.Delay(2000);
        }
        [Test]
        public async Task CassandraCRUDTests()
        {
            // Create client
            CosmosDBManagementClient cosmosDBManagementClient = GetCosmosDBManagementClient();

            Response isDatabaseNameExists = await cosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName);
            if (isDatabaseNameExists.Status != 200)
            {
                return;
            }

            CassandraKeyspaceCreateUpdateParameters cassandraKeyspaceCreateUpdateParameters = new CassandraKeyspaceCreateUpdateParameters
            (
                new CassandraKeyspaceResource(keyspaceName),
                new CreateUpdateOptions()
            );

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName, cassandraKeyspaceCreateUpdateParameters));
            Assert.NotNull(cassandraKeyspaceGetResults);
            Assert.AreEqual(keyspaceName, cassandraKeyspaceGetResults.Name);

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults1 = (await cosmosDBManagementClient.CassandraResources.GetCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName)).Value;
>>>>>>> pr/7
            Assert.NotNull(cassandraKeyspaceGetResults1);
            Assert.AreEqual(keyspaceName, cassandraKeyspaceGetResults1.Name);

            VerifyEqualCassandraDatabases(cassandraKeyspaceGetResults, cassandraKeyspaceGetResults1);

<<<<<<< HEAD
            CassandraKeyspaceCreateUpdateParameters cassandraKeyspaceCreateUpdateParameters2 = new CassandraKeyspaceCreateUpdateParameters(new CassandraKeyspaceResource(keyspaceName2), new CreateUpdateOptions(sampleThroughput, default));

            var cassandraKeyspaceResponse2 = await WaitForCompletionAsync(await CosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName2, cassandraKeyspaceCreateUpdateParameters2));

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults2 = cassandraKeyspaceResponse2.Value;
            Assert.NotNull(cassandraKeyspaceGetResults2);
            Assert.AreEqual(keyspaceName2, cassandraKeyspaceGetResults2.Name);

            var keyspacesResponseTask = CosmosDBManagementClient.CassandraResources.ListCassandraKeyspacesAsync(resourceGroupName, databaseAccountName).ToEnumerableAsync();
            List<CassandraKeyspaceGetResults> cassandraKeyspaces = keyspacesResponseTask.ConfigureAwait(false).GetAwaiter().GetResult();

            Assert.NotNull(cassandraKeyspaces);

            var throughputResponse = CosmosDBManagementClient.CassandraResources.GetCassandraKeyspaceThroughputAsync(resourceGroupName, databaseAccountName, keyspaceName2);
            ThroughputSettingsGetResults throughputSettingsGetResults = throughputResponse.ConfigureAwait(false).GetAwaiter().GetResult();
=======
            CassandraKeyspaceCreateUpdateParameters cassandraKeyspaceCreateUpdateParameters2 = new CassandraKeyspaceCreateUpdateParameters(id: default(string), name:default(string), type: default(string), location: location, tags: tags, resource: new CassandraKeyspaceResource(keyspaceName2), options: new CreateUpdateOptions
            {
                Throughput = sampleThroughput
            });

            CassandraKeyspaceGetResults cassandraKeyspaceGetResults2 = (await WaitForCompletionAsync(await cosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, keyspaceName2, cassandraKeyspaceCreateUpdateParameters2)));
            Assert.NotNull(cassandraKeyspaceGetResults2);
            Assert.AreEqual(keyspaceName2, cassandraKeyspaceGetResults2.Name);

            IAsyncEnumerable<CassandraKeyspaceGetResults> cassandraKeyspaces = cosmosDBManagementClient.CassandraResources.ListCassandraKeyspacesAsync(resourceGroupName, databaseAccountName);
            Assert.NotNull(cassandraKeyspaces);

            ThroughputSettingsGetResults throughputSettingsGetResults = (await cosmosDBManagementClient.CassandraResources.GetCassandraKeyspaceThroughputAsync(resourceGroupName, databaseAccountName, keyspaceName2)).Value;
>>>>>>> pr/7
            Assert.NotNull(throughputSettingsGetResults);
            Assert.NotNull(throughputSettingsGetResults.Name);
            Assert.AreEqual(throughputSettingsGetResults.Resource.Throughput, sampleThroughput);
            Assert.AreEqual(cassandraThroughputType, throughputSettingsGetResults.Type);

<<<<<<< HEAD
            //TODO: Migrate To Manual and Autoscale tests from example: https://github.com/Azure/azure-rest-api-specs/blob/master/specification/cosmos-db/resource-manager/Microsoft.DocumentDB/stable/2020-04-01/examples/CosmosDBCassandraTableMigrateToAutoscale.json
            //var migrateKeyspace = await cosmosDBClient.CassandraResources.StartMigrateCassandraKeyspaceToManualThroughputAsync(resourceGroupName, databaseAccountName, keyspaceName);
            //var taskResponseMigrate = migrateKeyspace.WaitForCompletionAsync();
            //var responseMigrate = taskResponseMigrate.ConfigureAwait(false).GetAwaiter().GetResult();
            //Assert.AreEqual(responseMigrate.Value.Resource.MinimumThroughput, responseMigrate.Value.Resource.Throughput);

            CassandraSchema cassandraSchema = new CassandraSchema(new List<Column> { new Column { Name = "columnA", Type = "int" }, new Column { Name = "columnB", Type = "ascii" } }, new List<CassandraPartitionKey> { new CassandraPartitionKey { Name = "columnA" } }, new List<ClusterKey> { new ClusterKey { Name = "columnB", OrderBy = "Asc" } });
            CassandraTableResource cassandraTableResource = new CassandraTableResource(tableName, default, cassandraSchema, default);
            CassandraTableCreateUpdateParameters cassandraTableCreateUpdateParameters = new CassandraTableCreateUpdateParameters(cassandraTableResource, new CreateUpdateOptions());

            Response<CassandraTableGetResults> cassandraResponse = await WaitForCompletionAsync(await CosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraTableAsync(resourceGroupName, databaseAccountName, keyspaceName, tableName, cassandraTableCreateUpdateParameters));
            CassandraTableGetResults cassandraTableGetResults = cassandraResponse.Value;
            Assert.NotNull(cassandraTableGetResults);

            VerifyCassandraTableCreation(cassandraTableGetResults, cassandraTableCreateUpdateParameters);

            var cassandraPageableResultsTask = CosmosDBManagementClient.CassandraResources.ListCassandraTablesAsync(resourceGroupName, databaseAccountName, keyspaceName).ToEnumerableAsync();
            List<CassandraTableGetResults> cassandraTables = cassandraPageableResultsTask.ConfigureAwait(false).GetAwaiter().GetResult();
            Assert.NotNull(cassandraTables);
        }

=======
            CassandraTableCreateUpdateParameters cassandraTableCreateUpdateParameters = new CassandraTableCreateUpdateParameters
            (
                new CassandraTableResource(tableName)
                {
                    Schema = new CassandraSchema
                    (
                        columns: new List<Column> { new Column { Name = "columnA", Type = "int" }, new Column { Name = "columnB", Type = "ascii" } },
                        clusterKeys: new List<ClusterKey> { new ClusterKey { Name = "columnB", OrderBy = "Asc" } },
                        partitionKeys: new List<CassandraPartitionKey> { new CassandraPartitionKey { Name = "columnA" } }
                    )
                },
                new CreateUpdateOptions()
            );

            CassandraTableGetResults cassandraTableGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.CassandraResources.StartCreateUpdateCassandraTableAsync(resourceGroupName, databaseAccountName, keyspaceName, tableName, cassandraTableCreateUpdateParameters));
            Assert.NotNull(cassandraTableGetResults);
            VerifyCassandraTableCreation(cassandraTableGetResults, cassandraTableCreateUpdateParameters);

            IAsyncEnumerable<CassandraTableGetResults> cassandraTables = cosmosDBManagementClient.CassandraResources.ListCassandraTablesAsync(resourceGroupName, databaseAccountName, keyspaceName);
            Assert.NotNull(cassandraTables);

            await foreach (CassandraTableGetResults cassandraTable in cassandraTables)
            {
                await cosmosDBManagementClient.CassandraResources.StartDeleteCassandraTableAsync(resourceGroupName, databaseAccountName, keyspaceName, cassandraTable.Name);
            }

            await foreach (CassandraKeyspaceGetResults cassandraKeyspace in cassandraKeyspaces)
            {
                await cosmosDBManagementClient.CassandraResources.StartDeleteCassandraKeyspaceAsync(resourceGroupName, databaseAccountName, cassandraKeyspace.Name);
            }
        }
>>>>>>> pr/7
        private void VerifyCassandraTableCreation(CassandraTableGetResults cassandraTableGetResults, CassandraTableCreateUpdateParameters cassandraTableCreateUpdateParameters)
        {
            Assert.AreEqual(cassandraTableGetResults.Resource.Id, cassandraTableCreateUpdateParameters.Resource.Id);
            Assert.AreEqual(cassandraTableGetResults.Resource.Schema.Columns.Count, cassandraTableCreateUpdateParameters.Resource.Schema.Columns.Count);
            for (int i = 0; i < cassandraTableGetResults.Resource.Schema.Columns.Count; i++)
            {
                Assert.AreEqual(cassandraTableGetResults.Resource.Schema.Columns[i].Name, cassandraTableCreateUpdateParameters.Resource.Schema.Columns[i].Name);
                Assert.AreEqual(cassandraTableGetResults.Resource.Schema.Columns[i].Type, cassandraTableCreateUpdateParameters.Resource.Schema.Columns[i].Type);
            }

            Assert.AreEqual(cassandraTableGetResults.Resource.Schema.ClusterKeys.Count, cassandraTableCreateUpdateParameters.Resource.Schema.ClusterKeys.Count);
            for (int i = 0; i < cassandraTableGetResults.Resource.Schema.ClusterKeys.Count; i++)
            {
                Assert.AreEqual(cassandraTableGetResults.Resource.Schema.ClusterKeys[i].Name, cassandraTableCreateUpdateParameters.Resource.Schema.ClusterKeys[i].Name);
            }

            Assert.AreEqual(cassandraTableGetResults.Resource.Schema.PartitionKeys.Count, cassandraTableCreateUpdateParameters.Resource.Schema.PartitionKeys.Count);
            for (int i = 0; i < cassandraTableGetResults.Resource.Schema.PartitionKeys.Count; i++)
            {
                Assert.AreEqual(cassandraTableGetResults.Resource.Schema.PartitionKeys[i].Name, cassandraTableCreateUpdateParameters.Resource.Schema.PartitionKeys[i].Name);
            }
        }

        private void VerifyEqualCassandraDatabases(CassandraKeyspaceGetResults expectedValue, CassandraKeyspaceGetResults actualValue)
        {
<<<<<<< HEAD
            Assert.AreEqual(expectedValue.Id, actualValue.Id);
            Assert.AreEqual(expectedValue.Name, actualValue.Name);
            Assert.AreEqual(expectedValue.Location, actualValue.Location);
            Assert.AreEqual(expectedValue.Tags, actualValue.Tags);
            Assert.AreEqual(expectedValue.Type, actualValue.Type);

            Assert.AreEqual(expectedValue.Options, actualValue.Options);

=======
>>>>>>> pr/7
            Assert.AreEqual(expectedValue.Resource.Id, actualValue.Resource.Id);
            Assert.AreEqual(expectedValue.Resource.Rid, actualValue.Resource.Rid);
            Assert.AreEqual(expectedValue.Resource.Ts, actualValue.Resource.Ts);
            Assert.AreEqual(expectedValue.Resource.Etag, actualValue.Resource.Etag);
        }
    }
}
