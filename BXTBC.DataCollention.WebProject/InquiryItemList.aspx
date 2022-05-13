<%@ Page Async="true"  Title="Items having inventory on hand" Language="C#" MasterPageFile="~/Mobile.master" AutoEventWireup="true" CodeFile="InquiryItemList.aspx.cs" Inherits="InquiryItemList" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    
    <script type="text/javascript"  >

    function GetRowValue(val)

    {

        // hardcoded value used to minimize the code. 

        // ControlID can instead be passed as query string to the popup window

        window.opener.document.getElementById("tbItem").value = val; 
        window.opener.document.getElementById("tbItem").focus();
        //__doPostBack('UpdatePanel1','');
        window.close();

    }

</script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" Runat="Server">

    <b>Items having inventory on hand</b>
    <asp:CheckBox ID="cbShowZero" runat="server" Text="Show zero?" AutoPostBack="true" Checked="false"  />
        <asp:GridView ID="gvItemJourList" runat="server" AutoGenerateColumns="False" onrowcommand="gvItemList_RowCommand"
         >
            <Columns>
            <asp:buttonfield buttontype="Link" commandname="Select" text="Select"/>            
            <asp:BoundField DataField="ItemNo" HeaderText="ItemNo" ReadOnly="True" 
                SortExpression="ItemNo" />
            <asp:BoundField DataField="QtyCalculated"
                HeaderText="QtyCalculated" ReadOnly="True" SortExpression="QtyCalculated" />
            <asp:BoundField DataField="Quantity"
                HeaderText="Quantity" ReadOnly="True" SortExpression="Quantity" />
            <%--<asp:BoundField DataField="AvailPhysical"
                HeaderText="Available Qty" ReadOnly="True" SortExpression="AvailPhysical" />--%>
            <%--<asp:BoundField DataField="WmsLocationId" HeaderText="Location" ReadOnly="True"
                SortExpression="WmsLocationId" />
            <asp:BoundField DataField="InventLocationId" HeaderText="Warehouse" ReadOnly="True"
                SortExpression="InventLocationId" />--%>

            </Columns>
        </asp:GridView>
     
        <table>
            <tr>
            <td >
            <asp:Button ID="BackButton" runat="server" Text="Back" 
            Width="65px" onclick="btnBackURL_Click" Font-Size="8pt" />    </td>
                </tr>
      
        </table>
    <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
</asp:Content>
