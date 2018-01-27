using IdentityServer4.Models;
using System.Collections.Generic;
using IdentityModel;

namespace IdentityServer
{
    public class Config
    {
        // scopes define the resources in your system
        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
        }

        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("api", "API")
                {
                    ApiSecrets =
                    {
                        new Secret("secret".Sha256())
                    },
                    UserClaims =
                    {
                        JwtClaimTypes.GivenName,
                        JwtClaimTypes.FamilyName,
                        "tid" // Microsoft Azure tenant id
                    }
                }
            };
        }
    }
}
