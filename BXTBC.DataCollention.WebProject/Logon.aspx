<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.master" CodeFile="Logon.aspx.cs" Inherits="Logon" Async="true" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function checkKey(controlID) {
            var theKey = window.event.keyCode;
            var newControl = "";
            if (theKey == 13) {
                event.returnValue = false;
                event.cancelBubble = true;
                switch (controlID) {
                    case "LoginControl":
                        newControl = "TbQuantity";
                        break;
                    default:
                        newControl = "Button1";
                }
                document.getElementById(newControl).focus();
            }
            if (theKey == 27) {
                event.returnValue = false;
                event.cancelBubble = true;
                switch (controlID) {
                    case "LoginControl":
                        newControl = "Button2";
                        break;
                    case "TbQuantity":
                        newControl = "Button2";
                        break
                    case "TbLocation":
                        newControl = "Button2";
                        break;
                    default:
                        newControl = "Button2";
                }
                document.getElementById(newControl).focus();
            }
        }

    </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="mainContainer" class="container">
        <h2>Log In
        <asp:UpdateProgress ID="updateProgress1" runat="server" DynamicLayout="false">
            <ProgressTemplate>
                <img src="../../Images/throbber.gif" alt="" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        </h2>
        <p>
            Please enter your username and password.
        </p>
        <asp:Panel runat="server" ID="pnlMain">
            <div class="container">
                <div class="row">
                    <div class="col-lg-12 ">
                        <asp:Login ID="LoginUser" runat="server" EnableViewState="false"
                            RenderOuterTable="false" OnAuthenticate="LoginUser_Authenticate" >
                            <LayoutTemplate>
                                <span class="failureNotification">
                                    <asp:Literal ID="FailureText" runat="server"></asp:Literal>
                                </span>
                                <asp:ValidationSummary ID="LoginUserValidationSummary" runat="server" CssClass="failureNotification"
                                    ValidationGroup="LoginUserValidationGroup" />
                                <div class="accountInfo">
                                    <fieldset class="login">
                                        <legend>Login Information</legend>
                                        <p>
                                            <asp:Label ID="UserNameLabel" runat="server" AssociatedControlID="UserName">Username:</asp:Label>

                                            <asp:TextBox ID="UserName" runat="server" CssClass="textEntry"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="UserNameRequired" runat="server" ControlToValidate="UserName"
                                                CssClass="failureNotification" ErrorMessage="User Name is required." ToolTip="User Name is required."
                                                ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                        </p>
                                        <p>
                                            <asp:Label ID="PasswordLabel" runat="server" AssociatedControlID="Password">Password:</asp:Label>
                                            <asp:TextBox ID="Password" runat="server" CssClass="passwordEntry" TextMode="Password"></asp:TextBox>
                                            <asp:RequiredFieldValidator ID="PasswordRequired" runat="server" ControlToValidate="Password"
                                                CssClass="failureNotification" ErrorMessage="Password is required." ToolTip="Password is required."
                                                ValidationGroup="LoginUserValidationGroup">*</asp:RequiredFieldValidator>
                                        </p>
                                        <p>
                                            <asp:CheckBox ID="RememberMe" runat="server" />
                                            <div>
                                                <asp:Label ID="RememberMeLabel" runat="server" AssociatedControlID="RememberMe" CssClass="inline">Keep me logged in</asp:Label>
                                            </div>

                                        </p>
                                    </fieldset>
                                    <p class="submitButton">
                                        <asp:Button ID="LoginButton" runat="server" CommandName="Login" Text="Log In"
                                            ValidationGroup="LoginUserValidationGroup" OnClientClick="this.disable=true" UseSubmitBehavior="false" />
                                    </p>
                                </div>
                            </LayoutTemplate>
                        </asp:Login>
                        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>
