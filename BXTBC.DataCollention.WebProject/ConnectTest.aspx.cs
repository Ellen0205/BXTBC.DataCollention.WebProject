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
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class ConnectTest : System.Web.UI.Page
{
    //public BXTHttpClientConnection conn;
    public string BCBaseURL = "https://api.businesscentral.dynamics.com";
    public string user = "BCADMIN";
    public string key = "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6ImpTMVhvMU9XRGpfNTJ2YndHTmd2UU8yVnpNYyIsImtpZCI6ImpTMVhvMU9XRGpfNTJ2YndHTmd2UU8yVnpNYyJ9.eyJhdWQiOiJodHRwczovL2FwaS5idXNpbmVzc2NlbnRyYWwuZHluYW1pY3MuY29tIiwiaXNzIjoiaHR0cHM6Ly9zdHMud2luZG93cy5uZXQvYzNhNmIyMjUtZGVmYy00YjJjLWFmMjYtNjZkMDY3MjkyMGE3LyIsImlhdCI6MTY1MTU0NjgyMSwibmJmIjoxNjUxNTQ2ODIxLCJleHAiOjE2NTE1NTA3MjEsImFpbyI6IkUyWmdZSWhNOTJMcmVENTFTcEdZN0RZbTMzV3JBQT09IiwiYXBwaWQiOiJiOGM4YWI4MC1kMTVjLTQxNjQtYWRkNi01NGMxYzg2MzhjYTEiLCJhcHBpZGFjciI6IjEiLCJpZHAiOiJodHRwczovL3N0cy53aW5kb3dzLm5ldC9jM2E2YjIyNS1kZWZjLTRiMmMtYWYyNi02NmQwNjcyOTIwYTcvIiwiaWR0eXAiOiJhcHAiLCJvaWQiOiI3OWU3OWZmZS0yYjUxLTRlNjEtYjQ0Ni04Mjk1Y2EwZmM4ZjgiLCJyaCI6IjAuQVNrQUpiS213X3plTEV1dkptYlFaeWtncHozdmJabHNzMU5CaGdlbV9Ud0J1SjhwQUFBLiIsInJvbGVzIjpbIkF1dG9tYXRpb24uUmVhZFdyaXRlLkFsbCIsImFwcF9hY2Nlc3MiLCJBUEkuUmVhZFdyaXRlLkFsbCJdLCJzdWIiOiI3OWU3OWZmZS0yYjUxLTRlNjEtYjQ0Ni04Mjk1Y2EwZmM4ZjgiLCJ0aWQiOiJjM2E2YjIyNS1kZWZjLTRiMmMtYWYyNi02NmQwNjcyOTIwYTciLCJ1dGkiOiJoTzRTcXRnY2cwT09LWnljcVNKMEFBIiwidmVyIjoiMS4wIn0.Li4JKu95gB3T_GhbZiC-H_kFvUupVk3BI3P3njEsDp8ph13qbBtdQd3Lc8-L4GQ0BPPOLbhsh1kQPacovBhrA73_rcon0pQpH0QAqBQfDscR_BB141paEuxffI-cE8my3r3T7P1XphqIOOsdxLikyIBC2zXMQcBIj1AuOU6rf-y-e9imdGLFyAK2a1F-auhaoUFdQtWgBHtcKqSMWmQ_nmeavDwQphuojiriv7ax-pZuZ4Mq8t_rwTtRnA0Uubxq709clUutY19rJ3nymQ14mecxLtvnmMPxq0UjTpP6anvksOV2Hbp2dvUBGbefGKT1ZvgZCLeNdJbrDGjBY1Vfhw";
    public string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    public string environment = "FLH-TestProdData";
    public string company = "FLH-Prod";
    public string webservice = "ODataV4";
    public string tableItemJournal = "BXTPhysicalInventoryJournals";
    public string grant_type = "client_credentials";
    public string client_id = "b8c8ab80-d15c-4164-add6-54c1c8638ca1";
    public string client_secret = "I.37Q~rq2MUte5v_Vy4A5HXybi9N3zocjbaFD";
    public string requestUrlInventory = null;
    //BXTHttpClientConnection conn = new BXTHttpClientConnection();

    protected void Page_Load(object sender, EventArgs e)
    {

    }


    protected async void btnClick(object sender, EventArgs e)
    {
        //bool result = false;
        //BXTHttpClientConnection conn = new BXTHttpClientConnection();
        //string token =  await conn.GetToken();
        //JObject company = await conn.GETCompanyInfo(token);
        //var companylist = company.Last.Children().Children();
        //gvComJourList.DataSource = companylist.ToList();
        //gvComJourList.DataBind();
    }
}