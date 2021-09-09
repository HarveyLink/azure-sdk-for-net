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
    public class WorkspaceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-WorkspaceContainer";
        private const string ResourceNamePrefix = "test-resource";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _resourceName = ResourceNamePrefix;

        public WorkspaceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            _ = await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation));
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);

            Assert.DoesNotThrowAsync(async () => _ = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceData()));

            var count = (await rg.GetWorkspaces().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);

            Assert.DoesNotThrowAsync(async () => _ = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceData()));

            Assert.DoesNotThrowAsync(async () => await rg.GetWorkspaces().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await rg.GetWorkspaces().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);

            WorkspaceCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceData()));

            resource.Value.Data.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await rg.GetWorkspaces().CreateOrUpdateAsync(
                _resourceName,
                resource.Value.Data));
            Assert.AreEqual("Updated", resource.Value.Data.Description);
        }

        [TestCase]
        [RecordedTest]
        public async Task CheckIfExists()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);

            Assert.DoesNotThrowAsync(async () => _ = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync());

            Assert.IsTrue(await rg.GetWorkspaces().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await rg.GetWorkspaces().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
