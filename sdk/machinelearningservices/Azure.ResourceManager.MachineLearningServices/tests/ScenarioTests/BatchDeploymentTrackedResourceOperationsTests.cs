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
    public class BatchDeploymentTrackedResourceOperationsTests : MachineLearningServicesManagerTestBase
    {
        private const string ResourceGroupNamePrefix = "test-BatchDeploymentTrackedResourceContainer";
        private const string WorkspacePrefix = "test-workspace";
        private const string BatchEndpointPrefix = "test-endpoint";
        private const string ResourceNamePrefix = "test-depolyment";
        private const string ComputeNamePrefix = "test-compute";
        private const string ModelContainerNamePrefix = "test-model";
        private const string CodeContainerNamePrefix = "test-code";
        private readonly Location _defaultLocation = Location.WestUS2;
        private string _resourceGroupName = ResourceGroupNamePrefix;
        private string _workspaceName = WorkspacePrefix;
        private string _resourceName = ResourceNamePrefix;
        private string _batchEndpointName = BatchEndpointPrefix;
        private string _computeName = ComputeNamePrefix;
        private string _modelContainerName = ModelContainerNamePrefix;
        private string _codeContainerName = CodeContainerNamePrefix;
        private string _environmentContainerName = "AzureML-sklearn-0.24-ubuntu18.04-py37-cpu";
        private string _environmentVerion = "7";

        public BatchDeploymentTrackedResourceOperationsTests(bool isAsync)
            : base(isAsync)
        {
        }

        [OneTimeSetUp]
        public async Task SetupResources()
        {
            _batchEndpointName = SessionRecording.GenerateAssetName(BatchEndpointPrefix);
            _resourceName = SessionRecording.GenerateAssetName(ResourceNamePrefix);
            _workspaceName = SessionRecording.GenerateAssetName(WorkspacePrefix);
            _resourceGroupName = SessionRecording.GenerateAssetName(ResourceGroupNamePrefix);
            _computeName = SessionRecording.GenerateAssetName(ComputeNamePrefix);
            _modelContainerName = SessionRecording.GenerateAssetName(ModelContainerNamePrefix);
            _codeContainerName = SessionRecording.GenerateAssetName(CodeContainerNamePrefix);

            // Create RG and Res with GlobalClient
            ResourceGroup rg = await (await GlobalClient.DefaultSubscription.GetResourceGroups()
                .CreateOrUpdateAsync(_resourceGroupName, new ResourceGroupData(_defaultLocation))).WaitForCompletionAsync();

            Workspace ws = await (await rg.GetWorkspaces().CreateOrUpdateAsync(
                _workspaceName,
                DataHelper.GenerateWorkspaceData())).WaitForCompletionAsync();
            //Endpoint
            BatchEndpointTrackedResource parent = await (await ws.GetBatchEndpointTrackedResources().CreateOrUpdateAsync(
                _batchEndpointName,
                DataHelper.GenerateBatchEndpointTrackedResourceData())).WaitForCompletionAsync();
            _ = await (await ws.GetComputeResources()
                .CreateOrUpdateAsync(_computeName, DataHelper.GenerateComputeResourceData())).WaitForCompletionAsync();
            //Code
            CodeContainerResource ccr = await (await ws.GetCodeContainerResources()
                .CreateOrUpdateAsync(_codeContainerName, DataHelper.GenerateCodeContainerResourceData())).WaitForCompletionAsync();
            CodeVersionResource code = await (await ccr.GetCodeVersionResources()
                .CreateOrUpdateAsync("1", DataHelper.GenerateCodeVersion())).WaitForCompletionAsync();
            //Model
            DatastorePropertiesResource datastore = await ws.GetDatastorePropertiesResources().GetAsync("azureml");
            ModelContainerResource mcr = await (await ws.GetModelContainerResources()
                .CreateOrUpdateAsync(_modelContainerName, DataHelper.GenerateModelContainerResourceData())).WaitForCompletionAsync();
            ModelVersionResource model = await (await mcr.GetModelVersionResources()
                .CreateOrUpdateAsync("1", DataHelper.GenerateModelVersionResourceData(datastore))).WaitForCompletionAsync();
            //Environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //Compute
            ComputeResource compute = await (await ws.GetComputeResources()
                .CreateOrUpdateAsync(_computeName, DataHelper.GenerateComputeResourceData())).WaitForCompletionAsync();
            //Depolyment
            _ = await parent.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                _resourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute));
            StopSessionRecording();
        }

        [TestCase]
        [RecordedTest]
        public async Task Delete()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource parent = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            //Code
            CodeContainerResource ccr = await ws.GetCodeContainerResources().GetAsync(_codeContainerName);
            CodeVersionResource code = await ccr.GetCodeVersionResources().GetAsync("1");
            //Model
            ModelContainerResource mcr = await ws.GetModelContainerResources().GetAsync(_modelContainerName);
            ModelVersionResource model = await mcr.GetModelVersionResources().GetAsync("1");
            //environment
            EnvironmentContainerResource ecr = await ws.GetEnvironmentContainerResources().GetAsync(_environmentContainerName);
            EnvironmentSpecificationVersionResource environment = await ecr.GetEnvironmentSpecificationVersionResources().GetAsync(_environmentVerion);
            //compute
            ComputeResource compute = await ws.GetComputeResources().GetAsync(_computeName);

            var deleteResourceName = Recording.GenerateAssetName(ResourceNamePrefix) + "_delete";
            BatchDeploymentCreateOrUpdateOperation res = null;
            Assert.DoesNotThrowAsync(async () => res = await parent.GetBatchDeploymentTrackedResources().CreateOrUpdateAsync(
                deleteResourceName,
                DataHelper.GenerateBatchDeploymentTrackedResourceData("BatchEndpoint/score.py", code, model, environment, compute)));
            Assert.DoesNotThrowAsync(async () => _ = await res.Value.DeleteAsync());
        }

        [TestCase]
        [RecordedTest]
        public async Task Get()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource parent = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);

            BatchDeploymentTrackedResource resource = await parent.GetBatchDeploymentTrackedResources().GetAsync(_resourceName);
            BatchDeploymentTrackedResource resource1 = await resource.GetAsync();
            resource.AssertAreEqual(resource1);
        }

        [TestCase]
        [RecordedTest]
        public async Task Update()
        {
            ResourceGroup rg = await Client.DefaultSubscription.GetResourceGroups().GetAsync(_resourceGroupName);
            Workspace ws = await rg.GetWorkspaces().GetAsync(_workspaceName);
            BatchEndpointTrackedResource parent = await ws.GetBatchEndpointTrackedResources().GetAsync(_batchEndpointName);
            BatchDeploymentTrackedResource resource = await parent.GetBatchDeploymentTrackedResources().GetAsync(_resourceName);
            PartialBatchDeploymentPartialTrackedResource update = new PartialBatchDeploymentPartialTrackedResource()
            {
                Identity = new Models.ResourceIdentity() { Type = ResourceIdentityAssignment.None}
            };
            update.Tags.Add("Tag1", "Content1");
            update.Properties = new PartialBatchDeployment() { Description = "Updated" };
            BatchDeploymentTrackedResource updatedResource = await resource.UpdateAsync(update);
            Assert.AreEqual("2333", updatedResource.Data.Tags["233"]);
            Assert.AreEqual("Updated", updatedResource.Data.Properties.Description);
        }
    }
}
