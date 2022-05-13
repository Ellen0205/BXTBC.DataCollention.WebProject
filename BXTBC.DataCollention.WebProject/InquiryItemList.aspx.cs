using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class InquiryItemList : System.Web.UI.Page
{
    string token;
    static string curUsername, curPassword;
    static string BCBaseURL = "https://api.businesscentral.dynamics.com";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";
    static string tableName = "BXTBCWHSUser";
    static string webservice = "ODataV4";
    static string tableItemJournal = "BXTPhysicalInventoryJournals";
    string requestValid = null;
    bool isSuccess = false;
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
        }
    }
    private async void BCDataCollection()
    {
        Session["CurrentWarehouse"] = "location";
        Session["CurrentCompany"] = "FLH-Test";
        //var response = client.PostAsync(baseURL,new FormUrlEncodedContent(new List<KeyValuePair<String, String>>)){
        // client.BaseAddress = new Uri(baseURL);

        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            string userAndPasswordToken =
            Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" +
            key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Basic {userAndPasswordToken}");

            var requestGetInvenInformation = $"{BCBaseURL}/v2.0/{tenantID}/{environment}/{webservice}/Company('{company}')/{tableItemJournal}";
            HttpResponseMessage response = await client.GetAsync(requestGetInvenInformation);

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

                        gvItemJourList.DataSource = item;

                        gvItemJourList.DataBind();
                    }
                }

            }
        }

    }

    private void btnxQuChong_Click(JArray array)
    {

    }

    protected void btnBackURL_Click(object sender, EventArgs e)
    {
        Response.Redirect("POReceiving.aspx");
    }

    protected void gvPOList_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemJourList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void GridViewHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gvItemJourList.PageIndex = e.NewPageIndex;
        BCDataCollection();
    }
    protected void Button_search_Click(object sender, EventArgs e)
    {
        BCDataCollection();
    }
    protected void lb_firstpage_Click(object sender, EventArgs e)
    {
        this.gvItemJourList.PageIndex = 0;
        BCDataCollection();
    }
    protected void lb_previouspage_Click(object sender, EventArgs e)
    {
        if (this.gvItemJourList.PageIndex > 0)
        {
            this.gvItemJourList.PageIndex--;
            BCDataCollection();
        }
    }
    protected void lb_nextpage_Click(object sender, EventArgs e)
    {
        if (this.gvItemJourList.PageIndex < this.gvItemJourList.PageCount)
        {
            this.gvItemJourList.PageIndex++;
            BCDataCollection();
        }
    }
    protected void lb_lastpage_Click(object sender, EventArgs e)
    {
        this.gvItemJourList.PageIndex = this.gvItemJourList.PageCount;
        BCDataCollection();
    }

    protected void btnSelect_Click(object sender, EventArgs e)
    {
        string itemId = "";
        bool isOK = true;
        string lineNumList = "";  //in a format 3,5,7...
        foreach (GridViewRow row in gvItemJourList.Rows)
        {
            if (row.RowType == DataControlRowType.DataRow) //avoid header/footer rows.
            {
                var myCheckBox = (CheckBox)row.FindControl("chkSelect");
                //myCheckBox.Checked tells you if it's checked or not, yay!
                if (myCheckBox.Checked)
                {
                    var curNo = (string)gvItemJourList.Rows[row.RowIndex].Cells[1].Text;
                    ViewState["currentRow"] = row.RowIndex;
                    //-----------------------------
                    //Session["currentItemChecked"] = row.RowIndex;
                    isOK = true;
                    //-----------------------------

                    if (itemId != curNo) //select two lines with different itemId
                    {
                        lblMessage.Text = "You cannot select lines with different item.";
                        isOK = false;
                        break;
                    }
                    else
                    {
                        
                        //qtyToReceive += int.Parse(gvPOList.Rows[row.RowIndex].Cells[7].Text);
                        //lineNumList += (string)gvItemList.Rows[row.RowIndex].Cells[1].Text + ",";
                        //lineNumList += "," + (string)gvPOList.Rows[row.RowIndex].Cells[1].Text;

                    }

                }
            }
        }

        if (isOK)
        {

            GridViewRow row = this.gvItemJourList.Rows[(int)ViewState["currentRow"]];
            Response.Redirect("~/CycleCount.aspx" + "?ItemNo=" + row.Cells[1].Text
                                                             + "&QtyCalculated=" + row.Cells[2].Text
                                                             + "&Quantity=" + row.Cells[3].Text
                                                             //+ "&Location=" + row.Cells[4].Text
                                                             );


        }
    }
    protected void gvItemList_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            int index = Convert.ToInt32(e.CommandArgument);

            // Retrieve the row that contains the button clicked 
            // by the user from the Rows collection.
            GridViewRow row = this.gvItemJourList.Rows[index];
            Response.Redirect("~/CycleCount.aspx" + "?ItemNo=" + row.Cells[1].Text
                                                             + "&QtyCalculated=" + row.Cells[2].Text
                                                             + "&Quantity=" + row.Cells[3].Text
                                                             //+ "&Location=" + row.Cells[4].Text
                                                             );

        }
    }
}