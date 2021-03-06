// <auto-generated>
// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License. See License.txt in the project root for
// license information.
//
// Code generated by Microsoft (R) AutoRest Code Generator.
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.
// </auto-generated>

namespace Microsoft.Azure.Management.Media.Models
{
    using Microsoft.Rest;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    /// <summary>
    /// Representing a list of FilterTrackPropertyConditions to select a track.
    /// The filters are combined using a logical AND operation.
    /// </summary>
    public partial class FilterTrackSelection
    {
        /// <summary>
        /// Initializes a new instance of the FilterTrackSelection class.
        /// </summary>
        public FilterTrackSelection()
        {
            CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the FilterTrackSelection class.
        /// </summary>
        /// <param name="trackSelections">The track selections.</param>
        public FilterTrackSelection(IList<FilterTrackPropertyCondition> trackSelections)
        {
            TrackSelections = trackSelections;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// Gets or sets the track selections.
        /// </summary>
        [JsonProperty(PropertyName = "trackSelections")]
        public IList<FilterTrackPropertyCondition> TrackSelections { get; set; }

        /// <summary>
        /// Validate the object.
        /// </summary>
        /// <exception cref="ValidationException">
        /// Thrown if validation fails
        /// </exception>
        public virtual void Validate()
        {
            if (TrackSelections == null)
            {
                throw new ValidationException(ValidationRules.CannotBeNull, "TrackSelections");
            }
            if (TrackSelections != null)
            {
                foreach (var element in TrackSelections)
                {
                    if (element != null)
                    {
                        element.Validate();
                    }
                }
            }
        }
    }
}
