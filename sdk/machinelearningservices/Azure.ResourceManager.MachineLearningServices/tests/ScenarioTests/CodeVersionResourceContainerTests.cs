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
    public class CodeVersionResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-CodeVersionResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ParentPrefix = "test-parent";
        private const string ResourceNamePrefix = "test-resource";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = "1"; // version expect numeric value
        private string _parentName = ParentPrefix;

        public CodeVersionResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _parentName = SessionRecording.GenerateAssetName(ParentPrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);

            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();

            _ = await ws.GetCodeContainerResources().CreateOrUpdateAsync(
                _parentName,
                DataHelper.GenerateCodeContainerResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            CodeContainerResource parent = await ws.GetCodeContainerResources().GetAsync(_parentName);

            Assert.DoesNotThrowAsync(async () => _ = await parent.GetCodeVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeVersion()));

            var count = (await parent.GetCodeVersionResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(count, 1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            CodeContainerResource parent = await ws.GetCodeContainerResources().GetAsync(_parentName);

            Assert.DoesNotThrowAsync(async () => _ = await parent.GetCodeVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeVersion()));

            Assert.DoesNotThrowAsync(async () => await parent.GetCodeVersionResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await parent.GetCodeVersionResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            CodeContainerResource parent = await ws.GetCodeContainerResources().GetAsync(_parentName);

            CodeVersionCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetCodeVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeVersion()));

            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await parent.GetCodeVersionResources().CreateOrUpdateAsync(
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
            CodeContainerResource parent = await ws.GetCodeContainerResources().GetAsync(_parentName);

            Assert.DoesNotThrowAsync(async () => _ = await (await parent.GetCodeVersionResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateCodeVersion())).WaitForCompletionAsync());

            Assert.IsTrue(await parent.GetCodeVersionResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await parent.GetCodeVersionResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
