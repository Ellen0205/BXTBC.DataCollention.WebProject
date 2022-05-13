using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Site : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (HeadLoginView.FindControl("HeadLoginName") != null)
        {
            string token = (string)Session["Token"];
            if (token != null)
            {
                string name = token.Split(':')[0];

                (HeadLoginView.FindControl("HeadLoginName") as System.Web.UI.WebControls.LoginName).FormatString = name;
            }

        }
    }
    protected void HeadLoginStatus_LoggingOut(object sender, EventArgs e)
    {
        //clear off session
        Session.Clear();

        Response.Redirect("~/Logon.aspx");
       // FormsAuthentication.SignOut();
    }
}
