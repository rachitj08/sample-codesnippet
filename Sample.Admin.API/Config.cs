//using IdentityServer4;
//using IdentityServer4.Models;
//using System.Collections.Generic;

//namespace Sample.Admin.API
//{
//    public static class Config
//    {

//        public static IEnumerable<IdentityResource> Ids =>
//            new IdentityResource[]
//            {
//                new IdentityResources.OpenId()
//            };

//        public static IEnumerable<ApiResource> GetApiResources()
//        {
//            return new List<ApiResource>
//            {
//                new ApiResource("invoice", "Invoice API")
//                {
//                    Scopes = new List<Scope>{new Scope("api1") }
//                }
//            };
//        }

//        public static IEnumerable<Client> Clients =>
//            new List<Client>
//                {
//                    new Client
//                    {
//                        ClientId = "client",
//                        ClientName = "ClientName",
//                        // no interactive user, use the clientid/secret for authentication
//                        AllowedGrantTypes = GrantTypes.ClientCredentials,

//                        // secret for authentication
//                        ClientSecrets =
//                        {
//                            new Secret("secret".Sha256())
//                        },
//                        AllowedScopes = new List<string> {"api1"},

//                    }
//                };

//    }
//}