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
using System.Collections.Specialized;

public partial class ReceivePurchOrder : System.Web.UI.Page
{
    string strInfolog = string.Empty;
    string token;
    static string tableName, message,curDoc,curLine, procedure;
    static string baseURL = "https://api.businesscentral.dynamics.com/";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";

    static decimal qty;
    static bool isSuccess = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        tbPOId.Focus();    
        if (Session["Token"] == null)
        {
            Response.Redirect("~/Logon.aspx");
        }
        token = Session["Token"].ToString();
        if (!this.IsPostBack)
        {
            
            ViewState["DocumentNo"] = Request.QueryString["DocumentNo"];
            ViewState["Type"] = Request.QueryString["Type"];
            ViewState["Item"] = Request.QueryString["ItemNo"];
            ViewState["POLine"] = Request.QueryString["LineNo"];
            ViewState["QtytoReceive"] = Request.QueryString["QtytoReceive"];
            ViewState["Location"] = Request.QueryString["Location"];
            //try
            //{
            //    if (Request.RequestType == "POST")
            //    {
            //        NameValueCollection para = Request.Params;
            //        if (para != null)
            //        {
            //            this.lblPOLine.Text = para["LineNo"].ToString();
            //            this.lblType.Text = para["Type"].ToString();
            //            this.tbPOId.Text = para["DocumentNo"].ToString();
            //            this.lblItemNo.Text = para["ItemNO"].ToString();
            //            this.lblDescription.Text = para["Description"].ToString();
            //            this.lblQtyToReceive.Text = para["QtytoReceive"].ToString();
            //            this.lblQtyReceived.Text = para["QtytoReceived"].ToString();
            //            this.lblQty.Text = para["Quantity"].ToString();
            //            this.lblLoc.Text = para["Location"].ToString();
            //        }
            //    }



            //}catch(Exception e1)
            //{
            //    e1.Message.ToString();
            //}

            this.tbPOId.Text = Request.QueryString["DocumentNo"];
            this.lblDescription.Text = Request.QueryString["Description"];
            this.lblPOLine.Text = Request.QueryString["LineNo"];
            this.lblType.Text = Request.QueryString["Type"];
            this.lblQtyToReceive.Text = Request.QueryString["QtytoReceive"];
            this.lblQty.Text = Request.QueryString["Quantity"];
            this.lblQtyReceived.Text = Request.QueryString["QtytoReceived"];
            this.lblLoc.Text = Request.QueryString["Location"];
            this.lblItemNo.Text = Request.QueryString["ItemNO"];

            this.lblMessage.ForeColor = System.Drawing.Color.Red;

            curLine = this.lblPOLine.Text;
            curDoc = this.tbPOId.Text ;
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected void tbQtyToReceive_TextChanged(object sender, EventArgs e)
    {

    }
    protected void btnPoLookup_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InquiryPOOpenList.aspx");

    }
    protected void validPost()
    {
        if (isSuccess)
        {
            this.lblMessage.Text = "OK";
        }
        else
        {
            this.lblMessage.Text = "failed";
        }
    }

    protected async void btnReceiveURL_Click(object sender, EventArgs e)
    {
        qty = decimal.Parse(this.tbQtyToReceive.Text);
        string tbQty = tbQtyToReceive.Text;
        string Received = lblQtyReceived.Text;
        string Quantity = lblQty.Text;

        int intQty = int.Parse(Quantity);
        int intRece = int.Parse(Received);
        int valideReceived = intQty - intRece;
        if (qty>valideReceived)
        {
            lblMessage.Text = "The quantity you input has exceeded";
        }
        else
        {
            if (await actionModifyQtyAsync())
            {
       
                this.tbQtyToReceive.Text = "";
                int intTbQty = int.Parse(tbQty);
                int ed = intRece + intTbQty;
                lblQtyReceived.Text = ed.ToString();
                lblQtyToReceive.Text = "";
                this.lblMessage.Text = "Receiving success";
            }
        }    
    }

    private static async Task<bool> actionModifyQtyAsync()
    {
        tableName = "BXTPurchaseOrderLine";
        procedure = "ModifyQtyToReceive";
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            client.DefaultRequestHeaders.TryAddWithoutValidation("If-Match", "*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content", "QtyToReceive:" + qty);
            var requstUri = $"{baseURL}v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curDoc}',{curLine})/NAV.{procedure}";
            var body = "{\"documentno\":\"" + curDoc + "\"," + "\"lineno\":" + curLine + "," + "\"qtytoreceive\":" + qty + "}";
            HttpContent contentPost = new StringContent(body, Encoding.UTF8, "application/json");
           
            HttpResponseMessage responsePost = await client.PostAsync(requstUri, contentPost);
            if (responsePost.IsSuccessStatusCode)
            {
                isSuccess = true;
            }
            else
            {
                isSuccess = false;
            }
            return isSuccess;
        }
    }

    //public static async void ApproveRelease()
    //{

    //    baseURL = "https://api.businesscentral.dynamics.com/";
    //    user = "BCADMIN";
    //    key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    //    tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    //    environment = "FLH-TestProdData";
    //    company = "FLH-Prod";
    //    tableName = "BXTPurchaseOrderLine";
    //    client.DefaultRequestHeaders.Accept.Clear();
    //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //    string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
    //    client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
    //    client.DefaultRequestHeaders.TryAddWithoutValidation("If-Match", "*");
    //    client.DefaultRequestHeaders.TryAddWithoutValidation("Content", "QtyToReceive:" + qty);
    //    var requstUri = $"{baseURL}v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curLine}','{curDoc}')";

    //    var body = @"{""QtytoReceive"": "+qty+"}";

    //    var method = new HttpMethod("PATCH");
    //    var httpRequestMessage =
    //    new HttpRequestMessage(new HttpMethod("PATCH"), requstUri)
    //    {
    //        Content = new StringContent(body, Encoding.UTF8, "application/json")
    //    };

    //    HttpResponseMessage response = await client.SendAsync(httpRequestMessage);
    //    if (response.IsSuccessStatusCode)
    //    {
    //        isSuccess = true;
    //    }
    //    else
    //    {
    //        isSuccess = false;
    //    }
    //}

    protected void btnHomeURL_Click(object sender, EventArgs e)
    {

    }
    protected void tbPOId_Changed(object sender, EventArgs s)
    {

    }

    protected void Item_Changed(object sender, EventArgs s)
    {

    }

    protected void ckMoveNext_CheckedChanged(object sender, EventArgs s)
    {

    }



    protected void btnPostURL_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ReceivePurchOrderItemList.aspx" + "?qty=" + qty.ToString()
                                                    // + "&Type=" + row.Cells[2].Text
                                                     );
        //this.tbQtyToReceive.Text = "";
        //if (await actionPurchPost())
        //{
        //    this.lblQtyToReceive.Text = qty.ToString();
        //    this.tbQtyToReceive.Text = "";
        //    this.lblMessage.Text = "Posting success";
        //}
        //else
        //{
        //    this.lblMessage.Text = message;
        //}
    }

    //private static async Task<bool> actionPurchPost()
    //{
    //    tableName = "BXTPurchaseOrderLine";
    //    procedure = "PurchPost";
    //    using (HttpClient client = new HttpClient())
    //    {
    //        client.DefaultRequestHeaders.Accept.Clear();
    //        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    //        string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
    //        client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
    //        var requstUri = $"{baseURL}v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curDoc}',{curLine})/NAV.{procedure}";
    //        var body = "{\"documentno\":\"" + curDoc + "\"}";
    //        var contentPost = new StringContent(body, Encoding.UTF8, "application/json");
    //        HttpResponseMessage responsePost = await client.PostAsync(requstUri, contentPost).ConfigureAwait(false);
    //        if (responsePost.IsSuccessStatusCode)
    //        {
    //            isSuccess = true;
    //        }
    //        else
    //        {
    //            message = responsePost.Content.ToString();
    //            isSuccess = false;
    //        }
    //        return isSuccess;
    //    }
           
    //}
}