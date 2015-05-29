<%@ Page Title="" Language="C#" MasterPageFile="~/Web_Forms/Master/WaiterMasterPage.master" AutoEventWireup="true" CodeFile="Profil.aspx.cs" Inherits="Web_Forms_Profil_Profil" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
    <div class="panel-heading page-header text-center"><h1>Profil</h1></div>
    <div class="panel-body">
        <div class="form-group" id="emailformgroup" runat="server">
            <label class="control-label" for="EmailTextBox">Adresa email</label>
            <asp:Label runat="server" CssClass="form-control" ID="EmailLabel"/>
        </div>
         <div class="form-group">
            <label class="control-label" for="LastNameTextBox">Nume</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="LastNameTextBox"/>
        </div>
        <div class="form-group">
            <label class="control-label" for="FirstNameTextBox">Prenume</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="FirstNameTextBox"/>
        </div>
        <div class="form-group" id="confirmPasswordFormGroup" runat="server">
            <label class="control-label" for="ConfirmPasswordTextBox">Confirmare parola</label>
            <asp:TextBox runat="server" CssClass="form-control" ID="ConfirmPasswordTextBox" TextMode="Password"/>
            <asp:CustomValidator ID="ConfirmPassword" runat="server" ErrorMessage="Parola nu corespunde!" ForeColor="Red" Display="Dynamic" ControlToValidate="ConfirmPasswordTextBox" OnServerValidate="confirmPasswordServerValidate" ValidateEmptyText="true"></asp:CustomValidator>
        </div>
        <div class="form-group">
            <label class="control-label">Specific preferat</label>
            <asp:CheckBoxList runat="server" ID="SpecificCheckBoxList">
            </asp:CheckBoxList>
        </div>
        <asp:Button ID="UpdateButton" CssClass="btn btn-primary" OnClick="updateButtonClick" runat="server" Text="Updateaza"></asp:Button>
    </div>
</asp:Content>

