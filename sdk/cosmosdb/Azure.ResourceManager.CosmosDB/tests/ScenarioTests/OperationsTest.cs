// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Azure.Core.TestFramework;
using NUnit.Framework;
using System.Threading.Tasks;

namespace Azure.ResourceManager.CosmosDB.Tests
{
    public class OperationsTest : CosmosDBManagementClientBase
    {
        private string location;

        // using an existing DB account, since Account provisioning takes 10-15 minutes
        private string resourceGroupName;
        public OperationsTest(bool isAsync) : base(isAsync)
        {
        }
        private void GenerateRandomVariables()
        {
            location = CosmosDBTestUtilities.Location;
            resourceGroupName = Recording.GenerateAssetName(CosmosDBTestUtilities.ResourceGroupPrefix);
        }
        [SetUp]
        public async Task ClearAndInitialize()
        {
            GenerateRandomVariables();
            if (Mode == RecordedTestMode.Record || Mode == RecordedTestMode.Playback)
            {
                InitializeClients();
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

        [TestCase]
        public async Task ListOperationsTest()
        {
            var operations = CosmosDBManagementClient.Operations.ListAsync();
            Assert.NotNull(operations);
            Assert.IsNotEmpty(await operations.ToEnumerableAsync());
        }
    }
}
