using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

public partial class POPosting : System.Web.UI.Page
{
    static HttpClient client = new HttpClient();
    string token;
    static string tableName, message, curDoc, procedure;
    static int curLine;
    static string baseURL = "https://api.businesscentral.dynamics.com/";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";
    string requestValid = null;
    static bool isSuccess = false;
    Nullable<int> currentIndex;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Token"] == null)
        {
            Response.Redirect("~/Logon.aspx");
        }
        token = Session["Token"].ToString();
        if (!IsPostBack)
        {
            BCDataCollection();
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
            curDoc = Request.QueryString["curDoc"];
            curDocument.Text = curDoc;
            

        }
    }
    private async void BCDataCollection()
    {

        baseURL = "https://api.businesscentral.dynamics.com";

        string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
        environment = "FLH-TestProdData";
        company = "FLH-Prod";
        string tableName = "BXTPurchaseOrderLine";
        string ErrorUser = null;
        Session["CurrentWarehouse"] = "location";
        Session["CurrentCompany"] = "FLH-Test";

        //var response = client.PostAsync(baseURL,new FormUrlEncodedContent(new List<KeyValuePair<String, String>>)){
        // client.BaseAddress = new Uri(baseURL);

        using (HttpClient client = new HttpClient())
        {
            //curDoc = "PO000000001";
            //curLine = 10000;
            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();
            tableName = "BXTPurchaseOrderLine";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");

            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}?$filter = Status eq 'modified' and DocumentNo eq '{curDoc}'";
            //var requestGetHeader = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/BXTPurchaseOrderHeader";
            HttpResponseMessage response = await client.GetAsync(requstUri);

            //JObject BCWHSUsers = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            //JObject users = JObject.Parse(BCWHSUsers.ToString());
            JObject productOrderList = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);

            JObject productOrderLists = JObject.Parse(productOrderList.ToString());

            foreach (JToken jt in productOrderLists.Children())
            {
                JProperty jProperty = jt.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                if (propertyName == "value")
                {
                    foreach (JToken jt2 in jProperty.Children())
                    {
                        JArray array = new JArray(jt2.Children());
                        var item = array.ToList();
                        if(item.Count == 0)
                        {
                            lblMessage.Text = "There is no item to post";
                        }
                        else
                        {
                            gvReceivedList.DataSource = item;

                            gvReceivedList.DataBind();
                        }
                        

                    }
                }

            }

        }

    }


    protected void btnBackURL_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/POReceiving.aspx");
    }

    protected void gvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReceivedList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void GridViewHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvReceivedList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void Button_search_Click(object sender, EventArgs e)
    {
        BCDataCollection();
    }
    protected void lb_firstpage_Click(object sender, EventArgs e)
    {
        this.gvReceivedList.PageIndex = 0;
        BCDataCollection();
    }
    protected void lb_previouspage_Click(object sender, EventArgs e)
    {
        if (this.gvReceivedList.PageIndex > 0)
        {
            this.gvReceivedList.PageIndex--;
            BCDataCollection();
        }
    }
    protected void lb_nextpage_Click(object sender, EventArgs e)
    {
        if (this.gvReceivedList.PageIndex < this.gvReceivedList.PageCount)
        {
            this.gvReceivedList.PageIndex++;
            BCDataCollection();
        }
    }
    protected void lb_lastpage_Click(object sender, EventArgs e)
    {
        this.gvReceivedList.PageIndex = this.gvReceivedList.PageCount;
        BCDataCollection();
    }

    protected async void ComfirmPosting_Click(object sender, EventArgs e)
    {
        foreach (GridViewRow row in gvReceivedList.Rows)
        {
            curLine = int.Parse(gvReceivedList.Rows[row.RowIndex].Cells[0].Text);
        }

        if (await actionPurchPost())
        {
            lblMessage.Text = "Posting success";
        }
        else
        {
            lblMessage.Text = message;
        }
       
    }

    private static async Task<bool> actionPurchPost()
    {

        tableName = "BXTPurchaseOrderLine";
        procedure = "PurchPost";
        using (HttpClient client = new HttpClient())
        {
            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");
            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curDoc}',{curLine})/NAV.{procedure}";
            var body = "{\"documentno\":\"" + curDoc + "\"}";
            var contentPost = new StringContent(body, Encoding.UTF8, "application/json");
            //HttpResponseMessage responsePost = await client.PostAsync(requstUri, contentPost).ConfigureAwait(false);

            HttpResponseMessage responsePost = await client.PostAsync(requstUri, contentPost);
            if (responsePost.IsSuccessStatusCode)
            {
                isSuccess = true;
            }
            else
            {
                message = responsePost.Content.ToString();
                isSuccess = false;
            }
            return isSuccess;
        }

    }

    protected void BackButton_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/POReceiving.aspx");
    }

    //protected async void Continue_Click(object sender, EventArgs e)
    //{
    //    //Response.Redirect("~/InquiryPOOpenList.aspx");
    //    if (await actionPurchPost())
    //    {
    //        //this.lblQtyToReceive.Text = Request.QueryString["qty"];
    //        //this.tbQtyToReceive.Text = "";
    //        this.lblMessage.Text = "Posting success";
    //    }
    //    else
    //    {
    //        this.lblMessage.Text = message;
    //    }
    //}

    //protected void Continue_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/POReceiving.aspx");
    //}
}