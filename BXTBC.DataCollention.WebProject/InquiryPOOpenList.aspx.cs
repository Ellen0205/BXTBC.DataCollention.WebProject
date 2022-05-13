using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InquiryPOOpenList : System.Web.UI.Page
{
    string token;
    static HttpClient client = new HttpClient();
    static string tableName, message, tbcurDoc, curDoc,lineNo, procedure, tableNameBC,DocumentType;
    static string baseURL = "https://api.businesscentral.dynamics.com/";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";
    string requestValid = null;
    bool isSuccess = false;
    Nullable<int> currentIndex;
    protected void Page_Load(object sender, EventArgs e)
    {

        if (Session["Token"] == null)
        {
            Response.Redirect("~/Logon.aspx");
        }
        token = Session["Token"].ToString();
        tbcurDoc = Request.QueryString["curDoc"];
        this.curDocumentNo.Text = tbcurDoc;
        if (!IsPostBack)
        {
            BCDataCollection();
        }
    }
    private async void BCDataCollection()
    {
        int site = 0;
        bool isSuccess = false;
        string isValid = null;
        //connect BC

        baseURL = "https://api.businesscentral.dynamics.com";
        user = "BCADMIN";
        key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
        string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
        environment = "FLH-TestProdData";
        company = "FLH-Prod";
        string tableName = "BXTPurchaseOrderLine";
        string ErrorUser = null;
        Session["CurrentWarehouse"] = "location";
        Session["CurrentCompany"] = "FLH-Test";
        //var response = client.PostAsync(baseURL,new FormUrlEncodedContent(new List<KeyValuePair<String, String>>)){
        // client.BaseAddress = new Uri(baseURL);
        
        using(HttpClient client = new HttpClient())
        {
            //gvPOList.DataSource = null;
            BXTHttpClientConnection conn = new BXTHttpClientConnection();
            string userAndPasswordToken = await conn.GetToken();
            //int curLine = 10000;
            tableName = "BXTPurchaseOrderLine";
            tableNameBC = "BXTBCPurchLine";
            procedure = "BCPurchLineClone";
            DocumentType = "Order";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");

            var requestGet = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}?$filter = DocumentNo eq '{tbcurDoc}'";
            //get user table data
            //HttpResponseMessage response1 = await client.GetAsync(baseURL + "/v2.0/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-Test/ODataV4/Company('My Company')/BXTBCWHSUser?$filter = Username eq '" + LoginUser.UserName + "'");
            //HttpResponseMessage reponseClone = await client.PostAsync(requestCopy, null);

            HttpResponseMessage response = await client.GetAsync(requestGet);

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
                        gvPOList.DataSource = item;
                        gvPOList.DataBind();
                    }
                }

            }
        }      

    }


    protected void btnBackURL_Click(object sender, EventArgs e)
    {
        Response.Redirect("POReceiving.aspx");
    }

    protected void gvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPOList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void GridViewHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvPOList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void Button_search_Click(object sender, EventArgs e)
    {
        BCDataCollection();
    }
    protected void lb_firstpage_Click(object sender, EventArgs e)
    {
        this.gvPOList.PageIndex = 0;
        BCDataCollection();
    }
    protected void lb_previouspage_Click(object sender, EventArgs e)
    {
        if (this.gvPOList.PageIndex > 0)
        {
            this.gvPOList.PageIndex--;
            BCDataCollection();
        }
    }
    protected void lb_nextpage_Click(object sender, EventArgs e)
    {
        if (this.gvPOList.PageIndex < this.gvPOList.PageCount)
        {
            this.gvPOList.PageIndex++;
            BCDataCollection();
        }
    }
    protected void lb_lastpage_Click(object sender, EventArgs e)
    {
        this.gvPOList.PageIndex = this.gvPOList.PageCount;
        BCDataCollection();
    } 

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string itemId = "";
        int qtyToReceive = 0;
        bool isOK = true;
        string lineNumList = "";  //in a format 3,5,7...
        foreach (GridViewRow row in gvPOList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow) //avoid header/footer rows.
            {
                var myCheckBox = (CheckBox)row.FindControl("chkSelect");
                //myCheckBox.Checked tells you if it's checked or not, yay!
                if (myCheckBox.Checked)
                {
                    //int curLine = int.Parse(gvPOList.Rows[row.RowIndex].Cells[0].Text);
                    var curItemId = (string)gvPOList.Rows[row.RowIndex].Cells[2].Text;
                    ViewState["currentRow"] = row.RowIndex;
                    //-----------------------------
                    //Session["currentItemChecked"] = row.RowIndex;
                    isOK = true;
                    //-----------------------------
                    if (itemId == "")
                    {
                        itemId = curItemId;
                        //qtyToReceive = int.Parse(gvPOList.Rows[row.RowIndex].Cells[4].Text);
                        //lineNumList = (string)gvItemList.Rows[row.RowIndex].Cells[1].Text + ",";
                        //lineNumList = (string)gvPOList.Rows[row.RowIndex].Cells[1].Text;
                    }
                    else if (itemId != curItemId) //select two lines with different itemId
                    {
                        lblMessage.Text = "You cannot select lines with different item.";
                        isOK = false;
                        break;
                    }
                    else
                    {
                        //qtyToReceive += int.Parse(gvPOList.Rows[row.RowIndex].Cells[4].Text);
                        //lineNumList += (string)gvItemList.Rows[row.RowIndex].Cells[1].Text + ",";
                        //lineNumList += "," + (string)gvPOList.Rows[row.RowIndex].Cells[1].Text;

                    }

                }
            }
        }

        if (isOK)
        {
           
            GridViewRow row = this.gvPOList.Rows[(int)ViewState["currentRow"]];

            //WebClient myWebClient = new WebClient();
            //NameValueCollection VarPost = new NameValueCollection();
            //VarPost.Add("LineNo", row.Cells[1].Text.Trim());
            //VarPost.Add("Type", row.Cells[2].Text.Trim());
            //VarPost.Add("DocumentNo", row.Cells[3].Text.Trim());
            //VarPost.Add("ItemNO", row.Cells[4].Text.Trim());
            //VarPost.Add("Description", row.Cells[5].Text.Trim());
            //VarPost.Add("BuyFromVendorNo", row.Cells[6].Text.Trim());
            //VarPost.Add("QtytoReceive", row.Cells[7].Text.Trim());
            //VarPost.Add("QtytoReceived", row.Cells[8].Text.Trim());
            //VarPost.Add("Quantity", row.Cells[9].Text.Trim());
            //VarPost.Add("Location", row.Cells[10].Text.Trim());
            //myWebClient.BaseAddress = @"http://localhost:52671/";
            //string url = "http://localhost:52671/ReceivePurchOrder.aspx";
            //string toPage = "ReceivePurchOrder.aspx";
            //byte[] byRemoteInfo = myWebClient.UploadValues(toPage, "POST", VarPost);

            //HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            //Encoding myEncoding = Encoding.UTF8;
            //request.Method = "POST";
            //request.KeepAlive = false;
            //request.AllowAutoRedirect = true;
            //request.ContentType = "application/x-www-form-urlencoded";
            //System.IO.Stream outputStream = request.GetRequestStream();
            //outputStream.Write(VarPost, 0, VarPost.Count);
            //outputStream.Close();

            //Console.WriteLine("\nResponse received was :\n{0}", Encoding.ASCII.GetString(byRemoteInfo));
            //string sRemoteInfo = System.Text.Encoding.UTF8.GetString(byRemoteInfo);­

            //string result = "?LineNo=" + row.Cells[1].Text
            //                                                + "&Type=" + row.Cells[2].Text
            //                                                + "&DocumentNo=" + row.Cells[3].Text
            //                                                + "&ItemNO=" + row.Cells[4].Text
            //                                                + "&BuyFromVendorNo=" + row.Cells[6].Text
            //                                                + "&QtytoReceive=" + row.Cells[7].Text
            //                                                + "&QtytoReceived=" + row.Cells[8].Text
            //                                                + "&Quantity=" + row.Cells[9].Text
            //                                                + "&Location=" + row.Cells[10].Text
            //                                                + "&Description=" + row.Cells[5].Text
            //                                                 ;
            string Des = System.Web.HttpUtility.UrlEncode(row.Cells[3].Text);
            Response.Redirect("~/POReceiving.aspx" + "?LineNo=" + row.Cells[1].Text
                                                            //+ "&Type=" + row.Cells[2].Text
                                                            + "&ItemNO=" + row.Cells[2].Text
                                                            + "&DocumentNo=" + curDocumentNo.Text
                                                            + "&BuyFromVendorNo=" + row.Cells[5].Text
                                                            + "&QtytoReceive=" + row.Cells[4].Text
                                                            + "&Location=" + row.Cells[6].Text
                                                            + "&Description=" + Des
                                                             );
            

        }
    }
}