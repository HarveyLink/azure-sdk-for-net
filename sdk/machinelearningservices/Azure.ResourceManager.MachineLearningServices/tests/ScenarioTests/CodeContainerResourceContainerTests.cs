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
    public class CodeContainerResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-CodeContainerResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;

        public CodeContainerResourceContainerTests(bool isAsync)
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

            Assert.DoesNotThrowAsync(async () => _ = await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeContainerResourceData()));

            var count = (await ws.GetCodeContainerResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            Assert.DoesNotThrowAsync(async () => _ = await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeContainerResourceData()));

            Assert.DoesNotThrowAsync(async () => await ws.GetCodeContainerResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await ws.GetCodeContainerResources().GetAsync("NonExistant"));
        }

        // BUGBUG CRUD not supported
        //[TestCase]
        //[RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            CodeContainerCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeContainerResourceData()));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _resourceName,
                resource.Value.Data.Properties));
            Assert.AreEqual("Updated", resource.Value.Data.Properties.Description);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            Assert.DoesNotThrowAsync(async () => _ = await (await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeContainerResourceData())).WaitForCompletionAsync());

            Assert.IsTrue(await ws.GetCodeContainerResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await ws.GetCodeContainerResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
