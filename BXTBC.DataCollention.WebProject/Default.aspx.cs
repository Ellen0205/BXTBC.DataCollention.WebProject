using System;
using System.ComponentModel.DataAnnotations;
using System.Web.DynamicData;

public partial class Default : System.Web.UI.Page
{
    string token, user;
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["Token"] == null)
        {
            Response.Redirect("~/Logon.aspx?Redirect=Default.aspx");
        }
        Response.Write(Session["Token"].ToString());
        token = (string)Session["Token"];
        user = token.Split(':')[0];
        if (!this.IsPostBack)
        {

        }
    }


}
