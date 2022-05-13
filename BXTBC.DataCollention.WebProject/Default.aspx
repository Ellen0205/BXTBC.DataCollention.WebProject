<%@ Page Language="C#" MasterPageFile="~/Site.master" CodeFile="Default.aspx.cs" Inherits="Default" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" Runat="Server">
    <script type="text/javascript">
        document.onkeyup = displayunicode;
        function displayunicode(e) {
            var theKey = window.event.keyCode;

            if (theKey == 27)//// Escape
                window.location.href("logon.aspx");
            if (theKey == 65)//// a
                window.location.href("POReceiving.aspx");
            if (theKey == 66)//// b
                window.location.href("CycleCount.aspx");


        }
    </script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <div id="MainContent_mnuMenu" class="container">
        <ul  class="level1 static"  style="width:auto;float:left;position:relative;list-style-type:none ">
            <li class="static">
                <asp:HyperLink ID = "HyperLinkRAFProdOrder" runat="server"  
                NavigateUrl="~/POReceiving.aspx" 
                    ToolTip="Receive items for Prod. orders" ForeColor="Black" Font-Underline="False">a. PO Receiving</asp:HyperLink>
            </li>
             <%--<li class="static">
                 <br>
                <asp:HyperLink ID = "HyperLink1" runat="server"  
                NavigateUrl="~/CycleCount.aspx" 
                    ToolTip="Receive items for Prod. orders" ForeColor="Black" Font-Underline="False">b. Cycle Count</asp:HyperLink>
            </li>--%>
        </ul>

    </div> 
    <asp:SiteMapDataSource ID="SiteMapDataSource" runat="server" />
</asp:Content>


