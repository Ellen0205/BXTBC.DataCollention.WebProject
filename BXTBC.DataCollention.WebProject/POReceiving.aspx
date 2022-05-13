<%@ Page Async="true" Title="Purchase receipts" Language="C#" AutoEventWireup="true" MasterPageFile="~/Mobile.master" CodeFile="POReceiving.aspx.cs" Inherits="POReceiving" %>

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
        <asp:panel runat="server" id="pnlConfirm" visible="false" borderwidth="3" bordercolor="#990000" width="200">
            <asp:Label runat="server" ID="lblConfirm" Text="Are you sure you want to short receive?" Font-Italic="true" ForeColor="Red" />
            <br />
            <asp:UpdateProgress runat="server" ID="updateProgress2" DynamicLayout="false">
                <ProgressTemplate>
                    <img src="../Images/throbber.gif" alt="" />
                </ProgressTemplate>
            </asp:UpdateProgress>
            <asp:Button runat="server" ID="btnOK" TabIndex="-1" Text="Yes" OnClientClick="this.disabled=true;" OnClick="btnOK_Click" UseSubmitBehavior="false" CausesValidation="false" />
            &nbsp;&nbsp;<asp:Button runat="server" ID="btnCancel" TabIndex="-1" Text="No" OnClick="btnCancel_Click" UseSubmitBehavior="false" CausesValidation="false" />
        </asp:panel>
        <asp:updateprogress runat="server" id="updateProgress1" dynamiclayout="false">
            <ProgressTemplate>
                <img src="../Images/throbber.gif" alt="" />
            </ProgressTemplate>
        </asp:updateprogress>
        <asp:UpdatePanel ID="UpdatePanelPO" UpdateMode="Conditional" runat="server">
            <ContentTemplate>
                <fieldset>
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
                </fieldset>
            </ContentTemplate>
            
        </asp:UpdatePanel>

        <asp:updatepanel id="UpdatePanel1"
            updatemode="Conditional"
            runat="server">
            <ContentTemplate>
                <fieldset>
                    <table id="table_item">
                          <tr>
                            <td>ItemNo :</td>
                            <td>
                                <asp:textbox id="tbItemNo" runat="server" autopostback="True" OnTextChanged ="Item_Changed"></asp:textbox>

                                <%--<asp:Button ID="getItem" runat="server" onclick="getItem_Click" 
                                    Text="Look up" Width="60px" Font-Size="8pt"  /> --%>
                            </td>

                            <td>       
                                <asp:Button runat="server" onclick="btnItemLookup_Click" ID="btnItemLookup"
                                    Text="Look up" Width="75px" Font-Size="8pt"  /> 
                            </td>
                        </tr>
                        <%--<tr>
                            <td>Vendor No:</td>
                            <td>
                                <asp:Label ID="lblVendor" runat="server" /></td>
                        </tr>--%>
                        <tr> 
                            <td>PO LineNum:</td>
                            <td>
                                <asp:Label ID="lblPOLine" runat="server" /></td>

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
                    </table>
                </fieldset>
            </ContentTemplate>
        </asp:updatepanel>

        <table>
            <tr>
                <td>QtyToReceive :</td>
                <td>
                    <asp:textbox id="tbQtyToReceive" runat="server" ontextchanged="tbQtyToReceive_TextChanged" TabIndex="2"></asp:textbox>

                </td>
            </tr>
            <tr>
                <td></td>
                <td
                </td>
            </tr>

            <tr>
                <td class="style1" colspan="3">
                    <asp:label id="lblMessage" runat="server" text=""></asp:label>
                </td>
            </tr>
        </table>
    </div>
    <table>
        <tr>
            <td>
                <asp:button id="btnReceiveURL" runat="server" text="Receive"
                    width="65px" onclick="btnReceiveURL_Click" font-size="8pt" onclientclick="this.disabled=true;" usesubmitbehavior="false" />
            </td>
            <td>
                <asp:button id="btnHomeURL" runat="server" text="Home"
                    width="50px" onclick="btnHomeURL_Click" font-size="8pt" />
            </td>
            <td>
                <asp:button id="btnPostURL" runat="server" text="Post"
                    width="50px" onclick="btnPostURL_Click" font-size="8pt" style="height: 26px" />
            </td>
    </table>
</asp:Content>
