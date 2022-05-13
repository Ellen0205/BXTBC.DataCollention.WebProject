<%@ Page Async="true" Language="C#" MasterPageFile="~/Mobile.master" AutoEventWireup="true" CodeFile="InquiryPOOpenList.aspx.cs" Inherits="InquiryPOOpenList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="Server">
    <script type="text/javascript">

        function GetRowValue(val) {

            // hardcoded value used to minimize the code. 

            // ControlID can instead be passed as query string to the popup window

            window.opener.document.getElementById("lblItem").value = val;
            window.opener.document.getElementById("lblItem").focus();
            //__doPostBack('UpdatePanel1','');
            window.close();

        }
        function SelectAllCheckboxes(spanChk) {
            // Added as ASPX uses SPAN for checkbox
            var oItem = spanChk.children;
            var theBox = (spanChk.type == "checkbox") ?
                spanChk : spanChk.children.item[0];
            xState = theBox.checked;
            elm = theBox.form.elements;

            for (i = 0; i < elm.length; i++)
                if (elm[i].type == "checkbox" &&
                    elm[i].id != theBox.id) {
                    //elm[i].click();
                    if (elm[i].checked != xState)
                        elm[i].click();
                    //elm[i].checked=xState;
                }
        }

    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="mainContainer" class="container">
        <b>Inquiry PO Receiving order line items</b>&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
        PO:
        <asp:Label ID="curDocumentNo" runat="server" Text="Label"></asp:Label>

        <asp:UpdateProgress runat="server" ID="updateProgress2" DynamicLayout="false">
            <ProgressTemplate>
                <img src="../Images/throbber.gif" alt="" />
            </ProgressTemplate>
        </asp:UpdateProgress>
        <div class="row">
            <div class="col-lg-12 ">
                <div class="table-responsive">
                    <asp:GridView ID="gvPOList" runat="server" AutoGenerateColumns="False"
                        AllowPaging="true" OnPageIndexChanging="gvPOList_PageIndexChanging" PageSize="20" Width="100%" CssClass="table table-striped table-bordered table-hover">
                        <Columns>
                            <%--<asp:buttonfield buttontype="" commandname="Select" text="Select"/>         --%>
                            <asp:TemplateField HeaderText="Select">
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkSelect" runat="server" />
                                </ItemTemplate>
                                <HeaderTemplate>
                                    <input id="chkAll" onclick="javascript: SelectAllCheckboxes(this);"
                                        runat="server" type="checkbox" />
                                </HeaderTemplate>
                            </asp:TemplateField>
                            
                            <asp:BoundField DataField="LineNo" HeaderText="LineNo" ReadOnly="True" SortExpression="LineNo" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" ItemStyle-Width="0"  />
                            <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" ReadOnly="True" SortExpression="ItemNo" ItemStyle-CssClass="visible-xs" />
                            <asp:BoundField DataField="Description" HeaderText="Description" ReadOnly="True" SortExpression="Description" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" />
                            <asp:BoundField DataField="QtytoReceive" HeaderText="QtytoReceive" ReadOnly="True" SortExpression="QtytoReceive" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                            <asp:BoundField DataField="BuyfromVendorNo" HeaderText="Buyfrom" ReadOnly="True" SortExpression="BuyfromVendorNo" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" Visible="false" />
                            <%--<asp:BoundField DataField="Quantity" HeaderText="Quantity" ReadOnly="True" SortExpression="Quantity" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />--%>
                            <asp:BoundField DataField="Location" HeaderText="Location" ReadOnly="True" SortExpression="Location" ItemStyle-CssClass="hidden-xs" HeaderStyle-CssClass="hidden-xs" />
                             <asp:BoundField DataField="DocumentNo" HeaderText="DocumentNo" ReadOnly="True" SortExpression="DocumentNo" HeaderStyle-CssClass="visible-lg" ItemStyle-CssClass="visible-lg" Visible="false" />
                        </Columns>
                        <PagerTemplate>
                            <asp:LinkButton ID="lb_firstpage" runat="server" OnClick="lb_firstpage_Click" class="btn btn-default btn-sm" >Home</asp:LinkButton>
                            <asp:LinkButton ID="lb_previouspage" runat="server" OnClick="lb_previouspage_Click" class="btn btn-default btn-sm" >Previous</asp:LinkButton>
                            <asp:LinkButton ID="lb_nextpage" runat="server" OnClick="lb_nextpage_Click" class="btn btn-default btn-sm" >Next</asp:LinkButton>
                            <asp:LinkButton ID="lb_lastpage" runat="server" OnClick="lb_lastpage_Click" class="btn btn-default btn-sm" >Last</asp:LinkButton>
                            <asp:Label ID="lbl_nowpage" runat="server" Text="<%#gvPOList.PageIndex+1 %>" ForeColor="#db530f"></asp:Label> page / total<asp:Label ID="lbl_totalpage" runat="server" Text="<%#gvPOList.PageCount %>" ForeColor="#db530f"></asp:Label>
                        </PagerTemplate>
                    </asp:GridView>
                </div>
            </div>
        </div>
        <table>
            <tr>
                <td>
                    <asp:Button ID="BackButton" runat="server" Text="Back"
                        Width="65px" OnClick="btnBackURL_Click" Font-Size="8pt" class="btn btn-primary" />
                </td>
                <td>
                    <asp:Button ID="SelectButton" runat="server" Text="Select"
                        Width="65px" OnClick="btnSelect_Click" Font-Size="8pt" class="btn btn-primary" />
                </td>
            </tr>
        </table>
        <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
    </div>
</asp:Content>
