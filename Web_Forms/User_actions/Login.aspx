<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Login.aspx.cs" Inherits="Web_Forms_User_actions_Login" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" runat="Server">
    <div class="panel-heading page-header text-center"><h1>Autentificare</h1></div>
    <div class="panel-body">
        <div id="status" runat="server" class="alert alert-danger text-center">
            <p id="errormessage" runat="server">Adresa de email/parola invalida!</p>
        </div>
        <div class="form-group" id="emailformgroup" runat="server">
            <label class="control-label">Adresa email</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="EmailTextBox" placeholder="Introduceti adresa email" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Adresa de email invalida!" ForeColor="Red" Display="Dynamic" ControlToValidate="EmailTextBox" OnServerValidate="emailServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" id="passwordformgroup" runat="server">
            <label class="control-label">Parola</label>
            <asp:TextBox runat="server" CssClass="form-control" placeholder="Introduceti parola" ID="PasswordTextBox" TextMode="Password" />
            <asp:CustomValidator ID="Password" runat="server"  ErrorMessage="Introduceti parola!" ForeColor="Red" Display="Dynamic"  ControlToValidate="PasswordTextBox" OnServerValidate="passwordServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <asp:Button ID="LoginButton" CssClass="btn btn-primary" runat="server" Text="Autentificare" OnClick="loginButtonClick"></asp:Button>
    </div>
</asp:Content>