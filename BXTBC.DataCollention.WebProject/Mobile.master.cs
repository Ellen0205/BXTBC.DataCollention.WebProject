using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Mobile : System.Web.UI.MasterPage
{
    string name;
    public bool Test { get; set; }
    protected override void OnPreRender(EventArgs e)
    {
        base.OnPreRender(e);

        if (!vldCust.IsValid)
        {
            if (vldCust.ForeColor == Color.Red)
                lblMessage.Text = "☼ An error occurred. See error below for details.";
            else if (vldCust.ForeColor == Color.Green)
                lblMessage.Text = "☼ Success!";
            else if (vldCust.ForeColor == Color.Blue)
                lblMessage.Text = "☼ Warning! See message below for details.";

            lblMessage.ForeColor = vldCust.ForeColor;
            lblMessage.Visible = true;
        }
        else
        {
            lblMessage.Text = "";
            lblMessage.Visible = false;
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HeadLoginView.FindControl("HeadLoginName") != null)
        {
            string token = (string)Session["Token"];
            name = token.Split(':')[0];

            (HeadLoginView.FindControl("HeadLoginName") as System.Web.UI.WebControls.LoginName).FormatString = name;
        }

        if (Session["Token"] != null)
        {
            Label lblComp = HeadLoginView.FindControl("lblCurrentComp") as Label;
            //lblComp.Text = (string)Session["CurrentCompany"];

            //Label lblWhs = HeadLoginView.FindControl("lblCurrentWhse") as Label;
            //lblWhs.Text = (string)Session["CurrentWarehouse"];


        }

        string url = Request.Url.LocalPath;

    }
    protected void HeadLoginStatus_LoggingOut(object sender, EventArgs e)
    {
        Session.Clear();

        Response.Redirect("~/Logon.aspx");
        FormsAuthentication.SignOut();
    }



    protected void lbHome_Click(object sender, EventArgs e)
    {
        Response.Redirect("~/Default.aspx");
    }

    protected void lbCurrentComp_Click(object sender, EventArgs e)
    {
        //string currentCompany = (string)Session["CurrentCompany"];
        //string currentWarehouse = (string)Session["CurrentWarehouse"];

        //d365 = AuthenticationUtility.ODataHelper.SingleInstance;
        //var companyList = d365.LegalEntities.ToList();
        //ddlCurrentComp.DataSource = (from r in companyList orderby r.LegalEntityId select r.LegalEntityId);
        //ddlCurrentComp.DataBind();
        //ddlCurrentComp.SelectedIndex = ddlCurrentComp.Items.IndexOf(ddlCurrentComp.Items.FindByText(currentCompany.ToUpper()));

        //var warehouseList = d365.KWSInventLocations.Where(t => t.DataAreaId == ddlCurrentComp.Text).ToList();
        //ddlCurrentWhse.DataSource = (from r in warehouseList orderby r.InventLocationId select r.InventLocationId);
        //ddlCurrentWhse.DataBind();
        //ddlCurrentWhse.SelectedIndex = ddlCurrentWhse.Items.IndexOf(ddlCurrentWhse.Items.FindByText(currentWarehouse));

        //this.pnlChangeWhse.Visible = true;

    }

    protected void lbCurrentWhse_Click(object sender, EventArgs e)
    {
        //string currentCompany = (string)Session["CurrentCompany"];
        //string currentWarehouse = (string)Session["CurrentWarehouse"];
        //d365 = AuthenticationUtility.ODataHelper.SingleInstance;
        //var companyList = d365.LegalEntities.ToList();

        //ddlCurrentComp.DataSource = (from r in companyList orderby r.LegalEntityId select r.LegalEntityId);
        //ddlCurrentComp.DataBind();
        //ddlCurrentComp.SelectedIndex = ddlCurrentComp.Items.IndexOf(ddlCurrentComp.Items.FindByText(currentCompany.ToUpper()));

        //var warehouseList = d365.KWSInventLocations.Where(t => t.DataAreaId == ddlCurrentComp.Text).ToList();
        //ddlCurrentWhse.DataSource = (from r in warehouseList orderby r.InventLocationId select r.InventLocationId);
        //ddlCurrentWhse.DataBind();
        //ddlCurrentWhse.SelectedIndex = ddlCurrentWhse.Items.IndexOf(ddlCurrentWhse.Items.FindByText(currentWarehouse));

        //this.pnlChangeWhse.Visible = true;
    }

    protected void btnSaveWhse_Click(object sender, EventArgs e)
    {
        //try
        //{
            //d365 = AuthenticationUtility.ODataHelper.SingleInstance;
            //var inventLocation = d365.KWSInventLocations.Where(t => t.InventLocationId == ddlCurrentWhse.Text
            //     && t.DataAreaId == ddlCurrentComp.Text.ToLower()).FirstOrDefault();
            //if (inventLocation != null && inventLocation.InventLocationId != string.Empty)
            //{
            //    this.lblChangeWhseError.Text = d365.KWSWorkUsers.Where(x => x.DataAreaId == ((string)Session["CurrentCompany"])).First().KWSCreateOrUpdateNewUser(name, inventLocation.DataAreaId, inventLocation.InventSiteId, inventLocation.InventLocationId).GetValue();
                //if (this.lblChangeWhseError.Text == "")
                //{
                //    Session["CurrentWarehouse"] = inventLocation.InventLocationId;
                //    Session["CurrentSite"] = inventLocation.InventSiteId;
                //    Session["CurrentCompany"] = inventLocation.DataAreaId;


                    //var whsWorkerUser = d365.KWSWorkUsers.Where(x => x.UserId == name && x.DataAreaId == (string)Session["CurrentCompany"]);
                    //if (whsWorkerUser != null && whsWorkerUser.Count() > 0)
                    //{
                    //    string companyId = (string)Session["CurrentCompany"];
                    //    string upperCaseCompanyId = companyId.ToUpper();
                    //    var hcmWorker = d365.KWSHcmEmployments.Where(x => x.CompanyInfo_FK_DataArea == upperCaseCompanyId
                    //    && x.WorkerName == whsWorkerUser.First().UserName);
                    //    if (hcmWorker != null && hcmWorker.Count() > 0)
                    //    {
                    //        Session["HcmWorker"] = hcmWorker.FirstOrDefault().Worker;
                    //    }
                    //    else
                    //    {
                    //        this.lblComp.Text = "Change compamy/warehouse is successful. But worker record was not found for this inventory worker in the new company.";
                    //    }
                    //}


                    //Label lblWhse = HeadLoginView.FindControl("lblCurrentWhse") as Label;
                    //lblWhse.Text = (string)Session["CurrentWarehouse"];
                   // Label lblComp = HeadLoginView.FindControl("lblCurrentComp") as Label;
                    //lblComp.Text = (string)Session["CurrentCompany"];
                    this.pnlChangeWhse.Visible = false;

                //    var tmp = d365.KWSMobileLogs.First()
                //.CreateActionLog((string)Session["UserId"], (string)Session["CurrentCompany"], KWSMobileTransType.UserSetting, (string)Session["CurrentWarehouse"], "", 0, "User setting was updated successfully.").GetValue();
        //        }
        //    }
        //}
        //catch (Exception _e)
        //{
        //    lblComp.Text = _e.Message;
        //}



    }

    protected void btnCancelWhse_Click(object sender, EventArgs e)
    {
        this.pnlChangeWhse.Visible = false;

    }
    protected void ddlCurrentComp_TextChanged(object sender, EventArgs e)
    {
        //d365 = AuthenticationUtility.ODataHelper.SingleInstance;

        //var warehouseList = d365.KWSInventLocations.Where(t => t.DataAreaId == ddlCurrentComp.Text.ToLower()).ToList();
        //ddlCurrentWhse.DataSource = (from r in warehouseList orderby r.InventLocationId select r.InventLocationId);
        //ddlCurrentWhse.DataBind();

    }
    protected void HeadLoginView_ViewChanged(object sender,EventArgs e)
    {

    }

}
