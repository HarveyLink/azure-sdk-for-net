// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
<<<<<<< HEAD

using Azure.Core.TestFramework;
using Azure.ResourceManager.CosmosDB.Models;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    [TestFixture]
    public class TableResourcesOperationsTests : CosmosDBManagementClientBase
    {
        private string resourceGroupName;
        private const string databaseAccountName = "db2672";
        private const string tableName = "tableName2527";
        private const int sampleThroughput = 700;
        private const int defaultThroughput = 400;
        private const int defaultMaxThroughput = 4000;
        public TableResourcesOperationsTests()
            : base(true)
        {
        }

        [SetUp]
        public async Task ClearAndInitialize()
        {
            if (Mode == RecordedTestMode.Record || Mode == RecordedTestMode.Playback)
            {
                InitializeClients();
                this.resourceGroupName = Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
                await CosmosDBTestUtilities.TryRegisterResourceGroupAsync(ResourceGroupsOperations,
                    CosmosDBTestUtilities.Location,
                    this.resourceGroupName);
            }
        }

        [TearDown]
        public async Task CleanupResourceGroup()
        {
            await CleanupResourceGroupsAsync();
        }

        [TestCase]
        public async Task TableCRUDTest()
        {
            // prepare a database account
            List<Location> locationList = new List<Location>();
            locationList.Add(new Location(id: null, locationName: "WEST US", documentEndpoint: null, provisioningState: null, failoverPriority: null, isZoneRedundant: null));
            var databaseAccountsCreateOrUpdateParameters = new DatabaseAccountCreateUpdateParameters(locationList);
            databaseAccountsCreateOrUpdateParameters.Kind = DatabaseAccountKind.GlobalDocumentDB;
            databaseAccountsCreateOrUpdateParameters.Location = "WEST US";
            databaseAccountsCreateOrUpdateParameters.Capabilities.Add(new Capability("EnableTable"));
            await WaitForCompletionAsync(
                await CosmosDBManagementClient.DatabaseAccounts.StartCreateOrUpdateAsync(resourceGroupName, databaseAccountName, databaseAccountsCreateOrUpdateParameters));
            Assert.AreEqual(200, CosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName).Result.Status);

            var tableCreateUpdateParameters1 = new TableCreateUpdateParameters(new TableResource(tableName), new CreateUpdateOptions());
            TableGetResults tableGetResults1 = (
                await WaitForCompletionAsync(
                    await CosmosDBManagementClient.TableResources.StartCreateUpdateTableAsync(resourceGroupName, databaseAccountName, tableName, tableCreateUpdateParameters1))).Value;
            Assert.IsNotNull(tableGetResults1);
            Assert.AreEqual(tableName, tableGetResults1.Resource.Id);

            TableGetResults tableGetResults2 = (await CosmosDBManagementClient.TableResources.GetTableAsync(resourceGroupName, databaseAccountName, tableName)).Value;
            Assert.IsNotNull(tableGetResults2);
            VerifyTables(tableGetResults1, tableGetResults2);
            var actualThroughput = (
                await CosmosDBManagementClient.TableResources.GetTableThroughputAsync(resourceGroupName, databaseAccountName, tableName)).Value.Resource.Throughput;
            Assert.AreEqual(defaultThroughput, actualThroughput);

            var tableCreateUpdateParameters2 = new TableCreateUpdateParameters(new TableResource(tableName), new CreateUpdateOptions(sampleThroughput, new AutoscaleSettings()));
            TableGetResults tableGetResults3 = (
                await WaitForCompletionAsync(
                    await CosmosDBManagementClient.TableResources.StartCreateUpdateTableAsync(resourceGroupName, databaseAccountName, tableName, tableCreateUpdateParameters2))).Value;
            Assert.IsNotNull(tableGetResults3);
            Assert.AreEqual(tableName, tableGetResults3.Resource.Id);
            actualThroughput = (await CosmosDBManagementClient.TableResources.GetTableThroughputAsync(resourceGroupName, databaseAccountName, tableName)).Value.Resource.Throughput;
            Assert.AreEqual(sampleThroughput, actualThroughput);

            TableGetResults tableGetResults4 = (await CosmosDBManagementClient.TableResources.GetTableAsync(resourceGroupName, databaseAccountName, tableName)).Value;
            Assert.IsNotNull(tableGetResults4);
            VerifyTables(tableGetResults3, tableGetResults4);

            List<TableGetResults> tables = await CosmosDBManagementClient.TableResources.ListTablesAsync(resourceGroupName, databaseAccountName).ToEnumerableAsync();
            Assert.IsNotNull(tables);
            Assert.AreEqual(1, tables.Count);
            VerifyTables(tableGetResults4, tables[0]);

            ThroughputSettingsGetResults throughputSettingsGetResults1 =
                await WaitForCompletionAsync(await CosmosDBManagementClient.TableResources.StartMigrateTableToAutoscaleAsync(resourceGroupName, databaseAccountName, tableName));
            Assert.IsNotNull(throughputSettingsGetResults1.Resource.AutoscaleSettings);
            Assert.AreEqual(defaultMaxThroughput, throughputSettingsGetResults1.Resource.AutoscaleSettings.MaxThroughput);
            Assert.AreEqual(defaultThroughput, throughputSettingsGetResults1.Resource.Throughput);

            ThroughputSettingsGetResults throughputSettingsGetResults2 =
                await WaitForCompletionAsync(await CosmosDBManagementClient.TableResources.StartMigrateTableToManualThroughputAsync(resourceGroupName, databaseAccountName, tableName));
            Assert.IsNull(throughputSettingsGetResults2.Resource.AutoscaleSettings);
            Assert.AreEqual(defaultMaxThroughput, throughputSettingsGetResults2.Resource.Throughput);

            var throughputSettingsUpdateParameters = new ThroughputSettingsUpdateParameters(new ThroughputSettingsResource(defaultThroughput, null, null, null));
            ThroughputSettingsGetResults throughputSettingsGetResults = (
                await WaitForCompletionAsync(
                    await CosmosDBManagementClient.TableResources.StartUpdateTableThroughputAsync(resourceGroupName, databaseAccountName, tableName, throughputSettingsUpdateParameters))).Value;
            Assert.AreEqual(defaultThroughput, throughputSettingsGetResults.Resource.Throughput);

            await WaitForCompletionAsync(await CosmosDBManagementClient.TableResources.StartDeleteTableAsync(resourceGroupName, databaseAccountName, tableName));
            tables = await CosmosDBManagementClient.TableResources.ListTablesAsync(resourceGroupName, databaseAccountName).ToEnumerableAsync();
            Assert.IsNotNull(tables);
            Assert.AreEqual(0, tables.Count);
        }

        private void VerifyTables(TableGetResults expectedValue, TableGetResults actualValue)
        {
            Assert.AreEqual(expectedValue.Id, actualValue.Id);
            Assert.AreEqual(expectedValue.Name, actualValue.Name);
=======
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
    //Message: The CosmosDB SDK does not support create the Table type accoun.Becaue need to manually create a Table type account.
    //TODO: When SDK support.
    public class TableResourcesOperationsTests: CosmosDBManagementClientBase
    {
        private string location;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName;
        private string databaseAccountName;
        private string tableName;
        private string tableName2;

        private string tableThroughputType = "Microsoft.DocumentDB/databaseAccounts/tables/throughputSettings";

        private int sampleThroughput = 700;

        private Dictionary<string, string> additionalProperties = new Dictionary<string, string>
        {
            {"foo","bar" }
        };
        private Dictionary<string, string> tags = new Dictionary<string, string>
        {
            {"key3","value3"},
            {"key4","value4"}
        };
        public TableResourcesOperationsTests(bool isAsync) : base(isAsync)
        {

        }
        private void GenerateRandomVariables()
        {
            location = "West US"; //CosmosDBTestUtilities.Location;
            resourceGroupName = "cosmosdbrg-test-8381"; //Create manually. Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
            databaseAccountName = "azuretable-test01"; //Create manually. Recording.GenerateAssetName("cosmosdb");
            tableName = Recording.GenerateAssetName("tableName");
            tableName2 = Recording.GenerateAssetName("tableName");
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
            // await CleanupResourceGroupsAsync();
            await Task.Delay(2000);
        }
        [Test]
        public async Task TableCRUDTests()
        {
            // Create client
            CosmosDBManagementClient cosmosDBManagementClient = GetCosmosDBManagementClient();

            int isDatabaseNameExists = (await cosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName)).Status;

            if (isDatabaseNameExists != 200)
            {
                return;
            }

            TableCreateUpdateParameters tableCreateUpdateParameters = new TableCreateUpdateParameters
            (
                new TableResource(tableName),
                new CreateUpdateOptions()
            );

            TableGetResults tableGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.TableResources.StartCreateUpdateTableAsync(resourceGroupName, databaseAccountName, tableName, tableCreateUpdateParameters));
            Assert.NotNull(tableGetResults);
            Assert.AreEqual(tableName, tableGetResults.Name);

            TableGetResults tableGetResults2 = (await cosmosDBManagementClient.TableResources.GetTableAsync(resourceGroupName, databaseAccountName, tableName)).Value;
            Assert.NotNull(tableGetResults2);
            Assert.AreEqual(tableName, tableGetResults2.Name);

            VerifyEqualTables(tableGetResults, tableGetResults2);

            TableCreateUpdateParameters tableCreateUpdateParameters2 = new TableCreateUpdateParameters
            (
                id: default(string),
                name: default(string),
                type: default(string),
                location: location,
                tags: tags,
                resource: new TableResource(tableName2),
                options: new CreateUpdateOptions
                {
                    Throughput = sampleThroughput
                }
            );

            TableGetResults tableGetResults3 = await WaitForCompletionAsync(await cosmosDBManagementClient.TableResources.StartCreateUpdateTableAsync(resourceGroupName, databaseAccountName, tableName2, tableCreateUpdateParameters2));
            Assert.NotNull(tableGetResults3);
            Assert.AreEqual(tableName2, tableGetResults3.Name);

            IAsyncEnumerable<TableGetResults> tables = cosmosDBManagementClient.TableResources.ListTablesAsync(resourceGroupName, databaseAccountName);
            Assert.NotNull(tables);

            ThroughputSettingsGetResults throughputSettingsGetResults = (await cosmosDBManagementClient.TableResources.GetTableThroughputAsync(resourceGroupName, databaseAccountName, tableName2)).Value;
            Assert.NotNull(throughputSettingsGetResults);
            Assert.NotNull(throughputSettingsGetResults.Name);
            Assert.AreEqual(throughputSettingsGetResults.Resource.Throughput, sampleThroughput);
            Assert.AreEqual(tableThroughputType, throughputSettingsGetResults.Type);

            await foreach (TableGetResults table in tables)
            {
                await cosmosDBManagementClient.TableResources.StartDeleteTableAsync(resourceGroupName, databaseAccountName, table.Name);
            }
        }
        private void VerifyEqualTables(TableGetResults expectedValue, TableGetResults actualValue)
        {
>>>>>>> pr/7
            Assert.AreEqual(expectedValue.Resource.Id, actualValue.Resource.Id);
            Assert.AreEqual(expectedValue.Resource.Rid, actualValue.Resource.Rid);
            Assert.AreEqual(expectedValue.Resource.Ts, actualValue.Resource.Ts);
            Assert.AreEqual(expectedValue.Resource.Etag, actualValue.Resource.Etag);
        }
    }
}
