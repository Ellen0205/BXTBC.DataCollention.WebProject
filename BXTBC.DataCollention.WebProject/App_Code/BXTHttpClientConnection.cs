using Microsoft.Identity.Client;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.IdentityModel.Tokens;
using IdentityModel.Client;
using Flurl.Http;

/// <summary>
/// Summary description for BXTBCPurchaseContract
/// </summary>
public class BXTHttpClientConnection
{

    public string BCBaseURL = "https://api.businesscentral.dynamics.com";
    public string user = "BCADMIN";
    //public string key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImpTMVhvMU9XRGpfNTJ2YndHTmd2UU8yVnpNYyIsImtpZCI6ImpTMVhvMU9XRGpfNTJ2YndHTmd2UU8yVnpNYyJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvYzNhNmIyMjUtZGVmYy00YjJjLWFmMjYtNjZkMDY3MjkyMGE3LyIsImlhdCI6MTY1MTU0NjgyMSwibmJmIjoxNjUxNTQ2ODIxLCJleHAiOjE2NTE1NTA3MjEsImFpbyI6IkUyWmdZSWhNOTJMcmVENTFTcEdZN0RZbTMzV3JBQT09IiwiYXBwaWQiOiJiOGM4YWI4MC1kMTVjLTQxNjQtYWRkNi01NGMxYzg2MzhjYTEiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9jM2E2YjIyNS1kZWZjLTRiMmMtYWYyNi02NmQwNjcyOTIwYTcvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI3OWU3OWZmZS0yYjUxLTRlNjEtYjQ0Ni04Mjk1Y2EwZmM4ZjgiLCJyaCI6IjAuQVNrQUpiS213X3plTEV1dkptYlFaeWtncHozdmJabHNzMU5CaGdlbV9Ud0J1SjhwQUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI3OWU3OWZmZS0yYjUxLTRlNjEtYjQ0Ni04Mjk1Y2EwZmM4ZjgiLCJ0aWQiOiJjM2E2YjIyNS1kZWZjLTRiMmMtYWYyNi02NmQwNjcyOTIwYTciLCJ1dGkiOiJoTzRTcXRnY2cwT09LWnljcVNKMEFBIiwidmVyIjoiMS4wIn0.Li4JKu95gB3T_GhbZiC-H_kFvUupVk3BI3P3njEsDp8ph13qbBtdQd3Lc8-L4GQ0BPPOLbhsh1kQPacovBhrA73_rcon0pQpH0QAqBQfDscR_BB141paEuxffI-cE8my3r3T7P1XphqIOOsdxLikyIBC2zXMQcBIj1AuOU6rf-y-e9imdGLFyAK2a1F-auhaoUFdQtWgBHtcKqSMWmQ_nmeavDwQphuojiriv7ax-pZuZ4Mq8t_rwTtRnA0Uubxq709clUutY19rJ3nymQ14mecxLtvnmMPxq0UjTpP6anvksOV2Hbp2dvUBGbefGKT1ZvgZCLeNdJbrDGjBY1Vfhw";
    public string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    public string environment = "FLH-TestProdData";
    public string company = "FLH-Prod";
    public string webservice = "ODataV4";
    public string tableItemJournal = "BXTPhysicalInventoryJournals";
    public string grant_type = "client_credentials";
    public string client_id = "cd291abf-161d-4892-95eb-6d247029bc29";
    public string client_secret = ".wE8Q~u6eCMxmEKg2PS_4moZPr8R6ZKTgJBjIdqA";
    public string scope= "https://api.businesscentral.dynamics.com/.default";
    public string requestUrlInventory = null;
    

    public async Task<JObject> validQty(string token,string ducomentNo,string lineNo)
    {
        using (HttpClient client = new HttpClient())
        {

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));
            
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var QtyInfo = $"{BCBaseURL}/v2.0/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-TestProdData/ODataV4/Company('FLH-Prod')/BXTPurchaseOrderLine('{ducomentNo}',{int.Parse(lineNo)})";
            HttpResponseMessage response = await client.GetAsync(QtyInfo);
            JObject QtyInfoLine = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            JObject QtyInfoLines = JObject.Parse(QtyInfoLine.ToString());
            QtyInfoLines.ToString();
            return QtyInfoLines;
        }
    }

    public async Task<JObject> GetPOHeader(string token)
    {
        using (HttpClient client = new HttpClient())
        {

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string accessToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(key));

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {token}");

            var requestGetInvenInformation = $"{BCBaseURL}/v2.0/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-TestProdData/ODataV4/Company('FLH-Prod')/BXTBCPurchHeader('Order','PO000000005')/NAV.BXTGetPurchHeader";

            HttpResponseMessage response = await client.PostAsync(requestGetInvenInformation,null);
            JObject itemJouranlline = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            JObject itemJouranllines = JObject.Parse(itemJouranlline.ToString());
            itemJouranllines.ToString();
            return itemJouranllines;
        }
    }
    public async Task<string> GetToken()
    {
        string baseUrl = "https://login.microsoftonline.com/c3a6b225-defc-4b2c-af26-66d0672920a7/oauth2/v2.0/token";
        string uri = "https://login.microsoftonline.com/common/oauth2/nativeclient";
        string identityUrl = $"{baseUrl}?client_id=b8c8ab80-d15c-4164-add6-54c1c8638ca1&response_type=code&redirect_uri = {uri}";
        //string identityUrl = "https://login.microsoftonline.com/c3a6b225-defc-4b2c-af26-66d0672920a7/oauth2/v2.0/token";
        TokenResponse tokenResponse;
        using (HttpClient httpClient = new HttpClient())
        {
            httpClient.BaseAddress = new Uri(identityUrl);

             DiscoveryDocumentResponse disco = await httpClient.GetDiscoveryDocumentAsync();

            tokenResponse = await httpClient.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = disco.TokenEndpoint,
                ClientId = client_id,
                ClientSecret = client_secret,
                Scope = scope
            });
        }
        return tokenResponse.AccessToken;
        //Console.WriteLine($"Access token: {tokenResponse.AccessToken}");
    }
    public async Task<JObject> GetCompanies(string accessToken)
    {
        var result =  await $"https://api.businesscentral.dynamics.com/v2.0/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-TestProdData/api/v2.0/companies"
            .WithOAuthBearerToken(accessToken)
            .GetJsonAsync();
        JObject conpanies = JsonConvert.DeserializeObject<JObject>(result.Content.ReadAsStringAsync().Result);
        JObject conpanieslines = JObject.Parse(conpanies.ToString());
        return conpanieslines;
    }

    
}