using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Text;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

public partial class POReceiving : System.Web.UI.Page
{
    string strInfolog = string.Empty;
    string token;
    static string tableName, message, curDoc, curDocF, curLine, curLineF, procedure, curItem;
    static string baseURL = "https://api.businesscentral.dynamics.com";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";

    static decimal qty;
    static bool isSuccess = false, isItemValide = false;
    protected void Page_Load(object sender, EventArgs e)
    {
        //tbPOId.Focus();
        if (Session["Token"] == null)
        {
            Response.Redirect("~/Logon.aspx");
        }
        token = Session["Token"].ToString();
        if (!this.IsPostBack)
        {
            GetPOHeader();
            //this.tbItemNo.Attributes.Add("onkeyUp", "Item_keyUp");
            //this.tbPOId.Attributes.Add("onblur", "javascript:__doPostBack('UpdatePanel1','');");
            this.tbItemNo.Attributes.Add("onblur", "javascript:__doPostBack('UpdatePanel1','');");
           // this.tbPOId.Attributes.Add("onblur", "javascript:__doPostBack('UpdatePanelPO','');");
            ViewState["DocumentNo"] = Request.QueryString["DocumentNo"];
            ViewState["Description"] = Request.QueryString["Description"];
            ViewState["Type"] = Request.QueryString["Type"];
            ViewState["ItemNo"] = Request.QueryString["ItemNo"];
            ViewState["POLine"] = Request.QueryString["LineNo"];
            ViewState["QtytoReceive"] = Request.QueryString["QtytoReceive"];
            ViewState["Location"] = Request.QueryString["Location"];

            this.tbItemNo.Text = Request.QueryString["ItemNO"];
            this.tbPOId.Text = Request.QueryString["DocumentNo"];
            this.lblDescription.Text = Request.QueryString["Description"];
            this.lblPOLine.Text = Request.QueryString["LineNo"];
            this.lblQtyToReceive.Text = Request.QueryString["QtytoReceive"];
            this.lblLoc.Text = Request.QueryString["Location"];


            this.lblMessage.ForeColor = System.Drawing.Color.Red;

            curLine = lblPOLine.Text;
            curDoc = tbPOId.Text;
            curItem = tbItemNo.Text;
            
        }
    }

    private async void GetPOHeader()
    {
        using (HttpClient client = new HttpClient())
        {

            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();
            tableName = "BXTPurchaseOrderLine";
            procedure = "BCPurchLineClone";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string userAndPasswordToken =
            //Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" +
            //key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");

            var requestGetFirst = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/BXTBCPurchLine?$top=1";
            HttpResponseMessage responseFirst = await client.GetAsync(requestGetFirst);
            JObject firstline = JsonConvert.DeserializeObject<JObject>(responseFirst.Content.ReadAsStringAsync().Result);
            JObject firstlinel = JObject.Parse(firstline.ToString());
            //var getfirstLine = firstlinel.Last.Children().Children();
            foreach (JToken jt in firstlinel.Children())
            {
                JProperty jProperty = jt.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                if (propertyName == "value")
                {
                    foreach (JToken jt2 in jProperty.Children())
                    {
                        JArray array = new JArray(jt2.Children());
                        if (!array.HasValues)
                        {
                            ViewState["isItemValide"] = false;
                        }
                        else
                        {
                            curDocF = jt2[0].Value<string>("DocumentNo");
                            curLineF = jt2[0].Value<string>("LineNo");
                        }

                    }
                }
            }
            int LineNo = int.Parse(curLineF);
            var requestHeaderCopy = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/BXTBCPurchHeader('Order','{curDocF}')/NAV.BXTGetPurchHeader";
            var requestLineCopy = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/BXTBCPurchLine('Order','{curDocF}',{LineNo})/NAV.BCPurchLineClone";
            HttpResponseMessage responseCloneL = await client.PostAsync(requestLineCopy, null);
            HttpResponseMessage responseCloneH = await client.PostAsync(requestHeaderCopy, null);
            
        }
    }

    protected void btnOK_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {

    }
    protected async void tbQtyToReceive_TextChanged(object sender, EventArgs e)
    {
        //BXTHttpClientConnection conn = new BXTHttpClientConnection();
        //string token = await conn.GetToken();
        //JObject QtyInfo = await conn.validQty(token, tbPOId.Text,lblPOLine.Text);
        
    }
    //protected void btnPoLookup_Click(object sender, EventArgs e)
    //{
    //    Response.Redirect("~/InquiryPOHeaderList.aspx");

    //}
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

        if (tbQtyToReceive.Text=="")
        {
            lblMessage.Text = "";
            tbQtyToReceive.Focus();
        }
        
        else
        {
            qty = decimal.Parse(this.tbQtyToReceive.Text);
            int lblQty = int.Parse(lblQtyToReceive.Text);
            int tbQty = int.Parse(tbQtyToReceive.Text);
            curLine = lblPOLine.Text;
            curDoc = tbPOId.Text;
            curItem = tbItemNo.Text;

            if (tbQty> lblQty)
            {
                lblMessage.Text = "The input value exceeds the remaining amount";
            }
            else
            {
               if( await actionModifyQtyAsync())
                {
                    this.lblMessage.Text = "Receiving success";
                    lblQtyToReceive.Text = (lblQty - tbQty).ToString();
                    tbQtyToReceive.Text = "";
                    tbItemNo.Text = "";
                    lblPOLine.Text = "";
                    lblDescription.Text = "";
                    lblLoc.Text = "";
                    lblQtyToReceive.Text = "";
                    tbItemNo.Focus();
                }   
            }
        }    
    }

    private static async Task<bool> actionModifyQtyAsync()
    {

        tableName = "BXTPurchaseOrderLine";
        procedure = "ModifyQtyToReceive";
        using (HttpClient client = new HttpClient())
        {
            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();

            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");
            client.DefaultRequestHeaders.TryAddWithoutValidation("If-Match", "*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content", "QtyToReceive:" + qty);
            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curDoc}',{curLine})/NAV.{procedure}";
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

    protected void btnHomeURL_Click(object sender, EventArgs e)
    {

    }
    

    protected void Item_Changed(object sender, KeyEventArgs s)
    {

        this.tbItemNo.Text = s.KeyCode + ":" + s.Modifiers + ":" + s.KeyData + ":" + "(" + s.KeyValue + ")+1";
    }

  
    protected async Task<bool> ItemLookup_currentPage()
    {
        using (HttpClient client = new HttpClient())
        {
            tableName = "BXTPurchaseOrderLine";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");
            client.DefaultRequestHeaders.TryAddWithoutValidation("If-Match", "*");
            client.DefaultRequestHeaders.TryAddWithoutValidation("Content", "QtyToReceive:" + qty);

            //var requstClone = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('Order','{curDoc}',10000)/NAV.BCPurchLineClone";

            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}?$filter = DocumentNo eq '{curDoc}' and ItemNo eq '{curItem}'";
            //HttpResponseMessage responseCopy = await client.PostAsync(requstUri, null);

            HttpResponseMessage response = await client.GetAsync(requstUri);

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
                        if (!array.HasValues)
                        {
                            ViewState["isItemValide"] = false;
                        }
                        else
                        {
                            ViewState["isItemValide"] = true;
                            //lblType.Text = jt2[0].Value<string>("Type");
                            lblPOLine.Text = jt2[0].Value<string>("LineNo");
                            lblDescription.Text = jt2[0].Value<string>("Description");
                            lblLoc.Text = jt2[0].Value<string>("Location");
                            lblQtyToReceive.Text = jt2[0].Value<string>("QtytoReceive");
                        }

                    }
                }

            }
        }
        return (bool)ViewState["isItemValide"];
    }



    protected void btnPostURL_Click(object sender, EventArgs e)
    {
        if (tbPOId.Text == "")
        {
            lblMessage.Text = "";
            btnPostURL.Focus();
        }
        else
        {
            Response.Redirect("~/POPosting.aspx" + "?curDoc=" + tbPOId.Text);
        }
        // + "?qty=" + qty.ToString()
                                                     // + "&Type=" + row.Cells[2].Text
                                                    // );
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

    
    protected async void tbPOId_Changed(object sender, EventArgs s)
    {
       
      
    }



    protected async void Item_Changed(object sender, EventArgs e)
    {
        curDoc = tbPOId.Text;
        curItem = tbItemNo.Text;

        if (curItem == "")
        {
            btnReceiveURL.Enabled = false;
        }
        else
        {
            lblMessage.Text = "";
            isItemValide = await ItemLookup_currentPage();
            if (isItemValide)
            {

                btnReceiveURL.Enabled = true;
            }
            else
            {
                lblMessage.Text = "The Item has been received or does not exist ";
                btnReceiveURL.Enabled = false;
            }
        }

    }

    protected void btnItemLookup_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InquiryPOOpenList.aspx" + "?curDoc=" + tbPOId.Text);
    }

    protected void btnPoLookup_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InquiryPOHeaderList.aspx");

    }
}