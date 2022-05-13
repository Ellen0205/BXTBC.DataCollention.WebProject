<%@ Page Async="true"  Title="Purchase receipts" Language="C#" AutoEventWireup="true" MasterPageFile="~/Mobile.master" CodeFile="ReceivePurchOrder.aspx.cs" Inherits="ReceivePurchOrder" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">

    <script type="text/javascript">
        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }
        document.onkeypress = stopRKey;
        document.onkeyup = displayunicode;

        function displayunicode(e) {
            var theKey = window.event.keyCode;
            if (theKey == 27)//// Escape
                window.location.href("default.aspx");
        }

    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div>
        <asp:Panel runat="server" ID="pnlConfirm" Visible="false" BorderWidth="3" BorderColor="#990000" Width="200">
            <asp:Label runat="server" ID="lblConfirm" Text="Are you sure you want to short receive?" Font-Italic="true" ForeColor="Red" />
            <br />
            <asp:UpdateProgress runat="server" ID="updateProgress2" DynamicLayout="false">
                <ProgressTemplate>
                    <img src="../Images/throbber.gif" alt="" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button runat="server" ID="btnOK" TabIndex="-1" Text="Yes" OnClientClick="this.disabled=true;" OnClick="btnOK_Click" UseSubmitBehavior="false" CausesValidation="false" />
            &nbsp;&nbsp;<asp:Button runat="server" ID="btnCancel" TabIndex="-1" Text="No" OnClick="btnCancel_Click" UseSubmitBehavior="false" CausesValidation="false" />
        </asp:Panel>
        <asp:UpdateProgress runat="server" ID="updateProgress1" DynamicLayout="false">
            <ProgressTemplate>
                <img src="../Images/throbber.gif" alt="" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <table>
            <tr>
                <td>PO#:    </td>
                <td>
                    <asp:TextBox ID="tbPOId" runat="server" OnTextChanged="tbPOId_Changed"
                        AutoPostBack="True"></asp:TextBox>
                </td>
                <td>
                    <asp:Button ID="btnPoLookup" runat="server" OnClick="btnPoLookup_Click"
                        Text="Look up" Width="60px" Font-Size="8pt" />
                </td>
            </tr>
        </table>
        <asp:UpdatePanel ID="UpdatePanel1"
            UpdateMode="Conditional"
            runat="server">
            <ContentTemplate>
                <fieldset>
                    <table>
                        <tr>
                            <td>Type:</td>
                            <td>
                                <asp:Label ID="lblType" runat="server" /></td>
                        </tr>
                        <tr> 
                            <td>PO LineNum:</td>
                            <td>
                                <asp:Label ID="lblPOLine" runat="server" /></td>

                        </tr>
                        <tr>
                            <td>ItemNo:</td>
                            <td>
                                <asp:Label ID="lblItemNo" runat="server" OnTextChanged="Item_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td>Description:</td>
                            <td>
                                <asp:Label ID="lblDescription" runat="server" OnTextChanged="Item_Changed" />
                            </td>
                        </tr>
                        <tr>
                            <td>Location:   </td>
                            <td>
                                <asp:Label ID="lblLoc" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>QtyToReceive:   </td>
                            <td>
                                <asp:Label ID="lblQtyToReceive" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>QuantityReceived:   </td>
                            <td>
                                <asp:Label ID="lblQtyReceived" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Quantity:   </td>
                            <td>
                                <asp:Label ID="lblQty" runat="server" />
                            </td>
                        </tr>
                    </table>
                </fieldset>
            </ContentTemplate>
        </asp:UpdatePanel>

        <table>
            <tr>
                <td>QtyToReceive :</td>
                <td>
                    <asp:TextBox ID="tbQtyToReceive" runat="server" OnTextChanged="tbQtyToReceive_TextChanged"></asp:TextBox>
                                
                </td>
            </tr>
            <tr>
                <td>
                </td>
                <td>
                    Please enter the quantity to receive
                </td>
            </tr>

            <tr>
                <td class="style1" colspan="3">
                    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
                </td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td>
                <asp:Button ID="btnReceiveURL" runat="server" Text="Receive"
                    Width="65px" OnClick="btnReceiveURL_Click" Font-Size="8pt" OnClientClick="this.disabled=true;" UseSubmitBehavior="false" />
            </td>
            <td>
                <asp:Button ID="btnHomeURL" runat="server" Text="Home"
                    Width="50px" OnClick="btnHomeURL_Click" Font-Size="8pt" />
            </td>
            <td>
                <asp:Button ID="btnPostURL" runat="server" Text="Post"
                    Width="50px" OnClick="btnPostURL_Click" Font-Size="8pt" style="height: 26px" />
            </td>
    </table>
</asp:Content>
