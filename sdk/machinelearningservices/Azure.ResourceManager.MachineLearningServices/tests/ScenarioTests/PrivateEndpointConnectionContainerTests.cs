// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Linq;
using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.Network;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class PrivateEndpointConnectionContainerTests : MachineLearningServicesManagerTestBase
    {
        public PrivateEndpointConnectionContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await CreateTestResourceGroup();
            var workspaceName = Recording.GenerateAssetName("testmlCreate");
            var workspace = await rg.GetWorkspaces().CreateOrUpdateAsync(workspaceName, DataHelper.GenerateWorkspaceData());
            await PreparePrivateEndpoint(rg, workspace.Value.Id.ToString());

            // This operation only support update
            var connectionParameter = new PrivateEndpointConnectionData()
            {
                PrivateLinkServiceConnectionState = new PrivateLinkServiceConnectionState()
                {
                    Status = PrivateEndpointServiceConnectionStatus.Rejected,
                    Description = "Update Rejected"
                }
            };
            // Connection name is a random value?
            var connections = await workspace.Value.GetPrivateEndpointConnections().GetAllAsync().ToEnumerableAsync();
            var connection = await workspace.Value.GetPrivateEndpointConnections().CreateOrUpdateAsync(connections.FirstOrDefault().Data.Name, connectionParameter).ConfigureAwait(false);
            Assert.IsTrue(connection.Value.Data.PrivateLinkServiceConnectionState.Status == PrivateEndpointServiceConnectionStatus.Rejected);
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await CreateTestResourceGroup();
            var workspaceName = Recording.GenerateAssetName("testmlCreate");
            var workspace = await rg.GetWorkspaces().CreateOrUpdateAsync(workspaceName, DataHelper.GenerateWorkspaceData());
            await PreparePrivateEndpoint(rg, workspace.Value.Id.ToString());

            var connections = await workspace.Value.GetPrivateEndpointConnections().GetAllAsync().ToEnumerableAsync();
            Assert.NotZero(connections.Count);
            foreach (var connection in connections)
            {
                Assert.NotNull(connection.Data.Name);
                Assert.NotNull(connection.Data.PrivateLinkServiceConnectionState.Status == PrivateEndpointServiceConnectionStatus.Approved);
            }
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await CreateTestResourceGroup();
            var workspaceName = Recording.GenerateAssetName("testmlCreate");
            var workspace = await rg.GetWorkspaces().CreateOrUpdateAsync(workspaceName, DataHelper.GenerateWorkspaceData());
            await PreparePrivateEndpoint(rg, workspace.Value.Id.ToString());

            var connections = await workspace.Value.GetPrivateEndpointConnections().GetAllAsync().ToEnumerableAsync();
            var connection = await workspace.Value.GetPrivateEndpointConnections().GetAsync(connections.FirstOrDefault().Data.Name).ConfigureAwait(false);
            Assert.NotNull(connection.Value.Data.Name);
            Assert.NotNull(connection.Value.Data.PrivateLinkServiceConnectionState.Status == PrivateEndpointServiceConnectionStatus.Approved);
        }

        [TestCase]
        [RecordedTest]
        public async Task TryGet()
        {
            ResourceGroup rg = await CreateTestResourceGroup();
            var workspaceName = Recording.GenerateAssetName("testmlCreate");
            var workspace = await rg.GetWorkspaces().CreateOrUpdateAsync(workspaceName, DataHelper.GenerateWorkspaceData());
            await PreparePrivateEndpoint(rg, workspace.Value.Id.ToString());

            var connections = await workspace.Value.GetPrivateEndpointConnections().GetAllAsync().ToEnumerableAsync();
            var connection = await workspace.Value.GetPrivateEndpointConnections().GetIfExistsAsync(connections.FirstOrDefault().Data.Name).ConfigureAwait(false);
            Assert.NotNull(connection.Value.Data.Name);
            Assert.NotNull(connection.Value.Data.PrivateLinkServiceConnectionState.Status == PrivateEndpointServiceConnectionStatus.Approved);
            connection = await workspace.Value.GetPrivateEndpointConnections().GetIfExistsAsync("foo").ConfigureAwait(false);
            Assert.IsNull(connection.Value);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
        {
            ResourceGroup rg = await CreateTestResourceGroup();
            var workspaceName = Recording.GenerateAssetName("testmlCreate");
            var workspace = await rg.GetWorkspaces().CreateOrUpdateAsync(workspaceName, DataHelper.GenerateWorkspaceData());
            await PreparePrivateEndpoint(rg, workspace.Value.Id.ToString());

            var connections = await workspace.Value.GetPrivateEndpointConnections().GetAllAsync().ToEnumerableAsync();
            var connection = await workspace.Value.GetPrivateEndpointConnections().CheckIfExistsAsync(connections.FirstOrDefault().Data.Name).ConfigureAwait(false);
            Assert.IsTrue(connection);
            connection = await workspace.Value.GetPrivateEndpointConnections().CheckIfExistsAsync("foo").ConfigureAwait(false);
            Assert.IsFalse(connection);
        }

        private async Task<ResourceGroup> CreateTestResourceGroup()
        {
            return await (await Client
                .DefaultSubscription
                .GetResourceGroups()
                .CreateOrUpdateAsync(
                    Recording.GenerateAssetName("testmlrg"),
                    new ResourceGroupData(Location.WestUS2))).WaitForCompletionAsync();
        }

        private async Task PreparePrivateEndpoint(ResourceGroup rg, string workspaceId)
        {
            // Create a VNet
            var vnetName = Recording.GenerateAssetName("testmlvnet");
            var vnetParameter = new VirtualNetworkData()
            {
                AddressSpace = new Network.Models.AddressSpace()
                {
                    AddressPrefixes = { "10.0.0.0/16" }
                },
                Location = TestEnvironment.Location,
                Subnets = {
                    new SubnetData() { Name = "frontendSubnet", AddressPrefix = "10.0.1.0/24", PrivateEndpointNetworkPolicies = "Disabled"},
                    new SubnetData() { Name = "backendSubnet", AddressPrefix = "10.0.2.0/24", PrivateEndpointNetworkPolicies = "Disabled"},
                }
            };
            var vnet = await (await rg.GetVirtualNetworks().CreateOrUpdateAsync(vnetName, vnetParameter)).WaitForCompletionAsync();

            // Create a PrivateEndpoint
            var connectionName = Recording.GenerateAssetName("testmlCon");
            var endpointName = Recording.GenerateAssetName("testmlep");
            var endpointParameter = new PrivateEndpointData()
            {
                Location = TestEnvironment.Location,
                PrivateLinkServiceConnections = {
                    new Network.Models.PrivateLinkServiceConnection()
                    {
                        Name = connectionName,
                        PrivateLinkServiceId = workspaceId,
                        GroupIds = { "amlworkspace" },
                        RequestMessage = "Please approve my connection."
                    }
                },
                Subnet = new SubnetData() { Id = vnet.Value.Data.Subnets.FirstOrDefault().Id }
            };
            var endpoint = await (await rg.GetPrivateEndpoints().CreateOrUpdateAsync(endpointName, endpointParameter)).WaitForCompletionAsync();
        }
    }
}
