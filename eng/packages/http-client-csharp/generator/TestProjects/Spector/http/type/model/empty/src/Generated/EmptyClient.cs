// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

// <auto-generated/>

#nullable disable

using System;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.Core;
using Azure.Core.Pipeline;

namespace _Type.Model.Empty
{
    public partial class EmptyClient
    {
        public EmptyClient() : this(new Uri("http://localhost:3000"), new EmptyClientOptions()) => throw null;

        public EmptyClient(Uri endpoint, EmptyClientOptions options) => throw null;

        public HttpPipeline Pipeline => throw null;

        public virtual Response PutEmpty(RequestContent content, RequestContext context = null) => throw null;

        public virtual Task<Response> PutEmptyAsync(RequestContent content, RequestContext context = null) => throw null;

        public virtual Response PutEmpty(EmptyInput input, CancellationToken cancellationToken = default) => throw null;

        public virtual Task<Response> PutEmptyAsync(EmptyInput input, CancellationToken cancellationToken = default) => throw null;

        public virtual Response GetEmpty(RequestContext context) => throw null;

        public virtual Task<Response> GetEmptyAsync(RequestContext context) => throw null;

        public virtual Response<EmptyOutput> GetEmpty(CancellationToken cancellationToken = default) => throw null;

        public virtual Task<Response<EmptyOutput>> GetEmptyAsync(CancellationToken cancellationToken = default) => throw null;

        public virtual Response PostRoundTripEmpty(RequestContent content, RequestContext context = null) => throw null;

        public virtual Task<Response> PostRoundTripEmptyAsync(RequestContent content, RequestContext context = null) => throw null;

        public virtual Response<EmptyInputOutput> PostRoundTripEmpty(EmptyInputOutput body, CancellationToken cancellationToken = default) => throw null;

        public virtual Task<Response<EmptyInputOutput>> PostRoundTripEmptyAsync(EmptyInputOutput body, CancellationToken cancellationToken = default) => throw null;
    }
}
