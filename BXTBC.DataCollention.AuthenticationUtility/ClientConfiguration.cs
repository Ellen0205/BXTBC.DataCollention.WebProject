using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BXTBC.DataCollention.AuthenticationUtility
{
    public partial class ClientConfiguration
    {
        public static ClientConfiguration Default { get { return ClientConfiguration.OneBox; } }

        public static ClientConfiguration OneBox = new ClientConfiguration()
        {
            // You only need to populate this section if you are logging on via a native app. For Service to Service scenarios in which you e.g. use a service principal you don't need that.
            UriString = "https://businesscentral.dynamics.com/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-Test/", //dev2
            //UriString = "https://kws-uat-ms.sandbox.operations.dynamics.com/", //UAT
            //UriString = "https://kws-prod-ms.operations.dynamics.com/", //PROD
            UserName = "grubby@finlandiahealth.com",
            // Insert the correct password here for the actual test.
            Password = "FFrto712",

            // You need this only if you logon via service principal using a client secret. See: https://docs.microsoft.com/en-us/dynamics365/unified-operations/dev-itpro/data-entities/services-home-page to get more data on how to populate those fields.
            // You can find that under AAD in the azure portal
            ActiveDirectoryResource = "https://businesscentral.dynamics.com/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-Test/", // Don't have a trailing "/". Note: Some of the sample code handles that issue.
            //ActiveDirectoryResource = "https://kws-uat-ms.sandbox.operations.dynamics.com", // Don't have a trailing "/". Note: Some of the sample code handles that issue.
            //ActiveDirectoryResource = "https://kws-prod-ms.operations.dynamics.com", // Don't have a trailing "/". Note: Some of the sample code handles that issue.
            ActiveDirectoryTenant = "https://login.windows.net/c3a6b225-defc-4b2c-af26-66d0672920a7.onmicrosoft.com", // Some samples: https://login.windows.net/yourtenant.onmicrosoft.com, https://login.windows.net/microsoft.com
            ActiveDirectoryClientAppId = "b8c8ab80-d15c-4164-add6-54c1c8638ca1",//"dedec74d-7ce3-469b-ba7f-80effd8edf29",  //DEV2
            //ActiveDirectoryClientAppId = "0a8c451f-237d-4d25-87a1-ae0af05ffee9", //UAT
            //ActiveDirectoryClientAppId = "54bd9877-5ece-461e-8e8b-c9cdc1fc82cd", //PRD
            // Insert here the application secret when authenticate with AAD by the application
            ActiveDirectoryClientAppSecret = "7c44ffa6-1c6f-4b38-9ef5-45abc0402d92",//"3FUweBjBBXiQ7EJ3iflJtx4Rc]SCb@..",  //dev2
            //ActiveDirectoryClientAppSecret = "hFU-:]qnlXXL547FTjT.3JkZMdR:vGfY",  //UAT
            //ActiveDirectoryClientAppSecret = "0BC7Q~k12SGGW1b2bgNm.nceYWc2XzVwnBqlF",  //PRD

            // Change TLS version of HTTP request from the client here
            // Ex: TLSVersion = "1.2"
            // Leave it empty if want to use the default version
            TLSVersion = "",

        };
        public string TLSVersion { get; set; }
        public string UriString { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string ActiveDirectoryResource { get; set; }
        public String ActiveDirectoryTenant { get; set; }
        public String ActiveDirectoryClientAppId { get; set; }
        public string ActiveDirectoryClientAppSecret { get; set; }
    }
}
