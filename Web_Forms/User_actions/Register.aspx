<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Register.aspx.cs" Inherits="Web_Forms_User_actions_Register" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel-heading page-header text-center"><h1>Inregistrare</h1></div>
    <div class="panel-body">
        <div class="form-group" id="emailformgroup" runat="server">
            <label class="control-label" for="EmailTextBox">Adresa email</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="EmailTextBox" placeholder="Introduceti adresa email" />
            <asp:CustomValidator ID="CustomValidator1" runat="server" ErrorMessage="Adresa de email invalida!" ForeColor="Red" Display="Dynamic" ControlToValidate="EmailTextBox" OnServerValidate="emailServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
         <div class="form-group">
            <label class="control-label" for="LastNameTextBox">Nume</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="LastNameTextBox" placeholder="Introduceti numele" />
        </div>
        <div class="form-group">
            <label class="control-label" for="FirstNameTextBox">Prenume</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="FirstNameTextBox" placeholder="Introduceti prenumele" />
        </div>
        <div class="form-group" id="passwordformgroup" runat="server">
            <label class="control-label" for="PasswordTextBox">Parola</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="PasswordTextBox" placeholder="Introduceti parola" TextMode="Password"/>
             <asp:CustomValidator ID="Password" runat="server"  ErrorMessage="Parola invalida!(minim 4 caractere/maxim 20 caractere)" ForeColor="Red" Display="Dynamic"  ControlToValidate="PasswordTextBox" OnServerValidate="passwordServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group" id="confirmpasswordformgroup" runat="server">
            <label class="control-label" for="ConfirmPasswordTextBox">Confirmare parola</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="ConfirmPasswordTextBox" placeholder="Reintroduceti parola" TextMode="Password"/>
            <asp:CustomValidator ID="ConfirmPassword" runat="server" ErrorMessage="Parola nu corespunde!" ForeColor="Red" Display="Dynamic" ControlToValidate="ConfirmPasswordTextBox" OnServerValidate="confirmPasswordServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group">
            <label class="control-label">Specific preferat</label>
            <asp:CheckBoxList runat="server" ID="SpecificCheckBoxList" DataSourceID="SqlDataSource1" DataTextField="DENUMIRE_SPECIFIC" DataValueField="DENUMIRE_SPECIFIC">
            </asp:CheckBoxList>
            <asp:SqlDataSource runat="server" ID="SqlDataSource1" ConnectionString='<%$ ConnectionStrings:ConnectionString %>' SelectCommand="SELECT [DENUMIRE_SPECIFIC] FROM [SPECIFIC] ORDER BY [DENUMIRE_SPECIFIC]"></asp:SqlDataSource>
        </div>
        <asp:Button ID="Button1" CssClass="btn btn-primary" runat="server" Text="Inregistrare"></asp:Button>
    </div>
</asp:Content>
