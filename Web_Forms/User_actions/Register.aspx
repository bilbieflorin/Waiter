<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Web_Forms_User_actions_Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel-heading page-header text-center"><h1>Register</h1></div>
    <div class="panel-body">
        <div class="form-group" id="emailformgroup" runat="server">
            <label class="control-label" for="EmailTextBox">Email address</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="EmailTextBox" placeholder="Enter email" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Adresa de email invalida!" ForeColor="Red" Display="Dynamic" ControlToValidate="EmailTextBox" OnServerValidate="Email_ServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
         <div class="form-group">
            <label class="control-label" for="FirstNameTextBox">First name</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="FirstNameTextBox" placeholder="Enter first name" />
        </div>
         <div class="form-group">
            <label class="control-label" for="LastNameTextBox">Last name</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="LastNameTextBox" placeholder="Enter last name" />
        </div>
        <div class="form-group" id="passwordformgroup" runat="server">
            <label class="control-label" for="PasswordTextBox">Password</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="PasswordTextBox" placeholder="Password" TextMode="Password"/>
             <asp:CustomValidator ID="Password" runat="server"  ErrorMessage="Parola invalida!(minim 4 caractere/maxim 20 caractere)" ForeColor="Red" Display="Dynamic"  ControlToValidate="PasswordTextBox" OnServerValidate="Password_ServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" id="confirmpasswordformgroup" runat="server">
            <label class="control-label" for="ConfirmPasswordTextBox">Confirm password</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="ConfirmPasswordTextBox" placeholder="Confirm password" TextMode="Password"/>
            <asp:CustomValidator ID="ConfirmPassword" runat="server" ErrorMessage="Parola nu corespunde!" ForeColor="Red" Display="Dynamic" ControlToValidate="ConfirmPasswordTextBox" OnServerValidate="ConfirmPassword_ServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Register"></asp:Button>
    </div>
</asp:Content>
