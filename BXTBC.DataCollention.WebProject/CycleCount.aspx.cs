using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class CycleCount : System.Web.UI.Page
{

    public BXTHttpClientConnection conn;
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

    protected void Page_Load(object sender, EventArgs e)
    {
        //tbItem.Focus();

        //tbItem.Attributes.Add("onkeydown", "checkKey(this.id)");
        //TbQuantity.Attributes.Add("onkeydown", "checkKey(this.id)");

        //TbItem.Attributes.Add("onchange", "fn_SetFocus();");

        //if (Session["Token"] == null)
        //{
        //    Response.Redirect("~/Logon.aspx");
        //}

        if (!IsPostBack)
        {
            GridViewDataBind();
            this.tbItemNo.Text = Request.QueryString["ItemNo"];
            this.lblQtyCalulated.Text = Request.QueryString["QtyCalculated"];
            this.lblQuantity.Text = Request.QueryString["Quantity"];
           
        }

        //btnCycleSubmit.Enabled = ViewState["IsItemValid"] != null
        //    && (bool)ViewState["IsItemValid"]
        //    && ViewState["IsLocationValid"] != null
        //    && (bool)ViewState["IsLocationValid"];
        //btnItemLookup.Enabled = true;

    }

   private async void GridViewDataBind()
    {
        //using (HttpClient client = new HttpClient())
        //{
        //    Uri path = new Uri(BCBaseURL);
        //    var requestGetInvenInformation = $"{BCBaseURL}/v2.0/{tenantID}/{environment}/api/v2.0/companies";
        //    var request = new HttpRequestMessage(HttpMethod.Post, requestGetInvenInformation);
        //    var form = new Dictionary<string, string>
        //    {
        //        {"grant_type", grant_type},
        //        {"client_secret", client_secret},
        //        {"client_secret", client_secret}
        //    };
        //    var response = await client.SendAsync(request);
        //    response.EnsureSuccessStatusCode();

           // client.DefaultRequestHeaders.Accept.Clear();
           // client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
           // string userAndPasswordToken =
           // Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" +
           // key));
           // client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
           // var requestGetInvenInformation = $"{BCBaseURL}/v2.0/{tenantID}/{environment}/{webservice}/Company('{company}')/{tableItemJournal}";
           // HttpResponseMessage response = await client.GetAsync(requestGetInvenInformation);
           // JObject itemJouranlline = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

           //// var itemJouranlLine = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            
           // JObject itemJouranllines = JObject.Parse(itemJouranlline.ToString());

            //foreach (JToken jt in itemJouranllines.Children())
            //{
            //    JProperty jProperty = jt.ToObject<JProperty>();
            //    string propertyName = jProperty.Name;
            //    if (propertyName == "value")
            //    {
            //        foreach (JToken jt2 in jProperty.Children())
            //        {
            //            JArray array = new JArray(jt2.Children());
            //            var item = array.ToList();

            //            //gvPOList.DataSource = item;

            //            //gvPOList.DataBind();
            //        }
            //    }

            //}
            //itemJouranllines.ToString();
            //JObject itemJournalLine = conn.GETInvenInformation("PHYS. INVE", "DEFAULT", "0000000005731").Result;
        //}
           
    }

    protected void btnItemLookup_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InquiryItemList.aspx");
        //Response.Redirect("~/InquiryItemList.aspx" + "?LocationId=" + this.tbLocation.Text);
        //this.tbLocation.Focus();
    }

    protected void tbLocation_TextChanged(object sender, EventArgs e)
    {

    }

    protected void TbQuantity_TextChanged(object sender, EventArgs e)
    {

    }

    protected void ddItemNo_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected async void btnCycleSubmit_Click(object sender, EventArgs e)
    {
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userAndPasswordToken =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" +
            key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var requestGetInvenInformation = $"{BCBaseURL}/v2.0/{tenantID}/{environment}/{webservice}/Company('{company}')/{tableItemJournal}";
            HttpResponseMessage responseSend = await client.GetAsync(requestGetInvenInformation);
            JObject itemJouranlline = JsonConvert.DeserializeObject<JObject>(responseSend.Content.ReadAsStringAsync().Result);

            // var itemJouranlLine = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

            JObject itemJouranllines = JObject.Parse(itemJouranlline.ToString());

            foreach (JToken jt in itemJouranllines.Children())
            {
                JProperty jProperty = jt.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                if (propertyName == "value")
                {
                    foreach (JToken jt2 in jProperty.Children())
                    {
                        JArray array = new JArray(jt2.Children());
                        var item = array.ToList();

                        //gvPOList.DataSource = item;

                        //gvPOList.DataBind();
                    }
                }

            }
            itemJouranllines.ToString();
            //JObject itemJournalLine = conn.GETInvenInformation("PHYS. INVE", "DEFAULT", "0000000005731").Result;
        }
    }
}
