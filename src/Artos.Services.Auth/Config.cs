using System;
using System.Collections.Generic;
using System.Security.Claims;
using IdentityServer4;
using IdentityServer4.Models;
using IdentityServer4.Test;


namespace Artos.Services.Auth
{
    public class Config
    {
        public Config()
        {
        }

        public static IEnumerable<IdentityResource> GetIdentityResources()
        {
            return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile(),
            };
        }

        public static List<TestUser> GetUsers()
        {
            return new List<TestUser>
            {
                new TestUser
                {
                    SubjectId = "1",
                    Username = "admin",
                    Password = "admin",
                    Claims = new []
                    {
                        new Claim("name", "admin"),
                        new Claim("website", "https://artos.com")
                    }
                },
                new TestUser
                {
                    SubjectId = "2",
                    Username = "user1",
                    Password = "password",
                    Claims = new []
                    {
                        new Claim("name", "user1"),
                        new Claim("website", "https://artos.com")
                    }
                }
            };
        }
        public static IEnumerable<ApiResource> GetApiResources()
        {
            return new List<ApiResource>
            {
                new ApiResource("masterdataapi", "Master Data API"),
                new ApiResource("transactionapi", "Transaction Data API"),

            };
        }

        public static IEnumerable<Client> GetClients()
        {
            return new List<Client>
    {
        new Client
        {
            ClientId = "mobileapp",

            // no interactive user, use the clientid/secret for authentication
            AllowedGrantTypes = GrantTypes.ClientCredentials,

            // secret for authentication
            ClientSecrets =
            {
                new Secret("mobile123".Sha256())
            },

            // scopes that client has access to
            AllowedScopes = { "masterdataapi", "transactionapi" }
                },
                new Client
                {
                    ClientId = "webapp1",ClientName="Web App send User & Password",
                    AllowedGrantTypes = GrantTypes.ResourceOwnerPassword,

                    ClientSecrets =
                    {
                        new Secret("web123".Sha256())
                    },
                    AllowedScopes = { "masterdataapi", "transactionapi" }
                },
                // OpenID Connect implicit flow client (MVC)
                new Client
                {
                    /*
                    ClientId = "mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Implicit,

                    // where to redirect to after login
                    RedirectUris = { "http://localhost:5002/signin-oidc" },

                    // where to redirect to after logout
                    PostLogoutRedirectUris = { "http://localhost:5002/signout-callback-oidc" },

                    AllowedScopes = new List<string>
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile
                    }
                    */
                    ClientId = "webapp2", 
                    ClientName = "web with openid",
                    AllowedGrantTypes = GrantTypes.HybridAndClientCredentials,
                    AllowAccessTokensViaBrowser = true,
                    ClientSecrets =
                    {
                        new Secret("web123".Sha256())
                    },

                    RedirectUris           = { "http://localhost:5005/signin-oidc" },
                    PostLogoutRedirectUris = { "http://localhost:5005/signout-callback-oidc" },
                    AllowedCorsOrigins =     { "http://localhost:5005" },

                    AllowedScopes =
                    {
                        IdentityServerConstants.StandardScopes.OpenId,
                        IdentityServerConstants.StandardScopes.Profile,
                        "masterdataapi",
                        "transactionapi"
                    },
                    AllowOfflineAccess = true
                }
    };
        }


    }
}
