// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.ComponentModel;

namespace Azure.ResourceManager.SecurityInsights.Models
{
    /// <summary> The type of repository. </summary>
    public readonly partial struct SourceControlRepoType : IEquatable<SourceControlRepoType>
    {
        private readonly string _value;

        /// <summary> Initializes a new instance of <see cref="SourceControlRepoType"/>. </summary>
        /// <exception cref="ArgumentNullException"> <paramref name="value"/> is null. </exception>
        public SourceControlRepoType(string value)
        {
            _value = value ?? throw new ArgumentNullException(nameof(value));
        }

        private const string GithubValue = "Github";
        private const string AzureDevOpsValue = "AzureDevOps";

        /// <summary> Github. </summary>
        public static SourceControlRepoType Github { get; } = new SourceControlRepoType(GithubValue);
        /// <summary> AzureDevOps. </summary>
        public static SourceControlRepoType AzureDevOps { get; } = new SourceControlRepoType(AzureDevOpsValue);
        /// <summary> Determines if two <see cref="SourceControlRepoType"/> values are the same. </summary>
        public static bool operator ==(SourceControlRepoType left, SourceControlRepoType right) => left.Equals(right);
        /// <summary> Determines if two <see cref="SourceControlRepoType"/> values are not the same. </summary>
        public static bool operator !=(SourceControlRepoType left, SourceControlRepoType right) => !left.Equals(right);
        /// <summary> Converts a string to a <see cref="SourceControlRepoType"/>. </summary>
        public static implicit operator SourceControlRepoType(string value) => new SourceControlRepoType(value);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object obj) => obj is SourceControlRepoType other && Equals(other);
        /// <inheritdoc />
        public bool Equals(SourceControlRepoType other) => string.Equals(_value, other._value, StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc />
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode() => _value != null ? StringComparer.InvariantCultureIgnoreCase.GetHashCode(_value) : 0;
        /// <inheritdoc />
        public override string ToString() => _value;
    }
}
