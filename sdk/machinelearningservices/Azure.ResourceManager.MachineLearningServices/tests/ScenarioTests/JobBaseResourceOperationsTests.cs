// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System.Threading.Tasks;
using Azure.Core.TestFramework;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.MachineLearningServices.Tests.Extensions;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using NUnit.Framework;

namespace Azure.ResourceManager.MachineLearningServices.Tests.ScenarioTests
{
    public class JobBaseResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-JobBaseResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string ResourceNamePrefix = "test-resource";
        private const string ComputeResourceNamePrefix = "test-compute";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _computerResourceName = ComputeResourceNamePrefix;
        private string _environmentContainerName = "AzureML-pytorch-1.7-ubuntu18.04-py37-cuda11-gpu";
        private string _environmentVersion = "9";
        public JobBaseResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computerResourceName = SessionRecording.GenerateAssetName(ComputeResourceNamePrefix);
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();
            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            ComputeResource compute = await (ws.GetComputeResources().CreateOrUpdate(_computerResourceName, DataHelper.GenerateComputeResourceData())).WaitForCompletionAsync();
            EnvironmentContainerResource ecr =
                await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment =
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            Assert.DoesNotThrowAsync(async () => _ = await (await ws.GetJobBaseResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateJobBaseResourceData(_resourceName, compute, environment))).WaitForCompletionAsync());
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computerResourceName);
            EnvironmentContainerResource ecr =
                await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment =
                await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVersion);
            var deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            JobBaseResource res = null;
            Assert.DoesNotThrowAsync(async () => res = await (await ws.GetJobBaseResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateJobBaseResourceData(_resourceName,compute,environment))).WaitForCompletionAsync());
            Assert.DoesNotThrowAsync(async () => _ = await res.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);

            JobBaseResource resource = await ws.GetJobBaseResources().GetAsync(_resourceName);
            JobBaseResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }
    }
}
