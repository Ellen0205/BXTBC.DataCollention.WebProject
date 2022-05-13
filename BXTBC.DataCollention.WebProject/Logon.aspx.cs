using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json.Linq;
using System.Web.Security;
using System.Web.SessionState;

public enum UserValidationEnum : int
{
    Success = 0,
    LoginFailed = 1,
    NotValidBCUser = 2,
    UserNotExist = 3,
    ConnectFailed = 4
}
public partial class Logon : System.Web.UI.Page
{

    //static string baseURL, user, key,tenantID, companyName, tableName, environment, company,message;
    string requestValid = null;
    static string curUsername, curPassword;
    static string baseURL = "https://api.businesscentral.dynamics.com";
    static string user = "BCADMIN";
    static string key = "8x3QwsmnEzRk9jbGCxPV/IlNZzrXNoXL/fPOpFTbgIU=";
    static string tenantID = "c3a6b225-defc-4b2c-af26-66d0672920a7";
    static string environment = "FLH-TestProdData";
    static string company = "FLH-Prod";
    static string tableName = "BXTBCWHSUser";



    protected void Page_Load(object sender, EventArgs e)
    {

        LoginUser.Focus();
        LoginUser.Attributes.Add("onkeydown", "checkKey(this.id)");
        Session.Clear();
        Session["Token"] = LoginUser.UserName + ":" + LoginUser.Password;
        Session["UserId"] = LoginUser.UserName;
        Session["CurrentCompany"] = "FLH-Prod";

    }
    protected async void LoginUser_Authenticate(object sender, AuthenticateEventArgs e)
    {
        //int a = 1;
        //UserValidationEnum userValidationResult = (UserValidationEnum)a;
        LoginUser.FailureText = "";
        UserValidationEnum userValidationResult = await IsValidCredentials(LoginUser.UserName, LoginUser.Password);
        lblMessage.ForeColor = System.Drawing.Color.Red;
        switch (userValidationResult)
        {
            case UserValidationEnum.Success:

                e.Authenticated = true;
                break;
            case UserValidationEnum.LoginFailed:
                lblMessage.Text = LoginUser.FailureText = "Service is not available or user/password is not correct";
                e.Authenticated = false;
                break;
            case UserValidationEnum.NotValidBCUser:
                lblMessage.Text = LoginUser.FailureText = "You are not added as an active user yet.";
                e.Authenticated = false;
                break;
            case UserValidationEnum.UserNotExist:
                lblMessage.Text = LoginUser.FailureText = "User isn't exist in the System";
                e.Authenticated = false;
                break;
            case UserValidationEnum.ConnectFailed:
                lblMessage.Text = LoginUser.FailureText = "logon failed ,Please check the internet connection";
                e.Authenticated = false;
                break;
            default:
                e.Authenticated = false;
                break;
        }

        if (e.Authenticated)
        {
            Session["Token"] = LoginUser.UserName + ":" + LoginUser.Password;
            Session["UserId"] = LoginUser.UserName;
            ViewState["AuthenticationValid"] = true;
            Session["CurrentCompany"] = "FLH-Prod";
            Response.Redirect("Default.aspx");
            //Response.Redirect("Default.aspx" + "?Token=" + token
            //                                 + "&UserID=" + LoginUser.UserName);
        }
    }


    public async Task<UserValidationEnum> IsValidCredentials(string userName, string passWord)
    {
        // Call web service to submit the document
        int site = 0;
        UserValidationEnum userValidationResult = (UserValidationEnum)site;
        //connect BC
        LoginUser.FailureText = "";
        BXTHttpClientConnection conn = new BXTHttpClientConnection();
        string userAndPasswordToken = await conn.GetToken();
        using (HttpClient client = new HttpClient())
        {
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //string userAndPasswordToken =
            //Convert.ToBase64String(Encoding.UTF8.GetBytes(user + ":" + key));
            client.DefaultRequestHeaders.TryAddWithoutValidation("Authorization", $"Bearer {userAndPasswordToken}");
            //get user table data
            //HttpResponseMessage response1 = await client.GetAsync(baseURL + "/v2.0/c3a6b225-defc-4b2c-af26-66d0672920a7/FLH-Test/ODataV4/Company('My Company')/BXTBCWHSUser?$filter = Username eq '" + LoginUser.UserName + "'");

            HttpResponseMessage response = await client.GetAsync(baseURL + "/v2.0/" + tenantID + "/" + environment + "/ODataV4/Company('" + company + "')/" + tableName + "?$filter = Username eq '" + LoginUser.UserName + "'");
            JObject BCWHSUsers = JsonConvert.DeserializeObject<JObject>(response.Content.ReadAsStringAsync().Result);
            JObject users = JObject.Parse(BCWHSUsers.ToString());

            foreach (JToken jt in users.Children())
            {
                JProperty jProperty = jt.ToObject<JProperty>();
                string propertyName = jProperty.Name;
                if (propertyName == "value")
                {
                    foreach (JToken jt2 in jProperty.Children())
                    {
                        JArray array = new JArray(jt2.Children());

                        if (!array.HasValues
                            //jt2[0].Value<string>("Username") == null || jt2[0].Value<string>("Username") != LoginUser.UserName
                            )
                        {
                            ViewState["AuthenticationValid"] = false;
                            userValidationResult = UserValidationEnum.UserNotExist;

                        }
                        else
                        {
                            string Username = jt2[0].Value<string>("Username");
                            string Password = jt2[0].Value<string>("Password");
                            string Status = jt2[0].Value<string>("inactive");
                            if (Password == LoginUser.Password && Status == "active")
                            {
                                Session.Clear();
                                Session["Token"] = userName + ":" + passWord;
                                Session["UserId"] = userName;
                                ViewState["AuthenticationValid"] = true;
                                Session["CurrentCompany"] = "FLH-Prod";
                                userValidationResult = UserValidationEnum.Success;
                            }
                            if (Password == LoginUser.Password && Status == "inactive")
                            {
                                ViewState["AuthenticationValid"] = false;
                                userValidationResult = UserValidationEnum.NotValidBCUser;
                            }
                            if (Password != LoginUser.Password)
                            {
                                ViewState["AuthenticationValid"] = false;
                                userValidationResult = UserValidationEnum.LoginFailed;
                                //message = "The user name or password is incorrect";
                            }

                        }
                    }
                }
                if (propertyName == "error")
                {
                    ViewState["AuthenticationValid"] = false;
                    userValidationResult = UserValidationEnum.ConnectFailed;
                    //message = "logon failed ,Please check the internet connection";
                    break;
                }
            }
            return userValidationResult;
        }

        return userValidationResult;
    }

}
