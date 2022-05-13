<%@ Page Async="true" Language="C#" MasterPageFile="~/Mobile.master" AutoEventWireup="true" CodeFile="ConnectTest.aspx.cs" Inherits="ConnectTest" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="Server">
    <asp:GridView ID="gvComJourList" runat="server" AutoGenerateColumns="False">
        <Columns>
            <asp:BoundField DataField="name" HeaderText="name" ReadOnly="True"
                SortExpression="name" />
            <asp:BoundField DataField="id"
                HeaderText="id" ReadOnly="True" SortExpression="id" />
        </Columns>
    </asp:GridView>
    <table runat="server">
        <tr>
            <td>
                <asp:Button runat="server" Text="Button" OnClick="btnClick"></asp:Button>
            </td>
        </tr>
    </table>
</asp:Content>
