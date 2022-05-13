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

public partial class ReceivePurchOrderItemList : System.Web.UI.Page
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
        //if (Session["Token"] == null)
        //{
        //    Response.Redirect("~/Logon.aspx");
        //}
        //token = Session["Token"].ToString();
        if (!IsPostBack)
        {
            BCDataCollection();
            this.lblMessage.ForeColor = System.Drawing.Color.Red;
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

        using (HttpClient client = new HttpClient())
        {
            curDoc = "PO000000001";
            tableName = "BXTPurchaseOrderLine";
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userAndPasswordToken =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" +
            key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}?$filter = Status eq 'modified'";
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
                        if (array.HasValues)
                        {
                            gvReceivedList.DataSource = item;
                            gvReceivedList.DataBind();
                        }
                        else
                        {
                            this.lblMessage.Text = "You need receive item before posting";
                        }
                        
                    }
                }

            }
        }

    }


    protected void btnBackURL_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/ReceivePurchOrder.aspx");
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
        string itemId = "";
        int qtyToReceive = 0;
        bool isOK = true;
        string lineNumList = "";  //in a format 3,5,7...

        foreach (GridViewRow row in gvReceivedList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow) //avoid header/footer rows.
            {
                var myCheckBox = (CheckBox)row.FindControl("chkSelect");
                //myCheckBox.Checked tells you if it's checked or not, yay!
                if (myCheckBox.Checked)
                {
                    var curItemId = (string)gvReceivedList.Rows[row.RowIndex].Cells[4].Text;
                    ViewState["currentRow"] = row.RowIndex;

                    
                    //-----------------------------
                    //Session["currentItemChecked"] = row.RowIndex;
                    //isOK = true;
                    //-----------------------------
                    if (itemId == "")
                    {
                        itemId = curItemId;
                        qtyToReceive = int.Parse(gvReceivedList.Rows[row.RowIndex].Cells[7].Text);
                        //lineNumList = (string)gvItemList.Rows[row.RowIndex].Cells[1].Text + ",";
                        lineNumList = (string)gvReceivedList.Rows[row.RowIndex].Cells[1].Text;
                    }
                    else if (itemId != curItemId) //select two lines with different itemId
                    {
                        lblMessage.Text = "You cannot select lines with different item.";
                        isOK = false;
                        break;
                    }
                    else
                    {
                        qtyToReceive += int.Parse(gvReceivedList.Rows[row.RowIndex].Cells[7].Text);
                        //lineNumList += (string)gvItemList.Rows[row.RowIndex].Cells[1].Text + ",";
                        lineNumList += "," + (string)gvReceivedList.Rows[row.RowIndex].Cells[1].Text+",";

                    }

                }
            }
        }

        if (isOK)
        {

            GridViewRow row = this.gvReceivedList.Rows[(int)ViewState["currentRow"]];
            curDoc = row.Cells[3].Text;
            curLine = int.Parse(row.Cells[1].Text);
            if (await actionPurchPost())
            {
                //this.lblQtyToReceive.Text = Request.QueryString["qty"];
                //this.tbQtyToReceive.Text = "";
                int ed = int.Parse(row.Cells[8].Text);
                int to = int.Parse(row.Cells[7].Text);
                ed += to;
                row.Cells[8].Text = ed.ToString();
                row.Cells[7].Text = "0";
                this.lblMessage.Text = "Posting success";
                //Response.Write("<script type='text/javascript'>alert('Posting success');setTimeout(function(){location.href='../InquiryPOOpenList.aspx'},2000);</script>");
            }
            else
            {
                this.lblMessage.Text = message;
            }

            //Response.Redirect("~/ReceivePurchOrder.aspx" + "?LineNo=" + row.Cells[1].Text
            //                                                + "&Type=" + row.Cells[2].Text
            //                                                + "&DocumentNo=" + row.Cells[3].Text
            //                                                + "&ItemNO=" + row.Cells[4].Text
            //                                                + "&Description=" + row.Cells[5].Text
            //                                                + "&BuyFromVendorNo" + row.Cells[6].Text
            //                                                + "&QtytoReceive=" + row.Cells[7].Text
            //                                                + "&QtytoReceived=" + row.Cells[8].Text
            //                                                + "&Quantity=" + row.Cells[9].Text
            //                                                + "&Location=" + row.Cells[10].Text
            //                                                );
        }
    }

    protected async void ComfirmPosting_Click1(object sender, EventArgs e)
    {

        //if (await actionPurchPost(curDoc, curLine))
        //{
        //    //this.lblQtyToReceive.Text = Request.QueryString["qty"];
        //    //this.tbQtyToReceive.Text = "";
        //    this.lblMessage.Text = "Posting success";
        //}
        //else
        //{
        //    this.lblMessage.Text = message;
        //}
    }
    private static async Task<bool> actionPurchPost()
    {
        tableName = "BXTPurchaseOrderLine";
        procedure = "PurchPost";
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userAndPasswordToken = Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");
            var requstUri = $"{baseURL}/v2.0/{tenantID}/{environment}/ODataV4/Company('{company}')/{tableName}('{curDoc}',{curLine})/NAV.{procedure}";
            var body = "{\"documentno\":\"" + curDoc + "\"}";
            var contentPost = new StringContent(body, Encoding.UTF8, "application/json");
            HttpResponseMessage responsePost = await client.PostAsync(requstUri, contentPost).ConfigureAwait(false);
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
        Response.Redirect("~/ReceivePurchOrder.aspx");
    }

    protected void Continue_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/InquiryPOOpenList.aspx");
    }
}