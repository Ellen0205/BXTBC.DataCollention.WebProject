<%@ Page Async="true" Title="Cycle Counting" Language="C#" MasterPageFile="~/Mobile.master" AutoEventWireup="true" CodeFile="CycleCount.aspx.cs" Inherits="CycleCount" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">
        function checkKey(controlID) {
            var theKey = window.event.keyCode;
            var newControl = "";
            if (theKey == 13) {
                event.returnValue = false;
                event.cancelBubble = true;
                switch (controlID) {
                    case "TbItem":
                        newControl = "TbQuantity";
                        break;
                    case "TbQuantity":
                        newControl = "TbLocation";
                        break
                    case "TbLocation":
                        newControl = "Button1";
                        break;
                    default:
                        newControl = "Button1";
                }
                document.getElementById(newControl).focus();
            }

        }
        <%--function fn_SetFocus() {
            var txt = document.getElementById('<%= tbItem.ClientID %>');
       document.getElementById('<%= tbItem.ClientID %>').focus();
        }--%>

        document.onkeyup = displayunicode;
        function displayunicode(e) {
            var theKey = window.event.keyCode;
            if (theKey == 27)//// Escape
                window.location.href("default.aspx");
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div class="container">
        <asp:UpdatePanel runat="server" ID="pnlMain">
            <ContentTemplate>

                <asp:Panel runat="server" ID="pnlMaster" DefaultButton="btnCycleSubmit">

                    <asp:UpdateProgress ID="updateProgress1" runat="server" DynamicLayout="false">
                        <ProgressTemplate>
                            <img src="../../Images/throbber.gif" alt="" />
                        </ProgressTemplate>
                    </asp:UpdateProgress>
                    <%-- <table>--%>
                    <div class="page-header">
                        <%--<caption>
                            <h1>Panels</h1>
                        </caption>--%>
                    </div>
                    <div class="row">
                        <div class="col-sm-4">
                            <div class="panel panel-default">
                                <div class="panel-heading">
                                    <caption>
                                        <h3 class="panel-title">Panel title</h3>
                                    </caption>
                                </div>
                                <div class="panel-body">
                                    <table>
                                        <tr>
                                            <td>Item: </td>
                                            <td>

                                                <asp:TextBox ID="tbItemNo" runat="server" AutoPostBack="true" Enabled="False"></asp:TextBox>

                                            </td>
                                            <td>
                                                <asp:Button ID="btnItemLookup" runat="server" Font-Size="8pt" Text="Look up" Width="60px" OnClick="btnItemLookup_Click" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Quantity: </td>
                                            <td>
                                                <asp:Label ID="lblQuantity" runat="server" align="right" AutoPostBack="True" OnTextChanged="tbLocation_TextChanged"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Qty Calulated: </td>
                                            <td>
                                                <asp:Label ID="lblQtyCalulated" runat="server" align="right" BorderStyle="NotSet" OnTextChanged="TbQuantity_TextChanged"></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td>Quantity Inventory: </td>
                                            <td>
                                                <asp:TextBox ID="tbQtyInventory" runat="server"></asp:TextBox>

                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>

                                        <tr>
                                            <td colspan="1"></td>
                                            <td colspan="1">
                                                <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                                            </td>
                                        </tr>
                                        <tr>
                                            <td colspan="3">
                                                <asp:Button ID="btnCycleSubmit" runat="server" CssClass="btn btn-sm btn-primary"  Text="Submit" UseSubmitBehavior="false"  OnClick="btnCycleSubmit_Click" />
                                                <asp:Button ID="btnNext" runat="server"  Text="Next" CssClass="btn btn-sm btn-primary" />
                                            </td>
                                        </tr>
                                        <tr>
                                            <td></td>
                                        </tr>
                                    </table>

                                </div>
                            </div>

                        </div>
                        <!-- /.col-sm-4 -->
                    </div>
                </asp:Panel>
            </ContentTemplate>
        </asp:UpdatePanel>
    </div>

</asp:Content>
