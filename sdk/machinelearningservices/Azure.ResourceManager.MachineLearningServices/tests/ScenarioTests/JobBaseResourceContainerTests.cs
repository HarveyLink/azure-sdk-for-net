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
    public class JobBaseResourceContainerTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-JobBaseResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private const string ComputeResourceNamePrefix = "test-compute";
        private const string ExperimentNamePrefix = "test-experiment";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _computerResourceName = ComputeResourceNamePrefix;
        private string _experimentName = ExperimentNamePrefix;
        private int _resourcecreated = 0;
        private string _environmentContainerName = "AzureML-sklearn-0.24.1-ubuntu18.04-py37-cpu-inference";
        private string _environmentVersion = "12";
        public JobBaseResourceContainerTests(bool isAsync)
         : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computerResourceName = SessionRecording.GenerateAssetName(ComputeResourceNamePrefix);
            _experimentName = SessionRecording.GenerateAssetName(ExperimentNamePrefix);
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            _ = ws.GetComputeResources().CreateOrUpdate(_computerResourceName, DataHelper.GenerateComputeResourceData());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task List()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            ComputeResource com = await ws.GetComputeResources().GetAsync(_computerResourceName);
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource esv = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            Assert.DoesNotThrowAsync(async () => _ = await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData(_experimentName, com, esv)));
            _resourcecreated++;
            var count = (await ws.GetJobBaseResources().GetAllAsync().ToEnumerableAsync()).Count;
            Assert.AreEqual(_resourcecreated, count);
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            ComputeResource com = await ws.GetComputeResources().GetAsync(_computerResourceName);
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentVersion);
            EnvironmentSpecificationVersionResource esv = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);

            Assert.DoesNotThrowAsync(async () => _ = await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData(_experimentName, com, esv)));
            _resourcecreated++;
            Assert.DoesNotThrowAsync(async () => await ws.GetJobBaseResources().GetAsync(_resourceName));
            Assert.ThrowsAsync<RequestFailedException>(async () => _ = await ws.GetJobBaseResources().GetAsync("NonExistant"));
        }

        [TestCase]
        [RecordedTest]
        public async Task CreateOrUpdate()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            ComputeResource com = await ws.GetComputeResources().GetAsync(_computerResourceName);
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource esv = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);

            JobCreateOrUpdateOperation resource = null;
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData(_experimentName, com, esv)));
            _resourcecreated++;
            resource.Value.Data.Properties.Description = "Updated";
            Assert.DoesNotThrowAsync(async () => resource = await ws.GetJobBaseResources().CreateOrUpdateAsync(
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
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            ComputeResource com = await ws.GetComputeResources().GetAsync(_computerResourceName);
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource esv = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);

            Assert.DoesNotThrowAsync(async () => _ = await (await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData(_experimentName, com, esv))).WaitForCompletionAsync());
            _resourcecreated++;
            Assert.IsTrue(await ws.GetJobBaseResources().CheckIfExistsAsync(_resourceName));
            Assert.IsFalse(await ws.GetJobBaseResources().CheckIfExistsAsync(_resourceName + "xyz"));
        }
    }
}
