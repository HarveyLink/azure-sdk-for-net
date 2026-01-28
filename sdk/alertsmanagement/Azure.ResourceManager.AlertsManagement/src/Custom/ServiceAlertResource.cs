// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

#nullable disable

using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using Azure.ResourceManager.AlertsManagement.Models;

namespace Azure.ResourceManager.AlertsManagement
{
    public partial class ServiceAlertResource
    {
        // The custom code for ChangeStateAsync/ChangeState were added because the parameter type of `Comment` changed in newer spec version.
        /// <summary>
        /// Change the state of an alert.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.AlertsManagement/alerts/{alertId}/changestate</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Alerts_ChangeState</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2019-05-05-preview</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="ServiceAlertResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="newState"> New state of the alert. </param>
        /// <param name="comment"> reason of change alert state. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual async Task<Response<ServiceAlertResource>> ChangeStateAsync(ServiceAlertState newState, string comment = null, CancellationToken cancellationToken = default)
            => await ChangeStateAsync(newState, new Comments() { CommentsValue = comment}, cancellationToken).ConfigureAwait(false);

        /// <summary>
        /// Change the state of an alert.
        /// <list type="bullet">
        /// <item>
        /// <term>Request Path</term>
        /// <description>/subscriptions/{subscriptionId}/providers/Microsoft.AlertsManagement/alerts/{alertId}/changestate</description>
        /// </item>
        /// <item>
        /// <term>Operation Id</term>
        /// <description>Alerts_ChangeState</description>
        /// </item>
        /// <item>
        /// <term>Default Api Version</term>
        /// <description>2019-05-05-preview</description>
        /// </item>
        /// <item>
        /// <term>Resource</term>
        /// <description><see cref="ServiceAlertResource"/></description>
        /// </item>
        /// </list>
        /// </summary>
        /// <param name="newState"> New state of the alert. </param>
        /// <param name="comment"> reason of change alert state. </param>
        /// <param name="cancellationToken"> The cancellation token to use. </param>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual Response<ServiceAlertResource> ChangeState(ServiceAlertState newState, string comment = null, CancellationToken cancellationToken = default)
            => ChangeState(newState, new Comments() { CommentsValue = comment }, cancellationToken);
    }
}
