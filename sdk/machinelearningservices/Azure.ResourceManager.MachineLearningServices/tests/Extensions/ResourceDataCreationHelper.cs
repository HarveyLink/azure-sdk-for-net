// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Collections.Generic;
using Azure.ResourceManager.MachineLearningServices.Models;
using Azure.ResourceManager.Resources;
using Azure.ResourceManager.Resources.Models;
using ResourceIdentityType = Azure.ResourceManager.Resources.Models.ResourceIdentityType;

namespace Azure.ResourceManager.MachineLearningServices.Tests.Extensions
{
    public class ResourceDataCreationHelper
    {
        private readonly MachineLearningServicesManagerTestBase _testBase;

        public ResourceDataCreationHelper(MachineLearningServicesManagerTestBase testBase)
        {
            _testBase = testBase;
        }

        public WorkspaceData GenerateWorkspaceData()
        {
            return new WorkspaceData
            {
                Location = Location.WestUS2,
                ApplicationInsights = _testBase.CommonAppInsightId,
                ContainerRegistry = _testBase.CommonAcrId,
                StorageAccount = _testBase.CommonStorageId,
                KeyVault = _testBase.CommonKeyVaultId,
                Identity = new Models.Identity
                {
                    Type = (Models.ResourceIdentityType?)ResourceIdentityType.SystemAssigned
                }
            };
        }

        public BatchDeploymentTrackedResourceData GenerateBatchDeploymentTrackedResourceDataData()
        {
            throw new NotImplementedException();
        }

        public BatchDeploymentTrackedResourceData GenerateBatchDeploymentTrackedResourceData(
            string scoringscript,
            CodeVersionResource code,
            ModelVersionResource model,
            EnvironmentSpecificationVersionResource environment,
            ComputeResource compute)
        {
            var batch = new BatchDeployment()
            {
                CodeConfiguration = new CodeConfiguration(scoringscript)
                {
                    CodeId = code.Id
                },
                Model = new IdAssetReference(model.Id)
                {
                    ReferenceType = ReferenceType.Id
                },
                EnvironmentId = environment.Id,
                Compute = new ComputeConfiguration()
                {
                    Target = compute.Id,
                    InstanceCount = 1,
                    IsLocal = false
                },
                MiniBatchSize = 10,
                OutputConfiguration = new BatchOutputConfiguration()
                {
                    OutputAction = BatchOutputAction.AppendRow,
                    AppendRowFileName = "output.csv"
                },
                RetrySettings = new BatchRetrySettings()
                {
                    MaxRetries = 3,
                    Timeout = TimeSpan.FromSeconds(30)
                }
            };
            return new BatchDeploymentTrackedResourceData(Location.WestUS2, batch);
        }

        public BatchEndpointTrackedResourceData GenerateBatchEndpointTrackedResourceData()
        {
            return new BatchEndpointTrackedResourceData(
                Location.WestUS2,
                new BatchEndpoint
                {
                    AuthMode = EndpointAuthMode.AADToken,
                    Description = "Test",
                });
        }

        public CodeContainer GenerateCodeContainerResourceData()
        {
            return new CodeContainer()
            {
                // BUGBUG
                //Description = "Test"
            };
        }

        //TODO: upload code to datastore
        public CodeVersion GenerateCodeVersion()
        {
            return new CodeVersion("hello.py");
        }

        public ComputeResourceData GenerateComputeResourceData()
        {
            // TODO: Take input to create different compute resource
            return new ComputeResourceData
            {
                Location = Location.WestUS2,
                Properties = new AmlCompute
                {
                    Properties = new AmlComputeProperties
                    {
                        ScaleSettings = new ScaleSettings(2),
                        VmSize = "Standard_DS2_v2",
                    }
                }
            };
        }

        public DataContainer GenerateDataContainerResourceData()
        {
            throw new NotImplementedException();
        }

        public DatastoreProperties GenerateDatastorePropertiesResourceData()
        {
            return new DatastoreProperties(
                new AzureBlobContents(
                    "track2mlstorage",
                    "datastore-container",
                    new AccountKeyDatastoreCredentials()
                    {
                        Secrets = new AccountKeyDatastoreSecrets()
                        {
                            Key = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes("key"))
                        }
                    },
                    "core.windows.net",
                    "https")
                ) {
                Description = "Description",
                IsDefault = true,
                LinkedInfo = new LinkedInfo() { LinkedId = "string", LinkedResourceName = "string", Origin = OriginType.Synapse },
                //Properties = { { "additionalProp1", "vaule1" } },
                Tags = { { "key1", "value1" }, { "key2", "value2" } }
            };
        }

        public DataVersion GenerateDataVersionResourceData()
        {
            return new DataVersion(@"https://azureopendatastorage.blob.core.windows.net/citydatacontainer/Safety/Release/city=SanFrancisco/*.parquet")
            {
                Tags ={
                    { "opendatasets", "city_safety_sanfrancisco" }},
                Properties ={
                    {"_TimeSeries_Column:FineGrainTimestamp_" , "dateTime" },
                    {"opendatasets","city_safety_sanfrancisco"},
                    {"opendatasets:EndDate","08/18/2021 00:00:00 +00:00"},
                    {"opendatasets:StartDate", "07/20/2021 00:00:00 +00:00"}},
                IsAnonymous = false,
                DatasetType = DatasetType.Simple
            };
        }

        public EnvironmentContainer GenerateEnvironmentContainerResourceData()
        {
            return new EnvironmentContainer
            {
                //Description = "Test"
            };
        }

        public EnvironmentSpecificationVersion GenerateEnvironmentSpecificationVersionResourceData()
        {
            return new EnvironmentSpecificationVersion
            {
                Description = "Test",
                Docker = new DockerBuild("FROM python:3.7-slim")
            };
        }
        public EnvironmentSpecificationVersion GenerateOnlineEndpointEnvironmentVersion()
        {
            return new EnvironmentSpecificationVersion
            {
                CondaFile =
                "name: model-env\n" +
                "channels:\n" +
                "  - conda-forge\n" +
                "  dependencies:\n" +
                "- python=3.7\n" +
                "  - numpy\n" +
                "  - pip\n  " +
                "- scikit-learn==0.24.1\n" +
                "  - scipy\n" +
                "  - pip:\n" +
                "    - azureml-defaults\n" +
                "    - inference-schema[numpy-support]\n" +
                "    - joblib\n" +
                "    - numpy\n" +
                "    - scipy\n",
                Description = "Test",
                Docker = new DockerImage(@"https://mcr.microsoft.com/azureml/openmpi3.1.2-cuda10.2-cudnn8-ubuntu18.04:20210507.v1")
            };
        }
        public EnvironmentSpecificationVersion GenerateBatchEndpointEnvironmentVersion()
        {
            return new EnvironmentSpecificationVersion
            {
                CondaFile =
                "name: mnist-env\n" +
                "channels:\n" +
                "  - conda-forge\n" +
                "dependencies:\n" +
                "  - python=3.6.2\n" +
                "  - pip\n" +
                "  - pip:\n" +
                "    - tensorflow==1.15.2\n" +
                "    - pillow\n" +
                "    - azureml-core\n" +
                "    - azureml-dataset-runtime[fuse]\n",
                Description = "Test",
                Docker = new DockerImage(@"https://mcr.microsoft.com/azureml/openmpi3.1.2-cuda10.2-cudnn8-ubuntu18.04:20210507.v1")
            };
        }
        public JobBase GenerateJobBaseResourceData(string experimentname, ComputeResource compute, EnvironmentSpecificationVersionResource env)
        {
            string command = "sleep 5";
            return new CommandJob(
                command,
                new ComputeConfiguration()
                {
                    IsLocal = true
                })
            {
                JobType = JobType.Command,
                Compute = new ComputeConfiguration()
                {
                    Target = compute.Id
                },
                ExperimentName = experimentname,
                Command = command,
                EnvironmentId = env.Id,
                Tags = { { "key1", "value1" } },
                Properties =
                {
                    {"ProcessInfoFile","azureml-logs/process_info.json"},
                    {"ProcessStatusFile","azureml-logs/process_status.json"}
                }
            };
        }

        public LabelingJob GenerateLabelingJobResourceData(DataContainerResource datacontainer, DataVersionResource data)
        {
            var labelingjob = new LabelingJob(JobType.Labeling)
            {
                DatasetConfiguration = new LabelingDatasetConfiguration()
                {
                    AssetName = datacontainer.Data.Id,
                    DatasetVersion = data.Data.Name
                },
                Description = "This is a test.",
                LabelingJobMediaProperties = new LabelingJobMediaProperties()
                {
                    MediaType = MediaType.Image
                }
            };
            var lb1 = new LabelClass() { DisplayName = "football" };
            var lb2 = new LabelClass() { DisplayName = "basketball" };
            var lc = new LabelCategory() { DisplayName = "DefaultCategory", AllowMultiSelect = false };
            lc.Classes.Add("football", lb1);
            lc.Classes.Add("basketball", lb2);
            labelingjob.LabelCategories.Add(
                "DefaultCategory", lc);
            return labelingjob;
        }

        public ModelContainer GenerateModelContainerResourceData()
        {
            return new ModelContainer() { Properties = { { "key1", "value1" } }, Description = "Description", Tags = { { "key1", "value1" } } };
        }
        //TODO: Upload model to datastore
        public ModelVersion GenerateModelVersionResourceData(DatastorePropertiesResource datastore)
        {
            return new ModelVersion("model.pkl")
            {
                DatastoreId = datastore.Data.Id,
                Description = "Model version description",
                Flavors = { { "python_function", new FlavorData() { Data = { { "loader_module", "myLoaderModule" } } } } },
                Tags = { { "key1", "value1" },{ "key2", "value2" } },
                Properties = { { "key1", "value1" }, { "key2", "value2" } }
            };
        }
        public OnlineDeploymentTrackedResourceData GenerateOnlineDeploymentTrackedResourceData(
                    string script,
                    CodeVersionResource code,
                    ModelVersionResource model,
                    EnvironmentSpecificationVersionResource env)
        {
            var od = new OnlineDeployment()
            {
                CodeConfiguration = new CodeConfiguration(script)
                {
                    CodeId = code.Id
                },
                Model = new IdAssetReference(model.Id)
                {
                    ReferenceType = ReferenceType.Id
                },
                EnvironmentId = env.Id,
                ScaleSettings = new ManualScaleSettings()
                {
                    MinInstances = 1,
                    MaxInstances = 2,
                    ScaleType = ScaleType.Manual,
                    InstanceCount = 1
                },
                AppInsightsEnabled = false,
                RequestSettings = new OnlineRequestSettings()
                {
                    RequestTimeout = TimeSpan.FromMinutes(1),
                    MaxConcurrentRequestsPerInstance = 1
                },
                LivenessProbe = new ProbeSettings()
                {
                    FailureThreshold = 30,
                    SuccessThreshold = 1,
                    Timeout = TimeSpan.FromSeconds(10),
                    Period = TimeSpan.FromSeconds(2),
                    InitialDelay = TimeSpan.FromSeconds(10)
                },
                EndpointComputeType = EndpointComputeType.Managed,
            };
            return new OnlineDeploymentTrackedResourceData(Location.WestUS2, od)
            {
                Kind = "Managed"
            };
        }

        public OnlineEndpointTrackedResourceData GenerateOnlineEndpointTrackedResourceData(GenericResource resource = default)
        {
            OnlineEndpoint properties = new OnlineEndpoint(EndpointAuthMode.AMLToken)
            {
                Traffic = { { "deployment1", 100 } },
                Description = "this is a test endpoint"
            };

            var identity = new Models.ResourceIdentity()
            {
                Type = ResourceIdentityAssignment.UserAssigned,
                //UserAssignedIdentities = { {
                //        resource.Id,
                //        new UserAssignedIdentityMeta()
                //    } }
            };
            return new OnlineEndpointTrackedResourceData(Location.WestUS2, properties) { Kind = "SampleKind", Identity = identity };
        }

        public PrivateEndpointConnectionData GeneratePrivateEndpointConnectionData()
        {
            throw new NotImplementedException();
        }

        public WorkspaceConnectionData GenerateWorkspaceConnectionData()
        {
            return new WorkspaceConnectionData()
            {
                Category = "ACR",
                Target = "www.facebook.com",
                AuthType = "PAT",
                Value = "secrets",
                //ValueFormat = ValueFormat.Json
            };
        }
    }
}
