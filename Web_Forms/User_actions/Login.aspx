<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Web_Forms_User_actions_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-heading page-header text-center"><h1>Login</h1></div>
    <div class="panel-body">
        <div class="form-group">
            <label for="exampleInputEmail1">Email address</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="EmailTextBox" placeholder="Enter email" Text=""/>
        </div>
        <div class="form-group">
            <label for="exampleInputPassword1">Password</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="PasswordTextBox" placeholder="Password" TextMode="Password" Text=""/>
        </div>
        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Login"></asp:Button>
    </div>
</asp:Content>