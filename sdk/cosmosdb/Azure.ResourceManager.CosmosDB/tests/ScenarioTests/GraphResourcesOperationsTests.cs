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
    //Message: The CosmosDB SDK does not support create the Gremlin type accoun.Becaue need to manually create a Gremlin type account.
    //TODO: When SDK support.
    public class GraphResourcesOperationsTests: CosmosDBManagementClientBase
    {
        private string location ;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName ;
        private string databaseAccountName;

        private string databaseName;
        private string databaseName2;
        private string gremlinGraphName;
        private string graphThroughputType;

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
        public GraphResourcesOperationsTests(bool isAsync) : base(isAsync)
        {

        }
        private void GenerateRandomVariables()
        {
            location = CosmosDBTestUtilities.Location;

            resourceGroupName = "cosmosdbrg-test-8381";//Create manually. //Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
            databaseAccountName = "germlin-test01"; //Create manually. //Recording.GenerateAssetName("cosmosdb");
            databaseName = Recording.GenerateAssetName("databaseName");
            databaseName2 = Recording.GenerateAssetName("databaseName");
            gremlinGraphName = Recording.GenerateAssetName("gremlinGraphName");
            graphThroughputType = "Microsoft.DocumentDB/databaseAccounts/gremlinDatabases/throughputSettings";
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
            // await CleanupResourceGroupsAsync();
            await Task.Delay(2000);
        }
        [Test]
        public async Task GraphCRUDTests()
        {
            // Create client
            CosmosDBManagementClient cosmosDBManagementClient = GetCosmosDBManagementClient();

            int isDatabaseNameExists = (await cosmosDBManagementClient.DatabaseAccounts.CheckNameExistsAsync(databaseAccountName)).Status;

            if (isDatabaseNameExists != 200)
            {
                return;
            }

            GremlinDatabaseCreateUpdateParameters gremlinDatabaseCreateUpdateParameters = new GremlinDatabaseCreateUpdateParameters
            (
                new GremlinDatabaseResource(databaseName),
                new CreateUpdateOptions()
            );

            GremlinDatabaseGetResults gremlinDatabaseGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.GremlinResources.StartCreateUpdateGremlinDatabaseAsync(resourceGroupName, databaseAccountName, databaseName, gremlinDatabaseCreateUpdateParameters));
            Assert.NotNull(gremlinDatabaseGetResults);
            Assert.AreEqual(databaseName, gremlinDatabaseGetResults.Name);

            GremlinDatabaseGetResults gremlinDatabaseGetResults1 = (await cosmosDBManagementClient.GremlinResources.GetGremlinDatabaseAsync(resourceGroupName, databaseAccountName, databaseName)).Value;
            Assert.NotNull(gremlinDatabaseGetResults1);
            Assert.AreEqual(databaseName, gremlinDatabaseGetResults1.Name);

            VerifyEqualGremlinDatabases(gremlinDatabaseGetResults, gremlinDatabaseGetResults1);

            GremlinDatabaseCreateUpdateParameters gremlinDatabaseCreateUpdateParameters2 = new GremlinDatabaseCreateUpdateParameters
            (
                id: default(string),
                name: default(string),
                type: default(string),
                location: location,
                tags: tags,
                resource: new GremlinDatabaseResource(databaseName2),
                options: new CreateUpdateOptions
                {
                    Throughput = sampleThroughput
                }
            );

            GremlinDatabaseGetResults gremlinDatabaseGetResults2 = await WaitForCompletionAsync(await cosmosDBManagementClient.GremlinResources.StartCreateUpdateGremlinDatabaseAsync(resourceGroupName, databaseAccountName, databaseName2, gremlinDatabaseCreateUpdateParameters2));
            Assert.NotNull(gremlinDatabaseGetResults2);
            Assert.AreEqual(databaseName2, gremlinDatabaseGetResults2.Name);

            IAsyncEnumerable<GremlinDatabaseGetResults> gremlinDatabases = cosmosDBManagementClient.GremlinResources.ListGremlinDatabasesAsync(resourceGroupName, databaseAccountName);
            Assert.NotNull(gremlinDatabases);

            ThroughputSettingsGetResults throughputSettingsGetResults = (await cosmosDBManagementClient.GremlinResources.GetGremlinDatabaseThroughputAsync(resourceGroupName, databaseAccountName, databaseName2)).Value;
            Assert.NotNull(throughputSettingsGetResults);
            Assert.NotNull(throughputSettingsGetResults.Name);
            Assert.AreEqual(throughputSettingsGetResults.Resource.Throughput, sampleThroughput);
            Assert.AreEqual(graphThroughputType, throughputSettingsGetResults.Type);

            GremlinGraphCreateUpdateParameters gremlinGraphCreateUpdateParameters = new GremlinGraphCreateUpdateParameters
            (
                id: default(string),
                name: default(string),
                type: default(string),
                location: location,
                tags: tags,
                resource: new GremlinGraphResource(gremlinGraphName)
                {
                    DefaultTtl = -1,
                    PartitionKey = new ContainerPartitionKey
                    (
                        kind: "Hash",
                        paths: new List<string> { "/address" },
                        version: null
                    ),
                    IndexingPolicy = new IndexingPolicy
                    (
                        automatic: true,
                        indexingMode: IndexingMode.Consistent,
                        includedPaths: new List<IncludedPath>
                        {
                            new IncludedPath { Path = "/*"}
                        },
                        excludedPaths: new List<ExcludedPath>
                        {
                            new ExcludedPath { Path = "/pathToNotIndex/*"}
                        },
                        compositeIndexes: new List<IList<CompositePath>>
                        {
                            new List<CompositePath>
                            {
                                new CompositePath { Path = "/orderByPath1", Order = CompositePathSortOrder.Ascending },
                                new CompositePath { Path = "/orderByPath2", Order = CompositePathSortOrder.Descending }
                            },
                            new List<CompositePath>
                            {
                                new CompositePath { Path = "/orderByPath3", Order = CompositePathSortOrder.Ascending },
                                new CompositePath { Path = "/orderByPath4", Order = CompositePathSortOrder.Descending }
                            }
                        },
                        new List<SpatialSpec>
                             {
                                 new SpatialSpec
                                 (
                                         "/*",
                                         new List<SpatialType>
                                         {
                                              new SpatialType("Point")
                                         }
                                 ),
                             }
                    )
                },
                options: new CreateUpdateOptions
                {
                    Throughput = sampleThroughput
                }
            );

            GremlinGraphGetResults gremlinGraphGetResults = await WaitForCompletionAsync(await cosmosDBManagementClient.GremlinResources.StartCreateUpdateGremlinGraphAsync(resourceGroupName, databaseAccountName, databaseName, gremlinGraphName, gremlinGraphCreateUpdateParameters));
            Assert.NotNull(gremlinGraphGetResults);
            VerifyGremlinGraphCreation(gremlinGraphGetResults, gremlinGraphCreateUpdateParameters);

            IAsyncEnumerable<GremlinGraphGetResults> gremlinGraphs = cosmosDBManagementClient.GremlinResources.ListGremlinGraphsAsync(resourceGroupName, databaseAccountName, databaseName);
            Assert.NotNull(gremlinGraphs);

            await foreach (GremlinGraphGetResults gremlinGraph in gremlinGraphs)
            {
               await cosmosDBManagementClient.GremlinResources.StartDeleteGremlinGraphAsync(resourceGroupName, databaseAccountName, databaseName, gremlinGraph.Name);
            }

            await foreach (GremlinDatabaseGetResults gremlinDatabase in gremlinDatabases)
            {
               await cosmosDBManagementClient.GremlinResources.StartDeleteGremlinDatabaseAsync(resourceGroupName, databaseAccountName, gremlinDatabase.Name);
            }
        }
        private void VerifyGremlinGraphCreation(GremlinGraphGetResults gremlinGraphGetResults, GremlinGraphCreateUpdateParameters gremlinGraphCreateUpdateParameters)
        {
            Assert.AreEqual(gremlinGraphGetResults.Resource.Id, gremlinGraphCreateUpdateParameters.Resource.Id);
            Assert.AreEqual(gremlinGraphGetResults.Resource.IndexingPolicy.IndexingMode, gremlinGraphCreateUpdateParameters.Resource.IndexingPolicy.IndexingMode);
            //Assert.AreEqual(gremlinGraphGetResults.Resource.IndexingPolicy.ExcludedPaths, gremlinGraphCreateUpdateParameters.Resource.IndexingPolicy.ExcludedPaths);
            Assert.AreEqual(gremlinGraphGetResults.Resource.PartitionKey.Kind, gremlinGraphCreateUpdateParameters.Resource.PartitionKey.Kind);
            Assert.AreEqual(gremlinGraphGetResults.Resource.PartitionKey.Paths, gremlinGraphCreateUpdateParameters.Resource.PartitionKey.Paths);
            Assert.AreEqual(gremlinGraphGetResults.Resource.DefaultTtl, gremlinGraphCreateUpdateParameters.Resource.DefaultTtl);
        }

        private void VerifyEqualGremlinDatabases(GremlinDatabaseGetResults expectedValue, GremlinDatabaseGetResults actualValue)
        {
            Assert.AreEqual(expectedValue.Resource.Id, actualValue.Resource.Id);
            Assert.AreEqual(expectedValue.Resource.Rid, actualValue.Resource.Rid);
            Assert.AreEqual(expectedValue.Resource.Ts, actualValue.Resource.Ts);
            Assert.AreEqual(expectedValue.Resource.Etag, actualValue.Resource.Etag);
        }
    }
}
