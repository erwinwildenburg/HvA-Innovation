// Copyright (c) Brock Allen & Dominick Baier. All rights reserved.
// Licensed under the Apache License, Version 2.0. See LICENSE in the project root for license information.


using IdentityServer4.Models;
using IdentityServer4.Validation;
using System.Threading.Tasks;

namespace IdentityServer.Extensions
{
    public class ExtensionGrantValidator : IExtensionGrantValidator
    {
        public Task ValidateAsync(ExtensionGrantValidationContext context)
        {
            var credential = context.Request.Raw.Get("custom_credential");

            context.Result = credential != null ? new GrantValidationResult(subject: "818727", authenticationMethod: "custom") : new GrantValidationResult(TokenRequestErrors.InvalidGrant, "invalid custom credential");

            return Task.CompletedTask;
        }

        public string GrantType => "custom";
    }
}