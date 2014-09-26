using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Claims;
using System.ServiceModel.Description;
using System.Threading;
using System.Web.Http;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.Xrm.Client;
using Microsoft.Xrm.Client.Services;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Discovery;

namespace WebApi.Controllers
{
    public class ValuesController : ApiController
    {
        private string _adminName = "administrator@dev.gc";
        private SecureString _adminPassword;

        // GET api/values
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/values/5
        public string Get(int id)
        {
            _adminPassword = ConvertToSecureString("Ts08mX#");

            var claimsPrincipal = ClaimsPrincipal.Current;
            var upn = claimsPrincipal.FindFirst("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/upn").Value;
            return upn;
            //return String.Join(", ", claimsPrincipal.Claims.Select(c => c.Type + " : " + c.Value));
            try
            {
                //return OperationContext.Current.ServiceSecurityContext.WindowsIdentity.Name;
                var abc = Thread.CurrentPrincipal as ClaimsPrincipal;

                //return ((WindowsIdentity)abc.Identity).Name;
                //return ((ClaimsIdentity) abc.Identity).Name;
            }
            catch(Exception ex)
            {
                return ex.Message;
            }

            var userCredentials = new ClientCredentials();
            userCredentials.UserName.UserName = _adminName;
            userCredentials.UserName.Password = ConvertToUnsecureString(_adminPassword);

            var connSvcUser = new Microsoft.Xrm.Client.CrmConnection
            {
                ServiceUri = new Uri("https://vsdev-ifd2011.dev.gc:7778/dkdev"),
                ClientCredentials = userCredentials
            };

            var orgServiceSvcUser = new OrganizationService(connSvcUser);
            var discService = new DiscoveryService(connSvcUser);
            var whoAmIService = (WhoAmIResponse) orgServiceSvcUser.Execute(new WhoAmIRequest());

            var orgId = whoAmIService.OrganizationId;

            var request = new RetrieveUserIdByExternalIdRequest();
            //request.ExternalId = "C:" + upn;
            request.OrganizationId = orgId;

            var response = (RetrieveUserIdByExternalIdResponse) discService.Execute(request);
            var userId = response.UserId;

            var connConnectingUser = new CrmConnection
            {
                ServiceUri = connSvcUser.ServiceUri,
                ClientCredentials = userCredentials,
                //Setting CallerId is the relevant part here
                CallerId = userId
            };

            var orgServiceConnectingUser = new OrganizationService(connConnectingUser);

            var acc = new Entity("account");
            acc["name"] = "!_Test";

            return orgServiceConnectingUser.Create(acc).ToString();
        }

        /// <summary>
        ///     Convert SecureString to unsecure string.
        /// </summary>
        /// <param name="securePassword">Pass SecureString for conversion.</param>
        /// <returns>unsecure string</returns>
        public string ConvertToUnsecureString(SecureString securePassword)
        {
            if(securePassword == null)
            {
                throw new ArgumentNullException("securePassword");
            }

            var unmanagedString = IntPtr.Zero;
            try
            {
                unmanagedString = Marshal.SecureStringToGlobalAllocUnicode(securePassword);
                return Marshal.PtrToStringUni(unmanagedString);
            }
            finally
            {
                Marshal.ZeroFreeGlobalAllocUnicode(unmanagedString);
            }
        }

        /// <summary>
        ///     Convert unsecure string to SecureString.
        /// </summary>
        /// <param name="password">Pass unsecure string for conversion.</param>
        /// <returns>SecureString</returns>
        public SecureString ConvertToSecureString(string password)
        {
            if(password == null)
            {
                throw new ArgumentNullException("password");
            }

            var securePassword = new SecureString();
            foreach(var c in password)
            {
                securePassword.AppendChar(c);
            }
            securePassword.MakeReadOnly();
            return securePassword;
        }

        // POST api/values
        public void Post([FromBody] string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}