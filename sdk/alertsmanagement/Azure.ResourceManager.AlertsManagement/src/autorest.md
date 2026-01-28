# Generated code configuration

Run `dotnet build /t:GenerateCode` to generate code.

``` yaml

azure-arm: true
csharp: true
library-name: AlertsManagement
namespace: Azure.ResourceManager.AlertsManagement
tag: package-all
output-folder: $(this-folder)/Generated
clear-output-folder: true
sample-gen:
  output-folder: $(this-folder)/../tests/Generated
  clear-output-folder: true
skip-csproj: true
modelerfour:
  flatten-payloads: false
  lenient-model-deduplication: true
use-model-reader-writer: true

# mgmt-debug:
#  show-serialized-names: true

```

### Tag: package-all

These settings apply only when `--tag=package-all` is specified on the command line.

``` yaml $(tag) == 'package-all'
title: AlertsManagementClient
description: AlertsManagement Client
openapi-type: arm

input-file:
  - https://github.com/welovej/azure-rest-api-specs/blob/ad6a3623771ab4002d9a0c8f1f5e86675c653cf1/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/AlertProcessingRules/stable/2021-08-08/AlertProcessingRules.json
  - https://github.com/welovej/azure-rest-api-specs/blob/55dc538d57221e3816619b008d65928ffdaec19d/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/AlertRuleRecommendations/preview/2023-08-01-preview/AlertRuleRecommendations.json
  - https://github.com/welovej/azure-rest-api-specs/blob/904fd7e4685b4d3c506b52afd323b63f83f20db9/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/AlertsManagement/preview/2025-05-25-preview/AlertsManagement.json
  - https://github.com/welovej/azure-rest-api-specs/blob/17d211d07bcde67d0511a35cb9d4313245eb6cad/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/PreviewAlertRule/preview/2025-07-01-preview/PreviewAlertRule.json
  - https://github.com/welovej/azure-rest-api-specs/blob/e835843a15f48b1df6ca61c217206e7bcf56e478/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/PrometheusRuleGroups/stable/2023-03-01/PrometheusRuleGroups.json
  - https://github.com/welovej/azure-rest-api-specs/blob/a77bab40da5cb83155e86ca1cd4ba645ec06f100/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/TenantActivityLogAlerts/preview/2023-04-01-preview/TenantActivityLogAlerts.json
  - https://github.com/Azure/azure-rest-api-specs/blob/0077b4a8c5071d3ab33fd9f9ba013581c9a66b8f/specification/alertsmanagement/resource-manager/Microsoft.AlertsManagement/preview/2019-05-05-preview/SmartGroups.json

rename-mapping:
  AlertModification.modifiedAt: modifiedOn|date-time
  AlertProcessingRuleProperties.enabled: IsEnabled
  AlertsSortByFields.startDateTime: StartOn
  AlertsSortByFields.lastModifiedDateTime: LastModifiedOn
  AlertsSummaryGroup.groupedby: GroupedBy
  AlertsSummaryGroupItem.groupedby: GroupedBy
  Essentials.startDateTime: StartOn|date-time
  Essentials.lastModifiedUserName: lastModifiedBy
  PatchObject.properties.enabled: IsEnabled
  SmartGroup.properties.lastModifiedUserName: lastModifiedBy
  SmartGroupModificationItem.modifiedAt: modifiedOn|date-time
  Schedule.effectiveFrom: -|date-time
  Schedule.effectiveUntil: -|date-time
  TimeRange.1h: OneHour
  TimeRange.1d: OneDay
  TimeRange.7d: SevenDays
  TimeRange.30d: ThirtyDays
  AddActionGroups: AlertProcessingRuleAddGroupsAction
  RemoveAllActionGroups: AlertProcessingRuleRemoveAllGroupsAction
  AlertModificationEvent: ServiceAlertModificationEvent
  AlertModificationItem: ServiceAlertModificationItemInfo
  Severity: ServiceAlertSeverity
  Identifier: RetrievedInformationIdentifier
  TimeRange: TimeRangeFilter
  DaysOfWeek: AlertsManagementDayOfWeek
  MonitorService: MonitorServiceSourceForAlert
  MonthlyRecurrence: AlertProcessingRuleMonthlyRecurrence
  WeeklyRecurrence:  AlertProcessingRuleWeeklyRecurrence
  SortOrder: AlertsManagementQuerySortOrder
  Action: AlertProcessingRuleAction
  ActionType: AlertProcessingRuleActionType
  Alert: ServiceAlert
  AlertsList: ServiceAlertListResult
  AlertState: ServiceAlertState
  ActionStatus: ServiceAlertActionStatus
  Essentials: ServiceAlertEssentials
  SignalType: ServiceAlertSignalType
  Condition: AlertProcessingRuleCondition
  Field: AlertProcessingRuleField
  Operator: AlertProcessingRuleOperator
  SmartGroupModificationItem: SmartGroupModificationItemInfo
  Schedule: AlertProcessingRuleSchedule
  Recurrence: AlertProcessingRuleRecurrence
  Recurrence.startTime: StartOn|time
  Recurrence.endTime: EndOn|time
  AlertsMetaData: ServiceAlertMetadata
  AlertsMetaDataProperties: ServiceAlertMetadataProperties
  AlertModification: ServiceAlertModification
  AlertsSortByFields: ListServiceAlertsSortByField
  AlertsSummary: ServiceAlertSummary
  AlertsSummaryGroupByField: GetServiceAlertSummaryGroupByField
  AlertsSummaryGroupItem: ServiceAlertSummaryGroupItemInfo
  MetadataIdentifier: ServiceAlertMetadataIdentifier
  AlertModificationProperties: ServiceAlertModificationProperties
  AlertProperties: ServiceAlertProperties
  AlertsSummaryGroup: ServiceAlertSummaryGroup
  AlertEnrichmentItem.type: EnrichmentType
  PrometheusEnrichmentItem.type: EnrichmentType

request-path-to-resource-name:
  /{scope}/providers/Microsoft.AlertsManagement/alerts/{alertId}: ServiceAlert

format-by-name-rules:
  'tenantId': 'uuid'
  'alertId': 'uuid'
  'smartGroupId': 'uuid'
  'ETag': 'etag'
  'location': 'azure-location'
  '*Uri': 'Uri'
  '*Uris': 'Uri'
  'actionGroupIds': 'arm-id'

override-operation-name:
  Alerts_MetaData: GetServiceAlertMetadata
  Alerts_GetSummary: GetServiceAlertSummary

acronym-mapping:
  CPU: Cpu
  CPUs: Cpus
  Os: OS
  Ip: IP
  Ips: IPs|ips
  ID: Id
  IDs: Ids
  VM: Vm
  VMs: Vms
  VMScaleSet: VmScaleSet
  DNS: Dns
  VPN: Vpn
  NAT: Nat
  WAN: Wan
  Ipv4: IPv4|ipv4
  Ipv6: IPv6|ipv6
  Ipsec: IPsec|ipsec
  SSO: Sso
  URI: Uri
  Etag: ETag|etag
  SCOM: Scom

directive:
  - from: AlertsManagement.json
    where: $
    transform: >
      for (var pathName in $.paths) {
          var path = $.paths[pathName];
          if (path.parameters) {
              for (var i = 0; i < path.parameters.length; i++) {
                  if (path.parameters[i].name === 'alertId') {
                      path.parameters[i]['format'] = 'uuid';
                  }
              }
          }
          for (var methodName in path) {
              if (methodName === 'parameters') continue;
              var operation = path[methodName];
              if (operation.parameters) {
                  for (var i = 0; i < operation.parameters.length; i++) {
                      if (operation.parameters[i].name === 'alertId') {
                          operation.parameters[i]['format'] = 'uuid';
                      }
                  }
              }
          }
      }
  - from: swagger-document
    where: $.definitions
    transform: >
      $.errorResponse['x-ms-client-name'] = 'SmartGroupErrorResponse';
      $.errorResponseBody['x-ms-client-name'] = 'SmartGroupErrorResponseBody';
      $.smartGroupProperties.properties.smartGroupState['x-ms-enum']['name'] = 'SmartGroupState';
  - from: swagger-document
    where: $.definitions
    transform: >
      $.errorResponse['x-ms-client-name'] = 'AlertProcessingRuleErrorResponse';
      $.errorResponseBody['x-ms-client-name'] = 'AlertProcessingRuleErrorResponseBody';
  - from: swagger-document
    where: $.definitions
    transform: >
      $.errorResponse['x-ms-client-name'] = 'AlertsManagementErrorResponse';
      $.errorResponseBody['x-ms-client-name'] = 'AlertsManagementErrorResponseBody';
  - from: swagger-document
    where: $.parameters
    transform: >
      $.smartGroupId['format'] = 'uuid';
  # Could not set ResourceTypeSegment for request path /{scope}
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alerts']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alerts/{alertId}']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alerts/{alertId}/changestate']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alerts/{alertId}/enrichments']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alerts/{alertId}/history']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  - from: swagger-document
    where: $.paths
    transform: >
        $['/{scope}/providers/Microsoft.AlertsManagement/alertsSummary']['get']['parameters'][1]['x-ms-skip-url-encoding'] = true;
  # [Error][Linked: https://github.com/Azure/autorest.csharp/issues/3288] Found more than 1 candidate for XX
  - remove-operation: Alerts_GetAllTenant
```
