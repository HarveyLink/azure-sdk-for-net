// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class WorkspaceConnectionContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-WorkspaceConnectionContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;

        public WorkspaceConnectionContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            _ = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            Assert.DoesNotThrowAsync(async () => _ = await ws.GetWorkspaceConnections().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceConnectionData()));

            var count = (await ws.GetWorkspaceConnections().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            Assert.DoesNotThrowAsync(async () => _ = await ws.GetWorkspaceConnections().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceConnectionData()));

            Assert.DoesNotThrowAsync(async () => await ws.GetWorkspaceConnections().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await ws.GetWorkspaceConnections().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            WorkspaceConnectionCreateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetWorkspaceConnections().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceConnectionData()));

            resource.Value.Data.Target = "www.google.com";
            resource.Value.Data.ValueFormat = null;
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetWorkspaceConnections().CreateOrUpdateAsync(
                _resourceName,
                resource.Value.Data));
            Assert.AreEqual("www.google.com", resource.Value.Data.Target);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            Assert.DoesNotThrowAsync(async () => _ = await (await ws.GetWorkspaceConnections().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceConnectionData())).WaitForCompletionAsync());

            Assert.IsTrue(await ws.GetWorkspaceConnections().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await ws.GetWorkspaceConnections().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
